using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium1b.Models;

public class Client
{
    // Klucz główny mapowany na kolumnę 'client_id'
    [Key]
    [Column("client_id")]
    public int ClientId { get; set; }
    
    // Właściwości mapowane na kolumny 'first_name', 'last_name', 'date_of_birth'
    [Column("first_name")]
    public string FirstName { get; set; } = null!; // Ostrzeżenie CS8618: Inicjalizacja lub 'null!' dla typów referencyjnych
    
    [Column("last_name")]
    public string LastName { get; set; } = null!; // Ostrzeżenie CS8618
    
    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }
    
    // Właściwość nawigacyjna do powiązanych wizyt
    // Używamy 'virtual' dla Entity Framework Core do obsługi lazy loading (jeśli włączone)
    // Inicjalizujemy kolekcję, aby uniknąć NullReferenceException
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}