using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalParticular
    {
        private readonly DbConnection dbConnection;

        public DalParticular()
        {
            dbConnection = new DbConnection();
        }

        public List<Particular> Select()
        {
            var particularList = new List<Particular>();

            try
            {
                using (var conn = dbConnection.GetConnection())
                {
                    using (var cmd = new SqlCommand("SELECT * FROM Particular", conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                particularList.Add(new Particular
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    Dni = reader.GetString(1),
                                    Nombre = reader.GetString(2),
                                    Apellidos = reader.GetString(3),
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

            return particularList;
        }

        public Particular? SelectById(int id)
        {
            Particular? particular = null;

            try
            {
                using (var conn = dbConnection.GetConnection())
                {
                    using (var cmd = new SqlCommand("SELECT * FROM Particular WHERE IdUsuario = @IdUsuario", conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", id);
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                particular = new Particular
                                {
                                    IdUsuario = reader.GetInt32(0),
                                    Dni = reader.GetString(1),
                                    Nombre = reader.GetString(2),
                                    Apellidos = reader.GetString(3),
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

            return particular;
        }

        public void Add(Particular particular)
        {
            if (particular == null)
                throw new ArgumentNullException(nameof(particular));

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO Particular (IdUsuario, Dni, Nombre, Apellidos) VALUES (@IdUsuario, @Dni, @Nombre, @Apellidos)", connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", particular.IdUsuario);
                        command.Parameters.AddWithValue("@Dni", particular.Dni);
                        command.Parameters.AddWithValue("@Nombre", particular.Nombre);
                        command.Parameters.AddWithValue("@Apellidos", particular.Apellidos);
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

        public void Update(Particular particular)
        {
            if (particular == null)
                throw new ArgumentNullException(nameof(particular));

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Particular SET Dni = @Dni, Nombre = @Nombre, Apellidos = @Apellidos WHERE IdUsuario = @IdUsuario", connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", particular.IdUsuario);
                        command.Parameters.AddWithValue("@Dni", particular.Dni);
                        command.Parameters.AddWithValue("@Nombre", particular.Nombre);
                        command.Parameters.AddWithValue("@Apellidos", particular.Apellidos);
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
                    using (var cmd = new SqlCommand("DELETE FROM Particular WHERE IdUsuario = @IdUsuario", conn))
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
