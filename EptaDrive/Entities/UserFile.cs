namespace EptaDrive.Entities;

public class UserFile : IEntity
{
    public long Id { get; set; }

    public string FileName { get; set; }

    public string FileExtension { get; set; }

    public Guid FileUUID { get; set; }

    public long UserId { get; set; }
}