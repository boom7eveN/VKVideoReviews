using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<T?> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    void Delete(T entity);
    void Update(T entity);
}