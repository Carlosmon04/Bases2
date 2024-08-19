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
using SoftwareProject.Formularios.Inventario;

namespace SoftwareProject.Formularios
{
    public partial class VerPaquetes : Form
    {
        SqlConnection cnx;
        DataTable TabPaquetes;
        int UsuarioID;
        int clienteID;

        public VerPaquetes(SqlConnection conexion, int usuarioID)
        {
            InitializeComponent();
            cnx = conexion;
            UsuarioID = usuarioID;
        }

        private void VerPaquetes_Load(object sender, EventArgs e)
        {
            try
            {
                TabPaquetes = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from dbo.TablaPaquetes()", cnx);
                adapter.Fill(TabPaquetes);
                dataGridView1.DataSource = TabPaquetes;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;
                RecuperarClienteID();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecuperarClienteID()
        {
            try
            {
                // Crear el comando SQL
                SqlCommand command = new SqlCommand("SELECT dbo.RecuperarClienteID(@UserID) AS ClienteID", cnx);
                command.Parameters.AddWithValue("@UserID", UsuarioID);

                // Ejecutar el comando y obtener el SqlDataReader
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Verificar si el valor recuperado no es nulo antes de intentar leerlo
                    if (!reader.IsDBNull(reader.GetOrdinal("ClienteID")))
                    {
                        clienteID = reader.GetInt32(reader.GetOrdinal("ClienteID"));
                        
                    }
                    else
                    {
                        MessageBox.Show("ClienteID es nulo o no se encontró.");
                    }
                }

                // Cerrar el SqlDataReader
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recuperar ClienteID: " + ex.Message);
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBusqueda.Text;

            string filtro = string.Format("Convert(Nombre, 'System.String') like '%{0}%'", busqueda);

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filtro;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si se ha seleccionado una fila
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                try
                {
                    // Obtén el valor de la columna 'PaqueteId'
                    object paqueteIdObj = TabPaquetes.DefaultView[e.RowIndex]["PaqueteId"];
                    int paqueteId = Convert.ToInt32(paqueteIdObj);

                    // Obtén el valor de la columna 'Nombre'
                    string nombre = TabPaquetes.DefaultView[e.RowIndex]["Nombre"].ToString();

                    // Obtén el valor de la columna 'Precio'
                    object precioObj = TabPaquetes.DefaultView[e.RowIndex]["Precio"];
                    float precio = Convert.ToSingle(precioObj);

                    // Obtén el valor de la columna 'Horas'
                    object horasObj = TabPaquetes.DefaultView[e.RowIndex]["CantidadHoras"];
                    int horas = Convert.ToInt32(horasObj);

                    // Verifica si el formulario 'form1' no es nulo antes de usarlo
                    if (form1 != null)
                    {
                        form1.OpenChildForm(new Paquetes(cnx,clienteID, paqueteId, nombre, precio, horas));
                    }
                }
                catch (Exception ex)
                {
                    // Maneja la excepción y muestra un mensaje de error si es necesario
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
