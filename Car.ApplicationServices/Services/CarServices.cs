using Microsoft.EntityFrameworkCore;
using Car.Core.Domain;
using Car.Core.Dto;
using Car.Core.ServiceInterface;
using Car.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required", nameof(dto.Name));
            
            if (string.IsNullOrWhiteSpace(dto.Model))
                throw new ArgumentException("Model is required", nameof(dto.Name));
            
            if (string.IsNullOrWhiteSpace(dto.Engine))
                throw new ArgumentException("Engine is required", nameof(dto.Name));
            
            if (string.IsNullOrWhiteSpace(dto.Color))
                throw new ArgumentException("Color is required", nameof(dto.Name));
            
            if (dto.TireCount == null || dto.TireCount <= 0)
                throw new ArgumentException("TireCount must be > 0", nameof(dto.TireCount));

            Car.Core.Domain.Car car = new();

            car.Id = Guid.NewGuid();
            car.Name = dto.Name;
            car.Model = dto.Model;
            car.Engine = dto.Engine;
            car.Color = dto.Color;
            car.TireCount = dto.TireCount ?? 0;
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
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (car == null)
                return null;

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required", nameof(dto.Name));

            if (string.IsNullOrWhiteSpace(dto.Model))
                throw new ArgumentException("Model is required", nameof(dto.Name));
            
            if (string.IsNullOrWhiteSpace(dto.Engine))
                throw new ArgumentException("Engine is required", nameof(dto.Name));
            
            if (string.IsNullOrWhiteSpace(dto.Color))
                throw new ArgumentException("Color is required", nameof(dto.Name));
            
            if (dto.TireCount == null || dto.TireCount <= 0)
                throw new ArgumentException("TireCount must be > 0", nameof(dto.TireCount));


            car.Id = dto.Id;
            car.Name = dto.Name;
            car.Model = dto.Model;
            car.Engine = dto.Engine;
            car.Color = dto.Color;
            car.TireCount = dto.TireCount ?? 0;

            car.CreatedAt = dto.CreatedAt;
            car.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return car;
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
