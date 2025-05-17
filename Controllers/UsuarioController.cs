using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ConcesionariaApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ConcesionariaRepository _concesionariaRepository;
        private readonly IConfiguration _configuration;

        public UsuarioController(UsuarioRepository usuarioRepository, IConfiguration config, ConcesionariaRepository concesionariaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = config;
            _concesionariaRepository = concesionariaRepository;
        }

        public async Task<IActionResult> Index(string? nombre, bool? esConcesionaria, bool? activo, int page = 1)
        {
            try
            {
                int pageSize = 10;

                var result = await _usuarioRepository.ObtenerUsuariosFiltradosAsync(nombre, esConcesionaria, activo, page, pageSize);

                ViewBag.PaginaActual = page;
                ViewBag.TotalPaginas = result.TotalPaginas;

                // Guardamos los filtros para mantenerlos en la vista
                ViewBag.Filtros = new
                {
                    nombre,
                    esConcesionaria,
                    activo
                };

                return View(result.Usuarios);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar los usuarios: {ex.Message}";
                return View(new List<Usuario>());
            }
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id.Value);
                if (usuario == null) return NotFound();
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al obtener los detalles del usuario: {ex.Message}";
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            try
            {
                // Validar el modelo
                // if (!ModelState.IsValid)
                // {
                //     return View(usuario);
                // }

                // Validar que la contraseña no sea nula o vacía
                if (string.IsNullOrWhiteSpace(usuario.Contraseña))
                {
                    ModelState.AddModelError("Password", "La contraseña es obligatoria.");
                    return View(usuario);
                }

                // Salt fijo (a modo de aprendizaje)
                string salt = _configuration["Salt"]; // Lee el salt del archivo de configuración
                if (string.IsNullOrEmpty(salt))
                {
                    throw new InvalidOperationException("El valor de 'Salt' no está configurado en appsettings.json.");
                }

                // Hashear la contraseña usando el salt fijo
                string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: usuario.Contraseña,
                    salt: Encoding.ASCII.GetBytes(salt),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                // Asignar la contraseña hasheada al usuario
                usuario.Contraseña = hashedPassword;

                // Guardar el usuario en la base de datos
                await _usuarioRepository.CreateUsuarioAsync(usuario);
                TempData["SuccessMessage"] = "Usuario cargado correctamente.";

                // Redirigir a la lista de usuarios
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Loggear el error
                // _logger.LogError(ex, "Error al insertar un usuario.");
                ViewBag.MensajeError = "Ocurrió un error inesperado. Por favor, inténtalo más tarde." + ex.Message;
                return View(usuario);
            }
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(IFormCollection form)
        {
            try
            {
                var nuevoUsuario = new Usuario
                {
                    Nombre = form["Nombre"],
                    Email = form["Email"],
                    Contraseña = form["Contraseña"],
                    Rol = "Usuario",
                    Telefono = form["Telefono"],
                    EsConcesionaria = bool.TryParse(form["EsConcesionaria"], out var esConce) && esConce,
                    Verificado = false,
                    Activo = true
                };

                // Hashear contraseña como hacés en el Create
                string salt = _configuration["Salt"];
                nuevoUsuario.Contraseña = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: nuevoUsuario.Contraseña,
                    salt: Encoding.ASCII.GetBytes(salt),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                await _usuarioRepository.CreateUsuarioAsync(nuevoUsuario);

                if (nuevoUsuario.EsConcesionaria == true)
                {
                    var concesionaria = new Concesionaria
                    {
                        UsuarioId = nuevoUsuario.Id, // después de persistir se asigna el ID
                        Nombre = form["NombreConcesionaria"],
                        Cuit = form["Cuit"],
                        Direccion = form["Direccion"]
                    };

                    await _concesionariaRepository.AddAsync(concesionaria);
                }

                TempData["SuccessMessage"] = "Usuario registrado correctamente. Ahora podes iniciar sesion.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = "Ocurrió un error: " + ex.Message;
                return View("Registro");
            }
        }


        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id.Value);
                if (usuario == null) return NotFound();
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar el formulario de edición: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return NotFound();

            try
            {

                await _usuarioRepository.UpdateUsuarioAsync(usuario);
                TempData["SuccessMessage"] = "Usuario actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al actualizar el usuario: {ex}";
                return View(usuario);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id.Value);
                if (usuario == null) return NotFound();
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar la confirmación de eliminación: {ex.Message}";
                return View();
            }
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _usuarioRepository.DeleteUsuarioAsync(id);
                TempData["SuccessMessage"] = "Usuario eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al eliminar el usuario: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// // Acción para mostrar el formulario de login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // Acción para procesar el formulario de login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nombreUsuario, string contrasenia)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasenia))
            {
                ViewBag.MensajeError = "Nombre de usuario y contraseña son obligatorios.";
                return View();
            }
            try
            {
                // Buscar el usuario por email o nombre de usuario
                var usuario = await _usuarioRepository.GetByEmailAsync(nombreUsuario);

                // Hashear la contraseña que viene del form para compararla
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: contrasenia,
                    salt: Encoding.ASCII.GetBytes(_configuration["Salt"]), // importante: debe ser el mismo salt usado al guardar la contraseña
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                if (usuario == null || usuario.Contraseña != hashed)
                {
                    ViewBag.MensajeError = "Usuario o contraseña incorrectos.";
                    return View();
                }

                // Crear los claims (información que se guarda en la cookie)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    // new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                    new Claim(ClaimTypes.Role, usuario.Rol), // importante si vas a usar roles
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())

                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = "Error interno: " + ex.Message;
                return View();
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }
    }
}
