using System.Data;
using MySql.Data.MySqlClient;

namespace test.Models;


public class RepositorioPropiedad
{
    protected readonly string connectionString;

    public RepositorioPropiedad()
    {
        connectionString = "Server=localhost;User=root;Password='';Database=test;SslMode=none";
    }


    public int Alta(Propiedad propiedad)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"INSERT INTO Propiedades 
					(nombre, Descripcion, Precio, Direccion, Habitaciones, Banos, Area, PropietarioId)
					VALUES (@nombre, @descripcion, @precio, @direccion, @habitaciones, @banos, @area, @propietarioId);
					SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", propiedad.Nombre);
                command.Parameters.AddWithValue("@descripcion", propiedad.Descripcion);
                command.Parameters.AddWithValue("@precio", propiedad.Precio);
                command.Parameters.AddWithValue("@direccion", propiedad.Direccion);
                command.Parameters.AddWithValue("@habitaciones", propiedad.Habitaciones);
                command.Parameters.AddWithValue("@banos", propiedad.Banos);
                command.Parameters.AddWithValue("@area", propiedad.Area);
                command.Parameters.AddWithValue("@propietarioId", propiedad.PropietarioId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                propiedad.IdPropiedad = res;
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
            string sql = @$"UPDATE Propiedades SET Estado = 0 WHERE IdPropiedad = @id";
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

    public int Editar(Propiedad propiedad)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "UPDATE Propiedades SET " +
            "Nombre=@nombre, Descripcion=@descripcion, Precio=@precio, Direccion=@direccion, Habitaciones=@habitaciones, Banos=@banos, Area=@area, PropietarioId=@propietarioId " +
            "WHERE IdPropiedad = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@nombre", propiedad.Nombre);
                command.Parameters.AddWithValue("@descripcion", propiedad.Descripcion);
                command.Parameters.AddWithValue("@precio", propiedad.Precio);
                command.Parameters.AddWithValue("@direccion", propiedad.Direccion);
                command.Parameters.AddWithValue("@habitaciones", propiedad.Habitaciones);
                command.Parameters.AddWithValue("@banos", propiedad.Banos);
                command.Parameters.AddWithValue("@area", propiedad.Area);
                command.Parameters.AddWithValue("@propietarioId", propiedad.PropietarioId);
                command.Parameters.AddWithValue("@id", propiedad.IdPropiedad);
                command.CommandType = CommandType.Text;
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }


    public List<Propiedad> ObtenerPropiedades()
    {
        var res = new List<Propiedad>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT IdPropiedad, prop.nombre, Descripcion, Precio, Direccion, Habitaciones, Banos, Area, PropietarioId,
					p.Nombre as propietarioNombre, p.Apellido
					FROM Propiedades prop INNER JOIN Propietarios p ON prop.PropietarioId = p.IdPropietario
                    WHERE prop.Estado = 1";
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propiedad propiedad = new Propiedad
                        {
                            IdPropiedad = reader.GetInt32("IdPropiedad"),
                            Nombre = reader.GetString("Nombre"),
                            Descripcion = reader.GetString("Descripcion"),
                            Precio = reader.GetDecimal("Precio"),
                            Direccion = reader.GetString("Direccion"),
                            Habitaciones = reader.GetInt32("Habitaciones"),
                            Banos = reader.GetInt32("Banos"),
                            Area = reader.GetDouble("Area"),
                            PropietarioId = reader.GetInt32("PropietarioId"),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("propietarioNombre"),
                                Apellido = reader.GetString("Apellido"),
                            }
                        };
                        res.Add(propiedad);
                    }
                }
                connection.Close();
            }
        }
        return res;
    }


    public Propiedad ObtenerPorId(int id)
    {
        Propiedad propiedad = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
					SELECT IdPropiedad, prop.Nombre, Descripcion, Precio, Direccion, Habitaciones, Banos, Area, PropietarioId, p.Nombre as propietarioNombre, p.Apellido
					FROM Propiedades prop JOIN Propietarios p ON prop.PropietarioId = p.IdPropietario
					WHERE {nameof(Propiedad.IdPropiedad)}=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    propiedad = new Propiedad
                    {
                        IdPropiedad = reader.GetInt32(nameof(Propiedad.IdPropiedad)),
                        Nombre = reader.GetString("Nombre"),
                        Descripcion = reader.GetString("Descripcion"),
                        Precio = reader.GetDecimal("Precio"),
                        Direccion = reader.GetString("Direccion"),
                        Habitaciones = reader.GetInt32("Habitaciones"),
                        Banos = reader.GetInt32("Banos"),
                        Area = reader.GetDouble("Area"),
                        PropietarioId = reader.GetInt32("PropietarioId"),
                        Duenio = new Propietario
                        {
                            IdPropietario = reader.GetInt32("PropietarioId"),
                            Nombre = reader.GetString("propietarioNombre"),
                            Apellido = reader.GetString("Apellido"),
                        }
                    };
                }
                connection.Close();
            }
        }
        return propiedad;
    }

    public IList<Propiedad> BuscarPorPropietario(int idPropietario)
    {
        List<Propiedad> res = new List<Propiedad>();
        Propiedad propiedad = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
					SELECT {nameof(Propiedad.IdPropiedad)}, Nombre, Descripcion, Precio, Direccion, Habitaciones, Banos, Area, PropietarioId, p.Nombre, p.Apellido
					FROM Propiedades prop JOIN Propietarios p ON prop.PropietarioId = p.IdPropietario
					WHERE PropietarioId=@idPropietario";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@idPropietario", MySqlDbType.Int32).Value = idPropietario;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propiedad = new Propiedad
                    {
                        IdPropiedad = reader.GetInt32(nameof(Propiedad.IdPropiedad)),
                        Nombre = reader.GetString("Nombre"),
                        Descripcion = reader.GetString("Descripcion"),
                        Precio = reader.GetDecimal("Precio"),
                        Direccion = reader.GetString("Direccion"),
                        Habitaciones = reader.GetInt32("Habitaciones"),
                        Banos = reader.GetInt32("Banos"),
                        Area = reader.GetDouble("Area"),
                        PropietarioId = reader.GetInt32("PropietarioId"),
                        Duenio = new Propietario
                        {
                            IdPropietario = reader.GetInt32("PropietarioId"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                        }
                    };
                    res.Add(propiedad);
                }
                connection.Close();
            }
        }
        return res;
    }
}