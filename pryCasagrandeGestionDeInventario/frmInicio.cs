using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            clsConexionBD.CargarCategoria(cboCategorias);
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
            conexion.Modificar(txtCodigo.Text,txtNombre.Text, cboCategorias.Text, txtPrecio.Text, txtStock.Text, txtDescripcion.Text);

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
}
