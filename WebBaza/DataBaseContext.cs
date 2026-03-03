using Microsoft.EntityFrameworkCore;
using System;
using WebBaza.Classes;


namespace WebBaza
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Product> Products { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) :
        base(options)
        {
            Database.EnsureCreated();
        }
    }
}
