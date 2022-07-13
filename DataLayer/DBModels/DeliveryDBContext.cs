using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataLayer.DBModels
{
    public partial class DeliveryDBContext : DbContext
    {
        public DeliveryDBContext()
        {
        }

        public DeliveryDBContext(DbContextOptions<DeliveryDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ConsistOf> ConsistOfs { get; set; }
        public virtual DbSet<Deliverer> Deliverers { get; set; }
        public virtual DbSet<Iuser> Iusers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Purchaser> Purchasers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-RA6QVFS;Database=DeliveryDB;Trusted_Connection=True;");
            }

            //optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Admin");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.UserId)
                    .HasConstraintName("FK_IUser_Admin");
            });

            modelBuilder.Entity<ConsistOf>(entity =>
            {
                entity.ToTable("ConsistOf");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ConsistOfs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsistOf_Product");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.ConsistOfs)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsistOf_Purchase");
            });

            modelBuilder.Entity<Deliverer>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Deliverer");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.ApprovedFromNavigation)
                    .WithMany(p => p.Deliverers)
                    .HasForeignKey(d => d.ApprovedFrom)
                    .HasConstraintName("FK_Deliverer_Admin");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Deliverer)
                    .HasForeignKey<Deliverer>(d => d.UserId)
                    .HasConstraintName("FK_Deliverer_IUser");
            });

            modelBuilder.Entity<Iuser>(entity =>
            {
                entity.ToTable("IUser");

                entity.HasIndex(e => e.Email, "IX_IUser_Email")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "IX_IUser_Username")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsFixedLength(true);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsFixedLength(true);

                entity.Property(e => e.PicturePath)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsFixedLength(true);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Ingredients)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.HasOne(d => d.AddedByNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.AddedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Admin");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");

                entity.Property(e => e.Comment)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.DeliverToAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasOne(d => d.DeliveredByNavigation)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.DeliveredBy)
                    .HasConstraintName("FK_Purchase_Deliverer");

                entity.HasOne(d => d.DeliveredToNavigation)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.DeliveredTo)
                    .HasConstraintName("FK_Purchase_Purchaser");
            });

            modelBuilder.Entity<Purchaser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Purchaser");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Purchaser)
                    .HasForeignKey<Purchaser>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchaser_IUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
