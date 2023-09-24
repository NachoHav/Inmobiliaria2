using System.Data;
using MySql.Data.MySqlClient;

namespace test.Models;

public class RepositorioPago
{
    protected readonly string connectionString;

    public RepositorioPago()
    {
        connectionString = "Server=localhost;User=root;Password='';Database=test;SslMode=none";
    }

    public int Alta(Pago pago)
    {
        int res = -1;

        using (var connection = new MySqlConnection(connectionString))
        {
            // Consulta para obtener el número de pago máximo para el contrato
            string maxNumeroPagoQuery = "SELECT MAX(NumeroPago) FROM Pagos WHERE ContratoId = @contratoId";

            using (var maxNumeroPagoCommand = new MySqlCommand(maxNumeroPagoQuery, connection))
            {
                maxNumeroPagoCommand.Parameters.AddWithValue("@contratoId", pago.ContratoId);
                connection.Open();

                // Obtener el número de pago máximo
                object maxNumeroPagoObj = maxNumeroPagoCommand.ExecuteScalar();
                int maxNumeroPago = DBNull.Value.Equals(maxNumeroPagoObj) ? 0 : Convert.ToInt32(maxNumeroPagoObj);

                // Calcular el próximo número de pago
                int nuevoNumeroPago = maxNumeroPago + 1;

                // Insertar el nuevo pago con el número de pago calculado
                string insertQuery = @"INSERT INTO Pagos (NumeroPago, Monto, Fecha, ContratoId)
                                        VALUES (@numeroPago, @monto, @fecha, @contratoId);
                                        SELECT LAST_INSERT_ID();";

                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@numeroPago", nuevoNumeroPago);
                    insertCommand.Parameters.AddWithValue("@monto", pago.Monto);
                    insertCommand.Parameters.AddWithValue("@fecha", pago.Fecha);
                    insertCommand.Parameters.AddWithValue("@contratoId", pago.ContratoId);

                    res = Convert.ToInt32(insertCommand.ExecuteScalar());
                    pago.IdPago = res;
                }

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
            string sql = @$"UPDATE Pagos SET Estado = 0 WHERE IdPago = @id";
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

    public int Editar(Pago pago)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"UPDATE Pagos SET 
                        NumeroPago = @numeroPago, Monto = @monto, Fecha = @fecha, ContratoId = @contratoId 
                        WHERE IdPago = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@numeroPago", pago.NumeroPago);
                command.Parameters.AddWithValue("@monto", pago.Monto);
                command.Parameters.AddWithValue("@fecha", pago.Fecha);
                command.Parameters.AddWithValue("@contratoId", pago.ContratoId);
                command.Parameters.AddWithValue("@id", pago.IdPago);
                command.CommandType = CommandType.Text;
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public List<Pago> ObtenerPagos(int idContrato)
    {
        var res = new List<Pago>();
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT p.IdPago, p.NumeroPago, p.Monto, p.Fecha, p.ContratoId
            FROM Pagos p
            WHERE ContratoId = @idContrato AND Estado = 1";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.Add("@idContrato", MySqlDbType.Int32).Value = idContrato;
                cmd.CommandType = CommandType.Text;
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pago pago = new Pago
                    {
                        IdPago = reader.GetInt32("IdPago"),
                        NumeroPago = reader.GetInt32("NumeroPago"),
                        Monto = reader.GetDouble("Monto"),
                        Fecha = reader.GetDateTime("Fecha"),
                        ContratoId = reader.GetInt32("ContratoId")
                        // Contrato = new Contrato
                        // {
                        //     IdContrato = reader.GetInt32("ContratoId"),
                        //     FechaInicio = reader.GetDateTime("FechaInicio"),
                        //     FechaFin = reader.GetDateTime("FechaFin"),
                        //     PropiedadId = reader.GetInt32("PropiedadId"),
                        //     InquilinoId = reader.GetInt32("InquilinoId"),
                        //     Estado = reader.GetInt32("Estado"),
                        //     Inquilino = new Inquilino
                        // }
                    };
                    res.Add(pago);
                };

                connection.Close();
            }
        }
        return res;
    }

    public Pago ObtenerPago(int idPago)
    {
        Pago res = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT IdPago, NumeroPago, Monto, Fecha, ContratoId
            FROM Pagos
            WHERE IdPago = @idPago AND Estado = 1";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.Add("@idPago", MySqlDbType.Int32).Value = idPago;
                cmd.CommandType = CommandType.Text;
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    res = new Pago
                    {
                        IdPago = reader.GetInt32("IdPago"),
                        NumeroPago = reader.GetInt32("NumeroPago"),
                        Monto = reader.GetDouble("Monto"),
                        Fecha = reader.GetDateTime("Fecha"),
                        ContratoId = reader.GetInt32("ContratoId")
                        // Contrato = new Contrato
                        // {
                        //     IdContrato = reader.GetInt32("ContratoId"),
                        //     FechaInicio = reader.GetDateTime("FechaInicio"),
                        //     FechaFin = reader.GetDateTime("FechaFin"),
                        //     PropiedadId = reader.GetInt32("PropiedadId"),
                        //     InquilinoId = reader.GetInt32("InquilinoId"),
                        //     Estado = reader.GetInt32("Estado"),
                        //     Inquilino = new Inquilino
                        // }
                    };

                };

                connection.Close();
            }
        }
        return res;
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

