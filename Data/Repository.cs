using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmployeeManagement22.Entity;

namespace EmployeeManagement22.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            // Special logic for Department: Prevent deletion if employees exist
            if (entity is Department department)
            {
                var employees = await _context.Employees
                    .Where(e => e.DepartmentId == department.Id)
                    .ToListAsync();

                if (employees.Any())
                {
                    // ✅ Option 2: Reassign employees to a default department
                    var defaultDept = await _context.Departments
                        .FirstOrDefaultAsync(d => d.Name == "General");

                    if (defaultDept == null)
                    {
                        defaultDept = new Department { Name = "General" };
                        await _context.Departments.AddAsync(defaultDept);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var emp in employees)
                    {
                        emp.DepartmentId = defaultDept.Id;
                    }

                    await _context.SaveChangesAsync(); // Save reassignment before deleting
                }
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}


