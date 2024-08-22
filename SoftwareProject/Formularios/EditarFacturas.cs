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
    public partial class EditarFacturas : Form
    {
        private SqlConnection cnx;
        private int FacturaID;
        private string TipoF;
        private int CLienteID;
        DataTable TabFDfacturas;
        public EditarFacturas(SqlConnection conexion,int Factura,String TipoFactura,int cliente)
        {
            InitializeComponent();
            cnx = conexion;
            FacturaID = Factura;
            TipoF = TipoFactura;
            CLienteID = cliente;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditarF(SqlConnection conexion,int factura,String tipo)
        {
            try
            {
                TabFDfacturas = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("spFacturasDetalles", conexion);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@FacturaId", factura);
                adapter.SelectCommand.Parameters.AddWithValue("@tipo", tipo);
                adapter.Fill(TabFDfacturas);
                dataGridView1.DataSource = TabFDfacturas;
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

        private void EditarFacturas_Load(object sender, EventArgs e)
        {
            EditarF(cnx, FacturaID, TipoF);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {

             int FacturaIDA = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["ArticuloId"];
                Console.WriteLine(FacturaID);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            if(dataGridView1.Rows.Count>0)
            {
                int ArticuloIDF = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["ArticuloId"];
                int FacturaIDF = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["FacturaID"];
               var value= Convert.ToSingle(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Total"]);
                var Cantidad = Convert.ToInt32(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Cantidad"]);
                

                EFacturaArticulo efa = new EFacturaArticulo(cnx, ArticuloIDF,FacturaIDF,CLienteID,value,Cantidad);
                efa.Visible = true ;

                Console.WriteLine(value);
                Console.WriteLine(Cantidad);
                this.Close();
            }

        }
    }
}
