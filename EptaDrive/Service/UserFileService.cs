using EptaDrive.Entities;
using EptaDrive.Repository.Interfaces;
using EptaDrive.Service.Interfaces;

namespace EptaDrive.Service;

public class UserFileService: IUserFileService
{
    private readonly IUserFileRepository _userFileRepository;

    public UserFileService(IUserFileRepository userFileRepository)
    {
        _userFileRepository = userFileRepository;
    }

    public async Task<UserFile> SaveAsync(Stream stream, string fileName, long userId, CancellationToken token)
    {
        return await _userFileRepository.SaveAsync(stream, fileName, userId, token);
    }

    public async Task<UserFileData> GetFileAsync(long userFileId, CancellationToken token)
    {
        return await _userFileRepository.GetFileAsync(userFileId, token);
    }

    public async Task<ICollection<UserFile>> GetByUserIdAsync(long userId, CancellationToken token)
    {
        return await _userFileRepository.GetByUserIdAsync(userId, token);
    }

    public async Task DeleteAsync(long userFileId, CancellationToken token)
    {
        await _userFileRepository.DeleteAsync(userFileId, token);
    }
}