using concesionaria_menichetti.Models;

public class VehiculoService
{
    private readonly ITbVehiculoRepository _vehiculoRepository;

    public VehiculoService(ITbVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    // Ejemplo: Método para obtener vehículos activos
    public async Task<IEnumerable<TbVehiculo>> ObtenerVehiculosActivosAsync()
    {
        return await _vehiculoRepository.GetVehiculosActivosAsync();
    }

    // Podrías agregar otros métodos de lógica de negocio según necesites.
}
