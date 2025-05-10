using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PlanesConcesionariaRepository : GenericRepository<PlanesConcesionarium>
{
    private readonly ConcesionariaContext _context;

    public PlanesConcesionariaRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlanesConcesionarium>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.PlanesConcesionaria
                .Include(p => p.ContratosPlanes)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los planes de concesionaria", ex);
        }
    }

    public async Task<PlanesConcesionarium?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.PlanesConcesionaria
                .Include(p => p.ContratosPlanes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el plan con ID {id}", ex);
        }
    }

    public IQueryable<PlanesConcesionarium> GetQueryable()
    {
        return _context.PlanesConcesionaria.AsQueryable();
    }
}
