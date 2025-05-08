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

    // Obtener todos los elementos asincrónicamente
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Obtener por ID asincrónicamente
    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Agregar un nuevo elemento asincrónicamente
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync(); // Guardar cambios después de agregar
    }

    // Actualizar un elemento asincrónicamente
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await SaveAsync(); // Guardar cambios después de actualizar
    }

    // Eliminar un elemento asincrónicamente
    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveAsync(); // Guardar cambios después de eliminar
    }

    // Guardar los cambios asincrónicamente
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}