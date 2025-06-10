using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kolokwium1a.Models;

public class Service
{
    [Key]
    [Column("service_id")]
    public int ServiceId { get; set; }
    
    [Required]
    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    [Column("base_fee",TypeName = "decimal(10,2)")]
    public decimal BaseFee { get; set; }
    
    public ICollection<AppointmentService> AppointmentServices { get; set; }
}