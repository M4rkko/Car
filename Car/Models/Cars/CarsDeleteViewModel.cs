namespace Car.Models.Cars
{
    public class CarsDeleteViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Model { get; set; }
        public string? Engine { get; set; }
        public string? Color { get; set; }
        public int TireCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
