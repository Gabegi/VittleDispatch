using Futures.Api.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Futures.Api.Data.Repositories
{
    /// <summary>
    /// A generic implementation of the repository pattern, providing the most basic functionality for any entity set 
    /// that does not have its own specific repository
    /// </summary>
    /// <typeparam name="T">A class representing one of the models</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected IFuturesContext _context;

        public GenericRepository(IFuturesContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);

        }

        public async Task Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);

                await _context.SaveChangesAsync();
            }
        }
    }
}
