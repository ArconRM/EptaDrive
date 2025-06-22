using EptaDrive.Entities;
using EptaDrive.Extensions;
using EptaDrive.Repository;
using EptaDrive.Repository.Interfaces;
using EptaDrive.Service;
using EptaDrive.Service.Interfaces;
using EptaDrive.Utlis;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

//TODO: увеличить серверные ограничения на размер файла

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserFileRepository, UserFileRepository>();
builder.Services.AddScoped<IUserFileService, UserFileService>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 268435456; // 256 MB
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 268435456; // 256 MB
});

builder.Services.AddDbContext<EptaDriveContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQL")));

builder.Services.AddIdentity<User, IdentityRole<long>>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<EptaDriveContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // options.LoginPath = "/Account/Login";
    // options.LogoutPath = "/Account/Logout";
    // options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});

builder.Services.TryAddScoped<SignInManager<User>>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole(UserRoles.User));

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole(UserRoles.Admin));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// await app.SeedRolesAsync();

app.Run();