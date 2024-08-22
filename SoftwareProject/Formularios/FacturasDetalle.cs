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

namespace SoftwareProject.Formularios
{
    public partial class FacturasDetalle : Form
    {

        private SqlConnection cnx;
        DataTable TabFacturas;
        public FacturasDetalle(SqlConnection conexion )
        {
            InitializeComponent();
            cnx = conexion;
        }

        private void FacturasDetalle_Load(object sender, EventArgs e)
        {
            CargarFacturas(cnx);
        }

        private void CargarFacturas(SqlConnection conexion)
        {
            try {

                TabFacturas = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("Select * from vFacturas",conexion);    
                adapter.Fill(TabFacturas);
                dataGridView1.DataSource = TabFacturas;
                dataGridView1.ReadOnly = true;               
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            }
            catch(SqlException ex)
                {
             MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {

                String Tipo = (String)TabFacturas.DefaultView[dataGridView1.CurrentRow.Index]["Tipo"];
                int FacturaID = (int)TabFacturas.DefaultView[dataGridView1.CurrentRow.Index]["FacturaID"];
                int clienteid = (int)TabFacturas.DefaultView[dataGridView1.CurrentRow.Index]["Clienteid"];
                EditarFacturas ef = new EditarFacturas(cnx, FacturaID, Tipo,clienteid);
                    ef.Visible = true;
 
            }
        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtCliente.Text;

            string filtro = string.Format("Convert(Nombre, 'System.String') like '%{0}%'", busqueda);

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;
        }

        private void cmxTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

         string filtro = string.Format("Convert(Tipo, 'System.String') like '%{0}%'", cmxTipo.SelectedItem.ToString());
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;

        }

      

        private void txtFecha_TextChanged(object sender, EventArgs e)
        {

            string busqueda = txtFecha.Text;

            string filtro = string.Format("Convert(Fecha, 'System.String') like '%{0}%'", busqueda);

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;

        }
    }
}
