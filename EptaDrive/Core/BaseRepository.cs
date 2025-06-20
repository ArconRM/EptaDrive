using EptaDrive.Entities;
using Microsoft.EntityFrameworkCore;

namespace EptaDrive.Core;

public class BaseRepository<T> : IRepository<T>
    where T : class, IEntity, new()
{
    private readonly DbContext _context;

    public BaseRepository(DbContext context)
    {
        _context = context;
    }

    public virtual async Task<T> GetAsync(long id, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();
        T result = await set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return result;
    }

    public virtual async Task<IEnumerable<T>> GetAsync(IEnumerable<long> ids, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();
        IEnumerable<T> result = await set.AsNoTracking().Where(e => ids.Contains(e.Id)).ToListAsync();
        return result;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();
        return set.AsNoTracking().ToList();
    }

    public virtual async Task DeleteAsync(long id, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();

        T entity = new T()
        {
            Id = id
        };

        set.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();

        await set.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();

        set.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}