using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;


public enum Roles
{
    Admin = 1,
    Empleado = 2
}

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Avatar { get; set; }
    public IFormFile AvatarFile { get; set; }
    public int Rol { get; set; }
    public string RolNombre => Rol > 0 ? ((Roles)Rol).ToString() : "";

    public static IDictionary<int, string> ObtenerRoles()
    {
        SortedDictionary<int, string> roles = new SortedDictionary<int, string>();

        Type tipo = typeof(Roles);

        foreach (var valor in Enum.GetValues(tipo))
        {
            roles.Add((int)valor, Enum.GetName(tipo, valor));
        }
        return roles;
    }
}