using MySql.Data.MySqlClient;

namespace test.Models;

public class RepositorioPropietario
{
    protected readonly string connectionString;

    public RepositorioPropietario()
    {
        connectionString = "Server=localhost;User=root;Password='';Database=test;SslMode=none";
    }

    public List<Propietario> ObtenerPropietarios()
    {
        var res = new List<Propietario>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT IdPropietario,Nombre,Apellido, Dni, Telefono, Email, Clave FROM Propietarios";
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new Propietario
                        {
                            IdPropietario = reader.GetInt32("IdPropietario"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Clave = reader.GetString("Clave")
                        });
                    }
                }
                connection.Close();
            }
        }
        return res;
    }

    public int Alta(Propietario propietario)
    {
        var res = -1;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO Propietarios(Nombre, Apellido, Dni, Telefono, Email, Clave) 
                        VALUES(@Nombre, @Apellido, @Dni, @Telefono, @Email, @Clave);
                        SELECT LAST_INSERT_ID()";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                cmd.Parameters.AddWithValue("@Dni", propietario.Dni);
                cmd.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                cmd.Parameters.AddWithValue("@Email", propietario.Email);
                cmd.Parameters.AddWithValue("@Clave", propietario.Clave);

                connection.Open();

                res = Convert.ToInt32(cmd.ExecuteScalar());
                propietario.IdPropietario = res;

                connection.Close();
            }
        }
        return res;
    }

    // public int Baja(Persona)

}
