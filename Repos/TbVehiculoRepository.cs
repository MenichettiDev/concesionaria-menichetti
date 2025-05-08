using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TbVehiculoRepository : GenericRepository<TbVehiculo>
{
    private readonly ConcesionariaContext _context;

    public TbVehiculoRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TbVehiculo>> GetVehiculosActivosAsync()
    {
        try
        {
            return await _context.TbVehiculos
                .Where(v => v.Estado == 1)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener vehículos activos", ex);
        }
    }
}
