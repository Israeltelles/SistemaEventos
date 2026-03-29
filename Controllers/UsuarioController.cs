using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaEventos.Models;

namespace SistemaEventos.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

      
        [HttpGet]
        public IActionResult CadastroAluno()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroAluno(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Tipo = "Aluno";
                return RedirectToAction("Index", "Login");
            }
            return View(usuario);
        }

        
        [Authorize(Roles = "Diretor")]
        [HttpGet]
        public IActionResult CadastroCoordenador()
        {
            return View();
        }

        [Authorize(Roles = "Diretor")]
        [HttpPost]
        public IActionResult CadastroCoordenador(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Tipo = "Coordenador";
                
                return RedirectToAction("Index");
            }
            return View(usuario);
        }
        [Authorize]
        public IActionResult Perfil()
        {
            return View();
        }
    }
}
