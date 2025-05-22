using concesionaria_menichetti.Models;
using concesionaria_menichetti.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ConcesionariaApp.Controllers;

public class FotosVehiculoController : Controller
{
    private readonly FotosVehiculoRepository _fotosVehiculoRepository;
    private readonly IWebHostEnvironment _env;

    public FotosVehiculoController(FotosVehiculoRepository repo, IWebHostEnvironment env)
    {
        _fotosVehiculoRepository = repo;
        _env = env;
    }

    public async Task<IActionResult> PorVehiculo(int id)
    {
        var fotos = await _fotosVehiculoRepository.BuscarPorVehiculoAsync(id);
        ViewBag.VehiculoId = id;
        return View(fotos);
    }

    public IActionResult Subir(int id)
    {
        ViewBag.VehiculoId = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Subir(int id, List<IFormFile> imagenes)
    {
        try
        {
            if (imagenes == null || imagenes.Count == 0)
                return BadRequest("No se recibieron archivos.");

            var rutaBase = Path.Combine(_env.WebRootPath, "Uploads", "Vehiculos", id.ToString());

            if (!Directory.Exists(rutaBase))
                Directory.CreateDirectory(rutaBase);

            foreach (var img in imagenes)
            {
                if (img.Length > 0)
                {
                    var ext = Path.GetExtension(img.FileName);
                    var nombreArchivo = $"{Guid.NewGuid()}{ext}";
                    var rutaArchivo = Path.Combine(rutaBase, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var nuevaFoto = new FotosVehiculo
                    {
                        VehiculoId = id,
                        FotoArchivo = $"/Uploads/Vehiculos/{id}/{nombreArchivo}",
                        Fecha = DateTime.Now
                    };

                    await _fotosVehiculoRepository.AltaAsync(nuevaFoto);
                }
            }

            return RedirectToAction("PorVehiculo", new { id });
        }
        catch (Exception ex)
        {
            // Logueá si usás logger: _logger.LogError(ex, "Error al subir imágenes");
            return StatusCode(500, "Error interno al procesar la subida: " + ex.Message);
        }
    }


    public async Task<IActionResult> Eliminar(int id)
    {
        var foto = await _fotosVehiculoRepository.ObtenerPorIdAsync(id);
        if (foto == null)
            return NotFound();

        return View(foto);
    }

    [HttpPost, ActionName("Eliminar")]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        try
        {
            var foto = await _fotosVehiculoRepository.ObtenerPorIdAsync(id);
            if (foto == null) return NotFound();

            var rutaFisica = Path.Combine(_env.WebRootPath, foto.FotoArchivo.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (System.IO.File.Exists(rutaFisica))
            {
                System.IO.File.Delete(rutaFisica);
            }

            await _fotosVehiculoRepository.BorrarAsync(id);

            return RedirectToAction("PorVehiculo", new { id = foto.VehiculoId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error interno al eliminar la imagen: " + ex.Message);
        }
    }

}
