using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReporteRepository : GenericRepository<Reporte>
{
    private readonly ConcesionariaContext _context;

    public ReporteRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reporte>> ObtenerTodosConDetallesAsync()
    {
        try
        {
            return await _context.Reportes
                .Include(r => r.Usuario)
                .Include(r => r.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los reportes con detalles", ex);
        }
    }

    public async Task<IEnumerable<Reporte>> ObtenerPorVehiculoIdAsync(int vehiculoId)
    {
        try
        {
            return await _context.Reportes
                .Where(r => r.VehiculoId == vehiculoId)
                .Include(r => r.Usuario)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener reportes por vehículo", ex);
        }
    }

    public async Task<IEnumerable<Reporte>> ObtenerPorUsuarioIdAsync(int usuarioId)
    {
        try
        {
            return await _context.Reportes
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener reportes por usuario", ex);
        }
    }

    public IQueryable<Reporte> GetQueryable()
    {
        return _context.Reportes.AsQueryable();
    }
}
