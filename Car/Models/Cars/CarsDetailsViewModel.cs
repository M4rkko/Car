namespace Car.Models.Cars
{
    public class CarsDetailsViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Model { get; set; }
        public string? Engine { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
