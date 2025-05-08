using concesionaria_menichetti.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class VehiculoService
{
    private readonly TbVehiculoRepository _vehiculoRepository;

    public VehiculoService(TbVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public async Task<IEnumerable<TbVehiculo>> ObtenerVehiculosActivosAsync()
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

    public async Task<TbVehiculo> GetVehiculoByIdAsync(int id)
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

    public async Task CreateVehiculoAsync(TbVehiculo vehiculo)
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

    public async Task UpdateVehiculoAsync(TbVehiculo vehiculo)
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
