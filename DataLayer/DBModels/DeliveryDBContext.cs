using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<ConsistOf> ConsistOf { get; set; }
        public virtual DbSet<Deliverer> Deliverer { get; set; }
        public virtual DbSet<Iuser> Iuser { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<Purchaser> Purchaser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-RA6QVFS;Database=DeliveryDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.UserId)
                    .HasConstraintName("FK_IUser_Admin");
            });

            modelBuilder.Entity<ConsistOf>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ConsistOf)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsistOf_Product");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.ConsistOf)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsistOf_Purchase");
            });

            modelBuilder.Entity<Deliverer>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.ApprovedFromNavigation)
                    .WithMany(p => p.Deliverer)
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

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .IsUnique();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsFixedLength();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsFixedLength();

                entity.Property(e => e.PicturePath)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsFixedLength();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Product)
                    .HasForeignKey<Product>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Admin");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.Comment)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.DeliverTo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.DeliveredByNavigation)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.DeliveredBy)
                    .HasConstraintName("FK_Purchase_Deliverer");

                entity.HasOne(d => d.DeliveredToNavigation)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.DeliveredTo)
                    .HasConstraintName("FK_Purchase_Purchaser");
            });

            modelBuilder.Entity<Purchaser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Purchaser)
                    .HasForeignKey<Purchaser>(d => d.UserId)
                    .HasConstraintName("FK_Purchaser_Purchaser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
