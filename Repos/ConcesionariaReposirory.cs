using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ConcesionariaRepository : GenericRepository<Concesionaria>
{
    private readonly ConcesionariaContext _context;

    public ConcesionariaRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Concesionaria>> ObtenerTodasAsync()
    {
        try
        {
            return await _context.Concesionarias
                .Include(c => c.Usuario)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las concesionarias", ex);
        }
    }

    public async Task<Concesionaria?> ObtenerPorUsuarioIdAsync(int usuarioId)
    {
        try
        {
            return await _context.Concesionarias
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la concesionaria del usuario con ID {usuarioId}", ex);
        }
    }

    public IQueryable<Concesionaria> GetQueryable()
    {
        return _context.Concesionarias.AsQueryable();
    }
}
