using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium1b.Models;

public class Visit
{
    // Klucz główny mapowany na kolumnę 'visit_id'
    [Key]
    [Column("visit_id")]
    public int VisitId { get; set; }
    
    // Klucze obce mapowane na 'client_id' i 'mechanic_id'
    [Column("client_id")]
    public int ClientId { get; set; }
    
    [Column("mechanic_id")]
    public int MechanicId { get; set; }
    
    // Właściwość mapowana na kolumnę 'date'
    [Column("date")]
    public DateTime Date { get; set; }
    
    // Właściwości nawigacyjne do Client i Mechanic
    // Używamy [ForeignKey] do jawnego określenia kolumny klucza obcego
    // 'null!' wskazuje kompilatorowi, że ta właściwość zostanie zainicjalizowana przez EF Core
    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; } = null!; // Ostrzeżenie CS8618
    
    [ForeignKey("MechanicId")]
    public virtual Mechanic Mechanic { get; set; } = null!; // Ostrzeżenie CS8618
    
    // WAŻNA ZMIANA: Usunięto pojedynczą właściwość 'Service'.
    // Relacja jest jeden-do-wielu poprzez tabelę łączącą VisitService.
    // Powinna być tylko kolekcja VisitServices.
    public virtual ICollection<VisitService> VisitServices { get; set; } = new List<VisitService>();
}