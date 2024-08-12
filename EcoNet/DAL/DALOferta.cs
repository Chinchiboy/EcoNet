using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalOferta
    {
        private readonly DbConnection dbConnection;

        public DalOferta()
        {
            dbConnection = new DbConnection();
        }

        public List<Oferta> Select()
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
                        Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Fkchat = reader.IsDBNull(reader.GetOrdinal("FKChat")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Aceptada = reader.IsDBNull(reader.GetOrdinal("Aceptada")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Aceptada")),
                        CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CreadorPor")),
                    });
                }
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
            Oferta? oferta = null;

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM Oferta WHERE IdOferta = @IdOferta", conn);
                cmd.Parameters.AddWithValue("@IdOferta", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    oferta = new Oferta
                    {
                        IdOferta = reader.GetInt32(reader.GetOrdinal("IdOferta")),
                        Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Fkchat = reader.IsDBNull(reader.GetOrdinal("FKChat")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKChat")),
                        Aceptada = reader.IsDBNull(reader.GetOrdinal("Aceptada")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Aceptada")),
                        CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CreadoPor")),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectById: {ex.Message}");
                throw;
            }

            return oferta;
        }

        public void Add(Oferta oferta)
        {
            if (oferta == null)
                throw new ArgumentNullException(nameof(oferta));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Oferta (IdOferta, Precio, Fkchat, Aceptada, CreadoPor) VALUES (@IdOferta, @Precio, @Fkchat, @Aceptada, @CreadoPor)", connection);
                command.Parameters.AddWithValue("@IdOferta", oferta.IdOferta);
                command.Parameters.AddWithValue("@Precio", oferta.Precio ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Fkchat", oferta.Fkchat ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Aceptada", oferta.Aceptada ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreadoPor", oferta.CreadoPor ?? (object)DBNull.Value);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Add: {ex.Message}");
                throw;
            }
        }

        public void Update(Oferta oferta)
        {
            if (oferta == null)
                throw new ArgumentNullException(nameof(oferta));

            try
            {
                using var connection = dbConnection.GetConnection();
                connection.Open();
                using var command = new SqlCommand("UPDATE Oferta SET Precio = @Precio, Fkchat = @Fkchat, Aceptada = @Aceptada, CreadoPor = @CreadoPor WHERE IdOferta = @IdOferta", connection);
                command.Parameters.AddWithValue("@IdOferta", oferta.IdOferta);
                command.Parameters.AddWithValue("@Precio", oferta.Precio ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Fkchat", oferta.Fkchat ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Aceptada", oferta.Aceptada ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreadoPor", oferta.CreadoPor ?? (object)DBNull.Value);
                command.ExecuteNonQuery();
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
                using var cmd = new SqlCommand("DELETE FROM Oferta WHERE IdOferta = @IdOferta", conn);
                cmd.Parameters.AddWithValue("@IdOferta", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Delete: {ex.Message}");
                throw;
            }
        }
    }
}
