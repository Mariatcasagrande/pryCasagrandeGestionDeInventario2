using System;
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
            clsConexionBDSQL.AgregarABase(txtCodigo.Text, txtNombre.Text, cboCategorias.Text, Convert.ToDecimal( txtPrecio.Text), Convert.ToInt32(txtStock.Text), txtDescripcion.Text);
            
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
