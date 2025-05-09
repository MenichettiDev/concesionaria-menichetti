using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VehiculoRepository : GenericRepository<Vehiculo>
{
    private readonly ConcesionariaContext _context;

    public VehiculoRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehiculo>> GetVehiculosActivosAsync()
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
    public IQueryable<Vehiculo> GetQueryable()
    {
        return _context.Vehiculos.AsQueryable();
    }

}
