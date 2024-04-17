using Microsoft.AspNetCore.Builder;
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
    var usuario = await context.Usuarios.FindAsync(id);

    if (usuario == null)
    {
        return Results.BadRequest("Usuario não localizado");
    }
    return Results.Ok(usuario);
});
app.MapPost("/Usuario", async (AppDbContext context, UsuarioModel usuario) =>
{
    context.Usuarios.Add(usuario);
    await context.SaveChangesAsync();

    return await GetUsuarios(context);
});

app.MapPut("/Usuarios", async(AppDbContext context, UsuarioModel usuario)=>
{
    var usuarioDb = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == usuario.Id);


    if (usuarioDb == null) return Results.BadRequest("Usuário Não Localizado");

    usuarioDb.Nome = usuario.Nome;
    usuarioDb.UserName = usuario.UserName;
    usuarioDb.Email = usuario.Email;

    context.Update(usuario);
    await context.SaveChangesAsync();

    return Results.Ok(await GetUsuarios(context));
});

app.MapDelete("/Usuarios/{id}", async (AppDbContext context, int id) =>
{
    var usuarioDb = await context.Usuarios.FindAsync(id);

    if (usuarioDb == null) return Results.NotFound("Usuário não localizado");

    context.Usuarios.Remove(usuarioDb); 
    await context.SaveChangesAsync();
    return Results.Ok(await GetUsuarios(context));

});

app.Run();


