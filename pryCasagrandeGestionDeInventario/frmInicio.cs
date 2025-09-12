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
            clsConexionBD clsConexionBD = new clsConexionBD();
            clsConexionBD.ConectarBD();
            clsConexionBD.CargarCategoria(cboCategorias);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            clsConexionBD clsConexionBD = new clsConexionBD();
            clsConexionBD.ConectarBD();
            clsConexionBD.AgregarABase(txtCodigo.Text, txtNombre.Text, txtDescripcion.Text);
        }
    }
}
