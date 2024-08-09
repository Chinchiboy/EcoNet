using EcoNet.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EcoNet.DAL
{
    public class DalEmpresa
    {
        private readonly DbConnection dbConnection;

        public DalEmpresa()
        {
            dbConnection = new DbConnection();
        }

        public List<Empresa> Select()
        {
            var empresaList = new List<Empresa>();

            using (var conn = dbConnection.GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT * FROM Empresa", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empresaList.Add(new Empresa
                            {
                                IdUsuario = reader.GetInt32(0),
                                Cif = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                EsRecicladora = reader.IsDBNull(3) ? (bool?)null : reader.GetBoolean(3),
                                Direccion = reader.IsDBNull(4) ? null : reader.GetString(4),
                                // Asumimos que IdUsuarioNavigation se maneja en otro lugar, ya que es una referencia a otra entidad
                            });
                        }
                    }
                }
            }
            return empresaList;
        }

        public Empresa? SelectById(int id)
        {
            Empresa? empresa = null;

            using (var conn = dbConnection.GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT * FROM Empresa WHERE IdUsuario = @IdUsuario", conn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            empresa = new Empresa
                            {
                                IdUsuario = reader.GetInt32(0),
                                Cif = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                EsRecicladora = reader.IsDBNull(3) ? (bool?)null : reader.GetBoolean(3),
                                Direccion = reader.IsDBNull(4) ? null : reader.GetString(4),
                                // Asumimos que IdUsuarioNavigation se maneja en otro lugar
                            };
                        }
                    }
                }
            }
            return empresa;
        }

        public void Add(Empresa empresa)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Empresa (IdUsuario, Cif, Nombre, EsRecicladora, Direccion) VALUES (@IdUsuario, @Cif, @Nombre, @EsRecicladora, @Direccion)", connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", empresa.IdUsuario);
                    command.Parameters.AddWithValue("@Cif", empresa.Cif);
                    command.Parameters.AddWithValue("@Nombre", empresa.Nombre);
                    command.Parameters.AddWithValue("@EsRecicladora", empresa.EsRecicladora.HasValue ? (object)empresa.EsRecicladora.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Direccion", empresa.Direccion ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Empresa empresa)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Empresa SET Cif = @Cif, Nombre = @Nombre, EsRecicladora = @EsRecicladora, Direccion = @Direccion WHERE IdUsuario = @IdUsuario", connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", empresa.IdUsuario);
                    command.Parameters.AddWithValue("@Cif", empresa.Cif);
                    command.Parameters.AddWithValue("@Nombre", empresa.Nombre);
                    command.Parameters.AddWithValue("@EsRecicladora", empresa.EsRecicladora.HasValue ? (object)empresa.EsRecicladora.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Direccion", empresa.Direccion ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("DELETE FROM Empresa WHERE IdUsuario = @IdUsuario", conn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
