using Microsoft.Data.SqlClient;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcoNet.DAL
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
            Anuncio anuncio = new List<Anuncio>();
            using (var conn = dbConnection.GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT * FROM Anuncio"
                    , conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            anuncio.Add(new Anuncio
                            {
                                IdAnuncio = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                Imagen = reader.IsDBNull(2) ? null : (byte[])reader.GetValue(2),
                                Descripcion = reader.GetInt32(3),
                                Precio = reader.GetDateTime(4),
                                FKBorradoPor = reader.GetDateTime(4),
                                FKUsuario = reader.GetDateTime(4),
                                EstaVendido = reader.GetDateTime(4),
                                
                        });
                        }
                    }
                }
            }
            return anuncio;
        }
        public void Add(Anuncio anuncio)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Anuncio (IdAnuncio, Titulo, Imagen, Descripcion, Precio, FKBorradoPor, FKUsuario,EstaVendido) VALUES ( @IdAnuncio, @Titulo, @Imagen, @Descripcion, @Precio, @FKBorradoPor, @FKUsuario, @EstaVendido)", connection))
                {
                    command.Parameters.AddWithValue("@IdAnuncio", anuncio.IdAnuncio);
                    command.Parameters.AddWithValue("@Titulo", anuncio.Titulo);
                    command.Parameters.AddWithValue("@Imagen", anuncio.Imagen);
                    command.Parameters.AddWithValue("@Descripcion", anuncio.Descripcion);
                    command.Parameters.AddWithValue("@Precio", anuncio.Precio);
                    command.Parameters.AddWithValue("@FKBorradoPor", anuncio.FKBorradoPor);
                    command.Parameters.AddWithValue("@FKUsuario", anuncio.FKUsuario);
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
                using (var command = new SqlCommand("UPDATE Anuncio SET IdAnuncio = @IdAnuncio, Titulo = @Titulo, Imagen = @Imagen, Descripcion = @Descripcion Precio = @Precio, FKBorradoPor = @FKBorradoPor, FKUsuario = @FKUsuario, EstaVendido = @EstaVendido   WHERE IdAnuncio = @IdAnuncio", connection))
                {
                    command.Parameters.AddWithValue("@IdAnuncio", anuncio.IdAnuncio);
                    command.Parameters.AddWithValue("@Titulo", anuncio.Titulo);
                    command.Parameters.AddWithValue("@Imagen", anuncio.Imagen);
                    command.Parameters.AddWithValue("@Descripcion", anuncio.Descripcion);
                    command.Parameters.AddWithValue("@Precio", anuncio.Precio);
                    command.Parameters.AddWithValue("@FKBorradoPor", anuncio.FKBorradoPor);
                    command.Parameters.AddWithValue("@FKUsuario", anuncio.FKUsuario);
                    command.Parameters.AddWithValue("@EstaVendido", anuncio.EstaVendido);
                }
            }
        }
        public void Delete(int id)
        {
        
                using (var conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    
                    var query = $"DELETE FROM Anuncio WHERE IdAnuncio IN ({id})";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            
        }
    }
}
