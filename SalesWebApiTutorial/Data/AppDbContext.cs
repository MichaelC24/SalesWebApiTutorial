﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebApiTutorial.Models;

namespace SalesWebApiTutorial.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Items> Items { get; set; } = default!;
        public DbSet<Orderline> OrderLines { get; set; } = default!;

    }
}
