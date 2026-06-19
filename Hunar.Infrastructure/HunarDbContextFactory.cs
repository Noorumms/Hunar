using Hunar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace Hunar.Infrastructure.Data
{
    // This class is ONLY used during migrations (design time)
    // EF Core looks for this automatically when you run Add-Migration
    // It tells EF how to build the DbContext without running the full app
    public class HunarDbContextFactory : IDesignTimeDbContextFactory<HunarDbContext>
    {
        public HunarDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HunarDbContext>();

            // Hardcode connection string just for migrations
            // This never runs in production — only during Add-Migration / Update-Database
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=HunarDb;Trusted_Connection=True;MultipleActiveResultSets=true"
            );

            return new HunarDbContext(optionsBuilder.Options);
        }
    }
}