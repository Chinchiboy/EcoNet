using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalChat
    {
        private readonly DbConnection dbConnection;        

        public DalChat()
        {
            dbConnection = new DbConnection();
        }

        /**
         * <summary>
         * Obtains all chat headers from the registered user
         * </summary>
         * <returns>
         * A list with it's chats if it was succesfull or null if there was an error
         * </returns>
         */
        public List<Chat>? SelectUserChats(int userId)
        {
            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand(@"SELECT * FROM Chat 
                                    WHERE FKVendedor = @UserId OR FKComprador = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                
                List<Chat> ChatList = new List<Chat>();
                while (reader.Read())
                {
                    ChatList.Add(new Chat
                    {
                        IdChat = reader.GetInt32(reader.GetOrdinal("IdChat")),
                        Fkanuncio = reader.GetInt32(reader.GetOrdinal("FKAnuncio")),
                        Fkvendedor = reader.GetInt32(reader.GetOrdinal("FKVendedor")),
                        Fkcomprador = reader.GetInt32(reader.GetOrdinal("FKComprador")),
                    });
                }

                conn.Close();
                reader.Close();
                return ChatList;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /**
         * <summary>
         * Obtains a chat header by the id of the chat
         * </summary>
         * <returns>
         * The header of the chat if it was successfull retriving it or a null if there was an error
         * </returns>
         */
        public Chat? SelectById(int id)
        {
            Chat? c = null;
            try
            {
                using var conn = dbConnection.GetConnection();

                using var cmd = new SqlCommand("SELECT * FROM Chat WHERE IdChat = @IdChat", conn);
                cmd.Parameters.AddWithValue("@IdChat", id);
                conn.Open();

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    c = new Chat
                    {
                        IdChat = reader.GetInt32(reader.GetOrdinal("IdChat")),
                        Fkanuncio = reader.GetInt32(reader.GetOrdinal("FKAnuncio")),
                        Fkvendedor = reader.GetInt32(reader.GetOrdinal("FKVendedor")),
                        Fkcomprador = reader.GetInt32(reader.GetOrdinal("FKComprador")),
                    };
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return c;
        }


        public void Add(Chat chat)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();
            using var command = new SqlCommand("INSERT INTO Chat (IdChat, Fkanuncio, Fkvendedor, Fkcomprador) VALUES (@IdChat, @Fkanuncio, @Fkvendedor, @Fkcomprador)", connection);
            command.Parameters.AddWithValue("@IdChat", chat.IdChat);
            command.Parameters.AddWithValue("@Fkanuncio", chat.Fkanuncio);
            command.Parameters.AddWithValue("@Fkvendedor", chat.Fkvendedor);
            command.Parameters.AddWithValue("@Fkcomprador", chat.Fkcomprador);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Update(Chat chat)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();
            using var command = new SqlCommand("UPDATE Chat SET Fkanuncio = @Fkanuncio, Fkvendedor = @Fkvendedor, Fkcomprador = @Fkcomprador WHERE IdChat = @IdChat", connection);
            command.Parameters.AddWithValue("@IdChat", chat.IdChat);
            command.Parameters.AddWithValue("@Fkanuncio", chat.Fkanuncio);
            command.Parameters.AddWithValue("@Fkvendedor", chat.Fkvendedor);
            command.Parameters.AddWithValue("@Fkcomprador", chat.Fkcomprador);
            command.ExecuteNonQuery();
            connection.Close();
        }

        /**
         * <summary>
         * Elimina el header de un chat
         * </summary>
         * <returns>
         * True if the operation was successfull false otherwise
         * </returns>
         */
        public bool Delete(int id)
        {
            using var conn = dbConnection.GetConnection();
            try
            {
                conn.Open();
                var query = "DELETE FROM Chat WHERE IdChat = @IdChat";
                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdChat", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
