using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UsuarioRepository : GenericRepository<Usuario>
{
    private readonly ConcesionariaContext _context;

    public UsuarioRepository(ConcesionariaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorEmailAsync(string email)
    {
        try
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener usuario por email", ex);
        }
    }

    public async Task<IEnumerable<Usuario>> ObtenerUsuariosActivosAsync()
    {
        try
        {
            return await _context.Usuarios
                .Where(u => u.Activo == true)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener usuarios activos", ex);
        }
    }

    public IQueryable<Usuario> GetQueryable()
    {
        return _context.Usuarios.AsQueryable();
    }
}
