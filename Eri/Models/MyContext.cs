using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Eri.Models;


namespace Eri.Models
{
    public class MyContext : DbContext
    {
        //public MyContext(DbContextOptions options) : base(options)
        //{

        //}

        public static string dbPath;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mydata.db");
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mydata.db");
            //optionsBuilder.UseSqlite($"Data Source=mydata.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
            SQLitePCL.Batteries_V2.Init();
        }
        public DbSet<Mst_Spend> Mst_Spend { get; set; }
        public DbSet<Mst_Income> Mst_Income { get; set; }
        public DbSet<Mst_Fixed> Mst_Fixed { get; set; }
        public DbSet<Mst_Available> Mst_Available { get; set; }
        public DbSet<Mst_User> Mst_User { get; set; }
        public DbSet<Tra_Spending> Tra_Spending { get; set; }
        public DbSet<Tra_Income> Tra_Income { get; set; }
    }
}