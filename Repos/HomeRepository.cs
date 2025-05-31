using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;


public class HomeRepository
{
    private readonly ConcesionariaContext _context;
    private readonly ILogger<HomeRepository> _logger;

    public HomeRepository(ConcesionariaContext context, ILogger<HomeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<Vehiculo> GetQueryable()
    {
        return _context.Vehiculos.AsQueryable();
    }

    public async Task<(List<Vehiculo> Vehiculos, int TotalPaginas)> ObtenerVehiculosFiltradosAsync(
    int? idMarca, int? idModelo, int? anoDesde, int? anoHasta, int? estado, int page, int pageSize)
    {
        var query = GetQueryable();

        query = query
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca)
            .Include(v => v.Destacados)
            .Include(v => v.FotosVehiculos);


        if (idMarca.HasValue)
        {
            query = query.Where(v => v.Modelo.IdMarca == idMarca.Value);
        }

        if (idModelo.HasValue)
        {
            query = query.Where(v => v.IdModelo == idModelo.Value);
        }

        if (estado.HasValue)
        {
            query = query.Where(v => v.Estado == estado.Value);
        }

        if (anoDesde.HasValue)
        {
            query = query.Where(v => v.Anio >= anoDesde.Value);
        }

        if (anoHasta.HasValue)
        {
            query = query.Where(v => v.Anio <= anoHasta.Value);
        }

        var totalVehiculos = await query.CountAsync();

        var vehiculos = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();



        int totalPaginas = (int)Math.Ceiling(totalVehiculos / (double)pageSize);

        return (vehiculos, totalPaginas);
    }



    public async Task<(Vehiculo vehiculo, bool tieneAcceso)> GetVehiculoByIdAsyncConFoto(int idVehiculo, int idUsuarioLogueado)

    {
        try
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
                .Include(v => v.FotosVehiculos)
                .Include(v => v.Usuario)
                .Include(v => v.AccesosPagados)
                .FirstOrDefaultAsync(v => v.Id == idVehiculo);

            if (vehiculo == null)
                return (null, false);

            // Verifica si el usuario logueado pagó acceso a este vehículo
            bool tieneAcceso = vehiculo.AccesosPagados.Any(a =>
                a.UsuarioId == idUsuarioLogueado && a.Activo == 1);

            return (vehiculo, tieneAcceso);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el vehículo con ID {idVehiculo}", ex);
        }
    }



    public async Task<List<Marca>> GetMarcasAsync()
    {
        return await _context.Marcas.OrderBy(m => m.Descripcion).ToListAsync();
    }

    public async Task<List<Modelo>> GetModelosAsync()
    {
        return await _context.Modelos.OrderBy(m => m.Descripcion).ToListAsync();
    }
}
