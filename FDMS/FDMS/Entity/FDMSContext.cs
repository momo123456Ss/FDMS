using Microsoft.EntityFrameworkCore;

namespace FDMS.Entity
{
    public class FDMSContext : DbContext
    {
        public FDMSContext(DbContextOptions options) : base(options) { }
        #region DbSet
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<General> Generals { get; set; }
        public DbSet<FDHistory> FDHistorys { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<Account_GroupPermission> Account_GroupPermissions { get; set; }
        public DbSet<SystemNofication> SystemNofications { get; set; }
        public DbSet<AccountSession> AccountSessions { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Flight_Account> Flight_Accounts { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentType_Permission> DocumentType_Permissions { get; set; }
        public DbSet<FlightDocument> FlightDocuments { get; set; }
        public DbSet<FlightDocument_GroupPermission> FlightDocument_GroupPermissions { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account_GroupPermission>()
                .HasOne(ag => ag.GroupPermissionNavigation)
                .WithMany(gr => gr.Account_GroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<DocumentType_Permission>()
                .HasOne(dgp => dgp.GroupPermissionNavigation)
                .WithMany(gp => gp.DocumentType_Permissions)
                .HasForeignKey(dpg => dpg.GroupPermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightDocument>()
               .HasOne(fd => fd.DocumentTypeNavigation)
               .WithMany(dt => dt.FlightDocuments)
               .HasForeignKey(fd => fd.DocumentTypeId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightDocument>()
              .HasOne(fd => fd.FlightDocumentNavigation)
              .WithMany(dt => dt.FlightDocuments)
              .HasForeignKey(fd => fd.FlightDocumentIdFK)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightDocument_GroupPermission>()
             .HasOne(fdg => fdg.GroupPermissionNavigation)
             .WithMany(g => g.FlightDocument_GroupPermissions)
             .HasForeignKey(fd => fd.GroupPermissionId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
