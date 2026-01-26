using Xunit;
using Car.ApplicationServices.Services;
using Car.Core.Dto;
using Car.Data;
using System;
using System.Threading.Tasks;


namespace CarTest
{
    public class CarTests
    {
        // #1 Testing if Create creates new car
        [Fact]
        public async Task Create_ShouldCreateCar()
        {
            // Arrange
            var context = TestDbFactory.Create();
            var service = new CarServices(context);

            var dto = new CarDto
            {
                Name = "BMW",
                Model = "M5",
                Engine = "V8",
                Color = "Black",
                TireCount = 4
            };

            // Act
            var result = await service.Create(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("BMW", result.Name);
            Assert.Equal("M5", result.Model);
            Assert.Equal("V8", result.Engine);
            Assert.Equal("Black", result.Color);
            Assert.Equal(4, result.TireCount);
            Assert.True(result.Id.HasValue);
            Assert.NotEqual(Guid.Empty, result.Id.Value);
        }

        // #2 Testing If details returns correct details
        [Fact]
        public async Task DetailAsync_ShouldReturnCar()
        {
            var context = TestDbFactory.Create();
            var service = new CarServices(context);

            var car = await service.Create(new CarDto
            {
                Name = "Audi",
                Model = "A6",
                Engine = "V6",
                Color = "Gray",
                TireCount = 4
            });

            var result = await service.DetailAsync(car.Id!.Value);

            Assert.NotNull(result);
            Assert.Equal("Audi", result.Name);
        }

        // #3 Testing if Updating Modifies correctly
        [Fact]
        public async Task Update_ShouldModifyCar()
        {
            var context = TestDbFactory.Create();
            var service = new CarServices(context);

            var car = await service.Create(new CarDto
            {
                Name = "Toyota",
                Model = "Corolla",
                Engine = "Hybrid",
                Color = "White",
                TireCount = 4
            });

            var oldModified = car.ModifiedAt;

            // Little delay
            await Task.Delay(5);

            var updated = await service.Update(new CarDto
            {
                Id = car.Id,
                Name = "Toyota",
                Model = "Supra",
                Engine = "Turbo",
                Color = "Red",
                TireCount = 4,
                CreatedAt = car.CreatedAt
            });

            Assert.Equal("Supra", updated.Model);
            Assert.Equal("Turbo", updated.Engine);
            Assert.Equal("Red", updated.Color);
            Assert.True(updated.ModifiedAt > oldModified);
        }


        // #4 Testing if Delete Removes the car
        [Fact]
        public async Task Delete_ShouldRemoveCar()
        {
            var context = TestDbFactory.Create();
            var service = new CarServices(context);

            var car = await service.Create(new CarDto
            {
                Name = "Ford",
                Model = "Focus",
                Engine = "EcoBoost",
                Color = "Blue",
                TireCount = 4
            });

            await service.Delete(car.Id!.Value);

            var result = await service.DetailAsync(car.Id.Value);

            Assert.Null(result);
        }

        // #5 Testing Id
        [Fact]
        public async Task DetailAsync_UnknownId_ReturnsNull()
        {
            var context = TestDbFactory.Create();
            var service = new CarServices(context);

            var result = await service.DetailAsync(Guid.NewGuid());

            Assert.Null(result);
        }

    }
}
