using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Entity Framework Core database context for Vehicle Showroom Management System
    /// Implements Unit of Work pattern
    /// </summary>
    public class VehicleShowroomDbContext : DbContext, IUnitOfWork
    {
        public VehicleShowroomDbContext(DbContextOptions<VehicleShowroomDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Model> Models { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<VehicleImage> VehicleImages { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
        public DbSet<SalesOrder> SalesOrders { get; set; } = null!;
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<ServiceOrder> ServiceOrders { get; set; } = null!;
        public DbSet<ServiceOrderItem> ServiceOrderItems { get; set; } = null!;

        // IUnitOfWork implementation
        public async Task<int> SaveChangesAsync()
        {
            // Set audit fields for entities being added or modified
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await Database.RollbackTransactionAsync();
        }

        // Model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and constraints
            ConfigureEntities(modelBuilder);
            ConfigureSoftDelete(modelBuilder);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            // User-Role many-to-many relationship
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Brand-Model one-to-many relationship
            modelBuilder.Entity<Model>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.Models)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Model-Vehicle one-to-many relationship
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Model)
                .WithMany(m => m.Vehicles)
                .HasForeignKey(v => v.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vehicle-VehicleImage one-to-many relationship
            modelBuilder.Entity<VehicleImage>()
                .HasOne(vi => vi.Vehicle)
                .WithMany(v => v.VehicleImages)
                .HasForeignKey(vi => vi.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer-SalesOrder one-to-many relationship
            modelBuilder.Entity<SalesOrder>()
                .HasOne(so => so.Customer)
                .WithMany(c => c.SalesOrders)
                .HasForeignKey(so => so.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // User-SalesOrder one-to-many relationship (SalesPerson)
            modelBuilder.Entity<SalesOrder>()
                .HasOne(so => so.SalesPerson)
                .WithMany(u => u.SalesOrders)
                .HasForeignKey(so => so.SalesPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // SalesOrder-SalesOrderItem one-to-many relationship
            modelBuilder.Entity<SalesOrderItem>()
                .HasOne(soi => soi.SalesOrder)
                .WithMany(so => so.SalesOrderItems)
                .HasForeignKey(soi => soi.SalesOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Vehicle-SalesOrderItem one-to-many relationship
            modelBuilder.Entity<SalesOrderItem>()
                .HasOne(soi => soi.Vehicle)
                .WithMany(v => v.SalesOrderItems)
                .HasForeignKey(soi => soi.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // SalesOrder-Invoice one-to-many relationship
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.SalesOrder)
                .WithMany(so => so.Invoices)
                .HasForeignKey(i => i.SalesOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice-Payment one-to-many relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Supplier-PurchaseOrder one-to-many relationship
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrder-PurchaseOrderItem one-to-many relationship
            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderItems)
                .HasForeignKey(poi => poi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Vehicle-PurchaseOrderItem one-to-many relationship
            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.Vehicle)
                .WithMany(v => v.PurchaseOrderItems)
                .HasForeignKey(poi => poi.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vehicle-ServiceOrder one-to-many relationship
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Vehicle)
                .WithMany(v => v.ServiceOrders)
                .HasForeignKey(so => so.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer-ServiceOrder one-to-many relationship
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Customer)
                .WithMany(c => c.ServiceOrders)
                .HasForeignKey(so => so.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceOrder-ServiceOrderItem one-to-many relationship
            modelBuilder.Entity<ServiceOrderItem>()
                .HasOne(soi => soi.ServiceOrder)
                .WithMany(so => so.ServiceOrderItems)
                .HasForeignKey(soi => soi.ServiceOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure string lengths and types
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.VIN)
                .HasMaxLength(17)
                .IsRequired();

            // Configure decimal precision
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Price)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<SalesOrderItem>()
                .Property(soi => soi.UnitPrice)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Subtotal)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TaxAmount)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasColumnType("decimal(12,2)");
        }

        private void ConfigureSoftDelete(ModelBuilder modelBuilder)
        {
            // Configure soft delete for all entities that implement ISoftDelete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
                }
            }
        }

        private LambdaExpression ConvertFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var filter = Expression.Lambda(Expression.Not(property), parameter);

            return filter;
        }
    }
}