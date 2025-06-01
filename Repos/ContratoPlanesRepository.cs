using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ContratoPlanesRepository : GenericRepository<ContratosPlane>
{
    private readonly ConcesionariaContext _context;

    public ContratoPlanesRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task CargarContratoPlan(int planId, int concesionariaId)
    {
        var contratoPlan = new ContratosPlane
        {
            ConcesionariaId = concesionariaId,
            PlanId = planId, // Aquí deberías asignar un PlanId válido si es necesario
            FechaInicio = DateOnly.FromDateTime(DateTime.Now),
            FechaFin = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
        };
        _context.ContratosPlanes.Add(contratoPlan);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ContratosPlane>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.ContratosPlanes
                .Include(c => c.Concesionaria)
                .Include(c => c.Plan)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los contratos de planes", ex);
        }
    }

    public async Task<IEnumerable<ContratosPlane>> ObtenerPorConcesionariaIdAsync(int concesionariaId)
    {
        try
        {
            return await _context.ContratosPlanes
                .Where(c => c.ConcesionariaId == concesionariaId)
                .Include(c => c.Concesionaria)
                .Include(c => c.Plan)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener los contratos de planes para la concesionaria con ID {concesionariaId}", ex);
        }
    }

    public async Task<ContratosPlane?> ObtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.ContratosPlanes
                .Include(c => c.Concesionaria)
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el contrato con ID {id}", ex);
        }
    }

    public IQueryable<ContratosPlane> GetQueryable()
    {
        return _context.ContratosPlanes.AsQueryable();
    }


}
