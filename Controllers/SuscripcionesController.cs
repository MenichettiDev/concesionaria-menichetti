using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConcesionariaApp.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly SuscripcionesRepository _suscripcionesRepository;

        public SuscripcionesController(SuscripcionesRepository suscripcionesRepository)
        {
            _suscripcionesRepository = suscripcionesRepository;
        }

        // INDEX
        public async Task<IActionResult> Index(string nombre, int page = 1)
        {
            try
            {
                int pageSize = 10;

                var result = await _suscripcionesRepository.ObtenerSuscripcionesFiltradosAsync(nombre, page, pageSize);

                ViewBag.PaginaActual = page;
                ViewBag.TotalPaginas = result.TotalPaginas;

                return View(result.Suscripciones);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar los Suscripciones: {ex.Message}";
                return View();
            }
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var Suscripcion = await _suscripcionesRepository.GetByIdAsync(id.Value);
                if (Suscripcion == null) return NotFound();
                return View(Suscripcion);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al obtener los detalles del Suscripcion: {ex.Message}";
                return View();
            }
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Suscripcione Suscripcion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Por favor revise los datos ingresados.";
                    return View(Suscripcion);
                }

                await _suscripcionesRepository.CreateSuscripcionAsync(Suscripcion);
                TempData["SuccessMessage"] = "Suscripcion creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al crear el Suscripcion: {ex.Message}";
                return View(Suscripcion);
            }
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var Suscripcion = await _suscripcionesRepository.GetSuscripcionByIdAsync(id.Value);
                if (Suscripcion == null) return NotFound();
                return View(Suscripcion);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar el formulario de edición: {ex.Message}";
                return View();
            }
        }

        // EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Suscripcione Suscripcion)
        {
            if (id != Suscripcion.Id) return NotFound();

            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Por favor revise los datos ingresados.";
                    return View(Suscripcion);
                }

                await _suscripcionesRepository.UpdateSuscripcionAsync(Suscripcion);
                TempData["SuccessMessage"] = "Suscripcion actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al actualizar el Suscripcion: {ex.Message}";
                return View(Suscripcion);
            }
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var Suscripcion = await _suscripcionesRepository.GetSuscripcionByIdAsync(id.Value);
                if (Suscripcion == null) return NotFound();
                return View(Suscripcion);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar la confirmación de eliminación: {ex.Message}";
                return View();
            }
        }

        // DELETE (POST)
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _suscripcionesRepository.DeleteSuscripcionAsync(id);
                TempData["SuccessMessage"] = "Suscripcion eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el Suscripcion: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Contratar(string nombre, int page = 1)
        {
            try
            {
                int pageSize = 10;

                var result = await _suscripcionesRepository.ObtenerSuscripcionesFiltradosAsync(nombre, page, pageSize);

                ViewBag.PaginaActual = page;
                ViewBag.TotalPaginas = result.TotalPaginas;

                return View(result.Suscripciones);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar los Suscripciones: {ex.Message}";
                return View();
            }
        }
    }
}
