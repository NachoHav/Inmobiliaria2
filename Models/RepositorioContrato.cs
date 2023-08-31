using System.Data;
using MySql.Data.MySqlClient;

namespace test.Models;

public class RepositorioContrato
{
    protected readonly string connectionString;

    public RepositorioContrato()
    {
        connectionString = "Server=localhost;User=root;Password='';Database=test;SslMode=none";
    }


    public int Alta(Contrato contrato)
    {
        int res = -1;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"INSERT INTO Contratos 
                    (FechaInicio, FechaFin, PropiedadId, InquilinoId)
                    VALUES (@fechaInicio, @fechaFin, @propiedadId, @inquilinoId);
                    SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@propiedadId", contrato.PropiedadId);
                command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                contrato.IdContrato = res;
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
            string sql = @$"UPDATE Contratos SET Estado = 0 WHERE IdContrato = @id";
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

    public int Editar(Contrato contrato)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"UPDATE Contratos SET 
            FechaInicio = @fechaInicio, FechaFin = @fechaFin, PropiedadId = @propiedadId, 
            InquilinoId = @inquilinoId WHERE IdContrato = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@propiedadId", contrato.PropiedadId);
                command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                command.Parameters.AddWithValue("@id", contrato.IdContrato);
                command.CommandType = CommandType.Text;
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public List<Contrato> ObtenerContratos()
    {
        var res = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT IdContrato, FechaInicio, FechaFin, PropiedadId, InquilinoId, c.Estado,
            i.IdInquilino, i.Nombre as inquilinoNombre, i.Apellido as inquilinoApellido, p.IdPropiedad, p.Nombre as propiedadNombre, p.Direccion as propiedadDireccion
            FROM Contratos c
            INNER JOIN Propiedades p ON c.PropiedadId = p.IdPropiedad 
            INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino
            WHERE c.Estado = 1";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32("IdContrato"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            PropiedadId = reader.GetInt32("PropiedadId"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Estado = reader.GetInt32("Estado"),
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32("IdInquilino"),
                                Nombre = reader.GetString("inquilinoNombre"),
                                Apellido = reader.GetString("inquilinoApellido"),
                            },
                            Propiedad = new Propiedad
                            {
                                IdPropiedad = reader.GetInt32("IdPropiedad"),
                                Nombre = reader.GetString("propiedadNombre"),
                                Direccion = reader.GetString("propiedadDireccion"),
                            }
                        };
                        res.Add(contrato);
                    }
                }
                connection.Close();
            }
        }
        return res;
    }

    public Contrato ObtenerPorId(int id)
    {
        Contrato contrato = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"SELECT c.IdContrato, c.FechaInicio, c.FechaFin, c.PropiedadId, c.InquilinoId, c.Estado,
                            i.Nombre as inquilinoNombre, i.Apellido as inquilinoApellido,
                            p.Nombre as propiedadNombre, p.Direccion as propiedadDireccion
                            FROM Contratos c
                            INNER JOIN Propiedades p ON c.PropiedadId = p.IdPropiedad 
                            INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino
                            WHERE {nameof(Contrato.IdContrato)}=@id AND c.Estado = 1";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    contrato = new Contrato
                    {
                        IdContrato = reader.GetInt32("IdContrato"),
                        FechaInicio = reader.GetDateTime("FechaInicio"),
                        FechaFin = reader.GetDateTime("FechaFin"),
                        PropiedadId = reader.GetInt32("PropiedadId"),
                        InquilinoId = reader.GetInt32("InquilinoId"),
                        Estado = reader.GetInt32("Estado"),
                        Inquilino = new Inquilino
                        {
                            IdInquilino = reader.GetInt32("IdInquilino"),
                            Nombre = reader.GetString("inquilinoNombre"),
                            Apellido = reader.GetString("inquilinoApellido"),
                        },
                        Propiedad = new Propiedad
                        {
                            IdPropiedad = reader.GetInt32("IdPropiedad"),
                            Nombre = reader.GetString("propiedadNombre"),
                            Direccion = reader.GetString("propiedadDireccion"),
                        }
                    };
                }
                connection.Close();
            }
        }
        return contrato;
    }

    //BUSCAR POR INQUILINO
    //BUSCAR POR PROPIEDAD
    //BUSCAR POR FECHAS
    //BUSCAR POR ESTADO = 0
}