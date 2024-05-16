using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.Models;

namespace RMS.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Store> Stores { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<PurchaseOrderCart> PurchaseOrderCarts { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
		public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Number = "P001",
					Description = "Dummy Product 1",
					Group = "Group A",
					UOM = "Unit",
					Type = "Type X",
					AmountInStock = 100,
					StoreId = 2
				},
				new Product
				{
					Id = 2,
					Number = "P002",
					Description = "Dummy Product 2",
					Group = "Group B",
					UOM = "Unit",
					Type = "Type Y",
					AmountInStock = 150,
					StoreId = 1
				},
				new Product
				{
					Id = 3,
					Number = "P003",
					Description = "Dummy Product 3",
					Group = "Group C",
					UOM = "Unit",
					Type = "Type Z",
					AmountInStock = 200,
					StoreId = 1
				},
				new Product
				{
					Id = 4,
					Number = "P003",
					Description = "Dummy Product 3",
					Group = "Group C",
					UOM = "Unit",
					Type = "Type Z",
					AmountInStock = 200,
					StoreId = 2
				},
				new Product
				{
					Id = 5,
					Number = "P003",
					Description = "Dummy Product 3",
					Group = "Group C",
					UOM = "Unit",
					Type = "Type Z",
					AmountInStock = 200,
					StoreId = 3
				}

				);

			modelBuilder.Entity<Store>().HasData(
				new Store { Id = 1, Name = "Store1", Description = "Medical Shop 1" },
				new Store { Id = 2, Name = "Store2", Description = "Medical Shop 2" },
				new Store { Id = 3, Name = "Store3", Description = "Medical Shop 3" }
				);


		}
	}
}
