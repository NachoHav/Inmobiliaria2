
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;

public class Contrato
{

    [Key]
    [Display(Name = "N° Contrato")]
    public int IdContrato { get; set; }

    [Display(Name = "Fecha de Inicio")]
    public DateTime FechaInicio { get; set; }

    [Display(Name = "Fecha de Fin")]
    public DateTime FechaFin { get; set; }

    [Display(Name = "Propiedad")]
    public int PropiedadId { get; set; }
    public Propiedad? Propiedad { get; set; }

    [Display(Name = "Inquilino")]
    public int InquilinoId { get; set; }
    public Inquilino? Inquilino { get; set; }

    public int Estado { get; set; }

    public string FechaInicioFormato => FechaInicio.ToString("dd/MM/yyyy");
    public string FechaFinFormato => FechaFin.ToString("dd/MM/yyyy");

}