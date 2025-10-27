using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using pryCasagrandeGestionInventario;

namespace pryCasagrandeGestionDeInventario
{
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            clsConexionBDSQL clsConexionBD = new clsConexionBDSQL();
            clsConexionBD.ConectarBD();
            clsConexionBD.CargarCategorias(cboCategorias);
            clsConexionBD.CargarCategorias(cboCategoriaBusqueda);
            clsConexionBDSQL conexion = new clsConexionBDSQL();
            conexion.ConectarBD();
            conexion.GenerarReporteInventarioConPuntos(chartInforme);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            clsConexionBDSQL clsConexionBDSQL = new clsConexionBDSQL();
            clsConexionBDSQL.ConectarBD();
            clsConexionBDSQL.AgregarABase(txtCodigo.Text, txtNombre.Text, cboCategorias.Text, Convert.ToDecimal(txtPrecio.Text), Convert.ToInt32(txtStock.Text), txtDescripcion.Text);

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            clsConexionBDSQL conexion = new clsConexionBDSQL();
            conexion.ConectarBD();


            string nombre = "";
            string categoria = "";
            string precio = "";
            string stock = "";
            string descripcion = "";

            bool existe = conexion.Buscar(txtCodigo.Text, ref nombre, ref categoria, ref precio, ref stock, ref descripcion);

            if (existe)
            {

                txtNombre.Text = nombre;
                cboCategorias.Text = categoria;
                txtPrecio.Text = precio;
                txtStock.Text = stock;
                txtDescripcion.Text = descripcion;
            }
            else
            {

                txtNombre.Clear();
                cboCategorias.SelectedIndex = -1;
                txtPrecio.Clear();
                txtStock.Clear();
                txtDescripcion.Clear();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            clsConexionBDSQL conexion = new clsConexionBDSQL();
            conexion.ConectarBD();
            conexion.Modificar(txtCodigo.Text, txtNombre.Text, cboCategorias.Text, txtPrecio.Text, txtStock.Text, txtDescripcion.Text);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            clsConexionBDSQL conexion = new clsConexionBDSQL();
            conexion.ConectarBD();

            if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
            {

                conexion.Eliminar(txtCodigo.Text);


                txtCodigo.Clear();
                txtNombre.Clear();
                cboCategorias.SelectedIndex = -1;
                txtPrecio.Clear();
                txtStock.Clear();
                txtDescripcion.Clear();

            }
        }


        private void btnBuscarPor_Click(object sender, EventArgs e)
        {
            clsConexionBDSQL conexion = new clsConexionBDSQL();
            conexion.ConectarBD();

            if (optCategorias.Checked)
            {
                conexion.Mostrar(dgvProductos, "Categoria", cboCategoriaBusqueda.SelectedItem.ToString());
                LimpiarControlesExcepto(cboCategoriaBusqueda);
            }
            else if (optCodigo.Checked)
            {
                conexion.Mostrar(dgvProductos, "Codigo", txtCodigo2.Text);
                LimpiarControlesExcepto(txtCodigo2);
            }
            else if (optNombre.Checked)
            {
                conexion.Mostrar(dgvProductos, "Nombre", txtNombre2.Text);
                LimpiarControlesExcepto(txtNombre2);
            }
            else
            {
                MessageBox.Show("Seleccione un criterio de búsqueda.");
            }
        }

        private void LimpiarControlesExcepto(Control controlActivo)
        {
            if (controlActivo != txtCodigo2) txtCodigo2.Clear();
            if (controlActivo != txtNombre2) txtNombre2.Clear();
            if (controlActivo != cboCategoriaBusqueda) cboCategoriaBusqueda.SelectedIndex = -1;
        }


        private void optCategorias_CheckedChanged(object sender, EventArgs e)
        {
            cboCategoriaBusqueda.Enabled = optCategorias.Checked;
            txtCodigo2.Enabled = !optCategorias.Checked;
            txtNombre2.Enabled = !optCategorias.Checked;
        }

        private void optCodigo_CheckedChanged(object sender, EventArgs e)
        {
            txtCodigo2.Enabled = optCodigo.Checked;
            txtNombre2.Enabled = !optCodigo.Checked;
            cboCategoriaBusqueda.Enabled = !optCodigo.Checked;
        }

        private void optNombre_CheckedChanged(object sender, EventArgs e)
        {
            txtNombre2.Enabled = optNombre.Checked;
            txtCodigo2.Enabled = !optNombre.Checked;
            cboCategoriaBusqueda.Enabled = !optNombre.Checked;
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabInforme_Click(object sender, EventArgs e)
        {}

        private void tabGestionProductos_Click(object sender, EventArgs e)
        {

        }
    }
}
