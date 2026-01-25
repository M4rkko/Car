using System;

namespace Car.Core.Dto
{
    public class CarDto
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }
        public string? Model { get; set; }
        public string? Engine { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
