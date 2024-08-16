using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet
{
    public class DalAnuncio
    {
        private readonly DbConnection dbConnection;

        public DalAnuncio()
        {
            dbConnection = new DbConnection();
        }

        public List<Anuncio> Select()
        {
            var anuncioList = new List<Anuncio>();
            using var conn = dbConnection.GetConnection();
            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    anuncioList.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return anuncioList;
        }

        public Anuncio SelectById(int id)
        {
            Anuncio anuncio = null;
            using var conn = dbConnection.GetConnection();
            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio WHERE IdAnuncio = @IdAnuncio", conn);
                cmd.Parameters.AddWithValue("@IdAnuncio", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    anuncio = new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    };
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return anuncio;
        }

        public List<Anuncio> SelectByTitle(string title)
        {
            var anuncioList = new List<Anuncio>();
            using var conn = dbConnection.GetConnection();
            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio WHERE Titulo LIKE @Titulo", conn);
                cmd.Parameters.AddWithValue("@Titulo", "%" + title + "%");
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    anuncioList.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return anuncioList;
        }

        public List<Anuncio> SelectByTag(string tag)
        {
            var anuncioList = new List<Anuncio>();
            using var conn = dbConnection.GetConnection();
            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio JOIN EtiquetaAnuncio ON IdAnuncio = FKAnuncio JOIN Etiqueta ON IdEtiqueta = FKEtiqueta WHERE DescripcionEtiqueta LIKE @Etiqueta", conn);
                cmd.Parameters.AddWithValue("@Etiqueta", "%" + tag + "%");
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    anuncioList.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return anuncioList;
        }

        public void Add(Anuncio anuncio)
        {
            using var connection = dbConnection.GetConnection();
            try
            {
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Anuncio (IdAnuncio, Titulo, Imagen, Descripcion, Precio, FkborradoPor, Fkusuario, EstaVendido) VALUES (@IdAnuncio, @Titulo, @Imagen, @Descripcion, @Precio, @FkborradoPor, @Fkusuario, @EstaVendido)", connection);
                command.Parameters.AddWithValue("@IdAnuncio", anuncio.IdAnuncio);
                command.Parameters.AddWithValue("@Titulo", anuncio.Titulo);
                command.Parameters.AddWithValue("@Imagen", anuncio.Imagen);
                command.Parameters.AddWithValue("@Descripcion", anuncio.Descripcion);
                command.Parameters.AddWithValue("@Precio", anuncio.Precio);
                command.Parameters.AddWithValue("@FkborradoPor", anuncio.FkborradoPor);
                command.Parameters.AddWithValue("@Fkusuario", anuncio.Fkusuario);
                command.Parameters.AddWithValue("@EstaVendido", anuncio.EstaVendido);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void Update(Anuncio anuncio)
        {
            using var connection = dbConnection.GetConnection();
            try
            {
                connection.Open();
                using var command = new SqlCommand("UPDATE Anuncio SET Titulo = @Titulo, Imagen = @Imagen, Descripcion = @Descripcion, Precio = @Precio, FkborradoPor = @FkborradoPor, Fkusuario = @Fkusuario, EstaVendido = @EstaVendido WHERE IdAnuncio = @IdAnuncio", connection);
                command.Parameters.AddWithValue("@IdAnuncio", anuncio.IdAnuncio);
                command.Parameters.AddWithValue("@Titulo", anuncio.Titulo);
                command.Parameters.AddWithValue("@Imagen", anuncio.Imagen);
                command.Parameters.AddWithValue("@Descripcion", anuncio.Descripcion);
                command.Parameters.AddWithValue("@Precio", anuncio.Precio);
                command.Parameters.AddWithValue("@FkborradoPor", anuncio.FkborradoPor);
                command.Parameters.AddWithValue("@Fkusuario", anuncio.Fkusuario);
                command.Parameters.AddWithValue("@EstaVendido", anuncio.EstaVendido);
                command.ExecuteNonQuery();
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
                using var cmd = new SqlCommand("DELETE FROM Anuncio WHERE IdAnuncio = @IdAnuncio", conn);
                cmd.Parameters.AddWithValue("@IdAnuncio", id);
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