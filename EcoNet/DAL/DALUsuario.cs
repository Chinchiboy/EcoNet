using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace EcoNet.DAL
{
    public class DalUsuario
    {
        private readonly DbConnection dbConnection;
        private readonly Hash HashSSHA;

        public DalUsuario()
        {
            dbConnection = new DbConnection();
            HashSSHA = new Hash();
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
                        Contraseña = reader.GetString(reader.GetOrdinal("Contrasena")),
                        FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                        FechaBaja = reader.IsDBNull(reader.GetOrdinal("FechaBaja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaBaja")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Municipio = reader.GetString(reader.GetOrdinal("Municipio")),
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
        public Usuario? AutenticationUserDal(string email, string password)
        {
            Usuario? usuario = null;

            try
            {
                using var conn = dbConnection.GetConnection();
                conn.Open();

                using (var cmd = new SqlCommand("SELECT IdUsuario, Usuario, Contrasena, Email FROM Usuario WHERE Email = @Email AND Contrasena = @Password", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                NombreUsuario = reader.GetString(reader.GetOrdinal("Usuario")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AutenticationUserDal: {ex.Message}");
                throw;
            }

            return usuario;
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
                        NombreUsuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        Contraseña = reader.GetString(reader.GetOrdinal("Contrasena")),
                        FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                        FechaBaja = reader.IsDBNull(reader.GetOrdinal("FechaBaja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaBaja")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Municipio = reader.GetString(reader.GetOrdinal("Municipio")),
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
            byte[] salt = HashSSHA.GenerateSalt();
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Usuario (Usuario, Contrasena, FechaAlta, FechaBaja, Telefono, Email, Municipio, EsAdmin, FotoPerfil) VALUES (@Usuario1, @Contraseña, @FechaAlta, @FechaBaja, @Telefono, @Email, @Municipio, @EsAdmin, @FotoPerfil)", connection);
                command.Parameters.AddWithValue("@Usuario1", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                command.Parameters.AddWithValue("@FechaBaja", usuario.FechaBaja);
                command.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Municipio", usuario.Municipio);
                command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                command.Parameters.AddWithValue("@FotoPerfil", usuario.FotoPerfil);
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
                using var command = new SqlCommand("UPDATE Usuario SET Usuario = @Usuario1, Contrasena = @Contraseña, FechaAlta = @FechaAlta, FechaBaja = @FechaBaja, Telefono = @Telefono, Email = @Email, Municipio = @Municipio, EsAdmin = @EsAdmin, FotoPerfil = @FotoPerfil WHERE IdUsuario = @IdUsuario", connection);
                command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                command.Parameters.AddWithValue("@Usuario1", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Contrasena", usuario.Contraseña);
                command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                command.Parameters.AddWithValue("@FechaBaja", usuario.FechaBaja);
                command.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Municipio", usuario.Municipio);
                command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                command.Parameters.AddWithValue("@FotoPerfil", usuario.FotoPerfil);
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
