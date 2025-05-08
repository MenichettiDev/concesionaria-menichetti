using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InmobiliariaApp.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly VehiculoService _vehiculoService;

        public VehiculoController(VehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;  // Usamos el servicio en lugar del DbContext directamente
        }

        // Acción para mostrar los vehículos activos en una vista
        public async Task<IActionResult> Index()
        {
            var vehiculosActivos = await _vehiculoService.ObtenerVehiculosActivosAsync();
            return View(vehiculosActivos);  // Pasa los vehículos a la vista
        }

        // Acción para mostrar los detalles de un vehículo
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id.Value);  // Usamos el servicio para obtener el vehículo
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción para procesar la creación del vehículo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbVehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                await _vehiculoService.CreateVehiculoAsync(vehiculo);  // Usamos el servicio para agregar el vehículo
                return RedirectToAction(nameof(Index));  // Redirige al listado de vehículos
            }
            return View(vehiculo);
        }

        // Acción para mostrar el formulario de edición
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id.Value);  // Usamos el servicio para obtener el vehículo
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // Acción para procesar la edición del vehículo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbVehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _vehiculoService.UpdateVehiculoAsync(vehiculo);  // Usamos el servicio para actualizar el vehículo
                return RedirectToAction(nameof(Index));  // Redirige al listado de vehículos
            }
            return View(vehiculo);
        }

        // Acción para mostrar la confirmación de eliminación
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id.Value);  // Usamos el servicio para obtener el vehículo
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // Acción para procesar la eliminación
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehiculoService.DeleteVehiculoAsync(id);  // Usamos el servicio para eliminar el vehículo
            return RedirectToAction(nameof(Index));  // Redirige al listado de vehículos
        }
    }
}
