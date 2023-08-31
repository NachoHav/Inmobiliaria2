
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;

public class Pago
{
    public int IdPago { get; set; }
    public double Monto { get; set; }
    public DateTime Fecha { get; set; }
    public int InquilinoId { get; set; }
    public int PropiedadId { get; set; }
    public int ContratoId { get; set; }

}