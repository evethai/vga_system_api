using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly VgaDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(VgaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? _dbSet.CountAsync() : _dbSet.CountAsync(filter);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex != null && pageSize != null)
            {
                query = query.Skip(((int)pageIndex - 1) * (int)pageSize).Take((int)pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetByIdGuidAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}

