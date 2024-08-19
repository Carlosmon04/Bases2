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
    public partial class NuevoServicio : Form
    {
        private bool isEditMode;
        private int servicioId;
        private SqlConnection cnx;
        public NuevoServicio(SqlConnection conexion)
        {
            InitializeComponent();
            cnx = conexion;
        }

        public NuevoServicio(SqlConnection conexion, bool editMode = false, int? id = null)
        {
            InitializeComponent();
            cnx = conexion;
            isEditMode = editMode;

            if (isEditMode && id.HasValue)
            {
                servicioId = id.Value;
                CargarServicioData(servicioId);
            }
        }

        private void CargarServicioData(int id)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("spObtenerServicioPorId", cnx))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SerciciosId", id);

                    cnx.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtPrecio.Text = reader["Precio"].ToString();
                        cmbEstado.SelectedItem = reader["Estado"].ToString();
                    }
                    cnx.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el servicio: " + ex.Message);
            }
            finally
            {
                if (cnx.State == ConnectionState.Open)
                {
                    cnx.Close();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            float precio = float.Parse(txtPrecio.Text);
            string estado = cmbEstado.SelectedItem.ToString();

            try
            {
                if (isEditMode)
                {
                    // Modo de edición
                    ActualizarServicio(servicioId, nombre, precio, estado, cnx);
                }
                else
                {
                    // Modo de agregar
                    AgregarServicio(nombre, precio, estado, cnx);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message);
            }
        }

        private void AgregarServicio(string nombre, float precio, string estado, SqlConnection conexion)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("spInsertarServicio", cnx))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Precio", precio);
                    command.Parameters.AddWithValue("@Estado", estado);

                    conexion.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int nuevoId = Convert.ToInt32(reader["SerciciosId"]);
                        MessageBox.Show("Nuevo Servicio insertado con ID: " + nuevoId);
                    }
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el servicio: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }

        private void ActualizarServicio(int id, string nombre, float precio, string estado, SqlConnection conexion)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("spActualizarServicio", cnx))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SerciciosId", id);
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Precio", precio);
                    command.Parameters.AddWithValue("@Estado", estado);

                    conexion.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Servicio actualizado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el servicio: " + ex.Message);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
        }
    }
}
