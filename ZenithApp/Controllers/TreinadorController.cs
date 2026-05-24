using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenithApp.Data;
using ZenithApp.Models;

namespace ZenithApp.Controllers
{
    public class TreinadorController : Controller
    {
        private readonly AppDbContext _context;

        public TreinadorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            int idTreinador = Convert.ToInt32(usuarioId);

            var convites = _context.ConvitesTreinador
                .Include(x => x.Atleta)
                    .ThenInclude(a => a.Login)
                .Where(x => x.IdTreinador == idTreinador)
                .ToList();

            var atletas = _context.TreinadorAtletas
                .Include(x => x.Atleta)
                .Where(x => x.IdTreinador == idTreinador)
                .ToList();

            // Treinos agrupados por atleta, com exercícios incluídos
            var treinos = _context.Treinos
                .Include(x => x.Atleta)
                .Include(x => x.Exercicios)
                .Where(x => x.IdTreinador == idTreinador)
                .ToList();

            ViewBag.TotalAtletas = atletas.Count;
            ViewBag.TotalTreinos = treinos.Count;   // ← contador do hero
            ViewBag.Convites = convites;
            ViewBag.Atletas = atletas;
            ViewBag.Treinos = treinos;

            return View("~/Views/Home/Treinador.cshtml");
        }

        [HttpPost]
        public IActionResult EnviarConvite(string emailAtleta)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");

            if (usuarioId == null)
            {
                TempData["Erro"] = "Sessão expirada. Faça login novamente.";
                return RedirectToAction("Login", "Auth");
            }

            int idTreinador = Convert.ToInt32(usuarioId);

            var login = _context.Logins
                .FirstOrDefault(x => x.Usuario == emailAtleta);

            if (login == null)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            if (login.NivelAcesso != "Atleta")
            {
                TempData["Erro"] = "Esse usuário não é um atleta.";
                return RedirectToAction("Index");
            }

            var atleta = _context.Atletas
                .FirstOrDefault(x => x.IdLogin == login.IdLogin);

            if (atleta == null)
            {
                TempData["Erro"] = "Perfil de atleta não encontrado.";
                return RedirectToAction("Index");
            }

            // Já são parceiros?
            bool jaVinculado = _context.TreinadorAtletas
                .Any(x => x.IdTreinador == idTreinador && x.IdAtleta == atleta.IdAtleta);

            if (jaVinculado)
            {
                TempData["Erro"] = "Esse atleta já está na sua equipe.";
                return RedirectToAction("Index");
            }

            bool conviteExiste = _context.ConvitesTreinador
                .Any(x =>
                    x.IdTreinador == idTreinador &&
                    x.IdAtleta == atleta.IdAtleta &&
                    x.Status == "Pendente"
                );

            if (conviteExiste)
            {
                TempData["Erro"] = "Já existe um convite pendente para esse atleta.";
                return RedirectToAction("Index");
            }

            var convite = new ConviteTreinador
            {
                IdTreinador = idTreinador,
                IdAtleta = atleta.IdAtleta,
                Status = "Pendente",
                DataEnvio = DateTime.Now
            };

            _context.ConvitesTreinador.Add(convite);
            _context.SaveChanges();

            TempData["Sucesso"] = $"Convite enviado para {atleta.Nome}!";
            return RedirectToAction("Index");
        }

        // Retorna atletas vinculados ao treinador para popular o select
        [HttpGet]
        public IActionResult CriarTreino(int idAtleta)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idTreinador = Convert.ToInt32(usuarioId);

            // Verifica se o atleta pertence a esse treinador
            bool vinculado = _context.TreinadorAtletas
                .Any(x => x.IdTreinador == idTreinador && x.IdAtleta == idAtleta);

            if (!vinculado)
            {
                TempData["Erro"] = "Atleta não encontrado na sua equipe.";
                return RedirectToAction("Index");
            }

            ViewBag.IdAtleta = idAtleta;
            return View("~/Views/Treinador/CriarTreino.cshtml");
        }

        [HttpPost]
        public IActionResult CriarTreino(
            int idAtleta,
            string nomeTreino,
            string? observacoes,
            List<string> exercicioNome,
            List<int> exercicioSeries,
            List<int> exercicioReps,
            List<string> metas)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idTreinador = Convert.ToInt32(usuarioId);

            var treino = new Treino
            {
                Nome = nomeTreino,
                Observacoes = observacoes,
                IdTreinador = idTreinador,
                IdAtleta = idAtleta,
                DataCriacao = DateTime.Now
            };

            _context.Treinos.Add(treino);
            _context.SaveChanges();

            // Salva exercícios
            for (int i = 0; i < exercicioNome.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(exercicioNome[i])) continue;

                _context.Exercicios.Add(new Exercicio
                {
                    Nome = exercicioNome[i],
                    Series = exercicioSeries.ElementAtOrDefault(i),
                    Repeticoes = exercicioReps.ElementAtOrDefault(i),
                    IdTreino = treino.IdTreino
                });
            }

            // Salva metas
            foreach (var meta in metas)
            {
                if (string.IsNullOrWhiteSpace(meta)) continue;

                _context.MetasSemana.Add(new MetaSemana
                {
                    Descricao = meta,
                    IdAtleta = idAtleta,
                    IdTreinador = idTreinador
                });
            }

            _context.SaveChanges();

            TempData["Sucesso"] = "Treino criado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ExcluirTreino(int idTreino)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idTreinador = Convert.ToInt32(usuarioId);

            var treino = _context.Treinos
                .Include(x => x.Exercicios)
                .FirstOrDefault(x => x.IdTreino == idTreino && x.IdTreinador == idTreinador);

            if (treino == null)
            {
                TempData["Erro"] = "Treino não encontrado.";
                return RedirectToAction("Index");
            }

            // Remove metas vinculadas ao mesmo atleta/treinador deste treino
            var metas = _context.MetasSemana
                .Where(m => m.IdAtleta == treino.IdAtleta && m.IdTreinador == idTreinador)
                .ToList();

            _context.MetasSemana.RemoveRange(metas);
            _context.Exercicios.RemoveRange(treino.Exercicios);
            _context.Treinos.Remove(treino);
            _context.SaveChanges();

            TempData["Sucesso"] = "Treino excluído.";
            return RedirectToAction("Index");
        }
    }
}