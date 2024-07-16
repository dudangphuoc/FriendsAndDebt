using Abp.Zero.EntityFrameworkCore;
using FriendsAndDebt.Authorization.Roles;
using FriendsAndDebt.Authorization.Users;
using FriendsAndDebt.FAD;
using FriendsAndDebt.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace FriendsAndDebt.EntityFrameworkCore
{
    public class FriendsAndDebtDbContext : AbpZeroDbContext<Tenant, Role, User, FriendsAndDebtDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<Friend> Friends { get; set; }

        public virtual DbSet<Board> Boards { get; set; }

        public virtual DbSet<Card> Cards { get; set; }

        public virtual DbSet<Debt> Debts { get; set; }

        public FriendsAndDebtDbContext(DbContextOptions<FriendsAndDebtDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //use my sql server
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("");
            modelBuilder.ConfigureBaseService();

            modelBuilder.Entity<Friend>(friend =>
            {
                friend.HasOne(f => f.Owner)
                    .WithMany()
                    .HasForeignKey(f => f.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                friend.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Board>(board =>
            {
                board.HasOne(b => b.Owner)
                    .WithMany()
                    .HasForeignKey(b => b.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                board.HasMany(b => b.Members)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("BoardMembers"));
            });

            modelBuilder.Entity<Card>(card =>
            {
                card.HasOne(c => c.Board)
                    .WithMany(b => b.Cards)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
