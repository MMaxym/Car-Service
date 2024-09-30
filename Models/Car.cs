using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models;
using System.ComponentModel.DataAnnotations;
public class Car
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Make { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Model { get; set; }
    
    public int Year { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Number { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Vin { get; set; }
    
    public int Mileage { get; set; }
    
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public ICollection<RepairRecord> RepairRecords { get; set; }
}
