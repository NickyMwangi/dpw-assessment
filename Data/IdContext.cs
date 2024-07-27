using Data.Entities;
using Data.Entities.Sales;
using Data.seed.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public partial class IdContext : IdentityDbContext<ApplicationUser>
{
    public IdContext(DbContextOptions<IdContext> opts) : base(opts)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers")
            .HasKey(r => new
            {
                r.Id
            })
            ;
        modelBuilder.Entity<AspNetUserRole>()
            .HasKey(r => new
            {
                r.RoleId,
                r.UserId
            })
            ;

        modelBuilder.Entity<AspNetUserLogin>()
            .HasKey(l => new
            {
                l.LoginProvider,
                l.ProviderKey,
                l.UserId
            });

        modelBuilder.Entity<AspNetUserToken>()
            .HasKey(l => new
            {
                l.UserId,
                l.LoginProvider,
                l.Name
            });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
