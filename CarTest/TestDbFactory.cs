using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car.Data;
using Microsoft.EntityFrameworkCore;

namespace CarTest
{
    public static class TestDbFactory
    {
        public static CarDbContext Create()
        {
            var options = new DbContextOptionsBuilder<CarDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new CarDbContext(options);
        }
    }
}
