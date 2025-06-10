using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium1b.Models;

public class Mechanic
{
    // Klucz główny mapowany na kolumnę 'mechanic_id'
    [Key]
    [Column("mechanic_id")]
    public int MechanicId { get; set; }
    
    // Właściwości mapowane na kolumny 'first_name', 'last_name', 'licence_number'
    [Column("first_name")]
    public string FirstName { get; set; } = null!; // Ostrzeżenie CS8618
    
    [Column("last_name")]
    public string LastName { get; set; } = null!; // Ostrzeżenie CS8618
    
    [Column("licence_number")]
    public string LicenceNumber { get; set; } = null!; // Ostrzeżenie CS8618
    
    // Właściwość nawigacyjna do powiązanych wizyt
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}