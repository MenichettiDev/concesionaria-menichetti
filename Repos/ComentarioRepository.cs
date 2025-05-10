using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ComentarioRepository : GenericRepository<Comentario>
{
    private readonly ConcesionariaContext _context;

    public ComentarioRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comentario>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.Comentarios
                .Include(c => c.Comprador)
                .Include(c => c.Vendedor)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los comentarios", ex);
        }
    }

    public async Task<IEnumerable<Comentario>> ObtenerPorVendedorIdAsync(int vendedorId)
    {
        try
        {
            return await _context.Comentarios
                .Include(c => c.Comprador)
                .Where(c => c.VendedorId == vendedorId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener comentarios para el vendedor con ID {vendedorId}", ex);
        }
    }

    public async Task<IEnumerable<Comentario>> ObtenerPorCompradorIdAsync(int compradorId)
    {
        try
        {
            return await _context.Comentarios
                .Include(c => c.Vendedor)
                .Where(c => c.CompradorId == compradorId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener comentarios del comprador con ID {compradorId}", ex);
        }
    }

    public IQueryable<Comentario> GetQueryable()
    {
        return _context.Comentarios.AsQueryable();
    }
}
