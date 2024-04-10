using Jonia0._3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Admin/Login";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(40);
    option.AccessDeniedPath = "/Inicio/ErrorPage";
});

builder.Services.AddDbContext<JoniaDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));


var permisos = await LoadPermissionsAsync(builder.Services);

builder.Services.AddAuthorization(option =>
{
    foreach(var permiso in permisos)
    {
        option.AddPolicy(permiso, policy =>
        policy.RequireClaim("Permiso", permiso));
    }
});

var nomrol = await LoadNombRolAsync(builder.Services);

builder.Services.AddAuthorization(option =>
{
    foreach (var nombre in nomrol)
    {
        option.AddPolicy(nombre, policy =>
        policy.RequireClaim("NombreRol", nombre));
    }
});

var app = builder.Build();

static async Task<List<string>> LoadPermissionsAsync(IServiceCollection services)
{
    using (var scope = services.BuildServiceProvider().CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<JoniaDbContext>();
        return await dbContext.Permisos.Select(p => p.Nombre).ToListAsync();
    }
}
static async Task<List<string>> LoadNombRolAsync(IServiceCollection services)
{
    using (var scope = services.BuildServiceProvider().CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<JoniaDbContext>();
        return await dbContext.Rols.Select(p => p.Nombre).ToListAsync();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

