using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace pryCasagrandeGestionInventario
{
    internal class clsConexionBDSQL
    {
        // Cadena de conexión al servidor SQL LocalDB
        string cadenaConexion = "Server=(localdb)\\MSSQLLocalDB;Database=gestion;Trusted_Connection=True;";

        // Objeto para manejar la conexión con la base de datos
        SqlConnection coneccionBaseDatos;

        // Objeto para enviar instrucciones SQL
        SqlCommand comandoBaseDatos;

        // Objeto para leer datos que vienen desde SQL
        SqlDataReader lectorDataReader;

        public string nombreBaseDeDatos;

        // Método para conectar a la base de datos
        public void ConectarBD()
        {
            try
            {
                // Crear la conexión usando la cadena
                coneccionBaseDatos = new SqlConnection(cadenaConexion);

                // Guardar el nombre de la base a la que estamos conectados
                nombreBaseDeDatos = coneccionBaseDatos.DataSource;

                // Abrir la conexión
                coneccionBaseDatos.Open();
            }
            catch (Exception error)
            {
                // Mostrar mensaje si ocurre error al conectar
                MessageBox.Show("Tiene un errorcito - " + error.Message);
            }
        }

        // Cargar categorías en un ComboBox desde la BD
        public void CargarCategorias(ComboBox combo)
        {
            try
            {
                // Preparar comando SQL
                comandoBaseDatos = new SqlCommand();
                comandoBaseDatos.Connection = coneccionBaseDatos;
                comandoBaseDatos.CommandType = System.Data.CommandType.Text;
                comandoBaseDatos.CommandText = "SELECT DISTINCT categoria FROM productos"; // Selecciona categorías sin repetir

                lectorDataReader = comandoBaseDatos.ExecuteReader(); // Ejecuta consulta

                // Agregar cada categoría al ComboBox
                while (lectorDataReader.Read())
                {
                    combo.Items.Add(lectorDataReader[0].ToString());
                }

                lectorDataReader.Close(); // Cerrar lector después de usar
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message);
            }
        }

        // Agregar un producto a la base de datos
        public void AgregarABase(string codigo, string nombre, string categoria, decimal precio, Int32 stock, string descripcion)
        {
            try
            {
                comandoBaseDatos = new SqlCommand();
                comandoBaseDatos.Connection = coneccionBaseDatos;
                comandoBaseDatos.CommandType = System.Data.CommandType.Text;

                // Insertar datos en la tabla productos
                comandoBaseDatos.CommandText = $"INSERT INTO productos (codigo, nombre, categoria, precio, stock, descripcion) VALUES ('{codigo}','{nombre}','{categoria}','{precio}','{stock}','{descripcion}')";

                // Ejecuta comando (INSERT es escritura)
                lectorDataReader = comandoBaseDatos.ExecuteReader();

                MessageBox.Show("Producto agregado con éxito.", "Operación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message);
            }
        }

        // Buscar un producto por código
        public bool Buscar(string codigo, ref string nombre, ref string categoria, ref string precio, ref string stock, ref string descripcion)
        {
            // Consulta SQL
            comandoBaseDatos = new SqlCommand(
                "SELECT nombre, categoria, precio, stock, descripcion FROM productos WHERE codigo = '" + codigo + "'",
                coneccionBaseDatos
            );

            lectorDataReader = comandoBaseDatos.ExecuteReader(); // Ejecuta SELECT

            // Si encontró el código
            if (lectorDataReader.Read())
            {
                MessageBox.Show("El código " + codigo + " ya existe en la base de datos.");

                // Guardar los datos en los parámetros pasados por referencia
                nombre = lectorDataReader["nombre"].ToString();
                categoria = lectorDataReader["categoria"].ToString();
                precio = lectorDataReader["precio"].ToString();
                stock = lectorDataReader["stock"].ToString();
                descripcion = lectorDataReader["descripcion"].ToString();

                lectorDataReader.Close();
                return true; // Indica que el producto existe
            }
            else
            {
                MessageBox.Show("El código " + codigo + " no existe. Puedes agregarlo.");
                lectorDataReader.Close();
                return false; // Indica que no lo encontró
            }
        }

        // Modificar un producto existente
        public void Modificar(string codigo, string nombre, string categoria, string precio, string stock, string descripcion)
        {
            try
            {
                // Asegurar que decimal tenga punto
                precio = precio.Replace(",", ".");

                // UPDATE: Actualizar datos del producto
                comandoBaseDatos = new SqlCommand(
                    "UPDATE productos SET nombre = '" + nombre +
                    "', categoria = '" + categoria +
                    "', precio = " + precio +
                    ", stock = " + stock +
                    ", descripcion = '" + descripcion +
                    "' WHERE codigo = '" + codigo + "'",
                    coneccionBaseDatos
                );

                // Cantidad de filas afectadas por el UPDATE
                int filasAfectadas = comandoBaseDatos.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto modificado correctamente.", "Operación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el producto. Verifica el código.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el producto: " + ex.Message);
            }
        }

        // Eliminar un producto por código
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

        // Mostrar productos en la grilla según criterio
        public bool Mostrar(DataGridView dgv, string criterio, string valor)
        {
            string consulta = "";

            // Filtros según la búsqueda elegida
            if (criterio == "Codigo")
                consulta = "SELECT * FROM productos WHERE codigo = '" + valor + "'";
            else if (criterio == "Nombre")
                consulta = "SELECT * FROM productos WHERE nombre LIKE '%" + valor + "%'";
            else if (criterio == "Categoria")
                consulta = "SELECT * FROM productos WHERE categoria = '" + valor + "'";

            comandoBaseDatos = new SqlCommand(consulta, coneccionBaseDatos);
            lectorDataReader = comandoBaseDatos.ExecuteReader();

            dgv.Rows.Clear(); // Limpia grilla antes de cargar
            bool hayResultados = false;

            // Cargar filas al DataGridView
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
                MessageBox.Show("No se encontraron resultados.");

            return hayResultados;
        }

        // Obtener stock para graficar
        public DataTable ObtenerInventarioParaReporte()
        {
            DataTable dtInventario = new DataTable();

            try
            {
                if (coneccionBaseDatos.State == ConnectionState.Closed)
                    coneccionBaseDatos.Open(); // Abrir conexión si está cerrada

                string consulta = "SELECT Nombre, Stock FROM productos ORDER BY Stock DESC";
                comandoBaseDatos = new SqlCommand(consulta, coneccionBaseDatos);

                SqlDataAdapter da = new SqlDataAdapter(comandoBaseDatos); // Adaptador para llenar tabla
                da.Fill(dtInventario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos de inventario: " + ex.Message);
            }

            return dtInventario;
        }

        // Generar gráfico del inventario
        public void GenerarReporteInventarioConPuntos(Chart chartInventario)
        {
            clsConexionBDSQL conexion = new clsConexionBDSQL();
            conexion.ConectarBD();
            DataTable dtReporte = conexion.ObtenerInventarioParaReporte();

            chartInventario.Series.Clear();
            chartInventario.Titles.Clear();
            chartInventario.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chartInventario.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

            // Crear serie para graficar stock
            Series serieInventario = chartInventario.Series.Add("Stock de Productos");
            serieInventario.ChartType = SeriesChartType.Column; // Barra vertical
            serieInventario.IsValueShownAsLabel = true; // Mostrar valores arriba de las barras

            int i = 0;
            foreach (DataRow dr in dtReporte.Rows)
            {
                try
                {
                    double valorStock = Convert.ToDouble(dr["Stock"]); // Convertir stock a número
                    string etiquetaNombre = dr["Nombre"].ToString(); // Obtener nombre del producto

                    serieInventario.Points.Add(valorStock); // Agregar valor al gráfico
                    serieInventario.Points[i].AxisLabel = etiquetaNombre; // Etiqueta en eje X
                    i++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al procesar el dato del producto: " + dr["Nombre"].ToString() + ". Detalle: " + ex.Message);
                }
            }

            chartInventario.ChartAreas[0].AxisX.Interval = 1; // Mostrar cada etiqueta
            chartInventario.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotar texto para que entre

            chartInventario.Titles.Add("Reporte de Stock Actual por Producto");
        }
    }

}