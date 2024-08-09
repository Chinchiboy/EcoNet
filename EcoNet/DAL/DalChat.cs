using Microsoft.Data.SqlClient;
using System;
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
        public List<Chat> Select()
        {
            Chat chat = new List<Chat>();
            using (var conn = dbConnection.GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT * FROM Chat"
                    , conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chat.Add(new Chat
                            {
                                IdChat = reader.GetInt32(0),
                                FKAnuncio = reader.GetInt32(1),
                                FKVendedor = reader.GetInt32(2),
                                FKComprador = reader.GetInt32(3),

                            });
                        }
                    }
                }
            }
            return chat;
        }
        public void Add(Chat chat)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Chat (IdChat, FKAnuncio, FKVendedor, FKComprador) VALUES ( @IdChat, @FKAnuncio, @FKVendedor, @FKComprador)", connection))
                {
                    command.Parameters.AddWithValue("@IdChat", chat.IdAnuncio);
                    command.Parameters.AddWithValue("@FKAnuncio", chat.Titulo);
                    command.Parameters.AddWithValue("@FKVendedor", chat.Imagen);
                    command.Parameters.AddWithValue("@FKComprador", chat.Descripcion);
                   
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Update(Chat chat)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Chat SET IdChat = @IdChat, FKAnuncio = @FKAnuncio, FKVendedor = @FKVendedor, FKComprador = @FKComprador WHERE IdChat = @IdChat", connection))
                {
                    command.Parameters.AddWithValue("@IdChat", chat.IdChat);
                    command.Parameters.AddWithValue("@FKAnuncio", chat.FKAnuncio);
                    command.Parameters.AddWithValue("@FKVendedor", chat.FKVendedor);
                    command.Parameters.AddWithValue("@FKComprador", chat.FKComprador);

                }
            }
        }
        public void Delete(int id)
        {

            using (var conn = dbConnection.GetConnection())
            {
                conn.Open();

                var query = $"DELETE FROM Chat WHERE IdChat IN ({id})";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
