using MySql.Data.MySqlClient;

namespace test.Models;

public class RepositorioInquilino
{
    protected readonly string connectionString;

    public RepositorioInquilino()
    {
        connectionString = "Server=localhost;User=root;Password='';Database=test;SslMode=none";
    }

    public List<Inquilino> ObtenerInquilinos()
    {
        var res = new List<Inquilino>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT IdInquilino,Nombre,Apellido, Dni, Telefono, Email FROM Inquilinos";
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new Inquilino
                        {
                            IdInquilino = reader.GetInt32("IdInquilino"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email")
                        });
                    }
                }
                connection.Close();
            }
        }
        return res;
    }

    public int Alta(Inquilino inquilino)
    {
        var res = -1;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO Inquilinos(Nombre, Apellido, Dni, Telefono, Email) 
                        VALUES(@Nombre, @Apellido, @Dni, @Telefono, @Email);
                        SELECT LAST_INSERT_ID()";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                cmd.Parameters.AddWithValue("@Dni", inquilino.Dni);
                cmd.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                cmd.Parameters.AddWithValue("@Email", inquilino.Email);

                connection.Open();

                res = Convert.ToInt32(cmd.ExecuteScalar());
                inquilino.IdInquilino = res;

                connection.Close();
            }
        }
        return res;
    }

    // public int Baja(Persona)

}
