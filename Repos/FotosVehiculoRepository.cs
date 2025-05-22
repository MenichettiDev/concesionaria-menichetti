using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;

namespace concesionaria_menichetti.Repositories;

public class FotosVehiculoRepository
{
    private readonly ConcesionariaContext _context;

    public FotosVehiculoRepository(ConcesionariaContext context)
    {
        _context = context;
    }

    public async Task<List<FotosVehiculo>> ObtenerTodosAsync()
    {
        return await _context.FotosVehiculos.Include(f => f.Vehiculo).ToListAsync();
    }

    public async Task<List<FotosVehiculo>> BuscarPorVehiculoAsync(int vehiculoId)
    {
        return await _context.FotosVehiculos
            .Where(f => f.VehiculoId == vehiculoId)
            .ToListAsync();
    }

    public async Task<FotosVehiculo?> ObtenerPorIdAsync(int id)
    {
        return await _context.FotosVehiculos.FindAsync(id);
    }

    public async Task<FotosVehiculo> AltaAsync(FotosVehiculo foto)
    {
        _context.FotosVehiculos.Add(foto);
        await _context.SaveChangesAsync();
        return foto;
    }

    public async Task ModificarAsync(FotosVehiculo foto)
    {
        _context.FotosVehiculos.Update(foto);
        await _context.SaveChangesAsync();
    }

    public async Task BorrarAsync(int id)
    {
        var foto = await ObtenerPorIdAsync(id);
        if (foto != null)
        {
            _context.FotosVehiculos.Remove(foto);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AgregarFotoVehiculoAsync(FotosVehiculo foto)
    {
        {
            _context.FotosVehiculos.Add(foto);
            await _context.SaveChangesAsync();
        }
    }

}
