using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace EcoNet
{
    public class DalAnuncio
    {
        private readonly DbConnection dbConnection;
        public List<Anuncio> AnuncioList;
        public DalAnuncio()
        {
            dbConnection = new DbConnection();
        }
        public List<Anuncio> Select()
        {
            AnuncioList = new List<Anuncio>();
            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AnuncioList.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.IsDBNull(reader.GetOrdinal("FKUsuario")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
            }
            return AnuncioList;
        }

        public Anuncio SelectById(int id)
        {
            Anuncio anuncio = null;

            using (var conn = dbConnection.GetConnection())
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
                        Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.IsDBNull(reader.GetOrdinal("FKUsuario")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    };
                }
            }

            return anuncio;
        }

        public List<Anuncio> SelectByTitle(string title)
        {
            var AnuncioList = new List<Anuncio>();
            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio WHERE Titulo LIKE @Titulo", conn);
                // Agregar el parámetro con comodines '%' para la búsqueda parcial
                cmd.Parameters.AddWithValue("@Titulo", "%" + title + "%");

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AnuncioList.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.IsDBNull(reader.GetOrdinal("FKUsuario")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
            }
            return AnuncioList;
        }


        public List<Anuncio> SelectByTag(string tag)
        {
            AnuncioList = new List<Anuncio>();
            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio JOIN EtiquetaAnuncio ON IdAnuncio = FKAnuncio JOIN Etiqueta ON IdEtiqueta = FKEtiqueta WHERE DescripcionEtiqueta like @Etiqueta", conn);
                cmd.Parameters.AddWithValue("@Etiqueta", tag);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AnuncioList.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.IsDBNull(reader.GetOrdinal("Precio")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.IsDBNull(reader.GetOrdinal("FKUsuario")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
            }
            return AnuncioList;
        }

        public void Add(Anuncio anuncio)
        {
            using var connection = dbConnection.GetConnection();
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
        public void Update(Anuncio anuncio)
        {
            using var connection = dbConnection.GetConnection();
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
        public void Delete(int id)
        {
            using var conn = dbConnection.GetConnection();
            conn.Open();
            var query = $"DELETE FROM Anuncio WHERE IdAnuncio = @IdAnuncio";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdAnuncio", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
