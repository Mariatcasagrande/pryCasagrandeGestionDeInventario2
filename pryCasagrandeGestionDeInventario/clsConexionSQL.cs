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
    internal class clsConexionBDSQL
    {
        //cadena de conexion
        string cadenaConexion = "Server=localhost;Database=gestion;Trusted_Connection=True;";
        //string cadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Alumno\\source\\repos\\pryCasagrandeGestionDeInventario2\\pryCasagrandeGestionDeInventario\\BasedeDatos\\dbGestionInventario1.accdb";
        //conector
        SqlConnection coneccionBaseDatos;
        //OleDbConnection coneccionBaseDatos;
        //comando
        SqlCommand comandoBaseDatos;
        //OleDbCommand comandoBaseDatos;

        SqlDataReader lectorDataReader;

        public string nombreBaseDeDatos;

        public void ConectarBD()
        {
            try
            {
                coneccionBaseDatos = new SqlConnection(cadenaConexion);
                //coneccionBaseDatos = new OleDbConnection(cadenaConexion);

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
            comandoBaseDatos = new SqlCommand();
            comandoBaseDatos.Connection = coneccionBaseDatos;
            comandoBaseDatos.CommandType =System.Data.CommandType.Text;

            comandoBaseDatos.CommandText = "SELECT nombre FROM productos";

            lectorDataReader =comandoBaseDatos.ExecuteReader();

            while (lectorDataReader.Read()) 
            {
                listaCategoria.Items.Add(lectorDataReader[0]);
            }
        }
        
        public void AgregarABase(string codigo, string nombre, string categoria, decimal precio, Int32 stock, string descripcion)
        {
            comandoBaseDatos = new SqlCommand();
            comandoBaseDatos.Connection = coneccionBaseDatos;
            comandoBaseDatos.CommandType = System.Data.CommandType.Text;

            comandoBaseDatos.CommandText = $"INSERT INTO productos (codigo, nombre, categoria, precio, stock, descripcion) VALUES ('{codigo}','{categoria}','{nombre}','{precio}','{stock}','{descripcion}')";

            lectorDataReader = comandoBaseDatos.ExecuteReader();

            MessageBox.Show("SE AGREGO CON EXITO");

        }

    }
}
