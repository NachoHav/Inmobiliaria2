using System.Data;
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

    public Inquilino ObtenerPorId(int id)
    {
        Inquilino inquilino = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email 
					FROM Inquilinos
					WHERE IdInquilino=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Dni = reader.GetString("Dni"),
                        Telefono = reader.GetString("Telefono"),
                        Email = reader.GetString("Email"),
                    };
                }
                connection.Close();
            }
        }
        return inquilino;
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

    public int Baja(int id)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"DELETE FROM Inquilinos WHERE IdInquilino = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Editar(Inquilino inquilino)
    {
        var res = -1;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE Inquilinos 
                    SET Nombre = @Nombre, Apellido = @Apellido, Dni = @Dni, 
                        Telefono = @Telefono, Email = @Email 
                    WHERE IdInquilino = @IdInquilino";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@IdInquilino", inquilino.IdInquilino);
                cmd.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                cmd.Parameters.AddWithValue("@Dni", inquilino.Dni);
                cmd.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                cmd.Parameters.AddWithValue("@Email", inquilino.Email);

                connection.Open();

                res = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int ObtenerNumeroTotalInquilinos()
    {
        int totalInquilinos = 0;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = "SELECT COUNT(*) FROM Inquilinos";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                connection.Open();
                totalInquilinos = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
        }

        return totalInquilinos;
    }

}
