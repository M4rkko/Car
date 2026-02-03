using System;
using System.Linq;
using System.Threading.Tasks;
using Car.ApplicationServices.Services;
using Car.Controllers;
using Car.Core.Domain;
using Car.Data;
using Car.Models.Cars;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CarTest.Controllers
{
    public class CarsControllerTests
    {
        private static CarsController CreateController(CarDbContext ctx)
        {
            var Service = new CarServices(ctx);
            return new CarsController(ctx, Service);
        }

        private static CarsCreateUpdateViewModel ValidVm() => new CarsCreateUpdateViewModel
        {
            Name = "BMW",
            Model = "M5",
            Engine = "V8",
            Color = "Black",
            TireCount = 4
        };

        // Returns
        [Fact]
        public async Task Index_ReturnsView_WithCarsFromDb()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            await controller.Create(ValidVm());
            await controller.Create(new CarsCreateUpdateViewModel
            {
                Name = "Audi",
                Model = "A6",
                Engine = "V6",
                Color = "Gray",
                TireCount = 4
            });

            var result = controller.Index();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IQueryable<CarsIndexViewModel>>(view.Model);

            var list = model.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, x => x.Name == "BMW" && x.Model == "M5");
            Assert.Contains(list, x => x.Name == "Audi" && x.Model == "A6");
        }

        // CREATE - invalid ModelState
        [Fact]
        public async Task Create_InvalidModelState_ReturnsCreateUpdateView_AndDoesNotInsert()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            controller.ModelState.AddModelError("Name", "Required");

            var vm = ValidVm();
            vm.Name = "";

            var result = await controller.Create(vm);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateUpdate", view.ViewName);
            Assert.Same(vm, view.Model);

            Assert.Equal(0, ctx.Cars.Count());
        }

        // CREATE - valid
        [Fact]
        public async Task Create_Valid_Redirects_AndInsertsCar()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            var result = await controller.Create(ValidVm());

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CarsController.Index), redirect.ActionName);

            var car = ctx.Cars.Single();
            Assert.Equal("BMW", car.Name);
            Assert.Equal("M5", car.Model);
            Assert.Equal("V8", car.Engine);
            Assert.Equal("Black", car.Color);
            Assert.Equal(4, car.TireCount);
            Assert.NotEqual(Guid.Empty, car.Id ?? Guid.Empty);
        }

        // DETAILS - negative
        [Fact]
        public async Task Details_UnknownId_ReturnsNotFound()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            var result = await controller.Details(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        // DETAILS - positive
        [Fact]
        public async Task Details_KnownId_ReturnsView_WithCorrectVm()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            await controller.Create(ValidVm());
            var id = ctx.Cars.Single().Id!.Value;

            var result = await controller.Details(id);

            var view = Assert.IsType<ViewResult>(result);
            var vm = Assert.IsType<CarsDetailsViewModel>(view.Model);

            Assert.Equal("BMW", vm.Name);
            Assert.Equal("M5", vm.Model);
        }

        // UPDATE - valid
        [Fact]
        public async Task Update_Valid_Redirects_AndUpdatesCar()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            await controller.Create(ValidVm());
            var car = ctx.Cars.Single();
            var id = car.Id!.Value;
            var oldModified = car.ModifiedAt;


            var updateVm = new CarsCreateUpdateViewModel
            {
                Id = id,
                Name = "BMW",
                Model = "M3",
                Engine = "I6",
                Color = "Blue",
                TireCount = 4,
                ModifiedAt = DateTime.Now 
            };


            await Task.Delay(5);

            var result = await controller.Update(updateVm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CarsController.Index), redirect.ActionName);

            var updated = ctx.Cars.Single();
            Assert.Equal("M3", updated.Model);
            Assert.Equal("I6", updated.Engine);
            Assert.Equal("Blue", updated.Color);
            Assert.True(updated.ModifiedAt > oldModified);
        }

        // UPDATE - Invalid
        [Fact]
        public async Task Update_InvalidModelState_ReturnsCreateUpdateView()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            await controller.Create(ValidVm());
            var car = ctx.Cars.Single();
            var oldModel = car.Model;

            controller.ModelState.AddModelError("Name", "Required");

            var vm = new CarsCreateUpdateViewModel
            {
                Id = car.Id,
                Name = "",
                Model = "NEW",
                Engine = "X",
                Color = "Red",
                TireCount = 4,
                ModifiedAt = DateTime.Now
            };

            var result = await controller.Update(vm);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateUpdate", view.ViewName);

            Assert.Equal(oldModel, ctx.Cars.Single().Model);
        }

        // DELETE CONFIRMATION - deletes
        [Fact]
        public async Task DeleteConfirmation_ExistingId_Redirects_AndRemovesCar()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            await controller.Create(ValidVm());
            var id = ctx.Cars.Single().Id!.Value;

            var result = await controller.DeleteConfirmation(id);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CarsController.Index), redirect.ActionName);

            Assert.Equal(0, ctx.Cars.Count());
        }
        
        // DELETE - invalid
        [Fact]
        public async Task Delete_UnknownId_ReturnsNotFound()
        {
            using var ctx = TestDbFactory.Create();
            var controller = CreateController(ctx);

            var result = await controller.Delete(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

    }
}
