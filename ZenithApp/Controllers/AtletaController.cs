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

            var convites = _context.ConvitesTreinador
                .Include(x => x.Treinador)
                .Where(x => x.IdAtleta == idAtleta && x.Status == "Pendente")
                .ToList();

            // Treinos do atleta com exercícios
            var treinos = _context.Treinos
                .Include(x => x.Exercicios)
                .Where(x => x.IdAtleta == idAtleta)
                .ToList();

            // Metas da semana
            var metas = _context.MetasSemana
                .Where(x => x.IdAtleta == idAtleta)
                .ToList();

            ViewBag.Treinos = treinos;
            ViewBag.Metas = metas;

            return View("~/Views/Home/Atleta.cshtml", convites);
        }

        [HttpPost]
        public IActionResult AceitarConvite(int idConvite)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var convite = _context.ConvitesTreinador
                .FirstOrDefault(x => x.Id == idConvite);

            if (convite == null)
            {
                TempData["Erro"] = "Convite não encontrado.";
                return RedirectToAction("Index");
            }

            // Garante que o atleta só aceita convites próprios
            int idAtleta = Convert.ToInt32(usuarioId);
            if (convite.IdAtleta != idAtleta)
            {
                TempData["Erro"] = "Ação não permitida.";
                return RedirectToAction("Index");
            }

            convite.Status = "Aceito";

            var relacao = new TreinadorAtleta
            {
                IdTreinador = convite.IdTreinador,
                IdAtleta = convite.IdAtleta
            };

            _context.TreinadorAtletas.Add(relacao);
            _context.SaveChanges();

            TempData["Sucesso"] = "Convite aceito! Treinador adicionado.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RecusarConvite(int idConvite)
        {
            string? usuarioId = HttpContext.Session.GetString("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var convite = _context.ConvitesTreinador
                .FirstOrDefault(x => x.Id == idConvite);

            if (convite == null)
            {
                TempData["Erro"] = "Convite não encontrado.";
                return RedirectToAction("Index");
            }

            int idAtleta = Convert.ToInt32(usuarioId);
            if (convite.IdAtleta != idAtleta)
            {
                TempData["Erro"] = "Ação não permitida.";
                return RedirectToAction("Index");
            }

            convite.Status = "Recusado";
            _context.SaveChanges();

            TempData["Erro"] = "Convite recusado.";
            return RedirectToAction("Index");
        }
    }
}