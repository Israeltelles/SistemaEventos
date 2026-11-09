using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEventos.Data;
using SistemaEventos.Models;

namespace SistemaEventos.Controllers
{
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListaEventos()
        {
            var eventos = await _context.Eventos
                .OrderBy(e => e.Data)
                .ThenBy(e => e.HoraInicio)
                .ToListAsync();

            return View(eventos);
        }

        public IActionResult Criar()
        {
            if (!UsuarioPodeGerenciarEvento())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(EventoModel evento)
        {
            if (!UsuarioPodeGerenciarEvento())
            {
                return RedirectToAction("Index", "Home");
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            evento.CoordenadorId = usuarioId.Value;
            ModelState.Remove("Coordenador");
            ModelState.Remove("CoordenadorId");

            ValidarEvento(evento);

            if (!ModelState.IsValid)
            {
                return View(evento);
            }

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Evento criado com sucesso.";
            return RedirectToAction("Gerenciar");
        }

        public async Task<IActionResult> Gerenciar()
        {
            if (!UsuarioPodeGerenciarEvento())
            {
                return RedirectToAction("Index", "Home");
            }

            var eventos = await _context.Eventos
                .OrderBy(e => e.Data)
                .ThenBy(e => e.HoraInicio)
                .ToListAsync();

            return View(eventos);
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListaEventos");
            }

            var evento = await _context.Eventos
                .Include(e => e.Coordenador)
                .FirstOrDefaultAsync(e => e.Id == id.Value);

            if (evento == null)
            {
                return RedirectToAction("ListaEventos");
            }

            return View(evento);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (!UsuarioPodeGerenciarEvento())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return RedirectToAction("Gerenciar");
            }

            var evento = await _context.Eventos.FindAsync(id.Value);

            if (evento == null)
            {
                return RedirectToAction("Gerenciar");
            }

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, EventoModel evento)
        {
            if (!UsuarioPodeGerenciarEvento())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != evento.Id)
            {
                return RedirectToAction("Gerenciar");
            }

            var eventoBanco = await _context.Eventos.FindAsync(id);

            if (eventoBanco == null)
            {
                return RedirectToAction("Gerenciar");
            }

            ModelState.Remove("Coordenador");
            ModelState.Remove("CoordenadorId");

            ValidarEvento(evento);

            if (!ModelState.IsValid)
            {
                evento.CoordenadorId = eventoBanco.CoordenadorId;
                return View(evento);
            }

            eventoBanco.Nome = evento.Nome;
            eventoBanco.Descricao = evento.Descricao;
            eventoBanco.Data = evento.Data;
            eventoBanco.Local = evento.Local;
            eventoBanco.HoraInicio = evento.HoraInicio;
            eventoBanco.HoraFim = evento.HoraFim;

            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Evento atualizado com sucesso.";
            return RedirectToAction("Gerenciar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            if (!UsuarioPodeGerenciarEvento())
            {
                return RedirectToAction("Index", "Home");
            }

            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                TempData["Mensagem"] = "Evento não encontrado.";
                return RedirectToAction("Gerenciar");
            }

            var certificados = _context.Certificados.Where(c => c.EventoId == id);
            var presencas = _context.Presencas.Where(p => p.EventoId == id);
            var inscricoes = _context.Inscricoes.Where(i => i.EventoId == id);

            _context.Certificados.RemoveRange(certificados);
            _context.Presencas.RemoveRange(presencas);
            _context.Inscricoes.RemoveRange(inscricoes);
            _context.Eventos.Remove(evento);

            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Evento excluído com sucesso.";
            return RedirectToAction("Gerenciar");
        }

        private bool UsuarioPodeGerenciarEvento()
        {
            var tipoUsuario = HttpContext.Session.GetString("Tipo");

            return tipoUsuario == "Coordenador" || tipoUsuario == "Diretor";
        }

        private void ValidarEvento(EventoModel evento)
        {
            if (evento.Data.Date < DateTime.Today)
            {
                ModelState.AddModelError("Data", "A data do evento não pode ser anterior à data atual.");
            }

            if (evento.HoraFim <= evento.HoraInicio)
            {
                ModelState.AddModelError("HoraFim", "A hora de término deve ser maior que a hora de início.");
            }
        }
    }
}
