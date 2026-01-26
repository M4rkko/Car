using System.ComponentModel.DataAnnotations;

namespace Car.Models.Cars
{
    public class CarsCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        public string? Engine { get; set; }
        [Required]
        public string? Color { get; set; }
        [Required]
        public int? TireCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}

