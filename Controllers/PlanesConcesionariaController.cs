using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConcesionariaApp.Controllers
{
    public class PlanesConcesionariaController : Controller
    {
        private readonly PlanesConcesionariaRepository _planesRepository;

        public PlanesConcesionariaController(PlanesConcesionariaRepository planesRepository)
        {
            _planesRepository = planesRepository;
        }

        // INDEX
        public async Task<IActionResult> Index(string nombre, int page = 1)
        {
            try
            {
                int pageSize = 10;

                var result = await _planesRepository.ObtenerPlanesFiltradosAsync(nombre, page, pageSize);

                ViewBag.PaginaActual = page;
                ViewBag.TotalPaginas = result.TotalPaginas;

                return View(result.Planes);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar los planes: {ex.Message}";
                return View();
            }
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var plan = await _planesRepository.GetByIdAsync(id.Value);
                if (plan == null) return NotFound();
                return View(plan);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al obtener los detalles del plan: {ex.Message}";
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
        public async Task<IActionResult> Create(PlanesConcesionarium plan)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Por favor revise los datos ingresados.";
                    return View(plan);
                }

                await _planesRepository.CreatePlanAsync(plan);
                TempData["SuccessMessage"] = "Plan creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al crear el plan: {ex.Message}";
                return View(plan);
            }
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var plan = await _planesRepository.GetPlanByIdAsync(id.Value);
                if (plan == null) return NotFound();
                return View(plan);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar el formulario de edición: {ex.Message}";
                return View();
            }
        }

        // EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PlanesConcesionarium plan)
        {
            if (id != plan.Id) return NotFound();

            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Por favor revise los datos ingresados.";
                    return View(plan);
                }

                await _planesRepository.UpdatePlanAsync(plan);
                TempData["SuccessMessage"] = "Plan actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al actualizar el plan: {ex.Message}";
                return View(plan);
            }
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var plan = await _planesRepository.GetPlanByIdAsync(id.Value);
                if (plan == null) return NotFound();
                return View(plan);
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
                await _planesRepository.DeletePlanAsync(id);
                TempData["SuccessMessage"] = "Plan eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el plan: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Contratar(string nombre, int page = 1)
        {
            try
            {
                int pageSize = 10;

                var result = await _planesRepository.ObtenerPlanesFiltradosAsync(nombre, page, pageSize);

                ViewBag.PaginaActual = page;
                ViewBag.TotalPaginas = result.TotalPaginas;

                return View(result.Planes);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar los planes: {ex.Message}";
                return View();
            }
        }

    }
}
