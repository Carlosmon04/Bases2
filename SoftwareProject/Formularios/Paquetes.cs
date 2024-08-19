using SoftwareProject.Formularios.Formularios_de_DELETE;
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
    public partial class Paquetes : Form
    {
        private SqlConnection cnx;
        private SqlCommand cmd;
        private SqlDataReader data;
        private String Nombre;
        private int Id,Horas;
        private float Precio;
        private int Cliente;


        private void Paquetes_Load(object sender, EventArgs e)
        {
            CargarDatos();
            txtNombre.Text = Nombre;
            txtHoras.Text = Horas.ToString();
            txtPrecio.Text = Precio.ToString();
        }

        public Paquetes(SqlConnection conexion,int cliente, int id, String nombre, float precio, int horas )
        {
            InitializeComponent();
            cnx = conexion;
            Cliente = cliente;
            Nombre = nombre;
            Id = id;
            Precio = precio;
            Horas = horas;
        }




        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();

            Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

            if (form1 != null)
            {
                form1.OpenChildForm(new VerPaquetes(cnx,Cliente));
            }
        }

        private void btnAdquirir_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "exec spAdquirirPaquete @Usuario, @Paquete, @Precio";
                SqlCommand command = new SqlCommand(query, cnx);
                command.Parameters.AddWithValue("@Usuario", Cliente);
                command.Parameters.AddWithValue("@Paquete", Id);
                command.Parameters.AddWithValue("@Precio", Precio);

                command.ExecuteNonQuery();
                MessageBox.Show("Paquete Adquirido con exito", "Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                if (form1 != null)
                {
                    form1.OpenChildForm(new VerPaquetes(cnx,Cliente));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
           

        }

        private void CargarDatos()
        {
            try
            {
                string query = "exec spRecuperarServicios @ID";

               
 
                SqlCommand command = new SqlCommand(query, cnx);
                command.Parameters.AddWithValue("@ID", Id);
                SqlDataReader reader = command.ExecuteReader();

                int yOffset = 5; 

               
                if (!this.Controls.Contains(panelLabels))
                {
                    this.Controls.Add(panelLabels);
                    panelLabels.AutoScroll = true; 
                }


                while (reader.Read())
                {
                    string nombre = reader.GetString(0);

                    Label nuevoLabel = new Label
                    {
                        Text = nombre,
                        AutoSize = true,
                        Location = new System.Drawing.Point(10, yOffset),
                        Font = new Font("Cambria Math", 8, FontStyle.Bold)
                    };

                    
                    panelLabels.Controls.Add(nuevoLabel);
                    yOffset += nuevoLabel.Height - 10;
                    nuevoLabel.ForeColor = Color.Black;
                   
                }

                reader.Close();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}

