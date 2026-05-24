using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenithApp.Data;
using ZenithApp.Models;

namespace ZenithApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioNome") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var login = await _context.Logins
                .FirstOrDefaultAsync(l => l.Usuario == model.Email);

            if (login == null || !BCrypt.Net.BCrypt.Verify(model.Password, login.Senha))
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos");
                return View(model);
            }

            // Busca nome do usuário dependendo do tipo
            string nomeUsuario = login.Usuario;

            if (login.NivelAcesso == "Atleta")
            {
                var atleta = await _context.Atletas.FirstOrDefaultAsync(a => a.IdLogin == login.IdLogin);
                if (atleta != null) nomeUsuario = atleta.Nome;
            }
            else if (login.NivelAcesso == "Treinador")
            {
                var treinador = await _context.Treinadores.FirstOrDefaultAsync(t => t.IdLogin == login.IdLogin);
                if (treinador != null) nomeUsuario = treinador.Nome;
            }

            HttpContext.Session.SetString("UsuarioNome", nomeUsuario);
            HttpContext.Session.SetString("UsuarioEmail", login.Usuario);
            HttpContext.Session.SetString("UsuarioTipo", login.NivelAcesso ?? "Atleta");

            TempData["Success"] = "Login realizado com sucesso!";
            return RedirectToAction("Index", "Home");
        }

        // ================= CADASTRO =================

        [HttpGet]
        public IActionResult Cadastro(string? tipoUsuario)
        {
            return View(new RegisterViewModel
            {
                TipoUsuario = tipoUsuario ?? "Atleta"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var emailExistente = await _context.Logins
                .AnyAsync(l => l.Usuario == model.Email);

            if (emailExistente)
            {
                ModelState.AddModelError("", "Esse e-mail já está cadastrado");
                return View(model);
            }

            // Cria o login
            var login = new Login
            {
                Usuario = model.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(model.Password),
                NivelAcesso = model.TipoUsuario
            };

            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            // Cria o perfil conforme o tipo
            if (model.TipoUsuario == "Atleta")
            {
                var atleta = new Atleta
                {
                    Nome = $"{model.FirstName} {model.LastName}",
                    IdLogin = login.IdLogin
                };
                _context.Atletas.Add(atleta);
            }
            else if (model.TipoUsuario == "Treinador")
            {
                var treinador = new Treinador
                {
                    Nome = $"{model.FirstName} {model.LastName}",
                    IdLogin = login.IdLogin
                };
                _context.Treinadores.Add(treinador);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Conta criada com sucesso!";
            return RedirectToAction("Login");
        }

        // ================= LOGOUT =================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}