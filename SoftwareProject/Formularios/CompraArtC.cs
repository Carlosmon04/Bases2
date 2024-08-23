using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;

namespace SoftwareProject.Formularios
{
    public partial class CompraArtC : Form
    {
        SqlConnection cnx;
        DataTable TabInv;
        String Articulo,Precio;
        int UsuarioID;
        int ArticuloID;

        public CompraArtC(SqlConnection Conexion, int UserID)
        {
            InitializeComponent();
            cnx = Conexion;
            UsuarioID = UserID;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ArticuloID = (int)TabInv.DefaultView[dataGridView1.CurrentRow.Index]["ArticuloID"];
                Articulo = TabInv.DefaultView[dataGridView1.CurrentRow.Index]["Nombre"].ToString();
                Precio = TabInv.DefaultView[dataGridView1.CurrentRow.Index]["precio"].ToString();

                Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                if (form1 != null)
                {
                    form1.OpenChildForm(new DetalleArtVenta(cnx,UsuarioID,Articulo,Precio,ArticuloID));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text;

            string filtro = string.Format("Convert(Nombre, 'System.String') like '%{0}%'", busqueda);

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;
        }

        private void CompraArtC_Load(object sender, EventArgs e)
        {
            try
            {
                TabInv = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from vArticulosvigentes", cnx);
                adapter.Fill(TabInv);
                dataGridView1.DataSource = TabInv;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;
                dataGridView1.Columns["ArticuloId"].Visible = false;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
