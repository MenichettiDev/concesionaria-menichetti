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

    public async Task<(List<PlanesConcesionarium> Planes, int TotalPaginas)> ObtenerPlanesFiltradosAsync(
    string nombre, int page, int pageSize)
    {
        var query = _context.PlanesConcesionaria
                            .Include(p => p.ContratosPlanes)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            query = query.Where(p => p.Nombre != null && p.Nombre.Contains(nombre));
        }

        var totalPlanes = await query.CountAsync();

        var planes = await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalPaginas = (int)Math.Ceiling(totalPlanes / (double)pageSize);

        return (planes, totalPaginas);
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

    public async Task<PlanesConcesionarium> GetPlanByIdAsync(int id)
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

    public async Task CreatePlanAsync(PlanesConcesionarium plan)
    {
        try
        {
            _context.PlanesConcesionaria.Add(plan);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el plan", ex);
        }
    }

    public async Task UpdatePlanAsync(PlanesConcesionarium plan)
    {
        try
        {
            _context.PlanesConcesionaria.Update(plan);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el plan con ID {plan.Id}", ex);
        }
    }

    public async Task DeletePlanAsync(int id)
    {
        try
        {
            var plan = await _context.PlanesConcesionaria.FindAsync(id);
            if (plan != null)
            {
                _context.PlanesConcesionaria.Remove(plan);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontró el plan con ID {id}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el plan con ID {id}", ex);
        }
    }
    public IQueryable<PlanesConcesionarium> GetQueryable()
    {
        return _context.PlanesConcesionaria.AsQueryable();
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


    public async Task<bool> ConcesionariaPuedePublicarAsync(int concesionariaId)
    {
        try
        {
            var concesionaria = await _context.Concesionarias
                .Include(c => c.ContratosPlanes.Where(cp => cp.Activo == true))
                    .ThenInclude(cp => cp.Plan)
                .Include(c => c.Usuario)
                    .ThenInclude(u => u.Vehiculos)
                .FirstOrDefaultAsync(c => c.Id == concesionariaId);

            if (concesionaria == null || concesionaria.Usuario == null)
                return false;

            var contratosActivos = concesionaria.ContratosPlanes;
            if (contratosActivos == null || !contratosActivos.Any())
                return false;

            int publicacionesPermitidas = contratosActivos
                .Where(c => c.Plan != null)
                .Sum(c => c.Plan.CantidadPublicaciones ?? 0);

            int publicacionesActuales = concesionaria.Usuario.Vehiculos?.Count ?? 0;

            return publicacionesActuales < publicacionesPermitidas;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al verificar publicaciones disponibles para la concesionaria {concesionariaId}: {ex.Message}");
            return false;
        }
    }


    public async Task<int> ObtenerPublicacionesRestantesConcesionariaAsync(int concesionariaId)
    {
        try
        {
            var concesionaria = await _context.Concesionarias
                .Include(c => c.ContratosPlanes.Where(cp => cp.Activo == true))
                    .ThenInclude(cp => cp.Plan)
                .Include(c => c.Usuario)
                    .ThenInclude(u => u.Vehiculos)
                .FirstOrDefaultAsync(c => c.Id == concesionariaId);

            if (concesionaria == null || concesionaria.Usuario == null)
                return 0;

            int publicacionesPermitidas = concesionaria.ContratosPlanes
                .Where(cp => cp.Plan != null)
                .Sum(cp => cp.Plan.CantidadPublicaciones ?? 0);

            int publicacionesActuales = concesionaria.Usuario.Vehiculos?.Count ?? 0;

            return Math.Max(publicacionesPermitidas - publicacionesActuales, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al calcular publicaciones restantes para la concesionaria {concesionariaId}: {ex.Message}");
            return 0;
        }
    }


}
