namespace cwiczenia12.DTOs 
{
    public class TripDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public List<CountryDto> Countries { get; set; } = new();
        public List<ClientDto> Clients { get; set; } = new();
    }

    public class CountryDto
    {
        public string Name { get; set; } = null!;
    }

    public class ClientDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }

    public class TripListResponseDto
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int AllPages { get; set; }
        public List<TripDto> Trips { get; set; } = new();
    }
}