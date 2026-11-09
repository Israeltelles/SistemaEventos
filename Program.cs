using SistemaEventos.Data;
using Microsoft.EntityFrameworkCore;
using SistemaEventos.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    if (!context.Usuarios.Any())
    {
        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<UsuarioModel>();

        var diretorPadrao = new UsuarioModel
        {
            Nome = "Diretor Geral",
            Email = "diretor@sistema.com",
            Telefone = "11999999999",
            Ra = null,
            Tipo = "Diretor"
        };

        diretorPadrao.Senha = hasher.HashPassword(diretorPadrao, "12345678");

        context.Usuarios.Add(diretorPadrao);
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
