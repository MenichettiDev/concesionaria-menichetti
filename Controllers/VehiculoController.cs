using concesionaria_menichetti.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConcesionariaApp.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly VehiculoRepository _vehiculoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly SuscripcionesRepository _suscripcionesRepository;
        private readonly PlanesConcesionariaRepository _planesRepository;
        private readonly ConcesionariaRepository _concesionariaRepository;
        private readonly ILogger<VehiculoController> _logger;

        public VehiculoController(ILogger<VehiculoController> logger, VehiculoRepository vehiculoRepository, UsuarioRepository usuarioRepository, SuscripcionesRepository suscripcionesRepository, PlanesConcesionariaRepository contratoPlanesRepository, ConcesionariaRepository concesionariaRepository)
        {
            _logger = logger;
            _vehiculoRepository = vehiculoRepository;
            _usuarioRepository = usuarioRepository;
            _suscripcionesRepository = suscripcionesRepository;
            _planesRepository = contratoPlanesRepository;
            _concesionariaRepository = concesionariaRepository;

        }

        public async Task<IActionResult> Index(int? idMarca, int? idModelo, int? anoDesde, int? anoHasta, int? estado = 1, int page = 1)
        {
            try
            {
                int pageSize = 10;
                int idUser = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(idUser); // o similar


                var result = await _vehiculoRepository.ObtenerVehiculosFiltradosAsync(idUser, idMarca, idModelo, anoDesde, anoHasta, estado, page, pageSize);

                // Obtenemos las marcas y modelos únicos
                var marcas = await _vehiculoRepository.GetMarcasAsync(); // Método para obtener las marcas
                var modelos = await _vehiculoRepository.GetModelosAsync(); // Método para obtener los modelos

                ViewBag.EsConcesionaria = usuario?.EsConcesionaria ?? false;

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

                //publicaciones restantes
                if (usuario?.EsConcesionaria == true)
                {
                    var concesionaria = await _concesionariaRepository.ObtenerPorUsuarioIdAsync(idUser); // o similar
                    Console.WriteLine($"idConcesionaria: {concesionaria?.Id}");
                    ViewBag.PublicacionesRestantes = await _planesRepository.ObtenerPublicacionesRestantesConcesionariaAsync(concesionaria.Id);
                }
                else
                {

                    ViewBag.PublicacionesRestantes = await _suscripcionesRepository.ObtenerPublicacionesRestantesAsync(idUser);
                }


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
                var vehiculo = await _vehiculoRepository.GetVehiculoByIdAsyncConFoto(id.Value);
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
        public async Task<IActionResult> Create([FromForm] Vehiculo vehiculo, List<IFormFile> Imagenes)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int idUser = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(idUser); // o similar
                var concesionaria = await _concesionariaRepository.ObtenerPorUsuarioIdAsync(idUser); // o similar

                if (userIdClaim == null) return Unauthorized();

                int usuarioId = int.Parse(userIdClaim.Value);

                // Asignar el ID del usuario autenticado
                vehiculo.UsuarioId = usuarioId;

                //Controlamos las suscripciones y disponibles
                if (usuario?.EsConcesionaria == true)
                {

                    if (!await _planesRepository.ConcesionariaPuedePublicarAsync(concesionaria.Id))
                    {
                        return BadRequest("Ya alcanzaste el límite de publicaciones según tu plan.");
                    }
                }
                else
                {

                    if (!await _suscripcionesRepository.UsuarioPuedePublicarAsync(usuarioId))
                    {
                        return BadRequest("Ya alcanzaste el límite de publicaciones según tu suscripción.");
                    }
                }


                await _vehiculoRepository.CreateVehiculoConFotosAsync(vehiculo, Imagenes);

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
                var vehiculo = await _vehiculoRepository.GetVehiculoByIdAsyncConFoto(id.Value);
                if (vehiculo == null) return NotFound();

                // También podrías cargar ViewBag.Marcas y Modelos si luego los usás en Vue para alguna edición dinámica
                var marcas = await _vehiculoRepository.GetMarcasAsync();
                var modelos = await _vehiculoRepository.GetModelosAsync();

                ViewBag.Marcas = marcas.Select(m => new { id = m.Id, descripcion = m.Descripcion }).ToList();
                ViewBag.Modelos = modelos.Select(m => new { id = m.Id, descripcion = m.Descripcion, idMarca = m.IdMarca }).ToList();

                return View(vehiculo);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar el formulario de edición: {ex.Message}";
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Vehiculo vehiculo, List<IFormFile>? Imagenes, [FromForm] List<int>? FotosAEliminar)
        {

            if (id != vehiculo.Id)
            {
                return NotFound();
            }


            if (Imagenes != null && Imagenes.Any())
            {
            }

            if (FotosAEliminar != null && FotosAEliminar.Any())
            {
            }

            try
            {
                await _vehiculoRepository.UpdateVehiculoConFotosAsync(vehiculo, Imagenes, FotosAEliminar);
                TempData["SuccessMessage"] = "Vehículo actualizado correctamente.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error al actualizar el vehículo: {ex.Message}";
                return View(vehiculo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Destacar(int id)
        {
            var vehiculo = await _vehiculoRepository.GetVehiculoByIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            try
            {
                var actualizado = await _vehiculoRepository.ActualizarDestacadoAsync(id);
                if (actualizado)
                    return Json(new { success = true, message = "Vehículo destacado correctamente." });
                else
                    return Json(new { success = false, message = "No se pudo actualizar el vehículo." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al destacar el vehículo: {ex.Message}" });
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
