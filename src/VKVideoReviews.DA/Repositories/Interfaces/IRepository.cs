using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<T?> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task DeleteAsync(T entity);
    Task<T> UpdateAsync(T entity);
}