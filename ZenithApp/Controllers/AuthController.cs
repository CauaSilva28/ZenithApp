using Microsoft.AspNetCore.Mvc;
using ZenithApp.Models;

namespace ZenithApp.Controllers
{
    public class AuthController : Controller
    {
        // ================= USUÁRIO TEMPORÁRIO =================

        private static RegisterViewModel? usuarioCadastrado;

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verifica se existe usuário cadastrado
            if (usuarioCadastrado != null)
            {
                if (
                    model.Email == usuarioCadastrado.Email &&
                    model.Password == usuarioCadastrado.Password
                )
                {
                    TempData["Success"] = "Login realizado com sucesso!";

                    return RedirectToAction("Index", "Home");
                }
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
    }
}