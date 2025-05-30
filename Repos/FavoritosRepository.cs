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

    public async Task<bool> EsFavoritoAsync(int usuarioId, int vehiculoId)
    {
        return await _context.Favoritos
            .AnyAsync(f => f.UsuarioId == usuarioId && f.VehiculoId == vehiculoId);
    }

    public async Task AgregarFavoritoAsync(int usuarioId, int vehiculoId)
    {
        if (!await EsFavoritoAsync(usuarioId, vehiculoId))
        {
            _context.Favoritos.Add(new Favorito
            {
                UsuarioId = usuarioId,
                VehiculoId = vehiculoId
            });

            await _context.SaveChangesAsync();
        }
    }

    public async Task QuitarFavoritoAsync(int usuarioId, int vehiculoId)
    {
        var favorito = await _context.Favoritos
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.VehiculoId == vehiculoId);

        if (favorito != null)
        {
            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();
        }
    }


    public IQueryable<Favorito> GetQueryable()
    {
        return _context.Favoritos.AsQueryable();
    }
}
