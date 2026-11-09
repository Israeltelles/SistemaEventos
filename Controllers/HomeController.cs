using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaEventos.Models;

namespace SistemaEventos.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.NomeUsuario = HttpContext.Session.GetString("Nome");
            ViewBag.TipoUsuario = HttpContext.Session.GetString("Tipo");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}