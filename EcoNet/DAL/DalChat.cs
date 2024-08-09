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
        public List<Chat> ChatList = new List<Chat>();

        public DalChat()
        {
            dbConnection = new DbConnection();
        }

        public List<Chat> Select()
        {
            ChatList = new List<Chat>(); // Asegúrate de inicializar la lista
            using (var conn = dbConnection.GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT * FROM Chat", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChatList.Add(new Chat
                            {
                                IdChat = reader.GetInt32(0),
                                Fkanuncio = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                Fkvendedor = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                Fkcomprador = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            });
                        }
                    }
                }
            }
            return ChatList;
        }

        public void Add(Chat chat)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Chat (IdChat, Fkanuncio, Fkvendedor, Fkcomprador) VALUES (@IdChat, @Fkanuncio, @Fkvendedor, @Fkcomprador)", connection))
                {
                    command.Parameters.AddWithValue("@IdChat", chat.IdChat);
                    command.Parameters.AddWithValue("@Fkanuncio", chat.Fkanuncio.HasValue ? (object)chat.Fkanuncio.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Fkvendedor", chat.Fkvendedor.HasValue ? (object)chat.Fkvendedor.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Fkcomprador", chat.Fkcomprador.HasValue ? (object)chat.Fkcomprador.Value : DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Chat chat)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Chat SET Fkanuncio = @Fkanuncio, Fkvendedor = @Fkvendedor, Fkcomprador = @Fkcomprador WHERE IdChat = @IdChat", connection))
                {
                    command.Parameters.AddWithValue("@IdChat", chat.IdChat);
                    command.Parameters.AddWithValue("@Fkanuncio", chat.Fkanuncio.HasValue ? (object)chat.Fkanuncio.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Fkvendedor", chat.Fkvendedor.HasValue ? (object)chat.Fkvendedor.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Fkcomprador", chat.Fkcomprador.HasValue ? (object)chat.Fkcomprador.Value : DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = dbConnection.GetConnection())
            {
                conn.Open();
                var query = "DELETE FROM Chat WHERE IdChat = @IdChat";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdChat", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
