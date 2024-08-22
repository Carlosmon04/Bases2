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
    public partial class EditarFServiciocs : Form
    {
        private SqlConnection cnx;
        private int FacturaID;
        private int ServicioID;
        private int SolicitudID;
        private float Total;
        DataTable TabServicios;
        public EditarFServiciocs(SqlConnection conexion, int factura, int servicio, int solicitud,float total)
        {
            InitializeComponent();
            cnx= conexion;
            FacturaID = factura;
            ServicioID = servicio;
            SolicitudID = solicitud;
            Total = total;
        }

        private void EditarFServiciocs_Load(object sender, EventArgs e)
        {
            ServiciosF(cnx);
        }


        private void ServiciosF(SqlConnection conexion)
        {
            try
            {

                TabServicios = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("Select * from vServicios", conexion);
                adapter.Fill(TabServicios);
                dataGridView1.DataSource = TabServicios;
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

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            ServiciosF(cnx);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count > 0)
            {

                String NombreS = (String)TabServicios.DefaultView[dataGridView1.CurrentRow.Index]["Nombre"];
                var precio = Convert.ToSingle(TabServicios.DefaultView[dataGridView1.CurrentRow.Index]["Precio"]);
                var ServicioID = Convert.ToInt32(TabServicios.DefaultView[dataGridView1.CurrentRow.Index]["SerciciosId"]);

                txtPrecio.Text = precio.ToString();
                txtServicioID.Text = ServicioID.ToString(); 
            
            }


        }

        private void btnConfirmarF_Click(object sender, EventArgs e)
        {

            try {


                if (dataGridView1.Rows.Count > 0) {

                    var Servicioid = Convert.ToInt32(TabServicios.DefaultView[dataGridView1.CurrentRow.Index]["SerciciosId"]);
                    var precio = Convert.ToSingle(TabServicios.DefaultView[dataGridView1.CurrentRow.Index]["Precio"]);
                    SqlCommand cmd = new SqlCommand("spServicios", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServicioID",Servicioid );
                    cmd.Parameters.AddWithValue("@FacturaID", FacturaID);
                    cmd.Parameters.AddWithValue("@precio", txtPrecio.Text);
                    cmd.Parameters.AddWithValue("@total", Total);
                    if (MessageBox.Show("Seguro de los Cambios?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                    { 
                    cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        MessageBox.Show("Cambios GUardados ", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                        }
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
    }
}
