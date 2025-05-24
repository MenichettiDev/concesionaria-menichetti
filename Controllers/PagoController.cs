using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace concesionaria_menichetti.Controllers
{
    public class PagosController : Controller
    {
        private readonly PagoRepository _pagosRepository;
        private readonly DestacadoRepository _destacadosRepository;

        public PagosController(PagoRepository pagosRepository, DestacadoRepository destacadosRepository)
        {
            _pagosRepository = pagosRepository;
            _destacadosRepository = destacadosRepository;
        }

        // GET Create
        [HttpGet]
        public IActionResult Create(string tipo, int? vehiculoId, decimal? monto)
        {
            if (string.IsNullOrEmpty(tipo) || monto == null)
            {
                ViewBag.ErrorMessage = "Parámetros inválidos para crear el pago.";
                return View();
            }

            var detalle = tipo switch
            {
                "Destacado" => $"Destacar vehículo ID {vehiculoId}",
                "Suscripcion" => "Pago de suscripción",
                "Plan" => "Contratación de plan",
                _ => "Pago genérico"
            };

            var model = new Pago
            {
                Tipo = tipo,
                Monto = monto.Value,
                Detalle = detalle
            };

            ViewBag.vehiculoId = vehiculoId;


            return View(model);
        }

        // POST Create
        [HttpPost]
        public async Task<IActionResult> Create(Pago model, int? vehiculoId)
        {

            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                ViewBag.ErrorMessage = "No se pudo obtener el ID del usuario.";
                return View(model);
            }

            try
            {
                var pago = new Pago
                {
                    Tipo = model.Tipo,
                    Monto = model.Monto,
                    Fecha = DateTime.Now,
                    Detalle = model.Detalle,
                    UsuarioId = userId
                };

                await _pagosRepository.CreatePagoAsync(pago);

                // Console.WriteLine("vehiculoId: " + vehiculoId);
                if (model.Tipo == "Destacado" && vehiculoId.HasValue)
                {
                    var VehiculoId = vehiculoId.Value;

                    await _destacadosRepository.MarcarVehiculoComoDestacadoAsync(VehiculoId);
                }


                TempData["SuccessMessage"] = "Pago registrado correctamente.";
                return RedirectToAction("Index", "Vehiculo");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al registrar el pago: {ex.InnerException}";
                return View(model);
            }
        }
    }
}
