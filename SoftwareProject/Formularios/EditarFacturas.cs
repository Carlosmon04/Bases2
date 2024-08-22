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
            if (dataGridView1.Rows.Count > 0 && TipoF == "A")
            {

             int articuloid = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["ArticuloId"];
                int facturaid = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["FacturaID"];
                var total = Convert.ToSingle(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Total"]);
                var qty = Convert.ToInt32(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Cantidad"]);

                if (MessageBox.Show("Seguro que quieres eliminar esta factura? ", "?>?>?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    EliminarFDarticulo(cnx, facturaid, articuloid, qty, total);
                }

                Console.WriteLine(FacturaID);
            }

            if (dataGridView1.Rows.Count > 0 && TipoF == "S")
            {
                int facturaid = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["FacturaID"];
                var price = Convert.ToSingle(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Precio"]);


                if (MessageBox.Show("Seguro que quieres eliminar esta factura? ", "?>?>?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                  DeleteFDServicio(cnx, facturaid, price);
                    MessageBox.Show("Eliminado con Exito", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            if (dataGridView1.Rows.Count > 0 && TipoF == "P")
            {

                int facturaid = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["FacturaID"];
                var price = Convert.ToSingle(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Precio"]);

                SqlCommand cmd = new SqlCommand("spEliminarFPaquetes", cnx);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@facturaID", facturaid);
                cmd.Parameters.AddWithValue("@precio", price);

                if (MessageBox.Show("Seguro que quieres eliminar esta factura? ", "?>?>?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { 
                
                cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("Eliminado con Exito ", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }






            }



        }

        private void EliminarFDarticulo(SqlConnection conexion, int factura, int articulo, int cantidad, float total)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spEliminarFArticulos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FacturaID", factura);
                cmd.Parameters.AddWithValue("@ArticuloID", articulo);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            if(dataGridView1.Rows.Count>0 && TipoF == "A")
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


            if (dataGridView1.Rows.Count > 0 && TipoF == "S")
            {
                int FacturaIDS = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["FacturaID"];
                int ServicioID = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["ServicioID"];
                int SolicitudID = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["SolicitudID"];
                var total = Convert.ToSingle(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Precio"]);
                EditarFServiciocs es = new EditarFServiciocs(cnx, FacturaIDS, ServicioID, SolicitudID,total);
                es.Visible = true ;

            }

            if (dataGridView1.Rows.Count > 0 && TipoF == "P")
            {
                int FacturaIDP = (int)TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["FacturaID"];
                var precioz = Convert.ToSingle(TabFDfacturas.DefaultView[dataGridView1.CurrentRow.Index]["Precio"]);


                EditarFacturaPaquete ep = new EditarFacturaPaquete(cnx,FacturaIDP,precioz);
                ep.Visible = true ; 
            
            
            }

        }

        private void DeleteFDServicio(SqlConnection conexion, int factura, float precio)
        {


            try
            {
                SqlCommand cmd = new SqlCommand("spDeleterFDServicio", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@facturaid", factura);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            } catch(SqlException ex) {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            EditarF(cnx, FacturaID, TipoF);

        }
    }
}
