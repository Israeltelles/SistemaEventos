using Microsoft.AspNetCore.Mvc;
using SistemaEventos.Data;
using SistemaEventos.Models;
using Microsoft.AspNetCore.Identity;

namespace SistemaEventos.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<UsuarioModel> _passwordHasher = new();

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string ra, string senha)
        {
            var login = ra?.Trim();
            var loginRaBruto = ApenasNumeros(login);
            var loginEmail = login?.ToLower();

            var usuario = _context.Usuarios
                .FirstOrDefault(u =>
                    (!string.IsNullOrWhiteSpace(loginRaBruto) && u.Ra == loginRaBruto)
                    ||
                    u.Email.ToLower() == loginEmail
                );

            if (usuario == null)
            {
                ModelState.AddModelError("", "RA/E-mail ou senha inválidos.");
                return View();
            }

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                senha
            );

            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "RA/E-mail ou senha inválidos.");
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Nome", usuario.Nome);
            HttpContext.Session.SetString("Tipo", usuario.Tipo);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }

        [HttpGet]
        public IActionResult CadastroAluno()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CadastroCoordenador()
        {
            var tipoUsuario = HttpContext.Session.GetString("Tipo");

            if (tipoUsuario != "Diretor")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Perfil()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId.Value);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.TelefoneFormatado = FormatarTelefone(usuario.Telefone);
            ViewBag.RaFormatado = FormatarRa(usuario.Ra);

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(UsuarioModel usuario, string confirmarSenha)
        {
            usuario.Nome = usuario.Nome?.Trim();
            usuario.Email = usuario.Email?.Trim();
            usuario.Telefone = ApenasNumeros(usuario.Telefone);
            usuario.Ra = ApenasNumeros(usuario.Ra);
            usuario.Tipo = "Aluno";

            ModelState.Remove("Tipo");

            string senhaDigitada = usuario.Senha ?? "";
            string confSenha = confirmarSenha;

            if (string.IsNullOrEmpty(confSenha) && Request.HasFormContentType)
            {
                confSenha = Request.Form["ConfirmarSenha"].ToString();
                if (string.IsNullOrEmpty(confSenha))
                {
                    confSenha = Request.Form["confirmarSenha"].ToString();
                }
            }

            senhaDigitada = senhaDigitada.Trim();
            confSenha = confSenha?.Trim() ?? "";

            ViewBag.Senha = senhaDigitada;
            ViewBag.ConfirmarSenha = confSenha;

            if (string.IsNullOrWhiteSpace(usuario.Ra))
            {
                ModelState.AddModelError("Ra", "O RA é obrigatório.");
            }
            else if (usuario.Ra.Length != 7)
            {
                ModelState.AddModelError("Ra", "O RA deve conter exatamente 7 números.");
            }
            else if (_context.Usuarios.Any(u => u.Ra == usuario.Ra))
            {
                ModelState.AddModelError("Ra", "Este RA já está cadastrado.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Telefone) && usuario.Telefone.Length != 10 && usuario.Telefone.Length != 11)
            {
                ModelState.AddModelError("Telefone", "O telefone deve conter 10 ou 11 números.");
            }

            if (string.IsNullOrWhiteSpace(senhaDigitada) || senhaDigitada != confSenha)
            {
                ModelState.AddModelError("Senha", "As senhas não conferem.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Email) && _context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
            }

            if (!ModelState.IsValid)
            {
                usuario.Senha = senhaDigitada;
                return View("CadastroAluno", usuario);
            }

            usuario.Senha = _passwordHasher.HashPassword(usuario, senhaDigitada);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarCoordenador(UsuarioModel usuario, string confirmarSenha)
        {
            var tipoUsuario = HttpContext.Session.GetString("Tipo");

            if (tipoUsuario != "Diretor")
            {
                return RedirectToAction("Index", "Home");
            }

            usuario.Nome = usuario.Nome?.Trim();
            usuario.Email = usuario.Email?.Trim();
            usuario.Telefone = ApenasNumeros(usuario.Telefone);
            usuario.Ra = ApenasNumeros(usuario.Ra);
            usuario.Tipo = "Coordenador";

            ModelState.Remove("Tipo");

            string senhaDigitada = usuario.Senha ?? "";
            string confSenha = confirmarSenha;

            if (string.IsNullOrEmpty(confSenha) && Request.HasFormContentType)
            {
                confSenha = Request.Form["ConfirmarSenha"].ToString();
                if (string.IsNullOrEmpty(confSenha))
                {
                    confSenha = Request.Form["confirmarSenha"].ToString();
                }
            }

            senhaDigitada = senhaDigitada.Trim();
            confSenha = confSenha?.Trim() ?? "";

            ViewBag.Senha = senhaDigitada;
            ViewBag.ConfirmarSenha = confSenha;

            if (string.IsNullOrWhiteSpace(usuario.Ra))
            {
                ModelState.AddModelError("Ra", "O RA é obrigatório.");
            }
            else if (usuario.Ra.Length != 7)
            {
                ModelState.AddModelError("Ra", "O RA deve conter exatamente 7 números.");
            }
            else if (_context.Usuarios.Any(u => u.Ra == usuario.Ra))
            {
                ModelState.AddModelError("Ra", "Este RA já está cadastrado.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Telefone) && usuario.Telefone.Length != 10 && usuario.Telefone.Length != 11)
            {
                ModelState.AddModelError("Telefone", "O telefone deve conter 10 ou 11 números.");
            }

            if (string.IsNullOrWhiteSpace(senhaDigitada) || senhaDigitada != confSenha)
            {
                ModelState.AddModelError("Senha", "As senhas não conferem.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Email) && _context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
            }

            if (!ModelState.IsValid)
            {
                usuario.Senha = senhaDigitada;
                return View("CadastroCoordenador", usuario);
            }

            usuario.Senha = _passwordHasher.HashPassword(usuario, senhaDigitada);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlterarTelefone(string telefone)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId.Value);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var telefoneBruto = ApenasNumeros(telefone);

            if (!string.IsNullOrWhiteSpace(telefoneBruto) && telefoneBruto.Length != 10 && telefoneBruto.Length != 11)
            {
                TempData["Erro"] = "O telefone deve conter 10 ou 11 números.";
                return RedirectToAction("Perfil");
            }

            usuario.Telefone = telefoneBruto;

            _context.SaveChanges();

            TempData["Mensagem"] = "Telefone atualizado com sucesso.";
            return RedirectToAction("Perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlterarSenha(string senhaAtual, string novaSenha, string confirmarSenha)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId.Value);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var resultadoVerificacao = _passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, senhaAtual);
            if (resultadoVerificacao == PasswordVerificationResult.Failed)
            {
                TempData["Erro"] = "Senha atual incorreta.";
                return RedirectToAction("Perfil");
            }

            if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 8)
            {
                TempData["Erro"] = "A nova senha deve ter pelo menos 8 caracteres.";
                return RedirectToAction("Perfil");
            }

            if (novaSenha != confirmarSenha)
            {
                TempData["Erro"] = "A nova senha e a confirmação não coincidem.";
                return RedirectToAction("Perfil");
            }

            usuario.Senha = _passwordHasher.HashPassword(usuario, novaSenha);

            _context.SaveChanges();

            TempData["Mensagem"] = "Senha alterada com sucesso.";
            return RedirectToAction("Perfil");
        }

        private static string? ApenasNumeros(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            var numeros = new string(valor.Where(char.IsDigit).ToArray());

            return string.IsNullOrWhiteSpace(numeros) ? null : numeros;
        }

        private static string FormatarTelefone(string? telefone)
        {
            var numeros = ApenasNumeros(telefone);

            if (string.IsNullOrWhiteSpace(numeros))
            {
                return "Não informado";
            }

            if (numeros.Length == 11)
            {
                return $"({numeros.Substring(0, 2)}) {numeros.Substring(2, 5)}-{numeros.Substring(7, 4)}";
            }

            if (numeros.Length == 10)
            {
                return $"({numeros.Substring(0, 2)}) {numeros.Substring(2, 4)}-{numeros.Substring(6, 4)}";
            }

            return telefone ?? "Não informado";
        }

        private static string FormatarRa(string? ra)
        {
            var numeros = ApenasNumeros(ra);

            if (string.IsNullOrWhiteSpace(numeros))
            {
                return "Não informado";
            }

            if (numeros.Length == 7)
            {
                return $"{numeros.Substring(0, 4)}/{numeros.Substring(4, 2)}-{numeros.Substring(6, 1)}";
            }

            return ra ?? "Não informado";
        }
    }
}