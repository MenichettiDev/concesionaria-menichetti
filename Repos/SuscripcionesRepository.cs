using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SuscripcionesRepository : GenericRepository<Suscripcione>
{
    private readonly ConcesionariaContext _context;

    public SuscripcionesRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<Suscripcione> Suscripciones, int TotalPaginas)> ObtenerSuscripcionesFiltradosAsync(
    string nombre, int page, int pageSize)
    {
        var query = _context.Suscripciones
                            .Include(p => p.ContratosSuscripcions)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            query = query.Where(p => p.Nombre != null && p.Nombre.Contains(nombre));
        }

        var totalSuscripciones = await query.CountAsync();

        var suscripciones = await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalPaginas = (int)Math.Ceiling(totalSuscripciones / (double)pageSize);

        return (suscripciones, totalPaginas);
    }


    public async Task<Suscripcione?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.Suscripciones
                .Include(p => p.ContratosSuscripcions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el Suscripcion con ID {id}", ex);
        }
    }

    public async Task<IEnumerable<Suscripcione>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.Suscripciones
                .Include(p => p.ContratosSuscripcions)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los Suscripciones de concesionaria", ex);
        }
    }

    public async Task<Suscripcione> GetSuscripcionByIdAsync(int id)
    {
        try
        {
            return await _context.Suscripciones
                .Include(p => p.ContratosSuscripcions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el Suscripcion con ID {id}", ex);
        }
    }

    public async Task CreateSuscripcionAsync(Suscripcione Suscripcion)
    {
        try
        {
            _context.Suscripciones.Add(Suscripcion);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el Suscripcion", ex);
        }
    }

    public async Task UpdateSuscripcionAsync(Suscripcione Suscripcion)
    {
        try
        {
            _context.Suscripciones.Update(Suscripcion);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el Suscripcion con ID {Suscripcion.Id}", ex);
        }
    }

    public async Task DeleteSuscripcionAsync(int id)
    {
        try
        {
            var Suscripcion = await _context.Suscripciones.FindAsync(id);
            if (Suscripcion != null)
            {
                _context.Suscripciones.Remove(Suscripcion);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontró la Suscripcion con ID {id}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la Suscripcion con ID {id}", ex);
        }
    }
    public IQueryable<Suscripcione> GetQueryable()
    {
        return _context.Suscripciones.AsQueryable();
    }

    public async Task<bool> UsuarioPuedePublicarAsync(int usuarioId)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.ContratosSuscripcions.Where(cs => cs.Activo == true))
                    .ThenInclude(cs => cs.Suscripcion)
                .Include(u => u.Vehiculos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                return false;

            var contratosActivos = usuario.ContratosSuscripcions;
            if (contratosActivos == null || !contratosActivos.Any())
                return false;

            int publicacionesPermitidas = contratosActivos.Sum(c => c.Suscripcion.CantidadPublicaciones);
            int publicacionesActuales = usuario.Vehiculos.Count();

            return publicacionesActuales < publicacionesPermitidas;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al verificar publicaciones disponibles para el usuario {usuarioId}: {ex.Message}");
            return false;
        }



    }
    public async Task<int> ObtenerPublicacionesRestantesAsync(int usuarioId)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.ContratosSuscripcions.Where(cs => cs.Activo == true))
                    .ThenInclude(cs => cs.Suscripcion)
                .Include(u => u.Vehiculos)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                return 0;

            int publicacionesPermitidas = usuario.ContratosSuscripcions.Sum(c => c.Suscripcion.CantidadPublicaciones);
            int publicacionesActuales = usuario.Vehiculos.Count();

            return Math.Max(publicacionesPermitidas - publicacionesActuales, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al calcular publicaciones restantes para el usuario {usuarioId}: {ex.Message}");
            return 0;
        }
    }



}
