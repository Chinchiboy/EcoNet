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
                        DescripcionEtiqueta = reader.IsDBNull(reader.GetOrdinal("DescripcionEtiqueta")) ? null : reader.GetString(reader.GetOrdinal("DescripcionEtiqueta")),
                    });
                }
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
                        DescripcionEtiqueta = reader.IsDBNull(reader.GetOrdinal("DescripcionEtiqueta")) ? null : reader.GetString(reader.GetOrdinal("DescripcionEtiqueta")),
                    };
                }
            }
            return etiqueta;
        }

        public void Add(Etiqueta etiqueta)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();
            using var command = new SqlCommand("INSERT INTO Etiqueta (IdEtiqueta, DescripcionEtiqueta) VALUES (@IdEtiqueta, @DescripcionEtiqueta)", connection);
            command.Parameters.AddWithValue("@IdEtiqueta", etiqueta.IdEtiqueta);
            command.Parameters.AddWithValue("@DescripcionEtiqueta", etiqueta.DescripcionEtiqueta ?? (object)DBNull.Value);
            command.ExecuteNonQuery();
        }

        public void Update(Etiqueta etiqueta)
        {
            using var connection = dbConnection.GetConnection();
            connection.Open();
            using var command = new SqlCommand("UPDATE Etiqueta SET DescripcionEtiqueta = @DescripcionEtiqueta WHERE IdEtiqueta = @IdEtiqueta", connection);
            command.Parameters.AddWithValue("@IdEtiqueta", etiqueta.IdEtiqueta);
            command.Parameters.AddWithValue("@DescripcionEtiqueta", etiqueta.DescripcionEtiqueta ?? (object)DBNull.Value);
            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = dbConnection.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Etiqueta WHERE IdEtiqueta = @IdEtiqueta", conn);
            cmd.Parameters.AddWithValue("@IdEtiqueta", id);
            cmd.ExecuteNonQuery();
        }
    }
}
