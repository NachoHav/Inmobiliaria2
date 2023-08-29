
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;
public class Propiedad
{
    [Key]
    [Display(Name = "Código")]
    public int IdPropiedad { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Descripcion { get; set; }
    [Required]
    public decimal Precio { get; set; }
    [Required]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; }
    [Required]
    public int Habitaciones { get; set; }
    [Required]
    [Display(Name = "Baños")]
    public int Banos { get; set; }
    [Required]
    public double Area { get; set; }
    [Required]
    public bool Estado { get; set; }

    [Display(Name = "Dueño")]
    public int PropietarioId { get; set; }

    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }

    [Display(Name = "Precio")]
    public string PrecioFormateado => Precio.ToString("N0");
}