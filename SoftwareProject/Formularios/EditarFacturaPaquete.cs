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
    public partial class EditarFacturaPaquete : Form
    {
        private SqlConnection cnx;
        private int FActuraID;
        private int PAqueteID;
        float Totalp;
        DataTable TabPaquetes;

        public EditarFacturaPaquete(SqlConnection conexion, int factura, float precio)
        {
            InitializeComponent();
            cnx = conexion;
            FActuraID = factura;         
            Totalp = precio;
        }

        private void EditarFacturaPaquete_Load(object sender, EventArgs e)
        {
            VistaPaquete(cnx);
        }

        private void VistaPaquete(SqlConnection conexion)
        {
            try
            {

                SqlDataAdapter adapter = new SqlDataAdapter("Select * from vPaquetes", conexion);
                TabPaquetes = new DataTable();
                adapter.Fill(TabPaquetes);
                dataGridView1.DataSource = TabPaquetes;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (SqlException ex)
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

                var paqueteID = Convert.ToInt32(TabPaquetes.DefaultView[dataGridView1.CurrentRow.Index]["PaqueteId"]);
                var precio = Convert.ToSingle(TabPaquetes.DefaultView[dataGridView1.CurrentRow.Index]["Precio"]);

                txtPrecio.Text = precio.ToString();
                txtServicioID.Text = paqueteID.ToString(); 


            
            }

        }

        private void btnConfirmarF_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    var Packahge = Convert.ToInt32(TabPaquetes.DefaultView[dataGridView1.CurrentRow.Index]["PaqueteId"]);

                    SqlCommand cmd = new SqlCommand("spPAqueteEditar", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@facturaID", FActuraID);
                    cmd.Parameters.AddWithValue("@precio", txtPrecio.Text);
                    cmd.Parameters.AddWithValue("@PaqueteID", Packahge);
                    cmd.Parameters.AddWithValue("@total", Totalp);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("Facturada Actualizada ", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                 }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            VistaPaquete(cnx);
        }
    }
}
