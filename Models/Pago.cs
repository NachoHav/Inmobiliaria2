
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;

public class Pago
{
    [Key]
    public int IdPago { get; set; }

    [Display(Name = "N° Pago")]
    public int NumeroPago { get; set; }

    [Display(Name = "Importe")]
    public double Monto { get; set; }
    public DateTime Fecha { get; set; }

    [Display(Name = "Contrato")]
    public int ContratoId { get; set; }
    public Contrato? Contrato { get; set; }

    public string MontoFormat => "$" + Monto.ToString("N0");
}