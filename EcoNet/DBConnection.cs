using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Data.SqlClient;


namespace EcoNet
{
    internal class DbConnection
    {
        private string servidor;
        private string nombreBaseDatos;
        private string usuario;
        private string password;
        private SqlConnection connection;

        public DbConnection(string servidor = "200.234.224.123,54321",
            string nombreBaseDatos = "EcoNet",
            string usuario = "sa",
            string password = "Sql#123456789")
        {
            this.servidor = servidor;
            this.nombreBaseDatos = nombreBaseDatos;
            this.usuario = usuario;
            this.password = password;
            this.connection = new SqlConnection(GetConnectionString());
        }


        public string Servidor { get => servidor; set => servidor = value; }

        public string NombreBaseDatos { get => nombreBaseDatos; set => nombreBaseDatos = value; }

        public string Usuario { get => usuario; set => usuario = value; }

        public string Password { get => password; set => password = value; }

        private string GetConnectionString()
        {
            return $"Data Source={servidor};Initial Catalog={nombreBaseDatos};User ID={usuario};Password={password};";
        }
        /*
        public void Open()
        {
            try
            {
                if (connection == null || connection.State == System.Data.ConnectionState.Closed)
                {
                    connection = new SqlConnection(GetConnectionString());
                    connection.Open();
                }
                else
                {
                    throw new InvalidOperationException("La conexión ya está abierta.");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Close()
        {
            try
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                    connection = null; // Liberar el recurso.
                }
                else
                {
                    throw new InvalidOperationException("La conexión ya está cerrada.");
                }
            }
            catch (Exception ex)
            {

            }
        }
        */
        public SqlConnection GetConnection()
        {
            //if (connection == null || connection.State == System.Data.ConnectionState.Closed)
            //{
            //    throw new InvalidOperationException("La conexión no está abierta.");
            //}

            if (string.IsNullOrEmpty(connection.ConnectionString))
                connection = new SqlConnection(GetConnectionString());

            return connection;
        }
    }
}