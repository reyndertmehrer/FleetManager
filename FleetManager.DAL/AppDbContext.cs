using FleetManager.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetManager.DAL;

public class AppDbContext : DbContext
{
    protected AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
