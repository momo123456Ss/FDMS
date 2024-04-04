using Microsoft.EntityFrameworkCore;

namespace FDMS.Entity
{
    public class FDMSContext : DbContext
    {
        public FDMSContext(DbContextOptions options) : base(options) { }
        #region DbSet
        public DbSet<Account> Accounts{ get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<General> Generals { get; set; }
        public DbSet<FDHistory> FDHistorys { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<Account_GroupPermission> Account_GroupPermissions { get; set; }
        public DbSet<SystemNofication> SystemNofications { get; set; }
        public DbSet<AccountSession> AccountSessions { get; set; }
        public DbSet<Flight> Flights { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account_GroupPermission>()
                .HasOne(ag => ag.GroupPermissionNavigation)
                .WithMany(gr => gr.Account_GroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
