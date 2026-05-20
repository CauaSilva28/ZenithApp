using Microsoft.AspNetCore.Mvc;
using ZenithApp.Models;

namespace ZenithApp.Controllers
{
    public class AuthController : Controller
    {
        private static RegisterViewModel? usuarioCadastrado;

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioNome") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (usuarioCadastrado != null &&
                model.Email == usuarioCadastrado.Email &&
                model.Password == usuarioCadastrado.Password)
            {
                // Salva nome, email e tipo na sessão
                HttpContext.Session.SetString("UsuarioNome", usuarioCadastrado.FirstName);
                HttpContext.Session.SetString("UsuarioEmail", usuarioCadastrado.Email);
                HttpContext.Session.SetString("UsuarioTipo", usuarioCadastrado.TipoUsuario);

                TempData["Success"] = "Login realizado com sucesso!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "E-mail ou senha inválidos");
            return View(model);
        }

        // ================= CADASTRO =================

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (usuarioCadastrado != null && usuarioCadastrado.Email == model.Email)
            {
                ModelState.AddModelError("", "Esse e-mail já está cadastrado");
                return View(model);
            }

            usuarioCadastrado = model;

            TempData["Success"] = "Conta criada com sucesso!";

            return RedirectToAction("Login");
        }

        // ================= LOGOUT ================= 
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}