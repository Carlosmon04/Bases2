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
    public partial class Actividad : Form
    {
        private SqlConnection cnx;
        private int UserID;
        DataTable TabPaquetesF;
        DataTable TabServiciosF;
        DataTable TabArticulosF;
        public Actividad(SqlConnection conexion , int usuario)
        {
            InitializeComponent();
            cnx = conexion;
            UserID = usuario;
        }



   

        private void Actividad_Load(object sender, EventArgs e)
        {
            DatosCliente(cnx, UserID);
            FacturasPaquete(cnx, UserID);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DatosCliente(SqlConnection conexion ,int usuario)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spClienteUnico", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", usuario);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    labelNombre.Text = reader["Nombre"].ToString();
                    labelDNI.Text = reader["DNI"].ToString();
                    labelCorreo.Text = reader["E_mail"].ToString();
                    labelTelefono.Text = reader["Telefono"].ToString();
                    labelDireccion.Text = reader["Direccion"].ToString();
                    labelUsername.Text = reader["Username"].ToString();
                    labelPaquete.Text = reader["PaqueteId"].ToString();
                    labelHoras.Text = reader["Horas"].ToString();

                }
                reader.Close();
                cmd.Dispose();


            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void FacturasServicio(SqlConnection conexion, int usuario)
        {
            try
            {
                TabServiciosF = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("spFacturasServicio", conexion);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@user", usuario);
                adapter.Fill(TabServiciosF);
                dataGridView1.DataSource = TabServiciosF;
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

        private void FacturasPaquete(SqlConnection conexion, int usuario)
        {
            try
            {
                TabPaquetesF = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("spFacturasPaquete", conexion);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@user", usuario);
                adapter.Fill(TabPaquetesF);
                dataGridView1.DataSource = TabPaquetesF;
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


        private void FacturasArticulo(SqlConnection conexion, int usuario)
        {
            try
            {
                TabArticulosF = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("spFacturasArticulo ", conexion);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.AddWithValue("@user", usuario);
                adapter.Fill(TabArticulosF);
                dataGridView1.DataSource = TabArticulosF;
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

        private void cmxFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmxFacturas.SelectedItem.ToString() == "Paquetes") FacturasPaquete(cnx, UserID);
            if (cmxFacturas.SelectedItem.ToString() == "Servicios") FacturasServicio(cnx, UserID);
            if (cmxFacturas.SelectedItem.ToString() == "Articulos") FacturasArticulo(cnx, UserID);



        }
    }
}
