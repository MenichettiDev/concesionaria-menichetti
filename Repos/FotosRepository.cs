using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FotosVehiculoRepository : GenericRepository<FotosVehiculo>
{
    private readonly ConcesionariaContext _context;

    public FotosVehiculoRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FotosVehiculo>> ObtenerFotosPorVehiculoAsync(int vehiculoId)
    {
        try
        {
            return await _context.FotosVehiculos
                .Where(f => f.VehiculoId == vehiculoId)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las fotos del vehículo con ID {vehiculoId}", ex);
        }
    }

    public async Task<FotosVehiculo?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.FotosVehiculos
                .Include(f => f.Vehiculo)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la foto con ID {id}", ex);
        }
    }

    public IQueryable<FotosVehiculo> GetQueryable()
    {
        return _context.FotosVehiculos.AsQueryable();
    }
}
