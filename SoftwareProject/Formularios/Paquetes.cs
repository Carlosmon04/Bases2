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
        private int Cliente, Usuario;


        private void Paquetes_Load(object sender, EventArgs e)
        {
            CargarDatos();
            txtNombre.Text = Nombre;
            txtHoras.Text = Horas.ToString();
            txtPrecio.Text = Precio.ToString();
        }

        public Paquetes(SqlConnection conexion, int cliente,int id, String nombre, float precio, int horas, int UsuarioID )
        {
            InitializeComponent();
            cnx = conexion;
            Cliente = cliente;
            Nombre = nombre;
            Id = id;
            Precio = precio;
            Horas = horas;
            Usuario = UsuarioID;
        }




        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();

            Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

            if (form1 != null)
            {
                form1.OpenChildForm(new VerPaquetes(cnx,Usuario));
            }
           

        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
    {
                string query = "SELECT * FROM FDPaquete p INNER JOIN Factura f ON f.FacturaID = p.FacturaID WHERE f.ClienteID = @ClienteID";

                using (SqlCommand command = new SqlCommand(query, cnx))
                {
                    command.Parameters.AddWithValue("@ClienteID", Cliente);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["PaqueteID"] != DBNull.Value)
                            {
                                MessageBox.Show("Este usuario ya tiene un Paquete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return; // No es necesario cerrar el reader aquí, se cierra automáticamente al salir del using
                            }
                        }
                    }

                    // Si llegamos aquí, el usuario no tiene un paquete y podemos proceder a adquirirlo
                    string query2 = "exec spAdquirirPaquete @Usuario, @Paquete, @Precio";

                    using (SqlCommand command2 = new SqlCommand(query2, cnx))
                    {
                        command2.Parameters.AddWithValue("@Usuario", Cliente);
                        command2.Parameters.AddWithValue("@Paquete", Id);
                        command2.Parameters.AddWithValue("@Precio", Precio);

                        command2.ExecuteNonQuery();
                        MessageBox.Show("Paquete Adquirido con éxito", "Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                        if (form1 != null)
                        {
                            form1.OpenChildForm(new VerPaquetes(cnx, Usuario));
                        }
                    }
                }
            }
    catch (Exception ex)
    {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

