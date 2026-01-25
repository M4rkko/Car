using Microsoft.EntityFrameworkCore;
using DomainCar = Car.Core.Domain.Car;

namespace Car.Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options)
            : base(options)
        {
        }

        public DbSet<DomainCar> Cars { get; set; }
    }
}
