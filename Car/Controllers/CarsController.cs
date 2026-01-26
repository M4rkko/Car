using Microsoft.AspNetCore.Mvc;
using Car.Core.Dto;
using Car.Core.ServiceInterface;
using Car.Data;
using Car.Models.Cars;

namespace Car.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarDbContext _context;
        private readonly ICarServices _carServices;

        public CarsController(CarDbContext context, ICarServices carServices)
        {
            _context = context;
            _carServices = carServices;
        }

        public IActionResult Index()
        {
            var result = _context.Cars
                .Select(x => new CarsIndexViewModel
                {
                    Id = x.Id ?? Guid.Empty,
                    Name = x.Name,
                    Model = x.Model,
                    Engine = x.Engine,
                    Color = x.Color,
                    TireCount = x.TireCount
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CarsCreateUpdateViewModel car = new();
            return View("CreateUpdate", car);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarsCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("CreateUpdate", vm);

            var dto = new CarDto
            {
                Name = vm.Name,
                Model = vm.Model,
                Engine = vm.Engine,
                Color = vm.Color,
                TireCount = vm.TireCount,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            await _carServices.Create(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var car = await _carServices.DetailAsync(id);

            if (car == null)
                return NotFound();

            var vm = new CarsDetailsViewModel
            {
                Name = car.Name,
                Model = car.Model,
                Engine = car.Engine,
                Color = car.Color,
                TireCount = car.TireCount,
                CreatedAt = car.CreatedAt,
                ModifiedAt = car.ModifiedAt
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var car = await _carServices.DetailAsync(id);

            if (car == null)
                return NotFound();

            var vm = new CarsCreateUpdateViewModel
            {
                Id = car.Id,
                Name = car.Name,
                Model = car.Model,
                Engine = car.Engine,
                Color = car.Color,
                TireCount = car.TireCount,
                CreatedAt = car.CreatedAt,
                ModifiedAt = car.ModifiedAt
            };

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CarsCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("CreateUpdate", vm);
            var dto = new CarDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Model = vm.Model,
                Engine = vm.Engine,
                Color = vm.Color,
                TireCount = vm.TireCount,
                ModifiedAt = vm.ModifiedAt
            };
            await _carServices.Update(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await _carServices.DetailAsync(id);

            if (car == null)
                return NotFound();

            var vm = new CarsDeleteViewModel
            {
                Id = car.Id ?? Guid.Empty,
                Name = car.Name,
                Model = car.Model,
                Engine = car.Engine,
                Color = car.Color,
                TireCount = car.TireCount,
                CreatedAt = car.CreatedAt,
                ModifiedAt = car.ModifiedAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _carServices.Delete(id);

            if (result == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
    }
}
