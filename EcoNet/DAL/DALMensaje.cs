using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalMensaje
    {
        private readonly DbConnection dbConnection;

        public DalMensaje()
        {
            dbConnection = new DbConnection();
        }
        public List<Mensaje> SelectChatUsers(int id)
        {
            List<Mensaje> mensajeList = new List<Mensaje>();

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT m.* FROM Mensaje m INNER JOIN Chat c ON m.FKChat = c.IdChat WHERE c.IdChat = @IdChat", conn);
                cmd.Parameters.AddWithValue("@IdChat", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensajeList.Add(new Mensaje
                    {
                        IdMensaje = reader.GetInt32(reader.GetOrdinal("IdMensaje")),
                        Texto = reader.GetString(reader.GetOrdinal("Texto")),
                        Fkchat = reader.IsDBNull(reader.GetOrdinal("FKChat")) ? null : reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Creador = reader.IsDBNull(reader.GetOrdinal("Creador")) ? null : reader.GetInt32(reader.GetOrdinal("Creador")),
                        HoraMensaje = reader.IsDBNull(reader.GetOrdinal("HoraMensaje")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("HoraMensaje")),
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

            return mensajeList;
        }
        public List<Mensaje> Select()
        {
            List<Mensaje> mensajeList = new List<Mensaje>();

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM Mensaje", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensajeList.Add(new Mensaje
                    {
                        IdMensaje = reader.GetInt32(reader.GetOrdinal("IdMensaje")),
                        Texto = reader.GetString(reader.GetOrdinal("Texto")),
                        Fkchat = reader.IsDBNull(reader.GetOrdinal("FKChat")) ? null : reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Creador = reader.IsDBNull(reader.GetOrdinal("Creador")) ? null : reader.GetInt32(reader.GetOrdinal("Creador")),
                        HoraMensaje = reader.IsDBNull(reader.GetOrdinal("HoraMensaje")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("HoraMensaje")),
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

            return mensajeList;
        }

        public Mensaje? SelectById(int id)
        {
            Mensaje? mensaje = null;

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM Mensaje WHERE IdMensaje = @IdMensaje", conn);
                cmd.Parameters.AddWithValue("@IdMensaje", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mensaje = new Mensaje
                    {
                        IdMensaje = reader.GetInt32(reader.GetOrdinal("IdMensaje")),
                        Texto = reader.GetString(reader.GetOrdinal("Texto")),
                        Fkchat = reader.IsDBNull(reader.GetOrdinal("FKChat")) ? null : reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Creador = reader.IsDBNull(reader.GetOrdinal("Creador")) ? null : reader.GetInt32(reader.GetOrdinal("Creador")),
                        HoraMensaje = reader.IsDBNull(reader.GetOrdinal("HoraMensaje")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("HoraMensaje")),
                    };
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectById: {ex.Message}");
                throw;
            }

            return mensaje;
        }

        public void Add(Mensaje mensaje)
        {
            if (mensaje == null)
                throw new ArgumentNullException(nameof(mensaje));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Mensaje (IdMensaje, Texto, Fkchat, Creador, HoraMensaje) VALUES (@IdMensaje, @Texto, @Fkchat, @Creador, @HoraMensaje)", connection);
                command.Parameters.AddWithValue("@IdMensaje", mensaje.IdMensaje);
                command.Parameters.AddWithValue("@Texto", mensaje.Texto);
                command.Parameters.AddWithValue("@Fkchat", mensaje.Fkchat ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Creador", mensaje.Creador ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@HoraMensaje", mensaje.HoraMensaje ?? (object)DBNull.Value);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Add: {ex.Message}");
                throw;
            }
        }

        public void Update(Mensaje mensaje)
        {
            if (mensaje == null)
                throw new ArgumentNullException(nameof(mensaje));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand("UPDATE Mensaje SET Texto = @Texto, Fkchat = @Fkchat, Creador = @Creador, HoraMensaje = @HoraMensaje WHERE IdMensaje = @IdMensaje", connection);
                command.Parameters.AddWithValue("@IdMensaje", mensaje.IdMensaje);
                command.Parameters.AddWithValue("@Texto", mensaje.Texto);
                command.Parameters.AddWithValue("@Fkchat", mensaje.Fkchat ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Creador", mensaje.Creador ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@HoraMensaje", mensaje.HoraMensaje ?? (object)DBNull.Value);
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
                using var cmd = new SqlCommand("DELETE FROM Mensaje WHERE IdMensaje = @IdMensaje", conn);
                cmd.Parameters.AddWithValue("@IdMensaje", id);
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
