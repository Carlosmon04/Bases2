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
    public partial class CuentasB : Form
    {
        private SqlConnection cnx;
        DataTable TabCuentas;
        public CuentasB(SqlConnection conexion)
        {
            InitializeComponent();
            cnx=conexion;
        }

        private void CuentasB_Load(object sender, EventArgs e)
        {
            TablaCuenta(cnx);
        }


        private void TablaCuenta(SqlConnection conexion)
        {

            try
            {
                TabCuentas = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter("spActualizarFinanzas", conexion);
                adapter.Fill(TabCuentas);
                dataGridView1.DataSource = TabCuentas;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            TablaCuenta(cnx);
        }
    }
}
