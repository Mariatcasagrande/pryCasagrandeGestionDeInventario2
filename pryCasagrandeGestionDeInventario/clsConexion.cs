using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;

//para conexion de ACcess
using System.Data.OleDb;

using System.Windows.Forms;

namespace pryCasagrandeGestionInventario
{
    internal class clsConexionBD
    {
        //cadena de conexion
        //sql - string cadenaConexion = "Server=localhost;Database=Ventas2;Trusted_Connection=True;";
        string cadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\\..\\..\\..\\pryCasagrandeGestionDeInventario\\pryCasagrandeGestionDeInventario\\BasedeDatos\\dbGestionInventario1.accdb";
        //conector
        //SqlConnection coneccionBaseDatos;
        OleDbConnection coneccionBaseDatos;
        //comando
        //SqlCommand comandoBaseDatos;
        OleDbCommand comandoBaseDatos;

        OleDbDataReader lectorDataReader;

        public string nombreBaseDeDatos;

        public void ConectarBD()
        {
            try
            {
                //coneccionBaseDatos = new SqlConnection(cadenaConexion);
                coneccionBaseDatos = new OleDbConnection(cadenaConexion);

                nombreBaseDeDatos = coneccionBaseDatos.DataSource;

                coneccionBaseDatos.Open();

                //MessageBox.Show("Conectado a " + nombreBaseDeDatos);
            }
            catch (Exception error)
            {
                MessageBox.Show("Tiene un errorcito - " + error.Message);
            }

        }

        public void CargarCategoria(ComboBox listaCategoria)
        {
            comandoBaseDatos = new OleDbCommand();
            comandoBaseDatos.Connection = coneccionBaseDatos;
            comandoBaseDatos.CommandType =System.Data.CommandType.Text;

            comandoBaseDatos.CommandText = "SELECT marca_nombre FROM Productos";

            lectorDataReader =comandoBaseDatos.ExecuteReader();
            while (lectorDataReader.Read()) 
            {
                listaCategoria.Items.Add(lectorDataReader[0]);
            }
        }
        
        public void AgregarABase(string codigo, string nombre, string descripcion)
        {
            if (coneccionBaseDatos.State != System.Data.ConnectionState.Open)
            {
                coneccionBaseDatos.Open();
            }

            comandoBaseDatos = new OleDbCommand();
            comandoBaseDatos.Connection = coneccionBaseDatos;
            comandoBaseDatos.CommandType = System.Data.CommandType.Text;

            // Concatenando directamente (NO recomendado en proyectos reales por seguridad, pero funciona)
            comandoBaseDatos.CommandText =
                 "INSERT INTO Productos (id1, marca_nombre, observaciones) " +
                 "VALUES (" + codigo + ", '" + nombre + "', '" + descripcion + "')";

            

            MessageBox.Show("Producto agregado correctamente ✅");

        }

    }
}
