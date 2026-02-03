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
        // Testing if Create creates new car
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

        // Create Negative test
        [Fact]
        public async Task Create_EmptyName_ShouldFail()
        {
            // Arrange
            var context = TestDbFactory.Create();
            var service = new CarServices(context);

            var dto = new CarDto
            {
                Name = "",
                Model = "M5",
                Engine = "V8",
                Color = "Black",
                TireCount = 4
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.Create(dto));
        }

        // Create Negaive test #2
        [Fact]
        public async Task Create_NegativeTireCount_ShouldThrow()
        {
            var context = TestDbFactory.Create();
            var service = new CarServices(context);

            var dto = new CarDto
            {
                Name = "BMW",
                Model = "M5",
                Engine = "V8",
                Color = "Black",
                TireCount = -1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.Create(dto));
        }

        // Testing If details returns correct details
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

        // Testing if Updating Modifies correctly
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

        // Update negative test 2 in 1
        [Fact]
        public async Task Update_NegativeTireCount_ShouldThrow()
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
            await Task.Delay(5);

            var dto = new CarDto
            {
                Id = car.Id,
                CreatedAt = car.CreatedAt,
                Name = "",
                Model = "Coro",
                Engine = "",
                Color = "White",
                TireCount = -1
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.Update(dto));
        }

        // Testing if Delete Removes the car
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

        // Testing Id
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
