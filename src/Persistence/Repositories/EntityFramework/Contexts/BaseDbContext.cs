using Application.Common.Utilities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repositories.EntityFramework.Contexts;

public class BaseDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }
    public DbSet<User> Users { get; set; }

    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(p =>
        {
            p.ToTable("Users").HasKey(k => k.Id);
            p.Property(p => p.Id).HasColumnName("Id");
            p.Property(p => p.FirstName).HasColumnName("FirstName");
            p.Property(p => p.LastName).HasColumnName("LastName");
            p.Property(p => p.Email).HasColumnName("Email");
            p.Property(p => p.PasswordHash).HasColumnName("PasswordHash");
            p.Property(p => p.PasswordSalt).HasColumnName("PasswordSalt");
            p.Property(p => p.Status).HasColumnName("Status").HasDefaultValue(true);
            p.Property(p => p.AuthenticatorType).HasColumnName("AuthenticatorType");
            p.HasMany(p => p.UserOperationClaims);
            p.HasMany(p => p.RefreshTokens);
        });


        modelBuilder.Entity<OperationClaim>(p =>
        {
            p.ToTable("OperationClaims").HasKey(k => k.Id);
            p.Property(p => p.Id).HasColumnName("Id");
            p.Property(p => p.Name).HasColumnName("Name");
        });

        modelBuilder.Entity<UserOperationClaim>(p =>
        {
            p.ToTable("UserOperationClaims").HasKey(k => k.Id);
            p.Property(p => p.Id).HasColumnName("Id");
            p.Property(p => p.UserId).HasColumnName("UserId");
            p.Property(p => p.OperationClaimId).HasColumnName("OperationClaimId");
            p.HasOne(p => p.OperationClaim);
            p.HasOne(p => p.User);
        });

        HashingHelper.CreatePasswordHash("SdQnv96ZA4$P78G1Ab3ONa$HC!tDVi", out var passwordHash, out var passwordSalt);

        User[] userEntitySeeds = { new("ce1bf1ec-4854-49b0-a7cc-6c8a902e0aa9", "admin", "admin", "admin@admin.com", passwordSalt, passwordHash, true, Domain.Enums.AuthenticatorType.None) };
        modelBuilder.Entity<User>().HasData(userEntitySeeds);

        OperationClaim[] operationClaimEntitySeeds = { new("0460d8c9-75a6-4d68-abc3-f249523f3e93", "admin") };
        modelBuilder.Entity<OperationClaim>().HasData(operationClaimEntitySeeds);

        UserOperationClaim[] userOperationClaimEntitySeeds = { new("5565e2ed-b81d-42b6-a26c-0d08f383ce5f", "ce1bf1ec-4854-49b0-a7cc-6c8a902e0aa9", "0460d8c9-75a6-4d68-abc3-f249523f3e93") };
        modelBuilder.Entity<UserOperationClaim>().HasData(userOperationClaimEntitySeeds);
    }
}