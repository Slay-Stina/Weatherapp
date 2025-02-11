﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weatherapp.Models
{
    internal class WeatherDbContext : DbContext
    {
        public DbSet<TempEntity> TempEntities { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Weatherapp;Trusted_Connection=True; TrustServerCertificate=True;");
        }
    }
}
