using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext to the container.
builder.Services.AddDbContext<HomeBankingContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Create a scope to get the DbContext instance

using (var scope = app.Services.CreateScope())
{

    //Aqui obtenemos todos los services registrados en la App
    var services = scope.ServiceProvider;
    try
    {

        // En este paso buscamos un service que este con la clase HomeBankingContext
        var context = services.GetRequiredService<HomeBankingContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ha ocurrido un error al enviar la información a la base de datos!");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();
 
app.Run();
