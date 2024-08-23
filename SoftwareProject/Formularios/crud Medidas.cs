using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SoftwareProject.Formularios
{
    public partial class crud_Medidas : Form
    {
        SqlConnection cnx;
        DataTable TabMedidas;
        SqlDataAdapter adpMedidas;
        public crud_Medidas(SqlConnection conexion)
        {
            InitializeComponent();
            cnx = conexion;

            adpMedidas = new SqlDataAdapter();
            adpMedidas.InsertCommand = comando("spAggMedida", conexion);
            adpMedidas.UpdateCommand = comando("spEditMedida", conexion);
            adpMedidas.DeleteCommand = comando("spBorrarMedida", conexion);

            dataGridView1.DefaultValuesNeeded += dataGridView1_DefaultValuesNeeded;
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private SqlCommand comando(String sql, SqlConnection cnx)
        {
            SqlCommand cmd = new SqlCommand(sql, cnx);
            cmd.CommandType = CommandType.StoredProcedure;

            if(sql == "spEditMedida" || sql == "spAggMedida")
            {
               cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50, "Nombre");
            }
            
            if (sql == "spEditMedida" || sql == "spBorrarMedida")
            {
                cmd.Parameters.Add("@MedidaID", SqlDbType.Int, 4, "MedidaID").SourceVersion = DataRowVersion.Original;
            }

            return cmd;
        }



        private void crud_Medidas_Load(object sender, EventArgs e)
        {
            

            try
            {
                TabMedidas = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from medida", cnx);
                adapter.Fill(TabMedidas);
                dataGridView1.DataSource = TabMedidas;
                dataGridView1.Columns["MedidaId"].ReadOnly = true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // Obtener el próximo ID incrementando el máximo actual
            int maxId = 0;
            if (TabMedidas.Rows.Count > 0)
            {
                maxId = Convert.ToInt32(TabMedidas.Compute("MAX(MedidaID)", string.Empty));
            }

            e.Row.Cells["MedidaID"].Value = maxId + 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                adpMedidas.Update(TabMedidas);
                MessageBox.Show("Cambios guardados exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Verificar que la fila no sea nueva y que se pueda eliminar
                if (!selectedRow.IsNewRow)
                {
                    // Eliminar la fila del DataTable
                    TabMedidas.Rows[selectedRow.Index].Delete();

                    try
                    {
                        // Aplicar la eliminación en la base de datos a través del SqlDataAdapter
                        adpMedidas.Update(TabMedidas);
                        MessageBox.Show("Fila eliminada exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la fila: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No se puede eliminar una fila nueva sin guardar.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }
        }
}
