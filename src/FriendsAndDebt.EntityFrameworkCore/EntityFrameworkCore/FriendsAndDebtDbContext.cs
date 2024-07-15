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
            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("");
            modelBuilder.ConfigureBaseService();
        }
    }
}
