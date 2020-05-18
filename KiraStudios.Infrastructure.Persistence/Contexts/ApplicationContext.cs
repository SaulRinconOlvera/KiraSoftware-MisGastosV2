
using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityModel.Navegation;
using KiraStudios.Domain.TokenModel.Tracking;
using KiraStudios.Domain.UbicationModel.Ubication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;

namespace KiraStudios.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext :
        IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly ILoggerFactory _loggerFactory;

        //  Ubication
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        //  Navegation
        public DbSet<Control> Controls { get; set; }
        public DbSet<SpecialAction> SpecialActions { get; set; }

        //  Identity
        public DbSet<RoleControl> RoleControls { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        //  Tracking
        public DbSet<TrackingToken> TrackingTokens { get; set; }

        public ApplicationContext(ILoggerFactory loggerFactory) { _loggerFactory = loggerFactory; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"));
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CreateUbitacion(modelBuilder);
            CreateIdentity(modelBuilder);
            CreateNavegation(modelBuilder);
            SetFilter(modelBuilder);
        }

        private void CreateNavegation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Control>().ToTable("Control");
            modelBuilder.Entity<SpecialAction>(sa => {
                sa.HasOne(sac => sac.Control).WithMany(c => c.SpecialActions).HasForeignKey(sac => sac.ControlId);
                sa.ToTable("SpecialAction");
            });
        }

        private void SetFilter(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var isActiveProperty = entityType.FindProperty("Enabled");
                if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "p");
                    var filter = Expression.Lambda(Expression.Property(parameter, isActiveProperty.PropertyInfo), parameter);
                    entityType.SetQueryFilter(filter);
                }
            }
        }

        private void CreateUbitacion(ModelBuilder modelBuilder)
        {
            //  Ubication
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<State>().ToTable("State");
            modelBuilder.Entity<City>().ToTable("City");
        }

        private void CreateIdentity(ModelBuilder modelBuilder)
        {
            //  Indentity
            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(e => e.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(e => e.NormalizedEmail).HasName("EmailIndex").IsUnique();
                b.HasIndex(e => e.NormalizedEmail).HasName("EmailIndex");
                b.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(e => e.UserName).HasMaxLength(256);
                b.Property(e => e.NormalizedUserName).HasMaxLength(256);
                b.Property(e => e.Email).HasMaxLength(256);
                b.Property(e => e.NormalizedEmail).HasMaxLength(256);
                b.HasOne(e => e.RefreshToken).WithOne(e => e.User)
                    .HasForeignKey<RefreshToken>(e => e.UserId);
                b.ToTable("User");
            });

            //.ToTable("User");
            modelBuilder.Entity<Role>(b =>
            {
                b.HasIndex(e => e.NormalizedName).HasName("RoleNameIndex").IsUnique();
                b.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(e => e.Name).HasMaxLength(256);
                b.Property(e => e.NormalizedName).HasMaxLength(256);
                b.ToTable("Role");
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.HasOne(roleClaim => roleClaim.Role).WithMany(role => role.Claims).HasForeignKey(roleClaim => roleClaim.RoleId);
                b.ToTable("RoleClaim");
            });

            modelBuilder.Entity<RoleControl>(b =>
            {
                b.HasOne(rc => rc.Role).WithMany(role => role.Controls).HasForeignKey(rc => rc.RoleId);
                b.HasOne(rc => rc.Control).WithMany(c => c.RoleControls).HasForeignKey(rc => rc.ControlId);
                b.ToTable("RoleControl");
            });

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.HasOne(userClaim => userClaim.User).WithMany(user => user.Claims).HasForeignKey(userClaim => userClaim.UserId);
                b.ToTable("UserClaim");
            });

            modelBuilder.Entity<UserLogin>(b =>
            {
                b.HasKey(e => new { e.LoginProvider, e.ProviderKey });
                b.Property(e => e.LoginProvider).HasMaxLength(128);
                b.Property(e => e.ProviderKey).HasMaxLength(128);
                b.HasOne(userLogin => userLogin.User).WithMany(user => user.Logins).HasForeignKey(userLogin => userLogin.UserId);
                b.ToTable("UserLogin");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(e => new { e.UserId, e.RoleId });
                b.HasOne(userRole => userRole.Role).WithMany(role => role.Users).HasForeignKey(userRole => userRole.RoleId);
                b.HasOne(userRole => userRole.User).WithMany(user => user.Roles).HasForeignKey(userRole => userRole.UserId);
                b.Property(p => p.Id).UseIdentityColumn();
                b.ToTable("UserRole");
            });

            modelBuilder.Entity<UserToken>(b =>
            {
                b.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
                b.Property(e => e.LoginProvider).HasMaxLength(256);
                b.Property(e => e.Name).HasMaxLength(256);
                b.HasOne(userToken => userToken.User).WithMany(user => user.Tokens).HasForeignKey(userToken => userToken.UserId);
                b.ToTable("UserToken");
            });
        }
    }
}
