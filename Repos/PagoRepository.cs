using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PagoRepository : GenericRepository<Pago>
{
    private readonly ConcesionariaContext _context;

    public PagoRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pago>> ObtenerPagosPorUsuarioAsync(int usuarioId)
    {
        try
        {
            return await _context.Pagos
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener pagos del usuario con ID {usuarioId}", ex);
        }
    }

    public async Task<Pago?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.Pagos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el pago con ID {id}", ex);
        }
    }

    public IQueryable<Pago> GetQueryable()
    {
        return _context.Pagos.AsQueryable();
    }
}
