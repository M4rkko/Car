using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car.Core.Domain;
using Car.Core.Dto;

namespace Car.Core.ServiceInterface
{
    public interface ICarServices
    {
        Task<Car.Core.Domain.Car> Create(CarDto dto);
        Task<Car.Core.Domain.Car> DetailAsync(Guid id);
        Task<Car.Core.Domain.Car> Update(CarDto dto);
        Task<Car.Core.Domain.Car> Delete(Guid id);

    }
}
