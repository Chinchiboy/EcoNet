using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalEtiqueta
    {
        private readonly DbConnection dbConnection;

        public DalEtiqueta()
        {
            dbConnection = new DbConnection();
        }

        public List<Etiqueta> Select()
        {
            List<Etiqueta> etiquetaList = new List<Etiqueta>();

            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Etiqueta", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    etiquetaList.Add(new Etiqueta
                    {
                        IdEtiqueta = reader.GetInt32(reader.GetOrdinal("IdEtiqueta")),
                        DescripcionEtiqueta = reader.GetString(reader.GetOrdinal("DescripcionEtiqueta")), // Eliminada la verificación de DBNull
                    });

                }
                conn.Close();
                reader.Close();
            }
            return etiquetaList;
        }

        public Etiqueta? SelectById(int id)
        {
            Etiqueta? etiqueta = null;

            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Etiqueta WHERE IdEtiqueta = @IdEtiqueta", conn);
                cmd.Parameters.AddWithValue("@IdEtiqueta", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    etiqueta = new Etiqueta
                    {
                        IdEtiqueta = reader.GetInt32(reader.GetOrdinal("IdEtiqueta")),
                        DescripcionEtiqueta = reader.GetString(reader.GetOrdinal("DescripcionEtiqueta")), // Eliminada la verificación de DBNull
                    };
                }
                conn.Close();
                reader.Close();
            }
            return etiqueta;
        }

        public List<Etiqueta> SelectEtiquetasByProductoId(int productoId)
        {
            List<Etiqueta> etiquetas = new List<Etiqueta>();

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand(@"
                SELECT e.IdEtiqueta, e.DescripcionEtiqueta
                FROM Etiqueta e
                INNER JOIN AnuncioEtiqueta ae ON e.IdEtiqueta = ae.IdEtiqueta
                WHERE ae.IdAnuncio = @IdAnuncio", conn);

                cmd.Parameters.AddWithValue("@IdAnuncio", productoId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    etiquetas.Add(new Etiqueta
                    {
                        IdEtiqueta = reader.GetInt32(reader.GetOrdinal("IdEtiqueta")),
                        DescripcionEtiqueta = reader.GetString(reader.GetOrdinal("DescripcionEtiqueta"))
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectEtiquetasByProductoId: {ex.Message}");
            }

            return etiquetas;
        }

        public List<int> SelectIdsByDescriptions(List<string> descripciones)
        {
            List<int> etiquetaIds = new();
            using var conn = dbConnection.GetConnection();

            try
            {
                var descripcionParams = string.Join(",", descripciones.Select(d => $"'{d}'"));

                var query = $"SELECT IdEtiqueta FROM Etiqueta WHERE DescripcionEtiqueta IN ({descripcionParams})";

                using var cmd = new SqlCommand(query, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    etiquetaIds.Add(reader.GetInt32(reader.GetOrdinal("IdEtiqueta")));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectIdsByDescriptions: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            return etiquetaIds;
        }


        public void Add(Etiqueta etiqueta)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();
            using var command = new SqlCommand("INSERT INTO Etiqueta (IdEtiqueta, DescripcionEtiqueta) VALUES (@IdEtiqueta, @DescripcionEtiqueta)", connection);
            command.Parameters.AddWithValue("@IdEtiqueta", etiqueta.IdEtiqueta);
            command.Parameters.AddWithValue("@DescripcionEtiqueta", etiqueta.DescripcionEtiqueta);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Update(Etiqueta etiqueta)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();
            using var command = new SqlCommand("UPDATE Etiqueta SET DescripcionEtiqueta = @DescripcionEtiqueta WHERE IdEtiqueta = @IdEtiqueta", connection);
            command.Parameters.AddWithValue("@IdEtiqueta", etiqueta.IdEtiqueta);
            command.Parameters.AddWithValue("@DescripcionEtiqueta", etiqueta.DescripcionEtiqueta);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Delete(int id)
        {
            using var conn = dbConnection.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Etiqueta WHERE IdEtiqueta = @IdEtiqueta", conn);
            cmd.Parameters.AddWithValue("@IdEtiqueta", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
