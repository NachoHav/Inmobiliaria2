using System.Data;
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
            var sql = "SELECT IdPropietario,Nombre,Apellido, Dni, Telefono, Email, Clave FROM Propietarios WHERE Estado = 1";
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

    public Propietario ObtenerPropietario(int id)
    {
        var res = new Propietario();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave 
					FROM Propietarios
					WHERE IdPropietario=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res = new Propietario
                    {
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Dni = reader.GetString("Dni"),
                        Telefono = reader.GetString("Telefono"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                    };
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

    public int Baja(int id)
    {
        var res = -1;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = "UPDATE Propietarios SET Estado = 0 WHERE IdPropietario = @id";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                res = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    // public int Baja(int id)
    // 	{
    // 		int res = -1;
    // 		using (SqlConnection connection = new SqlConnection(connectionString))
    // 		{
    // 			string sql = "DELETE FROM Propietarios WHERE IdPropietario = @id";
    // 			using (SqlCommand command = new SqlCommand(sql, connection))
    // 			{
    // 				command.CommandType = CommandType.Text;
    // 				command.Parameters.AddWithValue("@id", id);
    // 				connection.Open();
    // 				res = command.ExecuteNonQuery();
    // 				connection.Close();
    // 			}
    // 		}
    // 		return res;
    // 	}

    public int Modificacion(Propietario p)
    {
        var res = -1;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = "UPDATE Propietarios SET Nombre = @Nombre, Apellido = @Apellido, Dni = @Dni, Telefono = @Telefono, Email = @Email WHERE Id = @Id";
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", p.IdPropietario);
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", p.Apellido);
                cmd.Parameters.AddWithValue("@Dni", p.Dni);
                cmd.Parameters.AddWithValue("@Telefono", p.Telefono);
                cmd.Parameters.AddWithValue("@Email", p.Email);
                connection.Open();
                res = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }


}
