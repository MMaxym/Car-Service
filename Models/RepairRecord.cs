using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models;
using System.ComponentModel.DataAnnotations;
public class RepairRecord
{
    public int Id { get; set; }
    
    public DateTime ScheduledDate { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Status { get; set; }
    
    [Required]
    [StringLength(100)]
    public string RepairDescription { get; set; }
    
    public decimal Cost { get; set; }
    
    public int CarId { get; set; }
    [ForeignKey("CarId")]
    public Car Car { get; set; }
    
    public string MasterId { get; set; }
    [ForeignKey("MasterId")]
    public Master Master { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
