using EptaDrive.Core;
using EptaDrive.Entities;

namespace EptaDrive.Repository.Interfaces;

public interface IUserFileRepository
{
    Task<UserFile> SaveAsync(Stream stream, string fileName, long userId, CancellationToken token);
    
    Task<UserFileData> GetFileAsync(long userFileId, CancellationToken token);
    
    Task<ICollection<UserFile>> GetByUserIdAsync(long userId, CancellationToken token);
    
    Task DeleteAsync(long userFileId, CancellationToken token);
}