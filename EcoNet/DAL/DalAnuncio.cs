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
                using (var cmd = new SqlCommand("SELECT * FROM Anuncio", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AnuncioList.Add(new Anuncio
                            {
                                IdAnuncio = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                Imagen = reader.IsDBNull(2) ? null : (byte[])reader.GetValue(2),
                                Descripcion = reader.GetString(3),
                                Precio = reader.IsDBNull(4) ? (decimal?)null : reader.GetDecimal(4),
                                FkborradoPor = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                Fkusuario = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                EstaVendido = reader.GetBoolean(7)
                            });
                        }
                    }
                }
            }
            return AnuncioList;
        }
        public void Add(Anuncio anuncio)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Anuncio (IdAnuncio, Titulo, Imagen, Descripcion, Precio, FkborradoPor, Fkusuario, EstaVendido) VALUES (@IdAnuncio, @Titulo, @Imagen, @Descripcion, @Precio, @FkborradoPor, @Fkusuario, @EstaVendido)", connection))
                {
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
            }
        }
        public void Update(Anuncio anuncio)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Anuncio SET Titulo = @Titulo, Imagen = @Imagen, Descripcion = @Descripcion, Precio = @Precio, FkborradoPor = @FkborradoPor, Fkusuario = @Fkusuario, EstaVendido = @EstaVendido WHERE IdAnuncio = @IdAnuncio", connection))
                {
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
            }
        }
        public void Delete(int id)
        {
            using (var conn = dbConnection.GetConnection())
            {
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
}
