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
            List<Particular> particularList = new List<Particular>();
            using var conn = dbConnection.GetConnection();
            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Particular", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    particularList.Add(new Particular
                    {
                        IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                        Dni = reader.GetString(reader.GetOrdinal("Dni")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
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

            return particularList;
        }

        public Particular SelectById(int id)
        {
            Particular particular = null;
            using var conn = dbConnection.GetConnection();
            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Particular WHERE IdUsuario = @IdUsuario", conn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    particular = new Particular
                    {
                        IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                        Dni = reader.GetString(reader.GetOrdinal("Dni")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                    };
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            finally 
            { 
                conn.Close();
            }
            return particular;
        }

        public void Add(Particular particular)
        {
            if (particular == null)
                throw new ArgumentNullException(nameof(particular));

            using var connection = dbConnection.GetConnection();

            try
            {
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Particular (IdUsuario, Dni, Nombre, Apellidos) VALUES (@IdUsuario, @Dni, @Nombre, @Apellidos)", connection);
                command.Parameters.AddWithValue("@IdUsuario", particular.IdUsuario);
                command.Parameters.AddWithValue("@Dni", particular.Dni);
                command.Parameters.AddWithValue("@Nombre", particular.Nombre);
                command.Parameters.AddWithValue("@Apellidos", particular.Apellidos);
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

        public void Update(Particular particular)
        {
            if (particular == null)
                throw new ArgumentNullException(nameof(particular));

            using var connection = dbConnection.GetConnection();
            try
            {
                
                connection.Open();
                using var command = new SqlCommand("UPDATE Particular SET Dni = @Dni, Nombre = @Nombre, Apellidos = @Apellidos WHERE IdUsuario = @IdUsuario", connection);
                command.Parameters.AddWithValue("@IdUsuario", particular.IdUsuario);
                command.Parameters.AddWithValue("@Dni", particular.Dni);
                command.Parameters.AddWithValue("@Nombre", particular.Nombre);
                command.Parameters.AddWithValue("@Apellidos", particular.Apellidos);
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
                using var cmd = new SqlCommand("DELETE FROM Particular WHERE IdUsuario = @IdUsuario", conn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                cmd.ExecuteNonQuery();
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
