using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DestacadoRepository : GenericRepository<Destacado>
{
    private readonly ConcesionariaContext _context;

    public DestacadoRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Destacado>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.Destacados
                .Include(d => d.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los destacados", ex);
        }
    }

    public async Task MarcarVehiculoComoDestacadoAsync(int vehiculoId)
    {
        try
        {
            var destacado = new Destacado
            {
                VehiculoId = vehiculoId,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(30)
            };
            _context.Destacados.Add(destacado);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {

            throw new Exception($"Error al marcar el vehiculo como destacado, vehículo con ID {vehiculoId}", ex);
        }

    }
    public async Task<IEnumerable<Destacado>> ObtenerPorVehiculoIdAsync(int vehiculoId)
    {
        try
        {
            return await _context.Destacados
                .Where(d => d.VehiculoId == vehiculoId)
                .Include(d => d.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener destacados para el vehículo con ID {vehiculoId}", ex);
        }
    }

    public async Task<Destacado?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.Destacados
                .Include(d => d.Vehiculo)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el destacado con ID {id}", ex);
        }
    }

    public IQueryable<Destacado> GetQueryable()
    {
        return _context.Destacados.AsQueryable();
    }
}
