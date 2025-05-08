using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();     // Obtener todos los registros
    Task<T?> GetByIdAsync(object id);       // Buscar por ID
    Task AddAsync(T entity);                // Agregar nuevo registro
    void Update(T entity);                  // Actualizar registro
    void Delete(T entity);                  // Eliminar registro
    Task SaveAsync();                       // Guardar cambios en la DB
}
