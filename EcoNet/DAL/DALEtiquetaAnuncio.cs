using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalEtiquetaAnuncio
    {
        private readonly DbConnection dbConnection;

        public DalEtiquetaAnuncio()
        {
            dbConnection = new DbConnection();
        }

        public List<EtiquetaAnuncio> Select()
        {
            List<EtiquetaAnuncio> etiquetaAnuncioList = new List<EtiquetaAnuncio>();

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM EtiquetaAnuncio", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    etiquetaAnuncioList.Add(new EtiquetaAnuncio
                    {
                        IdEtiquetaAnuncio = reader.GetInt32(reader.GetOrdinal("IdEtiquetaAnuncio")),
                        Fketiqueta = reader.IsDBNull(reader.GetOrdinal("FKEtiqueta")) ? null : reader.GetInt32(reader.GetOrdinal("FKEtiqueta")),
                        Fkanuncio = reader.IsDBNull(reader.GetOrdinal("FKAnuncio")) ? null : reader.GetInt32(reader.GetOrdinal("FKAnuncio")),
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
            return etiquetaAnuncioList;
        }

        public EtiquetaAnuncio? SelectById(int id)
        {
            EtiquetaAnuncio? etiquetaAnuncio = null;

            try
            {
                using var conn = dbConnection.GetConnection();
                using var cmd = new SqlCommand("SELECT * FROM EtiquetaAnuncio WHERE IdEtiquetaAnuncio = @IdEtiquetaAnuncio", conn);
                cmd.Parameters.AddWithValue("@IdEtiquetaAnuncio", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    etiquetaAnuncio = new EtiquetaAnuncio
                    {
                        IdEtiquetaAnuncio = reader.GetInt32(reader.GetOrdinal("IdEtiquetaAnuncio")),
                        Fketiqueta = reader.IsDBNull(reader.GetOrdinal("FKEtiqueta")) ? null : reader.GetInt32(reader.GetOrdinal("FKEtiqueta")),
                        Fkanuncio = reader.IsDBNull(reader.GetOrdinal("FKAnuncio")) ? null : reader.GetInt32(reader.GetOrdinal("FKAnuncio")),
                    };
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en SelectById: {ex.Message}");
                throw;
            }

            return etiquetaAnuncio;
        }

        public void Add(EtiquetaAnuncio etiquetaAnuncio)
        {
            if (etiquetaAnuncio == null)
                throw new ArgumentNullException(nameof(etiquetaAnuncio));

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO EtiquetaAnuncio (IdEtiquetaAnuncio, Fketiqueta, Fkanuncio) VALUES (@IdEtiquetaAnuncio, @Fketiqueta, @Fkanuncio)", connection))
                    {
                        command.Parameters.AddWithValue("@IdEtiquetaAnuncio", etiquetaAnuncio.IdEtiquetaAnuncio);
                        command.Parameters.AddWithValue("@Fketiqueta", etiquetaAnuncio.Fketiqueta ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Fkanuncio", etiquetaAnuncio.Fkanuncio ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en Add: {ex.Message}");
                throw;
            }
        }

        public void Update(EtiquetaAnuncio etiquetaAnuncio)
        {
            if (etiquetaAnuncio == null)
                throw new ArgumentNullException(nameof(etiquetaAnuncio));

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE EtiquetaAnuncio SET Fketiqueta = @Fketiqueta, Fkanuncio = @Fkanuncio WHERE IdEtiquetaAnuncio = @IdEtiquetaAnuncio", connection))
                    {
                        command.Parameters.AddWithValue("@IdEtiquetaAnuncio", etiquetaAnuncio.IdEtiquetaAnuncio);
                        command.Parameters.AddWithValue("@Fketiqueta", etiquetaAnuncio.Fketiqueta ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Fkanuncio", etiquetaAnuncio.Fkanuncio ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en Update: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("DELETE FROM EtiquetaAnuncio WHERE IdEtiquetaAnuncio = @IdEtiquetaAnuncio", conn))
                    {
                        cmd.Parameters.AddWithValue("@IdEtiquetaAnuncio", id);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en Delete: {ex.Message}");
                throw;
            }
        }
    }
}
