
using System.ComponentModel.DataAnnotations;
namespace test.Models;
public class Propietario
{


    [Key]
    [Display(Name = "Código Int.")]
    public int IdPropietario { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido { get; set; }
    [Required]
    public string Dni { get; set; }
    [Required]
    public string Telefono { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Clave { get; set; }

}