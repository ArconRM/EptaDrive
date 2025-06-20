using EptaDrive.Entities;

namespace EptaDrive.Service.Interfaces;

public interface IUserFileService
{
    Task<UserFile> SaveAsync(Stream stream, string fileName, long userId, CancellationToken token);
    
    Task<UserFileData> GetFileAsync(long userFileId, CancellationToken token);
    
    Task<ICollection<UserFile>> GetByUserIdAsync(long userId, CancellationToken token);
    
    Task DeleteAsync(long userFileId, CancellationToken token);
}