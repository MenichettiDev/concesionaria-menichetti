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
        private readonly VehiculoRepository _vehiculoRepository;

        public PagosController(PagoRepository pagosRepository, DestacadoRepository destacadosRepository, VehiculoRepository vehiculoRepository)

        {
            _vehiculoRepository = vehiculoRepository;
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

        // POST Create //en id llega el id de suscripcion, de vehiculo o de plan 
        [HttpPost]
        public async Task<IActionResult> Create(Pago model, int? Id)
        {
            //obtengo id de usuario
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

                // Console.WriteLine("vehiculoId: " + Id);
                if (model.Tipo == "Destacado" && Id.HasValue)
                {
                    var VehiculoId = Id.Value;

                    await _destacadosRepository.MarcarVehiculoComoDestacadoAsync(VehiculoId);
                    await _vehiculoRepository.ActualizarDestacadoAsync(VehiculoId);
                    TempData["SuccessMessage"] = "Pago registrado y vehiculo destacado correctamente.";
                }
                else if (model.Tipo == "Suscripcion")
                {
                    TempData["SuccessMessage"] = "Pago de suscripción registrado correctamente.";
                }
                else if (model.Tipo == "Plan")
                {
                    TempData["SuccessMessage"] = "Pago de plan registrado correctamente.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Pago registrado correctamente.";
                }


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
