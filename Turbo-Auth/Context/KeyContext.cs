using Microsoft.EntityFrameworkCore;
using Turbo_Auth.Models.Accounts;
using Turbo_Auth.Models.Ai.Media;
using Turbo_Auth.Models.Suppliers;

namespace Turbo_Auth.Context;

public partial class KeyContext : DbContext
{
    public KeyContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IConfiguration _configuration;
    public KeyContext(DbContextOptions<KeyContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_configuration.GetConnectionString("ciko"), ServerVersion.Parse("8.0.35-mysql"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<SupplierKey>().ToTable("SupplierKeys");
        modelBuilder.Entity<ModelFee>().ToTable("ModelFees");
        modelBuilder.Entity<Model>().ToTable("Models");
        modelBuilder.Entity<NovitaModel>().ToTable("NovitaModels");

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<SupplierKey>? SupplierKeys
    {
        get;
        set;
    }
    public DbSet<ModelFee>? ModelFees
    {
        get;
        set;
    }
    public DbSet<Model>? Models
    {
        get;
        set;
    }

    public DbSet<NovitaModel>? NovitaModels
    {
        get;
        set;
    }
}