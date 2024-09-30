using CarService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarService.Data
{  
    public class DBContext : IdentityDbContext<User>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        
        public DbSet<Car> Cars { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<RepairRecord> RepairRecords { get; set; }
    }
}