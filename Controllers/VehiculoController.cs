using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InmobiliariaApp.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly VehiculoService _vehiculoService;

        public VehiculoController(VehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        public async Task<IActionResult> Index(string marca, string modelo, int? anoDesde, int? anoHasta, int? estado, int page = 1)
        {
            try
            {
                // Configuración de la cantidad de elementos por página
                int pageSize = 10;

                var result = await _vehiculoService.ObtenerVehiculosFiltradosAsync(marca, modelo, anoDesde, anoHasta, estado, page, pageSize);

                // Pasar los resultados y la información de paginación a la vista
                ViewBag.PaginaActual = page;
                ViewBag.TotalPaginas = result.TotalPaginas;

                return View(result.Vehiculos);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar los vehículos activos: {ex.Message}";
                return View(); // Vista vacía con error
            }
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id.Value);
                if (vehiculo == null) return NotFound();
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al obtener los detalles del vehículo: {ex.Message}";
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Por favor complete correctamente todos los campos.";
                    return View(vehiculo);
                }

                await _vehiculoService.CreateVehiculoAsync(vehiculo);
                TempData["SuccessMessage"] = "Vehículo creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error al crear el vehículo: {ex.Message}";
                return View(vehiculo);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id.Value);
                if (vehiculo == null) return NotFound();
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar el formulario de edición: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.Id) return NotFound();

            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Por favor revise los datos ingresados.";
                    return View(vehiculo);
                }

                await _vehiculoService.UpdateVehiculoAsync(vehiculo);
                TempData["SuccessMessage"] = "Vehículo actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error al actualizar el vehículo: {ex.Message}";
                return View(vehiculo);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id.Value);
                if (vehiculo == null) return NotFound();
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar la confirmación de eliminación: {ex.Message}";
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _vehiculoService.DeleteVehiculoAsync(id);
                TempData["SuccessMessage"] = "Vehículo eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error al eliminar el vehículo: {ex.Message}";
                var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id);
                return View("Delete", vehiculo);
            }
        }
    }
}
