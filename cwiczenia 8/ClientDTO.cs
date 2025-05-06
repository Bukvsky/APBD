namespace Tutorial8.Models.DTOs
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
    }

    public class CreateClientDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
    }

    public class ClientTripDTO
    {
        public int IdClient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ClientTripDetailsDTO> Trips { get; set; } = new List<ClientTripDetailsDTO>();
    }

    public class ClientTripDetailsDTO
    {
        public int IdTrip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Price { get; set; }
        public DateTime RegisteredAt { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public List<CountryDTO> Countries { get; set; } = new List<CountryDTO>();
    }
}
