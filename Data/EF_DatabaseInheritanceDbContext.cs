using EF_DatabaseInheritance.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_DatabaseInheritance.Data
{
    public class EF_DatabaseInheritanceDbContext : DbContext
    {

        public EF_DatabaseInheritanceDbContext(DbContextOptions<EF_DatabaseInheritanceDbContext> options) : base(options)
        {
        }

        // ---------------------- DbSets ----------------------
        // TPH - Payments
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<CardPayment> CardPayments { get; set; } = null!;
        public DbSet<UPIPayment> UpiPayments { get; set; } = null!;
        public DbSet<WalletPayment> WalletPayments { get; set; } = null!;

        // TPT - Users
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Seller> Sellers { get; set; } = null!;
        public DbSet<AdminUser> AdminUsers { get; set; } = null!;

        // TPC - Notifications
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<EmailNotification> EmailNotifications { get; set; } = null!;
        public DbSet<SmsNotification> SmsNotifications { get; set; } = null!;
        public DbSet<PushNotification> PushNotifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // ---------------------- TPH: Payments ---------------------

            modelBuilder.Entity<Payment>(entity =>
            {

                entity.ToTable("Payments");

                // Configure discriminator column
                entity.HasDiscriminator<string>("PaymentType")
                      .HasValue<CardPayment>("Card")
                      .HasValue<UPIPayment>("UPI")
                      .HasValue<WalletPayment>("Wallet");

                // Property configurations
                entity.Property(p => p.Amount)
                      .HasPrecision(18, 2)
                      .IsRequired();

                entity.Property(p => p.Currency)
                     .HasMaxLength(10)
                     .IsRequired();

                entity.Property(p => p.PaymentGateway)
                   .HasMaxLength(50)
                   .IsRequired();

                entity.Property(p => p.PaymentStatus)
                     .HasConversion<string>()
                     .HasMaxLength(20)
                     .IsRequired();

                entity.Property(p => p.TransactionId)
                     .HasMaxLength(100);

               CLSCompliantAttribute entity.Property(p => p.Remarks)
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<WalletPayment>(entity =>
            {

                entity.Property(w => w.WalletBalanceUsed)
                      .HasPrecision(18, 2)
                      .IsRequired();

                entity.Property(w => w.CashbackReceived)
                      .HasPrecision(18, 2);
            });

            // ---------------------- TPT: Users ----------------------
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Customer>().ToTable("Customers");

            modelBuilder.Entity<Seller>().ToTable("Sellers");

            modelBuilder.Entity<AdminUser>().ToTable("AdminUsers");

            // Base user configuration

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Email)
                     .IsRequired()
                     .HasMaxLength(100);

                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.PasswordHash)
                     .IsRequired()
                     .HasMaxLength(255);

                entity.HasIndex(u => u.Email)
                     .IsUnique()
                     .HasDatabaseName("IX_Users_Email");
            });

            // Customer-specific configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(c => c.PhoneNumber)
                     .HasMaxLength(15);

                entity.Property(c => c.ProfilePictureUrl)
                      .HasMaxLength(500);

                entity.Property(c => c.ShippingAddress)
                      .HasMaxLength(300);

                entity.Property(c => c.BillingAddress)
                      .HasMaxLength(300);
            });

            // Seller-specific configuration
            modelBuilder.Entity<Seller>(entity =>
            {
                entity.Property(s => s.BusinessName)
                       .HasMaxLength(150)
                       .IsRequired();

                entity.Property(s => s.GSTNumber)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(s => s.PANNumber)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(s => s.WarehouseAddress)
                      .HasMaxLength(300)
                      .IsRequired();

                entity.Property(s => s.BusinessLicenseNo)
                      .HasMaxLength(50);

                entity.Property(s => s.BankAccountNumber)
                      .HasMaxLength(30);

            });

            // AdminUser-specific configuration
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.Property(a => a.RoleName)
                     .HasMaxLength(50)
                     .IsRequired();

                entity.Property(a => a.Department)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(a => a.Permissions)
                      .HasColumnType("nvarchar(max)");
            });

            // ---------------------- TPC: Notifications ----------------------
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.UseTpcMappingStrategy();

                entity.Property(n => n.Message)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(n => n.Priority)
                      .HasMaxLength(20)
                      .HasDefaultValue("Normal");

                entity.Property(n => n.SentAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(n => n.SentBy)
                      .HasMaxLength(100);
            });

            // Email Notification
            modelBuilder.Entity<EmailNotification>(entity =>
            {
                entity.ToTable("EmailNotifications");

                entity.Property(e => e.RecipientEmail)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.Subject)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Cc)
                      .HasMaxLength(150);

                entity.Property(e => e.Bcc)
                      .HasMaxLength(150);
            });

            // SMS Notification
            modelBuilder.Entity<SmsNotification>(entity =>
            {
                entity.ToTable("SmsNotifications");

                entity.Property(s => s.RecipientNumber)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(s => s.SenderId)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(s => s.Provider)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);
            });

            // Push Notification
            modelBuilder.Entity<PushNotification>(entity =>
            {
                entity.ToTable("PushNotifications");

                entity.Property(p => p.DeviceToken)
                      .IsRequired()
                      .HasMaxLength(250);

                entity.Property(p => p.AppPlatform)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(p => p.AppVersion)
                      .HasMaxLength(20);
            });

        }

    }
   
}
