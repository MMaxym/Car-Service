namespace CarService.Models;
using System.ComponentModel.DataAnnotations;
public class Master : User
{
    [Required]
    [StringLength(100)]
    public string Specialization { get; set; }
}
