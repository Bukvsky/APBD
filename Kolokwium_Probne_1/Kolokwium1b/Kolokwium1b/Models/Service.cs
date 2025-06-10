using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium1b.Models;

public class Service
{
    // Klucz główny mapowany na kolumnę 'service_id'
    [Key]
    [Column("service_id")]
    public int ServiceId { get; set; }
    
    // Właściwości mapowane na kolumny 'name', 'base_fee'
    [Column("name")]
    public string Name { get; set; } = null!; // Ostrzeżenie CS8618
    
    [Column("base_fee")]
    public decimal BaseFee { get; set; }
    
    // Właściwość nawigacyjna do powiązanych usług wizyt
    // Już była inicjalizowana, co jest dobre.
    public virtual ICollection<VisitService> VisitServices { get; set; } = new List<VisitService>();
}