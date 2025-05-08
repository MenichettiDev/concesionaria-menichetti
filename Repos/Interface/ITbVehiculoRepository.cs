using System.Collections.Generic;
using System.Threading.Tasks;
using concesionaria_menichetti.Models;

public interface ITbVehiculoRepository : IGenericRepository<TbVehiculo>
{
    Task<IEnumerable<TbVehiculo>> GetVehiculosActivosAsync();
}
