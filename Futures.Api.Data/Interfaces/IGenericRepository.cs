
namespace Futures.Api.Data.Interfaces
{
    /// <summary>
    /// Interface for a generic repository. If we do not have specific tasks with an entity, we can use this for it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetById(int id);

        Task<IEnumerable<T>> GetAll();

        Task Insert(T entity);

        Task Update(T entity);

        Task DeleteAsync(int id);        
    }
}
