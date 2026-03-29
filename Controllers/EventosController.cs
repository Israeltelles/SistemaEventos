using Microsoft.AspNetCore.Mvc;

namespace SistemaEventos.Controllers
{
    public class EventosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
