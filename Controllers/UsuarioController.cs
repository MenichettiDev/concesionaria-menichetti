using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ConcesionariaApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
                await _usuarioRepository.CreateUsuarioAsync(usuario);
                TempData["SuccessMessage"] = "Usuario creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error al crear el usuario: {ex.Message}";
                return View(usuario);
            }
        }

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
    }
}
