using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Repositories;
using VKVideoReviews.DA.Repositories.Interfaces;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.DA.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly VkVideoReviewsDbContext _context;

    private bool _disposed;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(VkVideoReviewsDbContext context)
    {
        Genres = new GenresRepository(context);
        VideoTypes = new VideoTypesRepository(context);
        Videos = new VideosRepository(context);
        GenresVideos = new GenresVideosRepository(context);
        Reviews = new ReviewsRepository(context);
        Users = new UserRepository(context);
        Favorite = new FavoriteRepository(context);
        _context = context;
    }

    public IGenresRepository Genres { get; }
    public IVideoTypesRepository VideoTypes { get; }
    public IVideosRepository Videos { get; }
    public IGenresVideosRepository GenresVideos { get; }
    public IReviewsRepository Reviews { get; }
    public IUsersRepository Users { get; }

    public IFavoriteRepository Favorite { get; }

    public async Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel)
    {
        _transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
        return _transaction;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
        if (_transaction is not null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        _transaction = null;
        DetachAllEntities();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    private void DetachAllEntities()
    {
        foreach (var entry in _context.ChangeTracker.Entries()) entry.State = EntityState.Detached;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}