




using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace EcoNet.DAL
{
    public class DalUsuario
    {
        private readonly DbConnection dbConnection;

        public DalUsuario()
        {
            dbConnection = new DbConnection();
        }

        public List<Usuario> Select()
        {
            List<Usuario> usuarioList = new List<Usuario>();

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM Usuario", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarioList.Add(new Usuario
                    {
                        IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                        NombreUsuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                        FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                        FechaBaja = reader.IsDBNull(reader.GetOrdinal("FechaBaja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaBaja")),
                        Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Municipio = reader.IsDBNull(reader.GetOrdinal("Municipio")) ? null : reader.GetString(reader.GetOrdinal("Municipio")),
                        EsAdmin = reader.GetBoolean(reader.GetOrdinal("EsAdmin")),
                        FotoPerfil = reader.IsDBNull(reader.GetOrdinal("FotoPerfil")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("FotoPerfil"))
                    });
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Select: {ex.Message}");
                throw;
            }
          
            return usuarioList;
        }
        public string? AutenticationUserDal(string Email, string Password)
        {
            string? nombreUsuario = null;

            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT NombreUsuario FROM Usuario WHERE Email = @Email AND Contrasena = @Password", conn);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario"));
                }
                reader.Close();
                conn.Close();
            }


       
            return nombreUsuario;
        }

        public Usuario? SelectById(int id)
        {
            Usuario? usuario = null;

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario", conn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                        NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                        Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                        FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                        FechaBaja = reader.IsDBNull(reader.GetOrdinal("FechaBaja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaBaja")),
                        Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Municipio = reader.IsDBNull(reader.GetOrdinal("Municipio")) ? null : reader.GetString(reader.GetOrdinal("Municipio")),
                        EsAdmin = reader.GetBoolean(reader.GetOrdinal("EsAdmin")),
                        FotoPerfil = reader.IsDBNull(reader.GetOrdinal("FotoPerfil")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("FotoPerfil"))
                    };
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectById: {ex.Message}");
                throw;
            }
          
            return usuario;
        }

        public void Add(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Usuario (Usuario1, Contraseña, FechaAlta, FechaBaja, Telefono, Email, Municipio, EsAdmin, FotoPerfil) VALUES (@Usuario1, @Contraseña, @FechaAlta, @FechaBaja, @Telefono, @Email, @Municipio, @EsAdmin, @FotoPerfil)", connection);
                command.Parameters.AddWithValue("@Usuario1", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                command.Parameters.AddWithValue("@FechaBaja", (object)usuario.FechaBaja ?? DBNull.Value);
                command.Parameters.AddWithValue("@Telefono", (object)usuario.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Municipio", (object)usuario.Municipio ?? DBNull.Value);
                command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                command.Parameters.AddWithValue("@FotoPerfil", (object)usuario.FotoPerfil ?? DBNull.Value);
                command.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Add: {ex.Message}");
                throw;
            }


        }

        public void Update(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand("UPDATE Usuario SET Usuario1 = @Usuario1, Contraseña = @Contraseña, FechaAlta = @FechaAlta, FechaBaja = @FechaBaja, Telefono = @Telefono, Email = @Email, Municipio = @Municipio, EsAdmin = @EsAdmin, FotoPerfil = @FotoPerfil WHERE IdUsuario = @IdUsuario", connection);
                command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                command.Parameters.AddWithValue("@Usuario1", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                command.Parameters.AddWithValue("@FechaBaja", (object)usuario.FechaBaja ?? DBNull.Value);
                command.Parameters.AddWithValue("@Telefono", (object)usuario.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Municipio", (object)usuario.Municipio ?? DBNull.Value);
                command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                command.Parameters.AddWithValue("@FotoPerfil", (object)usuario.FotoPerfil ?? DBNull.Value);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Update: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using var conn = dbConnection.GetConnection();
                conn.Open();
                using var cmd = new SqlCommand("DELETE FROM Usuario WHERE IdUsuario = @IdUsuario", conn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Delete: {ex.Message}");
                throw;
            }
           
        }
    }
}
