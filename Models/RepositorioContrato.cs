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
                            i.Nombre as inquilinoNombre, i.Apellido as inquilinoApellido, i.IdInquilino,
                            p.Nombre as propiedadNombre, p.Direccion as propiedadDireccion, p.IdPropiedad
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

    public List<Contrato> ObtenerContratosPorFechas(DateTime? fechaInicio, DateTime? fechaFin)
    {
        var res = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT IdContrato, FechaInicio, FechaFin, PropiedadId, InquilinoId, c.Estado,
                    i.IdInquilino, i.Nombre as inquilinoNombre, i.Apellido as inquilinoApellido, 
                    p.IdPropiedad, p.Nombre as propiedadNombre, p.Direccion as propiedadDireccion
                    FROM Contratos c
                    INNER JOIN Propiedades p ON c.PropiedadId = p.IdPropiedad 
                    INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino
                    WHERE c.Estado = 1";

            if (fechaInicio != null)
            {
                sql += " AND FechaInicio <= @fechaFin";
            }

            if (fechaFin != null)
            {
                sql += " AND FechaFin >= @fechaInicio";
            }

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                if (fechaInicio != null)
                {
                    cmd.Parameters.Add("@fechaInicio", MySqlDbType.Date).Value = fechaInicio;
                }

                if (fechaFin != null)
                {
                    cmd.Parameters.Add("@fechaFin", MySqlDbType.Date).Value = fechaFin;
                }

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

    public List<Contrato> ObtenerContratosPropiedad(int idPropiedad)
    {
        var res = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT IdContrato, FechaInicio, FechaFin, PropiedadId, InquilinoId, c.Estado,
            i.IdInquilino, i.Nombre as inquilinoNombre, i.Apellido as inquilinoApellido, p.IdPropiedad, p.Nombre as propiedadNombre, p.Direccion as propiedadDireccion
            FROM Contratos c
            INNER JOIN Propiedades p ON c.PropiedadId = p.IdPropiedad 
            INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino
            WHERE c.Estado = 1 AND p.IdPropiedad = @idPropiedad";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.Add("@idPropiedad", MySqlDbType.Int32).Value = idPropiedad;
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


    public int ObtenerNumeroTotalContratosActivos()
    {
        int totalContratosActivos = 0;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = "SELECT COUNT(*) FROM Contratos WHERE Estado = 1";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                connection.Open();
                totalContratosActivos = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
        }

        return totalContratosActivos;
    }

    public int ObtenerNumeroContratosVencenProximamente(int meses)
    {
        int contratosVencenProximamente = 0;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = "SELECT COUNT(*) FROM Contratos WHERE Estado = 1 AND FechaFin >= CURDATE() AND FechaFin <= DATE_ADD(CURDATE(), INTERVAL @meses MONTH)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@meses", meses);
                connection.Open();
                contratosVencenProximamente = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
        }

        return contratosVencenProximamente;
    }

    //BUSCAR POR INQUILINO
    //BUSCAR POR PROPIEDAD
    //BUSCAR POR ESTADO = 0
}