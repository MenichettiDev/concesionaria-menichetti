using concesionaria_menichetti.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


public class VehiculoService
{
    private readonly VehiculoRepository _vehiculoRepository;

    public VehiculoService(VehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public async Task<(List<Vehiculo> Vehiculos, int TotalPaginas)> ObtenerVehiculosFiltradosAsync(string marca, string modelo, int? anoDesde, int? anoHasta, int? estado, int page, int pageSize)
    {
        var query = _vehiculoRepository.GetQueryable();

        // Filtros
        if (!string.IsNullOrEmpty(marca))
        {
            query = query.Where(v => v.Marca.Contains(marca));
        }

        if (!string.IsNullOrEmpty(modelo))
        {
            query = query.Where(v => v.Modelo.Contains(modelo));
        }

        if (estado.HasValue)
        {
            query = query.Where(v => v.Estado == estado.Value);
        }

        if (anoDesde.HasValue)
        {
            query = query.Where(v => v.Año >= anoDesde.Value);
        }

        if (anoHasta.HasValue)
        {
            query = query.Where(v => v.Año <= anoHasta.Value);
        }

        // Total de vehículos filtrados
        var totalVehiculos = await query.CountAsync();

        // Paginación
        var vehiculos = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        int totalPaginas = (int)Math.Ceiling(totalVehiculos / (double)pageSize);

        return (vehiculos, totalPaginas);
    }


    public async Task<IEnumerable<Vehiculo>> ObtenerVehiculosActivosAsync()
    {
        try
        {
            return await _vehiculoRepository.GetVehiculosActivosAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener vehículos activos desde el servicio", ex);
        }
    }

    public async Task<Vehiculo> GetVehiculoByIdAsync(int id)
    {
        try
        {
            return await _vehiculoRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el vehículo con ID {id}", ex);
        }
    }

    public async Task CreateVehiculoAsync(Vehiculo vehiculo)
    {
        try
        {
            await _vehiculoRepository.AddAsync(vehiculo);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el vehículo", ex);
        }
    }

    public async Task UpdateVehiculoAsync(Vehiculo vehiculo)
    {
        try
        {
            await _vehiculoRepository.UpdateAsync(vehiculo);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el vehículo con ID {vehiculo.Id}", ex);
        }
    }

    public async Task DeleteVehiculoAsync(int id)
    {
        try
        {
            var vehiculo = await _vehiculoRepository.GetByIdAsync(id);
            if (vehiculo != null)
            {
                await _vehiculoRepository.DeleteAsync(vehiculo);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el vehículo con ID {id}", ex);
        }
    }
}
