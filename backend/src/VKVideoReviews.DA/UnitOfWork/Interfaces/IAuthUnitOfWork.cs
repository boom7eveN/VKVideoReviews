using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.UnitOfWork.Interfaces;

public interface IAuthUnitOfWork : IDisposable
{
    IUsersRepository Users { get; }
    IUserTokensRepository UserTokens { get; }
    IUserAppSessionsRepository UserAppSessions { get; }
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChangesAsync();
}