using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UsuarioRepository
{
    private readonly ConcesionariaContext _context;

    public UsuarioRepository(ConcesionariaContext context)
    {
        _context = context;
    }

    public IQueryable<Usuario> GetQueryable()
    {
        return _context.Usuarios.AsQueryable();
    }

    public async Task<List<Usuario>> ObtenerUsuariosAsync()
    {
        try
        {
            return await _context.Usuarios.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los usuarios", ex);
        }
    }
    //usuarios filtrados
    public async Task<(List<Usuario> Usuarios, int TotalPaginas)> ObtenerUsuariosFiltradosAsync(string? nombre, bool? esConcesionaria, bool? activo, int page, int pageSize)
    {
        var query = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrEmpty(nombre))
            query = query.Where(u => u.Nombre != null && u.Nombre.Contains(nombre));

        if (esConcesionaria.HasValue)
            query = query.Where(u => u.EsConcesionaria == esConcesionaria);

        if (activo.HasValue)
            query = query.Where(u => u.Activo == activo);

        int totalItems = await query.CountAsync();
        int totalPaginas = (int)Math.Ceiling(totalItems / (double)pageSize);

        var usuarios = await query
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (usuarios, totalPaginas);
    }


    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        try
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el usuario con ID {id}", ex);
        }
    }

    public async Task CreateUsuarioAsync(Usuario usuario)
    {
        try
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el usuario", ex);
        }
    }

    public async Task UpdateUsuarioAsync(Usuario usuario)
    {
        try
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);
            if (usuarioExistente == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Solo actualizamos los campos que vienen del formulario
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.EsConcesionaria = usuario.EsConcesionaria;
            usuarioExistente.Verificado = usuario.Verificado;
            usuarioExistente.Activo = usuario.Activo;
            usuarioExistente.Telefono = usuario.Telefono;
            usuarioExistente.Ubicacion = usuario.Ubicacion;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el usuario con ID {usuario.Id}", ex);
        }
    }


    public async Task DeleteUsuarioAsync(int id)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Activo = false; // Baja lógica
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el usuario con ID {id}", ex);
        }
    }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
        // var usuario = new Usuario();
        try
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el usuario con email {email}", ex);
        }
    }



}
