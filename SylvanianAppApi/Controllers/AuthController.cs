using Microsoft.AspNetCore.Mvc;
using SylvanianAppApi.DTOs;
using SylvanianAppApi.Models;
using SylvanianAppApi.Data;

namespace SylvanianAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDTO dto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == dto.Email && u.Senha == dto.Senha && u.Ativo);
            if (usuario == null)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            return Ok(new { mensagem = "Login realizado com sucesso!", usuario = usuario.Nome });
        }

        [HttpPost("CriarConta")]
        public IActionResult CriarConta(CriarContaDTO dto)
        {
            if (_context.Usuarios.Any(u => u.Email == dto.Email))
            {
                return BadRequest("Já existe um usuário com esse e-mail.");
            }

            var novoUsuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                Ativo = true
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            return Ok(new { mensagem = "Conta criada com sucesso!" });
        }

        [HttpPut("inativar/{id}")]
        public IActionResult InativarConta(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null || !usuario.Ativo)
                return NotFound("Usuário não encontrado ou já inativo.");

            usuario.Ativo = false;
            _context.SaveChanges();

            return Ok("Usuário inativado com sucesso.");
        }

        [HttpPost("RecuperarSenha")]
        public IActionResult RecuperarSenha([FromBody] RecuperarSenhaDTO dto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == dto.Email && u.Ativo);
            if (usuario == null)
                return NotFound("Usuário não encontrado ou inativo.");

            // Simula o envio de e-mail
            return Ok("Se o e-mail estiver correto, uma mensagem foi enviada com instruções.");
        }

        [HttpGet("Ativos")]
        public IActionResult ListarUsuariosAtivos()
        {
            var usuarios = _context.Usuarios
                .Where(u => u.Ativo)
                .Select(u => new { u.Id, u.Nome, u.Email })
                .ToList();

            return Ok(usuarios);
        }

    }
}
