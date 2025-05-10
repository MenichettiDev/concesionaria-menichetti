using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EmpleadosConcesionariumRepository : GenericRepository<EmpleadosConcesionarium>
{
    private readonly ConcesionariaContext _context;

    public EmpleadosConcesionariumRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmpleadosConcesionarium>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.EmpleadosConcesionaria
                .Include(e => e.Concesionaria)
                .Include(e => e.Usuario)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los empleados de concesionarias", ex);
        }
    }

    public async Task<IEnumerable<EmpleadosConcesionarium>> ObtenerPorConcesionariaIdAsync(int concesionariaId)
    {
        try
        {
            return await _context.EmpleadosConcesionaria
                .Where(e => e.ConcesionariaId == concesionariaId)
                .Include(e => e.Usuario)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener empleados para la concesionaria con ID {concesionariaId}", ex);
        }
    }

    public async Task<IEnumerable<EmpleadosConcesionarium>> ObtenerPorUsuarioIdAsync(int usuarioId)
    {
        try
        {
            return await _context.EmpleadosConcesionaria
                .Where(e => e.UsuarioId == usuarioId)
                .Include(e => e.Concesionaria)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener datos del empleado con ID de usuario {usuarioId}", ex);
        }
    }

    public IQueryable<EmpleadosConcesionarium> GetQueryable()
    {
        return _context.EmpleadosConcesionaria.AsQueryable();
    }
}
