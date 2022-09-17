using MainModuleContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MainModuleContext.Context
{
    public class MainModuleContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public MainModuleContext()
        {

        }
        public MainModuleContext(DbContextOptions<MainModuleContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var configuration = new ConfigurationBuilder()
                        .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                        .AddJsonFile("appsettings.json").Build();

              var conn=  configuration.GetValue<string>("ConnectionString");
              optionsBuilder.UseSqlServer(conn);
            }
        }
        public DbSet<Employee> Employees { get; set; }

    }
}
