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
    int? idMarca, int? idModelo, int? anoDesde, int? anoHasta, int? estado, int page, int pageSize)
    {
        var query = GetQueryable();

        query = query
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca);


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
                .Include(v => v.FotosVehiculos) // 👈 Agregamos esto
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
