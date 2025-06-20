using EptaDrive.Entities;

namespace EptaDrive.Core;

public interface IRepository<T> where T : class
{
    Task<T> GetAsync(long id, CancellationToken token);

    Task<IEnumerable<T>> GetAsync(IEnumerable<long> ids, CancellationToken token);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken token);

    Task DeleteAsync(long id, CancellationToken token);

    Task<T> CreateAsync(T entity, CancellationToken token);

    Task<T> UpdateAsync(T entity, CancellationToken token);
}