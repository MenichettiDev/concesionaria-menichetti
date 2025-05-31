using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AccesosPagadoRepository : GenericRepository<AccesosPagado>
{
    private readonly ConcesionariaContext _context;

    public AccesosPagadoRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<AccesosPagado> CrearAccesoAsync(int usuarioId, int vehiculoId)
    {
        try
        {
            var nuevoAcceso = new AccesosPagado
            {
                UsuarioId = usuarioId,
                VehiculoId = vehiculoId,
                Fecha = DateTime.Now,
                Activo = 1
            };

            _context.AccesosPagados.Add(nuevoAcceso);
            await _context.SaveChangesAsync();

            return nuevoAcceso;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el acceso pagado", ex);
        }
    }


    public async Task<IEnumerable<AccesosPagado>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.AccesosPagados
                .Include(a => a.Usuario)
                .Include(a => a.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los accesos pagados", ex);
        }
    }

    public async Task<IEnumerable<AccesosPagado>> ObtenerPorUsuarioIdAsync(int usuarioId)
    {
        try
        {
            return await _context.AccesosPagados
                .Where(a => a.UsuarioId == usuarioId)
                .Include(a => a.Usuario)
                .Include(a => a.Vehiculo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener los accesos pagados para el usuario con ID {usuarioId}", ex);
        }
    }

    public async Task<AccesosPagado?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.AccesosPagados
                .Include(a => a.Usuario)
                .Include(a => a.Vehiculo)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el acceso pagado con ID {id}", ex);
        }
    }

    public IQueryable<AccesosPagado> GetQueryable()
    {
        return _context.AccesosPagados.AsQueryable();
    }
}
