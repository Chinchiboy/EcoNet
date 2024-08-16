﻿




using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace EcoNet.DAL
{
    public class DalUsuario
    {
        private readonly DbConnection dbConnection;
        private readonly Hash HashSSHA;

        public DalUsuario()
        {
            dbConnection = new DbConnection();
        }

        public List<Usuario> Select()
        {
            List<Usuario> usuarioList = new List<Usuario>();

            using var conn = dbConnection.GetConnection();

            try
            {
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
                        Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Municipio = reader.GetString(reader.GetOrdinal("Municipio")),
                        EsAdmin = reader.GetBoolean(reader.GetOrdinal("EsAdmin")),
                        FotoPerfil = reader.IsDBNull(reader.GetOrdinal("FotoPerfil")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("FotoPerfil"))
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                conn.Close();
            }
          
            return usuarioList;
        }
        public string? AutenticationUserDal(string username, string password)
        {
            string nombreUsuario = null;
            string storedHash = null;

            try
            {
                using var conn = dbConnection.GetConnection();
                conn.Open();

                using var cmd = new SqlCommand("SELECT Usuario, Contrasena FROM Usuario WHERE Usuario = @NombreUsuario", conn);
                cmd.Parameters.AddWithValue("@NombreUsuario", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario"));
                        storedHash = reader.GetString(reader.GetOrdinal("Contrasena"));
                    }
                    reader.Close();


                }
                conn.Close();
                
                if (storedHash != null)
                {
                    if (HashSSHA.VerifySSHA256Hash(password, storedHash))
                    {
                        return nombreUsuario;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AutenticationUserDal: {ex.Message}");
                throw;
            }

            return null;
        }

        public Usuario? SelectById(int id)
        {
            Usuario? usuario = null;

            using var conn = dbConnection.GetConnection();

            try
            {
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
                        Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Municipio = reader.GetString(reader.GetOrdinal("Municipio")),
                        EsAdmin = reader.GetBoolean(reader.GetOrdinal("EsAdmin")),
                        FotoPerfil = reader.IsDBNull(reader.GetOrdinal("FotoPerfil")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("FotoPerfil"))
                    };
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectById: {ex.Message}");
                throw;
            }
            finally
            {
                conn.Close();
            }
          
            return usuario;
        }

        public void Add(Usuario usuario)
        {
            byte[] salt = HashSSHA.GenerateSalt();
            string hashedPassword = HashSSHA.CreateSSHA256Hash(usuario.Contraseña, salt);
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            using var connection = dbConnection.GetConnection();

            try
            {
                
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Usuario (Usuario, Contrasena, FechaAlta, FechaBaja, Telefono, Email, Municipio, EsAdmin, FotoPerfil) VALUES (@Usuario1, @Contraseña, @FechaAlta, @FechaBaja, @Telefono, @Email, @Municipio, @EsAdmin, @FotoPerfil)", connection);
                command.Parameters.AddWithValue("@Usuario1", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Contraseña", hashedPassword);
                command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                command.Parameters.AddWithValue("@FechaBaja", (object)usuario.FechaBaja ?? DBNull.Value);
                command.Parameters.AddWithValue("@Telefono", (object)usuario.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Municipio", usuario.Municipio);
                command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                command.Parameters.AddWithValue("@FotoPerfil", (object)usuario.FotoPerfil ?? DBNull.Value);
                command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
             
            }
            finally
            {
                connection.Close();
            }


        }

        public void Update(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            using var connection = dbConnection.GetConnection();
            try
            {
                connection.Open();
                using var command = new SqlCommand("UPDATE Usuario SET Usuario = @Usuario1, Contrasena = @Contraseña, FechaAlta = @FechaAlta, FechaBaja = @FechaBaja, Telefono = @Telefono, Email = @Email, Municipio = @Municipio, EsAdmin = @EsAdmin, FotoPerfil = @FotoPerfil WHERE IdUsuario = @IdUsuario", connection);
                command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                command.Parameters.AddWithValue("@Usuario1", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Contrasena", usuario.Contraseña);
                command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                command.Parameters.AddWithValue("@FechaBaja", (object)usuario.FechaBaja ?? DBNull.Value);
                command.Parameters.AddWithValue("@Telefono", (object)usuario.Telefono ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Municipio", usuario.Municipio);
                command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                command.Parameters.AddWithValue("@FotoPerfil", (object)usuario.FotoPerfil ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using var conn = dbConnection.GetConnection();
            try
            {
                conn.Open();
                using var cmd = new SqlCommand("DELETE FROM Usuario WHERE IdUsuario = @IdUsuario", conn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
