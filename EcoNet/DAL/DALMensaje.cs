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

        /**
         * <summary>
         * Gets all messages from a chat
         * </summary>
         * <returns>
         * A list if the messages could be retrived a null if an error happened
         * </returns>
         */ 
        public List<Mensaje>? GetMessagesFromChat(int idChat)
        {
            List<Mensaje> mensajeList = new List<Mensaje>();
            using var conn = dbConnection.GetConnection();
            try
            {
                
                using var cmd = new SqlCommand("SELECT m.* FROM Mensaje m WHERE m.FKChat = @IdChat", conn);
                cmd.Parameters.AddWithValue("@IdChat", idChat);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensajeList.Add(new Mensaje
                    {
                        IdMensaje = reader.GetInt32(reader.GetOrdinal("IdMensaje")),
                        Texto = reader.GetString(reader.GetOrdinal("Texto")),
                        Fkchat = reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Creador = reader.GetInt32(reader.GetOrdinal("Creador")),
                        HoraMensaje = reader.GetDateTime(reader.GetOrdinal("HoraMensaje")),
                    });
                }
                reader.Close();
                return mensajeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /**
         * <summary>Gets a message by it's id</summary>
         * <returns>The message if it all went OK a null otherwise</returns>
         */
        public Mensaje? SelectById(int id)
        {
            Mensaje? mensaje = null;

            using var conn = dbConnection.GetConnection();
            try
            {   
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
                        Fkchat = reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Creador = reader.GetInt32(reader.GetOrdinal("Creador")),
                        HoraMensaje = reader.GetDateTime(reader.GetOrdinal("HoraMensaje")),
                    };
                    reader.Close();
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return mensaje;
        }

        public bool Add(Mensaje mensaje)
        {
            using var connection = dbConnection.GetConnection();
            
            try
            {
                if (mensaje == null)
                    throw new ArgumentNullException(nameof(mensaje));

                connection.Open();
                using var command = new SqlCommand("INSERT INTO Mensaje (Texto, Fkchat, Creador, HoraMensaje) VALUES (@Texto, @Fkchat, @Creador, @HoraMensaje)", connection);
                command.Parameters.AddWithValue("@Texto", mensaje.Texto);
                command.Parameters.AddWithValue("@Fkchat", mensaje.Fkchat);
                command.Parameters.AddWithValue("@Creador", mensaje.Creador);
                command.Parameters.AddWithValue("@HoraMensaje", mensaje.HoraMensaje);
                command.ExecuteNonQuery();
                return true;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool Update(Mensaje mensaje)
        {
            using var connection = dbConnection.GetConnection();
            
            try
            {
                if (mensaje == null)
                    throw new ArgumentNullException(nameof(mensaje));

                connection.Open();
                using var command = new SqlCommand("UPDATE Mensaje SET Texto = @Texto, Fkchat = @Fkchat, Creador = @Creador, HoraMensaje = @HoraMensaje WHERE IdMensaje = @IdMensaje", connection);
                command.Parameters.AddWithValue("@IdMensaje", mensaje.IdMensaje);
                command.Parameters.AddWithValue("@Texto", mensaje.Texto);
                command.Parameters.AddWithValue("@Fkchat", mensaje.Fkchat);
                command.Parameters.AddWithValue("@Creador", mensaje.Creador);
                command.Parameters.AddWithValue("@HoraMensaje", mensaje.HoraMensaje);
                command.ExecuteNonQuery();
                connection.Close();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
