using Microsoft.AspNetCore.Identity;

namespace EptaDrive.Entities;

public class User : IdentityUser<long>, IEntity
{
}