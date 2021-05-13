using Cursed.Enttities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed
{
    public class DB: DbContext
    {
        public DbSet<PartOfWarehouse> PartOfWarehouses { get; set; }
        public DbSet<Product>  Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<DepartNote> DepartNotes { get; set; }
        public DbSet<DeliveryNote> DeliveryNotes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sb = new SqlConnectionStringBuilder();
            sb.DataSource = @"WIN-0DJJ09S5CB3";
            sb.InitialCatalog = "CursedSkladq";
            sb.IntegratedSecurity = true;
            optionsBuilder.UseSqlServer(sb.ToString());
            base.OnConfiguring(optionsBuilder);
        }

        private DB()
        {
            Database.EnsureCreated();
        }


        static DB db;
        public static DB GetDb()
        {
            if (db == null)
                db = new DB();
            return db;
        }
    }
}
