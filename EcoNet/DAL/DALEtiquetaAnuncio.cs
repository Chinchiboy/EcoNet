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
            using var conn = dbConnection.GetConnection();
            try
            {
                using var cmd = new SqlCommand("SELECT * FROM EtiquetaAnuncio", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    etiquetaAnuncioList.Add(new EtiquetaAnuncio
                    {
                        IdEtiquetaAnuncio = reader.GetInt32(reader.GetOrdinal("IdEtiquetaAnuncio")),
                        Fketiqueta = reader.GetInt32(reader.GetOrdinal("FKEtiqueta")),
                        Fkanuncio = reader.GetInt32(reader.GetOrdinal("FKAnuncio")),
                    });
                }
                reader.Close();
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return etiquetaAnuncioList;
        }

        public EtiquetaAnuncio? SelectById(int id)
        {
            EtiquetaAnuncio? etiquetaAnuncio = null;
            using var conn = dbConnection.GetConnection();

            try
            {
                using var cmd = new SqlCommand("SELECT * FROM EtiquetaAnuncio WHERE IdEtiquetaAnuncio = @IdEtiquetaAnuncio", conn);
                cmd.Parameters.AddWithValue("@IdEtiquetaAnuncio", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    etiquetaAnuncio = new EtiquetaAnuncio
                    {
                        IdEtiquetaAnuncio = reader.GetInt32(reader.GetOrdinal("IdEtiquetaAnuncio")),
                        Fketiqueta = reader.GetInt32(reader.GetOrdinal("FKEtiqueta")),
                        Fkanuncio = reader.GetInt32(reader.GetOrdinal("FKAnuncio")),
                    };
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return etiquetaAnuncio;
        }

        public void Add(EtiquetaAnuncio etiquetaAnuncio)
        {
            if (etiquetaAnuncio == null)
                throw new ArgumentNullException(nameof(etiquetaAnuncio));

            using var connection = dbConnection.GetConnection();

            try
            {
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO EtiquetaAnuncio (IdEtiquetaAnuncio, Fketiqueta, Fkanuncio) VALUES (@IdEtiquetaAnuncio, @Fketiqueta, @Fkanuncio)", connection))
                    {
                        command.Parameters.AddWithValue("@IdEtiquetaAnuncio", etiquetaAnuncio.IdEtiquetaAnuncio);
                        command.Parameters.AddWithValue("@Fketiqueta", etiquetaAnuncio.Fketiqueta);
                        command.Parameters.AddWithValue("@Fkanuncio", etiquetaAnuncio.Fkanuncio);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void Update(EtiquetaAnuncio etiquetaAnuncio)
        {
            if (etiquetaAnuncio == null)
                throw new ArgumentNullException(nameof(etiquetaAnuncio));

            using (var connection = dbConnection.GetConnection())

            try
            { 
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE EtiquetaAnuncio SET Fketiqueta = @Fketiqueta, Fkanuncio = @Fkanuncio WHERE IdEtiquetaAnuncio = @IdEtiquetaAnuncio", connection))
                    {
                        command.Parameters.AddWithValue("@IdEtiquetaAnuncio", etiquetaAnuncio.IdEtiquetaAnuncio);
                        command.Parameters.AddWithValue("@Fketiqueta", etiquetaAnuncio.Fketiqueta);
                        command.Parameters.AddWithValue("@Fkanuncio", etiquetaAnuncio.Fkanuncio);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using var conn = dbConnection.GetConnection();
            try
            {
                conn.Open();
                using var cmd = new SqlCommand("DELETE FROM EtiquetaAnuncio WHERE IdEtiquetaAnuncio = @IdEtiquetaAnuncio", conn);
                cmd.Parameters.AddWithValue("@IdEtiquetaAnuncio", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }
    }
}
