using ConsTestEffectiveMobile.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsTestEffectiveMobile
{
    public class _DbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<CityDistrict> CityDistricts { get; set; } = null!;


        public _DbContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=emcnsltest.db");
        }

    }
}
