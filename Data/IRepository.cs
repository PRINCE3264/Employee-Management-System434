/*using EmployeeManagement22.Entity;

namespace EmployeeManagement22.Data
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll(int id);

        Task<T> FindById(int id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

       Task < int> SaveChangesAsync();
        Task<Department> FindByIdAsync(int id);
    }
}
*/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement22.Data
{
    public interface IRepository<T> where T : class
    {
        // Get all entities — no ID needed
        Task<List<T>> GetAll();

        // Find entity by id asynchronously
        Task<T> FindByIdAsync(int id);

        // Add a new entity
        Task AddAsync(T entity);

        // Update an existing entity
        Task UpdateAsync(T entity);

        // Delete entity by id
        Task DeleteAsync(int id);

        // Save changes explicitly if needed
        Task<int> SaveChangesAsync();
    }
}
