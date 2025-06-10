using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium1b.Models;

// WAŻNE: Dla klucza złożonego, najlepiej skonfigurować go w DbContext.OnModelCreating
// Ale tutaj dodajemy atrybuty dla kolumn.
// Aby EF Core rozpoznało klucz złożony, musisz dodać do VisitService:
// [PrimaryKey(nameof(VisitId), nameof(ServiceId))]
// lub skonfigurować w DbContext.OnModelCreating:
// modelBuilder.Entity<VisitService>().HasKey(vs => new { vs.ServiceId, vs.VisitId });
// Zgodnie z Twoim schematem SQL: CONSTRAINT Visit_Service_pk PRIMARY KEY (service_id,visit_id)

public class VisitService
{
    // Klucze obce i część klucza złożonego
    [Column("visit_id")]
    public int VisitId { get; set; }
    
    [Column("service_id")]
    public int ServiceId { get; set; }
    
    // Właściwość mapowana na kolumnę 'service_fee'
    [Column("service_fee")]
    public decimal ServiceFee { get; set; }
    
    // Właściwości nawigacyjne
    [ForeignKey("VisitId")]
    public virtual Visit Visit { get; set; } = null!; // Ostrzeżenie CS8618
    
    [ForeignKey("ServiceId")]
    public virtual Service Service { get; set; } = null!; // Ostrzeżenie CS8618
}