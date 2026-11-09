using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEventos.Data;
using SistemaEventos.Models;

namespace SistemaEventos.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InscricaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> MinhasInscricoes()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var inscricoes = await _context.Inscricoes
                .Include(i => i.Evento)
                .Where(i => i.UsuarioId == usuarioId)
                .OrderByDescending(i => i.DataInscricao)
                .ToListAsync();

            var presencasConfirmadas = await _context.Presencas
                .Where(p => p.UsuarioId == usuarioId && p.Confirmacao == true)
                .Select(p => p.EventoId)
                .ToListAsync();

            var certificadosEmitidos = await _context.Certificados
                .Where(c => c.UsuarioId == usuarioId)
                .Select(c => c.EventoId)
                .ToListAsync();

            var viewModel = new MinhasInscricoesViewModel
            {
                Inscricoes = inscricoes.Select(i => new InscricaoDetalheViewModel
                {
                    InscricaoId = i.Id,
                    EventoId = i.EventoId,
                    NomeEvento = i.Evento?.Nome ?? "—",
                    DataEvento = i.Evento?.Data ?? default,
                    Local = i.Evento?.Local ?? "—",
                    HoraInicio = i.Evento?.HoraInicio ?? default,
                    HoraFim = i.Evento?.HoraFim ?? default,
                    DataInscricao = i.DataInscricao,
                    PresencaConfirmada = presencasConfirmadas.Contains(i.EventoId),
                    CertificadoDisponivel = certificadosEmitidos.Contains(i.EventoId)
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscricao(int eventoId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var evento = await _context.Eventos.FindAsync(eventoId);

            if (evento == null)
            {
                TempData["Erro"] = "Evento não encontrado.";
                return RedirectToAction("ListaEventos", "Eventos");
            }

            var limiteInscricao = evento.Data.Date + evento.HoraInicio - TimeSpan.FromHours(2);

            if (DateTime.Now >= limiteInscricao)
            {
                TempData["Erro"] = $"As inscrições para \"{evento.Nome}\" estão encerradas.";
                return RedirectToAction("ListaEventos", "Eventos");
            }

            bool jaInscrito = await _context.Inscricoes
                .AnyAsync(i => i.UsuarioId == usuarioId && i.EventoId == eventoId);

            if (jaInscrito)
            {
                TempData["Erro"] = "Você já está inscrito neste evento.";
                return RedirectToAction("ListaEventos", "Eventos");
            }

            var inscricao = new InscricaoModel
            {
                UsuarioId = usuarioId.Value,
                EventoId = eventoId,
                DataInscricao = DateTime.Now
            };

            _context.Inscricoes.Add(inscricao);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = $"Inscrição realizada com sucesso em \"{evento.Nome}\"!";
            return RedirectToAction("ListaEventos", "Eventos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int inscricaoId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var inscricao = await _context.Inscricoes
                .FirstOrDefaultAsync(i => i.Id == inscricaoId && i.UsuarioId == usuarioId);

            if (inscricao == null)
            {
                TempData["Erro"] = "Inscrição não encontrada.";
                return RedirectToAction("MinhasInscricoes");
            }

            _context.Inscricoes.Remove(inscricao);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Inscrição cancelada com sucesso.";
            return RedirectToAction("MinhasInscricoes");
        }

        [HttpGet]
        public IActionResult ConfirmaPresenca()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmaPresenca(int eventoId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var inscricao = await _context.Inscricoes
                .FirstOrDefaultAsync(i => i.EventoId == eventoId && i.UsuarioId == usuarioId);

            if (inscricao == null)
            {
                TempData["Erro"] = "Você não está inscrito neste evento.";
                return RedirectToAction("ConfirmaPresenca");
            }

            var jaConfirmou = await _context.Presencas
                .AnyAsync(p => p.UsuarioId == usuarioId && p.EventoId == eventoId && p.Confirmacao);

            if (jaConfirmou)
            {
                TempData["Erro"] = "Sua presença já foi confirmada anteriormente.";
                return RedirectToAction("ConfirmaPresenca");
            }

            var novaPresenca = new PresencaModel
            {
                UsuarioId = usuarioId.Value,
                EventoId = eventoId,
                Confirmacao = true,
                DataConfirmacao = DateTime.Now
            };

            _context.Presencas.Add(novaPresenca);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Presença confirmada com sucesso!";
            return RedirectToAction("MinhasInscricoes");
        }

        [HttpGet]
        public async Task<IActionResult> GerarCodigo(int eventoId)
        {
            var tipo = HttpContext.Session.GetString("Tipo");

            if (tipo != "Coordenador" && tipo != "Diretor")
                return RedirectToAction("Index", "Home");

            var evento = await _context.Eventos.FindAsync(eventoId);

            if (evento == null)
                return NotFound();

            return View(evento);
        }

        [HttpGet]
        public async Task<IActionResult> Certificado()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var certificados = await _context.Certificados
                .Include(c => c.Evento)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.DataEmissao)
                .ToListAsync();

            return View(certificados);
        }
    }
}