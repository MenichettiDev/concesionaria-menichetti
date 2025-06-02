using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace concesionaria_menichetti.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly UsuarioRepository usuarioRepo;

        public UsuarioController(IConfiguration config, UsuarioRepository usuarioRepo)
        {
            this.config = config;
            this.usuarioRepo = usuarioRepo;
        }

        [HttpPost("login2")]
        [AllowAnonymous]
        public async Task<IActionResult> Login2([FromBody] LoginDTO login)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: login.Contrasenia,
                    salt: Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                var usuario = await usuarioRepo.GetByEmailAsync(login.NombreUsuario);
                if (usuario == null || usuario.Contraseña != hashed)
                    return Unauthorized("Usuario o contraseña incorrectos");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: config["TokenAuthentication:Issuer"],
                    audience: config["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = tokenString, expiration = token.ValidTo });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class LoginDTO
    {
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
    }
}
