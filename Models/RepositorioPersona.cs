using MySql.Data.MySqlClient;

namespace test.Models;

public class RepositorioPersona
{
    protected readonly string connectionString;

    public RepositorioPersona()
    {
        connectionString = "Server=localhost;User=root;Password='';Database=test;SslMode=none";
    }

    public List<Persona> ObtenerPersonas()
    {
        var res = new List<Persona>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT Id,Nombre,Email FROM Personas";
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new Persona
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Email = reader.GetString("Email")
                        });
                    }
                }
                connection.Close();
            }
        }
        return res;
    }

    public int Alta(Persona persona)
    {
        var res = -1;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO Personas(Nombre, Email) 
                        VALUES(@Nombre, @Email);
                        SELECT LAST_INSERT_ID()";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@Nombre", persona.Nombre);
                cmd.Parameters.AddWithValue("@Email", persona.Email);

                connection.Open();

                res = Convert.ToInt32(cmd.ExecuteScalar());
                persona.Id = res;

                connection.Close();
            }
        }
        return res;
    }

    // public int Baja(Persona)

}
