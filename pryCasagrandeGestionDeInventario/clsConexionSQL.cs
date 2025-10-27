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
using System.Data;

namespace pryCasagrandeGestionInventario
{
    internal class clsConexionBDSQL
    {
        //cadena de conexion
        //string cadenaConexion = "Server=localhost;Database=gestion;Trusted_Connection=True;";
        string cadenaConexion = "Server=(localdb)\\MSSQLLocalDB;Database=gestion;Trusted_Connection=True;";

        //string cadenaConexion = "Server=localhost;Database=gestion;Trusted_Connection=True;";
        string cadenaConexion = "Server=(localdb)\\MSSQLLocalDB;Database=gestion;Trusted_Connection=True;";
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

        public void CargarCategorias(ComboBox combo)
        {
            comandoBaseDatos = new SqlCommand();
            comandoBaseDatos.Connection = coneccionBaseDatos;
            comandoBaseDatos.CommandType = System.Data.CommandType.Text;

            comandoBaseDatos.CommandText = "SELECT DISTINCT categoria FROM productos";

            lectorDataReader = comandoBaseDatos.ExecuteReader();

            while (lectorDataReader.Read())
            {
                combo.Items.Add(lectorDataReader[0].ToString());
            }

            lectorDataReader.Close();
        }

        public void AgregarABase(string codigo, string nombre, string categoria, decimal precio, Int32 stock, string descripcion)
        {
            comandoBaseDatos = new SqlCommand();
            comandoBaseDatos.Connection = coneccionBaseDatos;
            comandoBaseDatos.CommandType = System.Data.CommandType.Text;

            comandoBaseDatos.CommandText = $"INSERT INTO productos (codigo, nombre, categoria, precio, stock, descripcion) VALUES ('{codigo}','{nombre}','{categoria}','{precio}','{stock}','{descripcion}')";

            lectorDataReader = comandoBaseDatos.ExecuteReader();

            MessageBox.Show("SE AGREGO CON EXITO");

        }
        public bool Buscar(string codigo, ref string nombre, ref string categoria, ref string precio, ref string stock, ref string descripcion)
        {
            comandoBaseDatos = new SqlCommand(
                "SELECT nombre, categoria, precio, stock, descripcion FROM productos WHERE codigo = '" + codigo + "'",
                coneccionBaseDatos
            );

            lectorDataReader = comandoBaseDatos.ExecuteReader();

            if (lectorDataReader.Read())
            {
                MessageBox.Show("El código " + codigo + " ya existe en la base de datos.");

                // Guardamos los valores en los parámetros de salida
                nombre = lectorDataReader["nombre"].ToString();
                categoria = lectorDataReader["categoria"].ToString();
                precio = lectorDataReader["precio"].ToString();
                stock = lectorDataReader["stock"].ToString();
                descripcion = lectorDataReader["descripcion"].ToString();

                lectorDataReader.Close();
                return true;
            }
            else
            {
                MessageBox.Show("El código " + codigo + " no existe. Puedes agregarlo.");
                lectorDataReader.Close();
                return false;
            }
        }
        public void Modificar(string codigo, string nombre, string categoria, string precio, string stock, string descripcion)
        {
            try
            {
                comandoBaseDatos = new SqlCommand(
                    "UPDATE productos SET nombre = '" + nombre +
                    "', categoria = '" + categoria +
                    "', precio = '" + precio +
                    "', stock = '" + stock +
                    "', descripcion = '" + descripcion +
                    "' WHERE codigo = '" + codigo + "'",
                    coneccionBaseDatos
                );

                int filasAfectadas = comandoBaseDatos.ExecuteNonQuery(); // ejecuta la actualización

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto modificado correctamente.");
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el producto. Verifica el código.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el producto: " + ex.Message);
            }

        }
        public void Eliminar(string codigo)
        {
            try
            {
                comandoBaseDatos = new SqlCommand(
                    "DELETE FROM productos WHERE codigo = '" + codigo + "'",
                    coneccionBaseDatos
                );

                int filasAfectadas = comandoBaseDatos.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto eliminado correctamente.");
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el producto. Verifica el código.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message);
            }
        }
        public bool Mostrar(DataGridView dgv, string criterio, string valor)
        {
            string consulta = "";
            if (criterio == "Codigo")
                consulta = "SELECT * FROM productos WHERE codigo = '" + valor + "'";
            else if (criterio == "Nombre")
                consulta = "SELECT * FROM productos WHERE nombre LIKE '%" + valor + "%'";
            else if (criterio == "Categoria")
                consulta = "SELECT * FROM productos WHERE categoria = '" + valor + "'";

            comandoBaseDatos = new SqlCommand(consulta, coneccionBaseDatos);
            lectorDataReader = comandoBaseDatos.ExecuteReader();

            dgv.Rows.Clear();


            bool hayResultados = false;

            while (lectorDataReader.Read())
            {
                dgv.Rows.Add(
                    lectorDataReader["codigo"].ToString(),
                    lectorDataReader["nombre"].ToString(),
                    lectorDataReader["categoria"].ToString(),
                    lectorDataReader["precio"].ToString(),
                    lectorDataReader["stock"].ToString(),
                    lectorDataReader["descripcion"].ToString()
                );
                hayResultados = true;
            }

            lectorDataReader.Close();

            if (!hayResultados)
            {
                MessageBox.Show("No se encontraron resultados.");
            }

            return hayResultados;

        }

    }

}