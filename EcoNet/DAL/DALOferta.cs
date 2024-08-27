using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.Json;

namespace EcoNet.DAL
{
    public class DalOferta
    {
        private readonly DbConnection dbConnection;

        public DalOferta()
        {
            dbConnection = new DbConnection();
        }

        public List<Oferta> SelectAll()
        {
            List<Oferta> ofertaList = new List<Oferta>();

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM Oferta", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ofertaList.Add(new Oferta
                    {
                        IdOferta = reader.GetInt32(reader.GetOrdinal("IdOferta")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Fkchat = reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Aceptada = reader.GetInt16(reader.GetOrdinal("Aceptada")),
                        CreadoPor = reader.GetInt32(reader.GetOrdinal("CreadoPor")),
                        FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
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
            
            return ofertaList;
        }

        public Oferta? SelectById(int id)
        {
            Oferta oferta = null;
            using var conn = dbConnection.GetConnection();
            try
            {
                
                using var cmd = new SqlCommand("SELECT * FROM Oferta WHERE IdOferta = @IdOferta", conn);
                cmd.Parameters.AddWithValue("@IdOferta", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    oferta = new Oferta
                    {
                        IdOferta = reader.GetInt32(reader.GetOrdinal("IdOferta")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Fkchat = reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Aceptada = reader.GetInt16(reader.GetOrdinal("Aceptada")),
                        CreadoPor = reader.GetInt32(reader.GetOrdinal("CreadoPor")),
                        FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
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

            return oferta;
        }

        /**
         * <summary>Gets all ofertas from a chat</summary>
         * <returns>A list of Ofertas in case it was successfull a null otherwise</returns>
         */
        public List<Oferta>? SelectOfertaByChat(int idChat)
        {
            using var conn = dbConnection.GetConnection();

            try
            {
                List<Oferta> ofertas = new();

                using var cmd = new SqlCommand("SELECT * FROM Oferta WHERE FkChat = @FkChat ORDER BY FechaCreacion ASC", conn);
                cmd.Parameters.AddWithValue("@FkChat", idChat);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Oferta oferta = new ()
                    {
                        IdOferta = reader.GetInt32(reader.GetOrdinal("IdOferta")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Fkchat = reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Aceptada = reader.GetInt16(reader.GetOrdinal("Aceptada")),
                        CreadoPor = reader.GetInt32(reader.GetOrdinal("CreadoPor")),
                        FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                    };
                    ofertas.Add(oferta);
                }

                reader.Close();
                return ofertas;
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


        public string Add(Oferta oferta)
        {
            if (oferta == null)
                throw new ArgumentNullException(nameof(oferta));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand(
                    "INSERT INTO Oferta (Precio, Fkchat, Aceptada, CreadoPor, FechaCreacion) " +
                    "OUTPUT Inserted.IdOferta, Inserted.Precio, Inserted.Fkchat, Inserted.Aceptada, Inserted.CreadoPor, Inserted.FechaCreacion " +
                    "VALUES (@Precio, @Fkchat, @Aceptada, @CreadoPor, @FechaCreacion)", connection);

                // Añadiendo los parámetros
                command.Parameters.AddWithValue("@Precio", oferta.Precio);
                command.Parameters.AddWithValue("@Fkchat", oferta.Fkchat);
                command.Parameters.AddWithValue("@Aceptada", oferta.Aceptada);
                command.Parameters.AddWithValue("@CreadoPor", oferta.CreadoPor);
                command.Parameters.AddWithValue("@FechaCreacion", oferta.FechaCreacion);

                // Ejecutar la consulta y leer el resultado
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Crear un objeto Oferta con los valores retornados
                        var createdOferta = new Oferta
                        {
                            IdOferta = reader.GetInt32(reader.GetOrdinal("IdOferta")),
                            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                            Fkchat = reader.GetInt32(reader.GetOrdinal("Fkchat")),
                            Aceptada = reader.GetInt16(reader.GetOrdinal("Aceptada")),
                            CreadoPor = reader.GetInt32(reader.GetOrdinal("CreadoPor")),
                            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                        };

                        // hacemos que coja el formato camel case ya que es nuestro estandard
                        var options = new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };

                        // Convertir el objeto Oferta a JSON
                        return JsonSerializer.Serialize(createdOferta, options);
                    }
                    else
                    {
                        throw new Exception("No se pudo insertar la oferta.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Add: {ex.Message}");
                throw;
            }
        }


        /**
         * <summary>Hace un update del campo del estado</summary>
         * <returns>A bool telling wether it could be updated or not</returns>
         */
        public bool UpdateStatus(int idOferta, Int16 acceptada)
        {
            if (idOferta < 0)
                throw new Exception("Incorrect id");
            if (acceptada != (Int16)1 && acceptada != (Int16)0 && acceptada != -1)
                throw new Exception("Valor incorrecto!");

            using var connection = dbConnection.GetConnection();

            try
            {
                connection.Open();
                using var command = new SqlCommand("UPDATE Oferta SET  Aceptada = @Aceptada WHERE IdOferta = @IdOferta", connection);
                command.Parameters.AddWithValue("@IdOferta", idOferta);
                command.Parameters.AddWithValue("@Aceptada", acceptada);
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
    }
}
