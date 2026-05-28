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
                .Where(x => x.IdTreinador == idTreinador && x.Status != "Removido")
                .ToList();

            var atletas = _context.TreinadorAtletas
                .Include(x => x.Atleta)
                    .ThenInclude(a => a.Login)
                .Where(x => x.IdTreinador == idTreinador)
                .ToList();

            var treinos = _context.Treinos
                .Include(x => x.Atleta)
                .Include(x => x.Exercicios)
                .Where(x => x.IdTreinador == idTreinador)
                .ToList();

            ViewBag.TotalAtletas = atletas.Count;
            ViewBag.TotalTreinos = treinos.Count;
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

            bool jaVinculado = _context.TreinadorAtletas
                .Any(x => x.IdTreinador == idTreinador && x.IdAtleta == atleta.IdAtleta);

            if (jaVinculado)
            {
                TempData["Erro"] = "Esse atleta já está na sua equipe.";
                return RedirectToAction("Index");
            }

            // Já existe qualquer convite entre esse par?
            var conviteExistente = _context.ConvitesTreinador
                .FirstOrDefault(x => x.IdTreinador == idTreinador && x.IdAtleta == atleta.IdAtleta);

            if (conviteExistente != null)
            {
                if (conviteExistente.Status == "Pendente")
                {
                    TempData["Erro"] = "Já existe um convite pendente para esse atleta.";
                    return RedirectToAction("Index");
                }

                // Reutiliza o registro existente em vez de criar um novo
                conviteExistente.Status = "Pendente";
                conviteExistente.DataEnvio = DateTime.Now;
                _context.SaveChanges();

                TempData["Sucesso"] = $"Convite reenviado para {atleta.Nome}!";
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

        [HttpGet]
        public IActionResult CriarTreino(int idAtleta)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idTreinador = Convert.ToInt32(usuarioId);

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

            _context.Exercicios.RemoveRange(treino.Exercicios);
            _context.Treinos.Remove(treino);
            _context.SaveChanges();

            TempData["Sucesso"] = "Treino excluído.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditarTreino(int idTreino)
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

            return View("~/Views/Treinador/EditarTreino.cshtml", treino);
        }

        [HttpPost]
        public IActionResult EditarTreino(
            int idTreino,
            string nomeTreino,
            string? observacoes,
            List<int> exercicioId,
            List<string> exercicioNome,
            List<int> exercicioSeries,
            List<int> exercicioReps)
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

            treino.Nome = nomeTreino;
            treino.Observacoes = observacoes;

            _context.Exercicios.RemoveRange(treino.Exercicios);

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

            _context.SaveChanges();

            TempData["Sucesso"] = "Treino atualizado!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoverAtleta(int idAtleta)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idTreinador = Convert.ToInt32(usuarioId);

            var vinculo = _context.TreinadorAtletas
                .FirstOrDefault(x => x.IdTreinador == idTreinador && x.IdAtleta == idAtleta);

            if (vinculo == null)
            {
                TempData["Erro"] = "Atleta não encontrado na sua equipe.";
                return RedirectToAction("Index");
            }

            _context.TreinadorAtletas.Remove(vinculo);

            // Atualiza todos os convites desse par para "Removido"
            var convitesDoAtleta = _context.ConvitesTreinador
                .Where(x => x.IdTreinador == idTreinador && x.IdAtleta == idAtleta)
                .ToList();

            foreach (var c in convitesDoAtleta)
                c.Status = "Removido";

            _context.SaveChanges();

            TempData["Sucesso"] = "Atleta removido da equipe.";
            return RedirectToAction("Index");
        }
    }
}