using Microsoft.AspNetCore.Mvc;
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

        // Acción para mostrar los vehículos activos en una vista
        public async Task<IActionResult> Index()
        {
            var vehiculosActivos = await _vehiculoService.ObtenerVehiculosActivosAsync();
            return View(vehiculosActivos);  // Pasa los vehículos a la vista
        }
    }
}