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
    public partial class EFacturaArticulo : Form
    {
        private SqlConnection cnx;
        private int Articuloid;
        private int CLienteID;
        private int FacturaID;
        private float Totalfloat;
        private int Cantidad;
        DataTable TabArticulos;
        public EFacturaArticulo(SqlConnection conexion,int articulo,int factura,int cliente,float value,int cantidad)
        {
            InitializeComponent();
            cnx = conexion;
            Articuloid = articulo;
            CLienteID = cliente;
            FacturaID = factura;
            Totalfloat = value;
            Cantidad = cantidad;
        }

        private void EFacturaArticulo_Load(object sender, EventArgs e)
        {

            Inventario(cnx);
            Console.WriteLine(CLienteID);
            Console.WriteLine(FacturaID);
        }

        private void Inventario(SqlConnection conexion)
        {

            try
            {
                TabArticulos = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("Select * from vArticulos", conexion);
                adapter.Fill(TabArticulos);
                dataGridView1.DataSource = TabArticulos;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (SqlException ex) {

              MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

            

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {

                float ArticuloPrecio = Convert.ToSingle(TabArticulos.DefaultView[dataGridView1.CurrentRow.Index]["precioBase"]);

                txtCantidad.Text = Convert.ToString(Cantidad);
                txtPrecio.Text = Convert.ToString(ArticuloPrecio * 1.15);
                txtSubtotal.Text = Convert.ToString(ArticuloPrecio * 1.15 * Cantidad);

            }



        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfirmarfacturaA(SqlConnection conexion, int factura, int articulo, 
                                       float total, float totalNuevo, int cantidad, int cantidadNueva,
                                                                                           int medida,int artviejo)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {

                    


                    SqlCommand cmd = new SqlCommand("spNuevoArticuloEditado", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@facturaID", factura);
                    cmd.Parameters.AddWithValue("@ArticuloID", articulo);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@totalnuevo", totalNuevo);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@cantidadnueva", cantidadNueva);
                    cmd.Parameters.AddWithValue("@medida", medida);
                    cmd.Parameters.AddWithValue("@ArticuloViejo", artviejo);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("Cambios con Exitos", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count > 0)
            {
                int x = 0;
                float ArticuloPrecio = Convert.ToSingle(TabArticulos.DefaultView[dataGridView1.CurrentRow.Index]["precioBase"]);
                String cuanto = txtCantidad.Text;
                if (!string.IsNullOrWhiteSpace(txtCantidad.Text))
                {
                     x = Convert.ToInt32(txtCantidad.Text);
                }

                txtSubtotal.Text = Convert.ToString(ArticuloPrecio * 1.15 * x);

            }


        }

        private void btnConfirmarF_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count > 0)
            {
                int ArticuloID = (int)TabArticulos.DefaultView[dataGridView1.CurrentRow.Index]["ArticuloId"];
                String Medida = (String)TabArticulos.DefaultView[dataGridView1.CurrentRow.Index]["Medida"];
                int j=0;
                if (Medida == "Metro") j = 2;
                if (Medida == "Unidad") j = 1;
                
                float x = Convert.ToSingle(txtSubtotal.Text);
                int y = Convert.ToInt32(txtCantidad.Text);
                Console.WriteLine(j);

                if(MessageBox.Show("Seguro?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Stop)==DialogResult.Yes)

                ConfirmarfacturaA(cnx, FacturaID, ArticuloID, Totalfloat, x, Cantidad, y, j,Articuloid);
                Console.WriteLine(ArticuloID);
                Console.WriteLine(Articuloid);
                this.Close();
            }
        }
    }
}
