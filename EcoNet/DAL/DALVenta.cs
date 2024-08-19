using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalVenta
    {
        private readonly DbConnection dbConnection;

        public DalVenta()
        {
            dbConnection = new DbConnection();
        }

        public List<Venta> Select()
        {
            List<Venta> ventaList = new List<Venta>();
            using var conn = dbConnection.GetConnection();

            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Venta", conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ventaList.Add(new Venta
                    {
                        IdVenta = reader.GetInt32(reader.GetOrdinal("IdVenta")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Fkoferta = reader.GetInt32(reader.GetOrdinal("FKOferta"))
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
            return ventaList;
        }

        public Venta SelectById(int id)
        {
            Venta venta = null;
            using var conn = dbConnection.GetConnection();

            try
            {
                using var cmd = new SqlCommand("SELECT * FROM Venta WHERE IdVenta = @IdVenta", conn);
                cmd.Parameters.AddWithValue("@IdVenta", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    venta = new Venta
                    {
                        IdVenta = reader.GetInt32(reader.GetOrdinal("IdVenta")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Fkoferta = reader.GetInt32(reader.GetOrdinal("FKOferta"))
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

            return venta;
        }

        public void Add(Venta venta)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta));

            using var connection = dbConnection.GetConnection();

            try
            {
                
                connection.Open();
                using var command = new SqlCommand("INSERT INTO Venta (Precio, Fkoferta) VALUES (@Precio, @Fkoferta)", connection);
                command.Parameters.AddWithValue("@Precio", (object)venta.Precio ?? DBNull.Value);
                command.Parameters.AddWithValue("@Fkoferta", (object)venta.Fkoferta ?? DBNull.Value);
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

        public void Update(Venta venta)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta));

            using var connection = dbConnection.GetConnection();
            
            try
            {
                connection.Open();
                using var command = new SqlCommand("UPDATE Venta SET Precio = @Precio, Fkoferta = @Fkoferta WHERE IdVenta = @IdVenta", connection);
                command.Parameters.AddWithValue("@IdVenta", venta.IdVenta);
                command.Parameters.AddWithValue("@Precio", (object)venta.Precio ?? DBNull.Value);
                command.Parameters.AddWithValue("@Fkoferta", (object)venta.Fkoferta ?? DBNull.Value);
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
                using var cmd = new SqlCommand("DELETE FROM Venta WHERE IdVenta = @IdVenta", conn);
                cmd.Parameters.AddWithValue("@IdVenta", id);
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
