using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

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
            var usuarioList = new List<Usuario>();

            try
            {
                using (var conn = dbConnection.GetConnection())
                {
                    using (var cmd = new SqlCommand("SELECT * FROM Usuario", conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuarioList.Add(new Usuario
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    Usuario1 = reader.GetString(1),
                                    Contraseña = reader.GetString(2),
                                    FechaAlta = reader.GetDateTime(3),
                                    FechaBaja = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                    Telefono = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Email = reader.GetString(6),
                                    Municipio = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    EsAdmin = reader.GetBoolean(8),
                                    FotoPerfil = reader.IsDBNull(9) ? null : (byte[])reader.GetValue(9)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en Select: {ex.Message}");
                throw;
            }

            return usuarioList;
        }

        public Usuario? SelectById(int id)
        {
            Usuario? usuario = null;

            try
            {
                using (var conn = dbConnection.GetConnection())
                {
                    using (var cmd = new SqlCommand("SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario", conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", id);
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    Usuario1 = reader.GetString(1),
                                    Contraseña = reader.GetString(2),
                                    FechaAlta = reader.GetDateTime(3),
                                    FechaBaja = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                    Telefono = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Email = reader.GetString(6),
                                    Municipio = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    EsAdmin = reader.GetBoolean(8),
                                    FotoPerfil = reader.IsDBNull(9) ? null : (byte[])reader.GetValue(9)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
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
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO Usuario (Usuario1, Contraseña, FechaAlta, FechaBaja, Telefono, Email, Municipio, EsAdmin, FotoPerfil) VALUES (@Usuario1, @Contraseña, @FechaAlta, @FechaBaja, @Telefono, @Email, @Municipio, @EsAdmin, @FotoPerfil)", connection))
                    {
                        command.Parameters.AddWithValue("@Usuario1", usuario.Usuario1);
                        command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                        command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                        command.Parameters.AddWithValue("@FechaBaja", (object)usuario.FechaBaja ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Telefono", (object)usuario.Telefono ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", usuario.Email);
                        command.Parameters.AddWithValue("@Municipio", (object)usuario.Municipio ?? DBNull.Value);
                        command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                        command.Parameters.AddWithValue("@FotoPerfil", (object)usuario.FotoPerfil ?? DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
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
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Usuario SET Usuario1 = @Usuario1, Contraseña = @Contraseña, FechaAlta = @FechaAlta, FechaBaja = @FechaBaja, Telefono = @Telefono, Email = @Email, Municipio = @Municipio, EsAdmin = @EsAdmin, FotoPerfil = @FotoPerfil WHERE IdUsuario = @IdUsuario", connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                        command.Parameters.AddWithValue("@Usuario1", usuario.Usuario1);
                        command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                        command.Parameters.AddWithValue("@FechaAlta", usuario.FechaAlta);
                        command.Parameters.AddWithValue("@FechaBaja", (object)usuario.FechaBaja ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Telefono", (object)usuario.Telefono ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", usuario.Email);
                        command.Parameters.AddWithValue("@Municipio", (object)usuario.Municipio ?? DBNull.Value);
                        command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                        command.Parameters.AddWithValue("@FotoPerfil", (object)usuario.FotoPerfil ?? DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en Update: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("DELETE FROM Usuario WHERE IdUsuario = @IdUsuario", conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en Delete: {ex.Message}");
                throw;
            }
        }
    }
}
