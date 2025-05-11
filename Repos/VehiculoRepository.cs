using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VehiculoRepository
{
    private readonly ConcesionariaContext _context;

    public VehiculoRepository(ConcesionariaContext context)
    {
        _context = context;
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
            query = query.Where(v => v.Año >= anoDesde.Value);
        }

        if (anoHasta.HasValue)
        {
            query = query.Where(v => v.Año <= anoHasta.Value);
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



}
