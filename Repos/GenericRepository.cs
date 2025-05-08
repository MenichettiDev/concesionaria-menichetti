using concesionaria_menichetti.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ConcesionariaContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ConcesionariaContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener todos los registros de {typeof(T).Name}", ex);
        }
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        try
        {
            return await _dbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener {typeof(T).Name} por ID", ex);
        }
    }

    public async Task AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Error al agregar {typeof(T).Name}", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            await SaveAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception($"Error de concurrencia al actualizar {typeof(T).Name}", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Error al actualizar {typeof(T).Name}", ex);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Error al eliminar {typeof(T).Name}", ex);
        }
    }

    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Error al guardar los cambios en la base de datos", ex);
        }
    }
}