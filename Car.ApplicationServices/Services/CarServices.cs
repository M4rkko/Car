using Microsoft.EntityFrameworkCore;
using Car.Core.Domain;
using Car.Core.Dto;
using Car.Core.ServiceInterface;
using Car.Data;

namespace Car.ApplicationServices.Services
{
    public class CarServices : ICarServices
    {
        private readonly CarDbContext _context;

        public CarServices(CarDbContext context)
        {
            _context = context;
        }

        public async Task<Car.Core.Domain.Car> Create(CarDto dto)
        {
            Car.Core.Domain.Car car = new();

            car.Id = Guid.NewGuid();
            car.Name = dto.Name;
            car.Model = dto.Model;
            car.Engine = dto.Engine;
            car.CreatedAt = DateTime.Now;
            car.ModifiedAt = DateTime.Now;

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car.Core.Domain.Car> DetailAsync(Guid id)
        {
            var result = await _context.Cars
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Car.Core.Domain.Car> Update(CarDto dto)
        {
            Car.Core.Domain.Car domain = new();

            domain.Id = dto.Id;
            domain.Name = dto.Name;
            domain.Model = dto.Model;
            domain.Engine = dto.Engine;

            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = DateTime.Now;

            _context.Cars.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Car.Core.Domain.Car> Delete(Guid id)
        {
            var result = await _context.Cars
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Cars.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
