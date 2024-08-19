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
    public partial class CrearServicios : Form
    {
        private SqlConnection cnx;
        private int userID;
        public CrearServicios()
        {
            InitializeComponent();
        }

        public CrearServicios(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            cnx = conexion;
            userID = usuario;
        }

        private void CrearServicios_Load(object sender, EventArgs e)
        {
            CargarServicios();
        }

        private void CargarServicios()
        {
            try
            {
                DataTable TabServicio = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Servicio", cnx);
                adapter.Fill(TabServicio);
                dataGridView1.DataSource = TabServicio;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrió un Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int ServicioId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["SerciciosId"].Value);
                    var confirmResult = MessageBox.Show("¿Estás seguro de eliminar este servicio?",
                                            "Confirmar Eliminación",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.Yes)
                    {
                        using (SqlCommand cmd = new SqlCommand("spEliminarServicio", cnx))
                        {
                            cnx.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ServicioId", ServicioId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un servicio para eliminar.");
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            CargarServicios();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["SerciciosId"].Value);
                NuevoServicio form = new NuevoServicio(cnx, true, selectedId);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CargarServicios();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un servicio para editar.");
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            NuevoServicio from = new NuevoServicio(cnx);
            if (from.ShowDialog() == DialogResult.OK)
            {
                CargarServicios();
            }
        }
    }
}
