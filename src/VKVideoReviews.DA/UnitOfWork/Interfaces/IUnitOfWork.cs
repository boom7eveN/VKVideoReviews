using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    IGenresRepository Genres { get; }
    IVideoTypesRepository VideoTypes { get; }
    IVideosRepository Videos { get; }
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChangesAsync();
}