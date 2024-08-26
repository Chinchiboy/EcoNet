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
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
                reader.Close();
                conn.Close();
            }
         
            return AnuncioList;
        }

        public Anuncio? SelectById(int id)
        {
            Anuncio? anuncio = null;

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
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    };

                }
                reader.Close();
                conn.Close();
            }

            return anuncio;
        }

        public List<Anuncio> SelectByEtiquetas(List<Etiqueta> etiquetas)
        {
            List<Anuncio> anunciosRelacionados = new List<Anuncio>();

            if (etiquetas == null || !etiquetas.Any())
            {
                return anunciosRelacionados;
            }

            try
            {
                using var conn = dbConnection.GetConnection();
                var query = @"
                SELECT DISTINCT a.*
                FROM Anuncio a
                INNER JOIN AnuncioEtiqueta ae ON a.IdAnuncio = ae.IdAnuncio
                WHERE ae.IdEtiqueta IN (@IdEtiquetas)";

                using var cmd = new SqlCommand(query, conn);
                var idEtiquetas = string.Join(",", etiquetas.Select(e => e.IdEtiqueta));
                cmd.Parameters.AddWithValue("@IdEtiquetas", idEtiquetas);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    anunciosRelacionados.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectByEtiquetas: {ex.Message}");
            }

            return anunciosRelacionados;
        }

        public List<Anuncio> SelectByEtiquetas(List<int> idEtiquetas)
        {
            List<Anuncio> anunciosRelacionados = new List<Anuncio>();

            if (idEtiquetas == null || !idEtiquetas.Any())
            {
                return anunciosRelacionados;
            }

            try
            {
                using var conn = dbConnection.GetConnection();

                var query = $@"SELECT DISTINCT a.*
                            FROM Anuncio a
                            INNER JOIN EtiquetaAnuncio ae ON a.IdAnuncio = ae.FKAnuncio
                            WHERE ae.FKEtiqueta IN ({string.Join(",", idEtiquetas)})";

                using var cmd = new SqlCommand(query, conn);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    anunciosRelacionados.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SelectByEtiquetas: {ex.Message}");
            }

            return anunciosRelacionados;
        }

        public List<Anuncio> SelectByEtiqueta(int etiquetaId)
        {
            List<Anuncio> anunciosRelacionados = new List<Anuncio>();

            using var conn = dbConnection.GetConnection();
            try
            {
                var query = @"
                        SELECT a.*
                        FROM Anuncio a
                        INNER JOIN AnuncioEtiqueta ae ON a.IdAnuncio = ae.IdAnuncio
                        WHERE ae.IdEtiqueta = @IdEtiqueta";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdEtiqueta", etiquetaId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    anunciosRelacionados.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Imagen")),
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
                Console.WriteLine($"Error en SelectByEtiqueta: {ex.Message}");
            }

            return anunciosRelacionados;
        }

        public List<Anuncio> SelectByTitle(string title)
            {
            var AnuncioList = new List<Anuncio>();
            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio WHERE Titulo LIKE @Titulo", conn);
                cmd.Parameters.AddWithValue("@Titulo", "%" + title + "%");

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AnuncioList.Add(new Anuncio
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
                reader.Close();
                conn.Close();
            }
            return AnuncioList;
        }

        public List<Anuncio> SelectByTag(string tag)
        {
            if (tag == "Mostrar todo")
                return Select();

            AnuncioList = new List<Anuncio>();
            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio JOIN EtiquetaAnuncio ON IdAnuncio = FKAnuncio JOIN Etiqueta ON IdEtiqueta = FKEtiqueta WHERE DescripcionEtiqueta like @Etiqueta", conn);
                cmd.Parameters.AddWithValue("@Etiqueta", "%" + tag + "%");
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
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
                reader.Close();
                conn.Close();
            }
            return AnuncioList;
        }

        public List<Anuncio> Search(string textobusqueda)
        {
            var AnuncioList = new List<Anuncio>();
            textobusqueda = textobusqueda?.ToLower() ?? string.Empty;
            using (var conn = dbConnection.GetConnection())
            {
                using var cmd = new SqlCommand("SELECT * FROM Anuncio WHERE  LOWER(Titulo) LIKE @textoBusqueda OR LOWER(Descripcion) LIKE @textoBusqueda", conn);
                cmd.Parameters.AddWithValue("@textobusqueda", "%" + textobusqueda + "%");

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AnuncioList.Add(new Anuncio
                    {
                        IdAnuncio = reader.GetInt32(reader.GetOrdinal("IdAnuncio")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        Imagen = !reader.IsDBNull(reader.GetOrdinal("Imagen")) ? (byte[])reader.GetValue(reader.GetOrdinal("Imagen")) : null,
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        FkborradoPor = reader.IsDBNull(reader.GetOrdinal("FKBorradoPor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FKBorradoPor")),
                        Fkusuario = reader.GetInt32(reader.GetOrdinal("FKUsuario")),
                        EstaVendido = reader.GetBoolean(reader.GetOrdinal("EstaVendido"))
                    });
                }
                reader.Close();
                conn.Close();
            }
            return AnuncioList;
        }

        public int Add(Anuncio anuncio)
        {
            using var connection = dbConnection.GetConnection();
            using var command = new SqlCommand("INSERT INTO Anuncio (Titulo, Imagen, Descripcion, Precio, Fkusuario, EstaVendido) OUTPUT INSERTED.IdAnuncio VALUES (@Titulo, @Imagen, @Descripcion, @Precio, @Fkusuario, @EstaVendido)", connection);
            command.Parameters.AddWithValue("@Titulo", anuncio.Titulo);
            command.Parameters.AddWithValue("@Imagen", anuncio.Imagen);
            command.Parameters.AddWithValue("@Descripcion", anuncio.Descripcion);
            command.Parameters.AddWithValue("@Precio", anuncio.Precio);
            command.Parameters.AddWithValue("@Fkusuario", anuncio.Fkusuario);
            command.Parameters.AddWithValue("@EstaVendido", anuncio.EstaVendido);
            connection.Open();
            int newId = (int)command.ExecuteScalar();
            return newId;
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
            connection.Close();
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
            conn.Close();
        }
    }
}
