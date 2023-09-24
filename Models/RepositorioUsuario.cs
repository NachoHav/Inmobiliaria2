using System.Data;
using MySql.Data.MySqlClient;

namespace test.Models;


public class RepositorioUsuario
{
    protected readonly string connectionString;

    public RepositorioUsuario()
    {
        connectionString = "Server=localhost;User=root;Password='';Database=test;SslMode=none";
    }

    public int Alta(Usuario usuario)
    {
        int nuevoUsuarioId = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string sql = @"INSERT INTO Usuarios (Nombre, Apellido, Email, Password, Avatar, Rol)
                      VALUES (@Nombre, @Apellido, @Email, @Password, @Avatar, @Rol);
                      SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Password", usuario.Password);
                if (String.IsNullOrEmpty(usuario.Avatar))
                    command.Parameters.AddWithValue("@Avatar", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Avatar", usuario.Avatar);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);

                nuevoUsuarioId = Convert.ToInt32(command.ExecuteScalar());
                usuario.IdUsuario = nuevoUsuarioId;
                connection.Close();
            }
        }
        return nuevoUsuarioId;
    }

    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new(connectionString))
        {
            string sql = "UPDATE Usuarios SET Estado = 0 WHERE IdUsuario = @id";
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

    public int Editar(Usuario user)
    {
        int res = -1;
        using (MySqlConnection connection = new(connectionString))
        {
            string sql = @"UPDATE Usuarios SET Nombre = @nombre, Apellido = @apellido, 
                                Email = @email, Avatar = @avatar ,Rol = @rol WHERE IdUsuario = @id"; //Agregar Avatar aca
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", user.Nombre);
                command.Parameters.AddWithValue("@apellido", user.Apellido);
                command.Parameters.AddWithValue("@avatar", user.Avatar);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@rol", user.Rol);
                command.Parameters.AddWithValue("@id", user.IdUsuario);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public int CambiarContraseña(Usuario user)
    {
        int res = -1;
        using (MySqlConnection connection = new(connectionString))
        {
            string sql = @"UPDATE Usuarios SET Nombre = @nombre, Apellido = @apellido, 
                                Email = @email, Avatar = @avatar, Password = @password ,Rol = @rol WHERE IdUsuario = @id"; //Agregar Avatar aca
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", user.Nombre);
                command.Parameters.AddWithValue("@apellido", user.Apellido);
                command.Parameters.AddWithValue("@avatar", user.Avatar);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@rol", user.Rol);
                command.Parameters.AddWithValue("@id", user.IdUsuario);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public int CambiarAvatar(int id, string nuevoAvatar)
    {
        try
        {
            int res = -1;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE Usuarios SET Avatar = @nuevoAvatar WHERE IdUsuario = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nuevoAvatar", nuevoAvatar);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        catch (Exception ex)
        {

            throw;
        }
    }




    public IList<Usuario> ObtenerUsuarios()
    {
        IList<Usuario> res = new List<Usuario>();
        using (MySqlConnection connection = new(connectionString))
        {
            string sql = @"SELECT IdUsuario, Nombre, Apellido, Avatar, Email, Password, Rol
					FROM Usuarios WHERE Estado = 1";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Usuario user = new()
                    {
                        IdUsuario = reader.GetInt32("IdUsuario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader.IsDBNull("Avatar") ? null : reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password"),
                        Rol = reader.GetInt32("Rol"),
                    };
                    res.Add(user);
                }
                connection.Close();
            }
        }
        return res;
    }

    public Usuario ObtenerUsuario(int id)
    {
        Usuario usuario = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "SELECT IdUsuario, Nombre, Apellido, Avatar, Email, Password, Rol FROM Usuarios WHERE IdUsuario = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password"),
                        Rol = reader.GetInt32("Rol"),
                    };


                    if (!reader.IsDBNull(reader.GetOrdinal("Avatar")))
                    {
                        usuario.Avatar = reader.GetString("Avatar");
                    }
                }
                connection.Close();
            }
        }
        return usuario;
    }


    public Usuario ObtenerPorEmail(string email)
    {
        Usuario user = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT IdUsuario, Nombre, Apellido, Avatar, Email, Password, Rol
                FROM Usuarios WHERE Email = @email";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new Usuario
                    {
                        IdUsuario = reader.GetInt32("IdUsuario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password"),
                        Rol = reader.GetInt32("Rol"),
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("Avatar")))
                    {
                        user.Avatar = reader.GetString("Avatar");
                    }
                }
                connection.Close();
            }
        }
        return user;
    }

}