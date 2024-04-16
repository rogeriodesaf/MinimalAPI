using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
 }
      public DbSet<UsuarioModel> Usuarios { get; set; }
}