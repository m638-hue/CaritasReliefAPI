using CaritasReliefAPI.Schema;
using Microsoft.EntityFrameworkCore;

namespace CaritasReliefAPI.DBContext
{
    public class SQLContext : DbContext
    {
        public DbSet<Donantes> Donantes { get; set; }

        public DbSet<Recibos> Recibos { get; set; }

        public DbSet<Recolectores> Recolectores { get; set; }

        public DbSet<Logins> Logins { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public SQLContext(DbContextOptions<SQLContext> options) : base(options) 
        { 
        }
    }
}
