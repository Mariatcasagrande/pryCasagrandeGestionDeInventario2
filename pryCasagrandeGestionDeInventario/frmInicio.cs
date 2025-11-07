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
        // ACÁ DECLARÁS LA CONEXIÓN UNA SOLA VEZ PARA TODO EL FORM
        clsConexionBDSQL conexion = new clsConexionBDSQL();

        public frmInicio()
        {
            InitializeComponent();
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            conexion.ConectarBD(); // Se conecta una sola vez

            conexion.CargarCategorias(cboCategorias);
            conexion.CargarCategorias(cboCategoriaBusqueda);

            conexion.GenerarReporteInventarioConPuntos(chartInforme);
            btnBuscar.Enabled = false;
            btnAgregar.Enabled = true;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validación: comprobar que no haya campos vacíos
            if (txtCodigo.Text == "" || txtNombre.Text== "" || cboCategorias.Text == "" ||
                txtPrecio.Text == "" || txtStock.Text == "" || txtDescripcion.Text == "")
            {
                MessageBox.Show(" Por favor, complete todos los campos antes de agregar.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            conexion.AgregarABase(
                txtCodigo.Text,
                txtNombre.Text,
                cboCategorias.Text,
                Convert.ToDecimal(txtPrecio.Text),
                Convert.ToInt32(txtStock.Text),
                txtDescripcion.Text
            );

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = "", categoria = "", precio = "", stock = "", descripcion = "";

            bool existe = conexion.Buscar(txtCodigo.Text, ref nombre, ref categoria, ref precio, ref stock, ref descripcion);

            if (existe)
            {
                // Se encontró → CARGA DATOS
                cboCategorias.Text = categoria;
                txtNombre.Text = nombre;
                txtPrecio.Text = precio;
                txtStock.Text = stock;
                txtDescripcion.Text = descripcion;

                btnAgregar.Enabled = false;    //Solo me deja agregar si no existe
                btnModificar.Enabled = true;   //solo me deja modificar si existe
                btnEliminar.Enabled = true;    //solo me deja eliminar si existe
            }
            else
            {
                // No existe → LIMPIA CAMPOS
                txtNombre.Clear();
                cboCategorias.SelectedIndex = -1;
                txtPrecio.Clear();
                txtStock.Clear();
                txtDescripcion.Clear();

                btnAgregar.Enabled = true;    //Solo me deja agregar si no existe
                btnModificar.Enabled = false;  //solo me deja modificar si existe
                btnEliminar.Enabled = false;    //solo me deja eliminar si existe
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text == "" || txtNombre.Text == "" || cboCategorias.Text == "" ||
                txtPrecio.Text == "" || txtStock.Text == "" ||  txtDescripcion.Text == "")
            {
                MessageBox.Show("Por favor complete todos los campos antes de modificar.",
                    "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            conexion.Modificar(
                txtCodigo.Text,
                txtNombre.Text,
                cboCategorias.Text,
                txtPrecio.Text,
                txtStock.Text,
                txtDescripcion.Text
            );
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
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

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            btnBuscar.Enabled = !string.IsNullOrWhiteSpace(txtCodigo.Text);
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

        private void optCategorias_CheckedChanged_1(object sender, EventArgs e)
        {
            cboCategoriaBusqueda.Enabled = optCategorias.Checked;
            txtCodigo2.Enabled = !optCategorias.Checked;
            txtNombre2.Enabled = !optCategorias.Checked;
        }

        private void optCodigo_CheckedChanged_1(object sender, EventArgs e)
        {

            txtCodigo2.Enabled = optCodigo.Checked;
            txtNombre2.Enabled = !optCodigo.Checked;
            cboCategoriaBusqueda.Enabled = !optCodigo.Checked;
        }

        private void optNombre_CheckedChanged_1(object sender, EventArgs e)
        {
            txtNombre2.Enabled = optNombre.Checked;
            txtCodigo2.Enabled = !optNombre.Checked;
            cboCategoriaBusqueda.Enabled = !optNombre.Checked;
        }
    }

}
