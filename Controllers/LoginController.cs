using Microsoft.AspNetCore.Mvc;

namespace SistemaEventos.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
