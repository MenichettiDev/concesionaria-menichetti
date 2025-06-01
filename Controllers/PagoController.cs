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
        private readonly ContratoSuscripcionRepository _contratoSuscripcionRepository;
        private readonly ContratoPlanesRepository _contratoPlanesRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ConcesionariaRepository _concesionariaRepository;
        private readonly AccesosPagadoRepository _accesosRepository;

        public PagosController(PagoRepository pagosRepository, DestacadoRepository destacadosRepository, VehiculoRepository vehiculoRepository, ContratoPlanesRepository contratoPlanesRepository, ContratoSuscripcionRepository contratoSuscripcionRepository, UsuarioRepository usuarioRepository, ConcesionariaRepository concesionariaRepository, AccesosPagadoRepository accesosRepository)

        {
            _vehiculoRepository = vehiculoRepository;
            _pagosRepository = pagosRepository;
            _destacadosRepository = destacadosRepository;
            _contratoSuscripcionRepository = contratoSuscripcionRepository;
            _contratoPlanesRepository = contratoPlanesRepository;
            _usuarioRepository = usuarioRepository;
            _concesionariaRepository = concesionariaRepository;
            _accesosRepository = accesosRepository;
        }

        // GET Create, el id puede ser de suscripcion, vehiculo o plan
        [HttpGet]
        public IActionResult Create(string tipo, int? id, decimal? monto)
        {
            if (string.IsNullOrEmpty(tipo) || monto == null)
            {
                ViewBag.ErrorMessage = "Parámetros inválidos para crear el pago.";
                return View();
            }

            var detalle = tipo switch
            {
                "Destacado" => $"Destacar vehículo ID {id}",
                "Suscripcion" => $"Pago de suscripción ID {id}",
                "Plan" => $"Contratación de plan ID {id}",
                "Acceso" => $"Informacion vehiculo {id}",
                _ => "Pago genérico"
            };

            var model = new Pago
            {
                Tipo = tipo,
                Monto = monto.Value,
                Detalle = detalle
            };

            ViewBag.Id = id;


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

                if (model.Tipo == "Destacado")
                {
                    // Console.WriteLine("vehiculoId: " + Id);
                    var VehiculoId = Id.Value;

                    await _destacadosRepository.MarcarVehiculoComoDestacadoAsync(VehiculoId);
                    await _vehiculoRepository.ActualizarDestacadoAsync(VehiculoId);
                    TempData["SuccessMessage"] = "Pago registrado y vehiculo destacado correctamente.";
                }
                else if (model.Tipo == "Suscripcion")
                {

                    var idSuscripcion = Id.Value;
                    int idUser = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
                    await _contratoSuscripcionRepository.CargarContratoSuscripcion(idSuscripcion, idUser);

                    TempData["SuccessMessage"] = "Pago de suscripcion registrado correctamente.";
                }
                else if (model.Tipo == "Plan")
                {
                    var idPlan = Id.Value;
                    int idUser = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
                    var Concesionaria = await _concesionariaRepository.ObtenerPorUsuarioIdAsync(idUser);

                    await _contratoPlanesRepository.CargarContratoPlan(idPlan, Concesionaria.Id);
                    TempData["SuccessMessage"] = "Pago de plan registrado correctamente.";
                }
                else if (model.Tipo == "Acceso")
                {
                    var idVehiculo = Id.Value;
                    int idUser = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

                    await _accesosRepository.CrearAccesoAsync(idUser, idVehiculo);
                    TempData["SuccessMessage"] = "Pago de acceso registrado correctamente.";
                    return RedirectToAction("Details", "Home", new { id = idVehiculo });
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
