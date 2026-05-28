using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenithApp.Data;
using ZenithApp.Models;

namespace ZenithApp.Controllers
{
    public class AtletaController : Controller
    {
        private readonly AppDbContext _context;

        public AtletaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            string? usuarioTipo = HttpContext.Session.GetString("UsuarioTipo");

            if (usuarioId == null || usuarioTipo != "Atleta")
                return RedirectToAction("Login", "Auth");

            int idAtleta = Convert.ToInt32(usuarioId);

            var atleta = _context.Atletas
                .FirstOrDefault(x => x.IdAtleta == idAtleta);

            var convites = _context.ConvitesTreinador
                .Include(x => x.Treinador)
                .Where(x => x.IdAtleta == idAtleta && x.Status == "Pendente")
                .ToList();

            var treinos = _context.Treinos
                .Include(x => x.Exercicios)
                .Where(x => x.IdAtleta == idAtleta)
                .ToList();

            var metas = _context.MetasSemana
                .Where(x => x.IdAtleta == idAtleta)
                .ToList();

            ViewBag.Atleta = atleta;
            ViewBag.Treinos = treinos;
            ViewBag.Metas = metas;

            return View("~/Views/Home/Atleta.cshtml", convites);
        }

        // ─── Convites ────────────────────────────────────────────────

        [HttpPost]
        public IActionResult AceitarConvite(int idConvite)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idAtleta = Convert.ToInt32(usuarioId);

            var convite = _context.ConvitesTreinador
                .FirstOrDefault(x => x.Id == idConvite);

            if (convite == null || convite.IdAtleta != idAtleta)
            {
                TempData["Erro"] = "Convite não encontrado.";
                return RedirectToAction("Index");
            }

            convite.Status = "Aceito";

            bool jaVinculado = _context.TreinadorAtletas
                .Any(x => x.IdTreinador == convite.IdTreinador && x.IdAtleta == idAtleta);

            if (!jaVinculado)
            {
                _context.TreinadorAtletas.Add(new TreinadorAtleta
                {
                    IdTreinador = convite.IdTreinador,
                    IdAtleta = convite.IdAtleta
                });
            }

            _context.SaveChanges();

            TempData["Sucesso"] = "Convite aceito! Treinador adicionado.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RecusarConvite(int idConvite)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idAtleta = Convert.ToInt32(usuarioId);

            var convite = _context.ConvitesTreinador
                .FirstOrDefault(x => x.Id == idConvite);

            if (convite == null || convite.IdAtleta != idAtleta)
            {
                TempData["Erro"] = "Convite não encontrado.";
                return RedirectToAction("Index");
            }

            convite.Status = "Recusado";
            _context.SaveChanges();

            TempData["Erro"] = "Convite recusado.";
            return RedirectToAction("Index");
        }

        // ─── Treinos ──────────────────────────────────────────────────

        [HttpPost]
        public IActionResult ConcluirTreino(int idTreino)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idAtleta = Convert.ToInt32(usuarioId);

            var treino = _context.Treinos
                .FirstOrDefault(x => x.IdTreino == idTreino && x.IdAtleta == idAtleta);

            if (treino == null)
            {
                TempData["Erro"] = "Treino não encontrado.";
                return RedirectToAction("Index");
            }

            // Toggle: permite desmarcar também
            treino.Concluido = !treino.Concluido;

            // Recalcula frequência: treinos concluídos nos últimos 7 dias
            var inicioSemana = DateTime.Today.AddDays(-6);
            int frequencia = _context.Treinos
                .Count(x => x.IdAtleta == idAtleta
                          && x.Concluido
                          && x.DataCriacao >= inicioSemana);

            // Inclui o treino atual caso esteja sendo marcado agora
            if (treino.Concluido && treino.DataCriacao < inicioSemana)
                frequencia += 1;

            var atleta = _context.Atletas.FirstOrDefault(x => x.IdAtleta == idAtleta);
            if (atleta != null)
                atleta.Frequencia = frequencia;

            _context.SaveChanges();

            TempData["Sucesso"] = treino.Concluido ? "Treino marcado como concluído!" : "Treino desmarcado.";
            return RedirectToAction("Index");
        }

        // ─── Métricas ─────────────────────────────────────────────────

        [HttpPost]
        public IActionResult AtualizarMetricas(decimal? peso, decimal? massaMuscular)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Auth");

            int idAtleta = Convert.ToInt32(usuarioId);

            var atleta = _context.Atletas.FirstOrDefault(x => x.IdAtleta == idAtleta);

            if (atleta == null)
            {
                TempData["Erro"] = "Perfil não encontrado.";
                return RedirectToAction("Index");
            }

            if (peso.HasValue)
                atleta.Peso = peso.Value;

            if (massaMuscular.HasValue)
                atleta.MassaMuscular = massaMuscular.Value;

            _context.SaveChanges();

            TempData["Sucesso"] = "Métricas atualizadas!";
            return RedirectToAction("Index");
        }
    }
}