using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FavoritoRepository : GenericRepository<Favorito>
{
    private readonly ConcesionariaContext _context;

    public FavoritoRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Favorito>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.Favoritos
                .Include(f => f.Usuario)
                .Include(f => f.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los favoritos", ex);
        }
    }

    public async Task<IEnumerable<Favorito>> ObtenerPorUsuarioIdAsync(int usuarioId)
    {
        try
        {
            return await _context.Favoritos
                .Where(f => f.UsuarioId == usuarioId)
                .Include(f => f.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener favoritos para el usuario con ID {usuarioId}", ex);
        }
    }

    public async Task<Favorito?> ObtenerPorUsuarioYVehiculoAsync(int usuarioId, int vehiculoId)
    {
        try
        {
            return await _context.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.VehiculoId == vehiculoId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el favorito para usuario ID {usuarioId} y vehículo ID {vehiculoId}", ex);
        }
    }

    public IQueryable<Favorito> GetQueryable()
    {
        return _context.Favoritos.AsQueryable();
    }
}
