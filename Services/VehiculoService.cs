using concesionaria_menichetti.Models;

public class VehiculoService
{
    private readonly TbVehiculoRepository _vehiculoRepository;

    public VehiculoService( TbVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public async Task<IEnumerable<TbVehiculo>> ObtenerVehiculosActivosAsync()
    {
        return await _vehiculoRepository.GetVehiculosActivosAsync();
    }

    public async Task<TbVehiculo> GetVehiculoByIdAsync(int id)
    {
        return await _vehiculoRepository.GetByIdAsync(id);  // Deberás agregar este método en el repositorio
    }

    public async Task CreateVehiculoAsync(TbVehiculo vehiculo)
    {
        await _vehiculoRepository.AddAsync(vehiculo);  // Deberás agregar este método en el repositorio
    }

    public async Task UpdateVehiculoAsync(TbVehiculo vehiculo)
    {
        await _vehiculoRepository.UpdateAsync(vehiculo);  // Deberás agregar este método en el repositorio
    }

    public async Task DeleteVehiculoAsync(int id)
    {
        var vehiculo = await _vehiculoRepository.GetByIdAsync(id);
        if (vehiculo != null)
        {
            await _vehiculoRepository.DeleteAsync(vehiculo);  // Deberás agregar este método en el repositorio
        }
    }
}
