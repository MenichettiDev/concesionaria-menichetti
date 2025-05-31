using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VehiculoRepository
{
    private readonly ConcesionariaContext _context;
    private readonly ILogger<VehiculoRepository> _logger;

    public VehiculoRepository(ConcesionariaContext context, ILogger<VehiculoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<Vehiculo> GetQueryable()
    {
        return _context.Vehiculos.AsQueryable();
    }

    public async Task<(List<Vehiculo> Vehiculos, int TotalPaginas)> ObtenerVehiculosFiltradosAsync(
    int idUsuario, int? idMarca, int? idModelo, int? anoDesde, int? anoHasta, int? estado, int page, int pageSize)
    {
        var query = GetQueryable();

        query = query
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca)
            .Include(v => v.Destacados);

        query = query.Where(v => v.UsuarioId == idUsuario);

        if (idMarca.HasValue)
        {
            query = query.Where(v => v.Modelo.IdMarca == idMarca.Value);
        }

        if (idModelo.HasValue)
        {
            query = query.Where(v => v.IdModelo == idModelo.Value);
        }

        if (estado.HasValue)
        {
            query = query.Where(v => v.Estado == estado.Value);
        }

        if (anoDesde.HasValue)
        {
            query = query.Where(v => v.Anio >= anoDesde.Value);
        }

        if (anoHasta.HasValue)
        {
            query = query.Where(v => v.Anio <= anoHasta.Value);
        }

        var totalVehiculos = await query.CountAsync();

        var vehiculos = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();



        int totalPaginas = (int)Math.Ceiling(totalVehiculos / (double)pageSize);

        return (vehiculos, totalPaginas);
    }


    public async Task<IEnumerable<Vehiculo>> ObtenerVehiculosActivosAsync()
    {
        try
        {
            return await _context.Vehiculos
                .Where(v => v.Estado == 1)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener vehículos activos", ex);
        }
    }

    public async Task<Vehiculo> GetVehiculoByIdAsync(int id)
    {
        try
        {
            return await _context.Vehiculos
                .Include(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el vehículo con ID {id}", ex);
        }
    }

    public async Task<Vehiculo> GetVehiculoByIdAsyncConFoto(int id)
    {
        try
        {


            return await _context.Vehiculos
                .Include(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
                .Include(v => v.FotosVehiculos)
                .Include(v => v.Usuario)
                .Include(v => v.AccesosPagados)
                .FirstOrDefaultAsync(v => v.Id == id);


        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el vehículo con ID {id}", ex);
        }
    }


    public async Task CreateVehiculoAsync(Vehiculo vehiculo)
    {
        try
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el vehículo", ex);
        }
    }

    public async Task CreateVehiculoConFotosAsync(Vehiculo vehiculo, List<IFormFile> imagenes)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        var rutasImagenesGuardadas = new List<string>();

        try
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            if (imagenes != null && imagenes.Count > 0)
            {
                var uploadsPath = Path.Combine("wwwroot", "uploads");

                // Crear la carpeta si no existe
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }


                foreach (var archivo in imagenes)
                {
                    if (archivo.Length > 0)
                    {
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                        var ruta = Path.Combine(uploadsPath, nombreArchivo);

                        using (var stream = new FileStream(ruta, FileMode.Create))
                        {
                            await archivo.CopyToAsync(stream);
                        }

                        rutasImagenesGuardadas.Add(ruta);

                        var foto = new FotosVehiculo
                        {
                            VehiculoId = vehiculo.Id,
                            FotoArchivo = "/uploads/" + nombreArchivo,
                            Fecha = DateTime.Now
                        };

                        _context.FotosVehiculos.Add(foto);
                    }
                }
            }
            else
            {
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            foreach (var ruta in rutasImagenesGuardadas)
            {
                if (File.Exists(ruta))
                {
                    File.Delete(ruta);
                }
            }

            throw new Exception("Error al crear vehículo con fotos", ex);
        }
    }



    public async Task UpdateVehiculoAsync(Vehiculo vehiculo)
    {
        try
        {
            _context.Vehiculos.Update(vehiculo);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el vehículo con ID {vehiculo.Id}", ex);
        }
    }

    public async Task UpdateVehiculoConFotosAsync(Vehiculo vehiculo, List<IFormFile>? nuevasImagenes, List<int>? idsFotosEliminar)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        var rutasNuevasImagenesGuardadas = new List<string>();

        try
        {
            _logger.LogInformation("Iniciando actualización del vehículo con ID: {VehiculoId}", vehiculo.Id);
            _logger.LogInformation("ID del modelo recibido: {IdModelo}", vehiculo.IdModelo);

            // 1. Actualizamos el vehículo
            _context.Vehiculos.Update(vehiculo);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Vehículo actualizado en base de datos.");

            // 2. Eliminamos fotos seleccionadas
            if (idsFotosEliminar != null && idsFotosEliminar.Count > 0)
            {
                _logger.LogInformation("Eliminando {Count} fotos del vehículo ID: {VehiculoId}", idsFotosEliminar.Count, vehiculo.Id);

                var fotosAEliminar = await _context.FotosVehiculos
                    .Where(f => idsFotosEliminar.Contains(f.Id) && f.VehiculoId == vehiculo.Id)
                    .ToListAsync();

                foreach (var foto in fotosAEliminar)
                {
                    var rutaFisica = Path.Combine("wwwroot", foto.FotoArchivo.TrimStart('/'));
                    if (File.Exists(rutaFisica))
                    {
                        File.Delete(rutaFisica);
                        _logger.LogInformation("Archivo eliminado: {Ruta}", rutaFisica);
                    }

                    _context.FotosVehiculos.Remove(foto);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Fotos eliminadas correctamente.");
            }

            // 3. Agregamos nuevas imágenes
            if (nuevasImagenes != null && nuevasImagenes.Count > 0)
            {
                _logger.LogInformation("Agregando {Count} nuevas imágenes para vehículo ID: {VehiculoId}", nuevasImagenes.Count, vehiculo.Id);

                var uploadsPath = Path.Combine("wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                    _logger.LogInformation("Directorio creado: {Ruta}", uploadsPath);
                }

                foreach (var archivo in nuevasImagenes)
                {
                    if (archivo.Length > 0)
                    {
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                        var ruta = Path.Combine(uploadsPath, nombreArchivo);

                        using (var stream = new FileStream(ruta, FileMode.Create))
                        {
                            await archivo.CopyToAsync(stream);
                        }

                        rutasNuevasImagenesGuardadas.Add(ruta);
                        _logger.LogInformation("Imagen guardada en: {Ruta}", ruta);

                        var nuevaFoto = new FotosVehiculo
                        {
                            VehiculoId = vehiculo.Id,
                            FotoArchivo = "/uploads/" + nombreArchivo,
                            Fecha = DateTime.Now
                        };

                        _context.FotosVehiculos.Add(nuevaFoto);
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Nuevas imágenes asociadas al vehículo correctamente.");
            }

            await transaction.CommitAsync();
            _logger.LogInformation("Transacción completada correctamente para vehículo ID: {VehiculoId}", vehiculo.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error durante la actualización del vehículo con ID: {VehiculoId}", vehiculo.Id);

            foreach (var ruta in rutasNuevasImagenesGuardadas)
            {
                if (File.Exists(ruta))
                {
                    File.Delete(ruta);
                    _logger.LogInformation("Archivo eliminado tras rollback: {Ruta}", ruta);
                }
            }

            throw new Exception($"Error al actualizar el vehículo con fotos: {ex.Message}", ex);
        }
    }

    public async Task<bool> ActualizarDestacadoAsync(int id)
    {
        var vehiculo = await _context.Vehiculos.FindAsync(id);
        if (vehiculo == null)
            return false;
        vehiculo.Destacado = 1;
        _context.Vehiculos.Update(vehiculo);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }



    public async Task DeleteVehiculoAsync(int id)
    {
        try
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el vehículo con ID {id}", ex);
        }
    }

    public async Task<IEnumerable<Modelo>> GetModelosAsync()
    {
        try
        {
            return await _context.Modelos
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los modelos", ex);
        }
    }

    public async Task<IEnumerable<Marca>> GetMarcasAsync()
    {
        try
        {
            return await _context.Marcas
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las marcas", ex);
        }
    }

    public async Task BajaLogicaVehiculoAsync(int id)
    {
        try
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                vehiculo.Estado = 0; // Estado inactivo
                _context.Vehiculos.Update(vehiculo);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontró el vehículo con ID {id}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al dar de baja el vehículo con ID {id}", ex);
        }
    }

    public async Task AgregarFotoVehiculoAsync(FotosVehiculo foto)
    {
        {
            _context.FotosVehiculos.Add(foto);
            await _context.SaveChangesAsync();
        }
    }

}
