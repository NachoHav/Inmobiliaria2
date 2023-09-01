
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;

public class Pago
{
    [Key]
    [Display(Name = "N° Pago")]
    public int IdPago { get; set; }
    public double Monto { get; set; }
    public DateTime Fecha { get; set; }

    [Display(Name = "Inquilino")]
    public int InquilinoId { get; set; }
    public Inquilino? Inquilino { get; set; }
    public int PropiedadId { get; set; }
    public Propiedad? Propiedad { get; set; }
    public int ContratoId { get; set; }
    public Contrato? Contrato { get; set; }

}