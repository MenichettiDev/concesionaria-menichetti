using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InmobiliariaApp.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly VehiculoRepository _vehiculoRepository;

        public VehiculoController(VehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task<IActionResult> Index(int? idMarca, int? idModelo, int? anoDesde, int? anoHasta, int? estado, int page = 1)
        {
            try
            {
                int pageSize = 10;

                var result = await _vehiculoRepository.ObtenerVehiculosFiltradosAsync(idMarca, idModelo, anoDesde, anoHasta, estado, page, pageSize);

                // Obtenemos las marcas y modelos únicos
                var marcas = await _vehiculoRepository.GetMarcasAsync(); // Método para obtener las marcas
                var modelos = await _vehiculoRepository.GetModelosAsync(); // Método para obtener los modelos


                //pasamos la data a la vista
                ViewBag.Marcas = marcas.Select(m => new
                {
                    id = m.Id,
                    descripcion = m.Descripcion
                }).ToList();

                ViewBag.Modelos = modelos.Select(m => new
                {
                    id = m.Id,
                    descripcion = m.Descripcion,
                    idMarca = m.IdMarca
                }).ToList();

                ViewBag.PaginaActual = page;
                ViewBag.TotalPaginas = result.TotalPaginas;

                return View(result.Vehiculos);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar los vehiculos: {ex.Message}";
                return View();
            }
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var vehiculo = await _vehiculoRepository.GetVehiculoByIdAsync(id.Value);
                if (vehiculo == null) return NotFound();
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al obtener los detalles del vehiculo: {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> Create()
        {

            // Obtenemos las marcas y modelos únicos
            var marcas = await _vehiculoRepository.GetMarcasAsync(); // Método para obtener las marcas
            var modelos = await _vehiculoRepository.GetModelosAsync(); // Método para obtener los modelos


            //pasamos la data a la vista
            ViewBag.Marcas = marcas.Select(m => new
            {
                id = m.Id,
                descripcion = m.Descripcion
            }).ToList();

            ViewBag.Modelos = modelos.Select(m => new
            {
                id = m.Id,
                descripcion = m.Descripcion,
                idMarca = m.IdMarca
            }).ToList();


            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {
            try
            {

                await _vehiculoRepository.CreateVehiculoAsync(vehiculo);
                TempData["SuccessMessage"] = "Vehiculo creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrio un error al crear el vehiculo: {ex.Message}";
                return View(vehiculo);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var vehiculo = await _vehiculoRepository.GetVehiculoByIdAsync(id.Value);
                if (vehiculo == null) return NotFound();
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar el formulario de edicion: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
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

                vehiculo.UsuarioId = 1; //TODO: modificar cuando aplique login
                vehiculo.Estado = 1;
                await _vehiculoRepository.UpdateVehiculoAsync(vehiculo);
                TempData["SuccessMessage"] = "Vehiculo actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrio un error al actualizar el vehiculo: {ex.Message}";
                return View(vehiculo);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var vehiculo = await _vehiculoRepository.GetVehiculoByIdAsync(id.Value);
                if (vehiculo == null) return NotFound();
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar la confirmacion de eliminacion: {ex.Message}";
                return View();
            }
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _vehiculoRepository.BajaLogicaVehiculoAsync(id);
                TempData["SuccessMessage"] = "Vehiculo eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrio un error al eliminar el vehiculo: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
