using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

async Task<List<UsuarioModel>> GetUsuarios(AppDbContext context)
{
    return await context.Usuarios.ToListAsync();
}

app.MapGet("/Usuarios" ,async (AppDbContext context) =>
{
    return await GetUsuarios(context);
    });

app.MapGet("/Usuario/{id}", async (AppDbContext context, int id) =>
{
    var usuarios = await context.Usuarios.FindAsync(id);

    return Results.Ok(usuarios);
});

app.MapPost("/Usuario", async (AppDbContext context, UsuarioModel usuario) =>
{
    context.Usuarios.Add(usuario);
    await context.SaveChangesAsync();

    return await GetUsuarios(context);
});
app.Run();

