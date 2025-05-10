using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SuscripcionRepository : GenericRepository<Suscripcione>
{
    private readonly ConcesionariaContext _context;

    public SuscripcionRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Suscripcione>> ObtenerSuscripcionesActivasAsync()
    {
        try
        {
            return await _context.Suscripciones
                .Where(s => s.Activa == true)
                .Include(s => s.Usuario)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener suscripciones activas", ex);
        }
    }

    public async Task<Suscripcione?> ObtenerPorUsuarioIdAsync(int usuarioId)
    {
        try
        {
            return await _context.Suscripciones
                .Where(s => s.UsuarioId == usuarioId && s.Activa == true)
                .OrderByDescending(s => s.FechaInicio)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la suscripción del usuario", ex);
        }
    }

    public IQueryable<Suscripcione> GetQueryable()
    {
        return _context.Suscripciones.AsQueryable();
    }
}
