using EptaDrive.Core;
using EptaDrive.Entities;
using EptaDrive.Repository.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace EptaDrive.Repository;

public class UserFileRepository : IUserFileRepository
{
    private readonly string _fileStoragePath = Path.Combine(AppContext.BaseDirectory, "UsersFiles");
    private readonly EptaDriveContext _context;

    public UserFileRepository(EptaDriveContext context)
    {
        _context = context;
        
        if (!Directory.Exists(_fileStoragePath))
        {
            Directory.CreateDirectory(_fileStoragePath);
        }
    }

    public async Task<UserFile> SaveAsync(Stream stream, string fileName, long userId, CancellationToken token)
    {
        DbSet<UserFile> set  = _context.Set<UserFile>();
        
        UserFile userFile = new UserFile()
        {
            FileName = Path.GetFileName(fileName),
            FileExtension = Path.GetExtension(fileName),
            UserId = userId,
            FileUUID = Guid.NewGuid()
        };
        
        await set.AddAsync(userFile, token);
        await _context.SaveChangesAsync(token);
        
        var filePath = GetFilePath(userFile.FileUUID, userFile.FileExtension);
        await using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream, token);
        
        return userFile;
    }

    public async Task<UserFileData> GetFileAsync(long userFileId, CancellationToken token)
    {
        DbSet<UserFile> set = _context.Set<UserFile>();
        UserFile userFile = await set.FirstOrDefaultAsync(f => f.Id == userFileId, token);
        if (userFile == null)
            throw new FileNotFoundException("Файл не найден.");

        string filePath = GetFilePath(userFile.FileUUID, userFile.FileExtension);
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл не найден на диске.");

        var stream = File.OpenRead(filePath);

        // Определяем content-type по имени файла
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(userFile.FileExtension, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return new UserFileData
        {
            Stream = stream,
            FileName = userFile.FileName,
            ContentType = contentType
        };
    }

    public async Task<ICollection<UserFile>> GetByUserIdAsync(long userId, CancellationToken token)
    {
        DbSet<UserFile> set = _context.Set<UserFile>();
        List<UserFile> result = await set.Where(uf => uf.UserId == userId).ToListAsync(token);
        return result;
    }

    public async Task DeleteAsync(long userFileId, CancellationToken token)
    {
        DbSet<UserFile> set = _context.Set<UserFile>();

        var userFile = await set.FirstOrDefaultAsync(f => f.Id == userFileId, token);
        if (userFile == null)
            throw new FileNotFoundException("Файл не найден в базе");

        var filePath = GetFilePath(userFile.FileUUID, userFile.FileExtension);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        set.Remove(userFile);
        await _context.SaveChangesAsync(token);
    }

    private string GetFilePath(Guid fileUuid, string extension = ".dat")
    {
        return Path.Combine(_fileStoragePath, $"{fileUuid}{extension}");
    }
}