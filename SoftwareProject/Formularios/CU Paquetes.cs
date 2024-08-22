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
    public partial class CU_Paquetes : Form
    {
        SqlConnection cnx;
        DataTable TabServicios;
        DataTable TabDisponibles;
        bool Editar;
        int PaqueteID, Horas;
        float Precio;
        String Nombre;


        public CU_Paquetes(SqlConnection conexion, bool edit, int Paquete = 0,String nombre = "", float precio = 0, int horas = 0)
        {
            InitializeComponent();
            cnx = conexion;
            Editar = edit;
            PaqueteID = Paquete;
            Nombre = nombre;
            Precio = precio;
            Horas = horas;
        }

        private void CU_Paquetes_Load(object sender, EventArgs e)
        {
            if (Editar == true)
            {
                label2.Text = "Paquete a Editar";
                label1.Text = "Servicios Actuales del Paquete";
                label4.Text = "Servicios Disponibles";
                txtNombre.Text = Nombre;
                txtPrecio.Text = Precio.ToString();
                txtHoras.Text = Horas.ToString();
                button1.Text = "Actualizar";
               
                try
                {
                    string query = "select s.serciciosID as ServicioID,s.Nombre,s.Precio from PaqueteServicio ps inner join Paquete p on p.PaqueteId = ps.PaqueteId inner join Servicio s on s.SerciciosId = ps.ServicioId where p.PaqueteId = @PaqueteID";
                    SqlCommand cmd = new SqlCommand(query,cnx);
                    cmd.Parameters.AddWithValue("@PaqueteID", PaqueteID);
                    TabDisponibles = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(TabDisponibles);
                    dataGridView1.DataSource = TabDisponibles;
                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.ScrollBars = ScrollBars.Both;
                   
                    TabServicios = new DataTable();
                    SqlDataAdapter adapter2 = new SqlDataAdapter("select * from Servicio", cnx);
                    adapter2.Fill(TabServicios);
                    dataGridView2.DataSource = TabServicios;
                    dataGridView2.ReadOnly = true;
                    dataGridView2.AllowUserToAddRows = false;
                    dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView2.ScrollBars = ScrollBars.Both;

                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                dataGridView2.Visible = false;
                label4.Visible = false;
                btnEliminarS.Visible = false;  
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Editar)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spActualizarPaquete", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaqueteID", PaqueteID);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Precio", float.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@Horas", Int32.Parse(txtHoras.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Se ha actualizado la informacion con exito", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception)
                {

                    throw;
                }
            }

            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spAgregarPaquete", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Precio", float.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@Horas", Int32.Parse(txtHoras.Text));
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Se ha agregado el Paquete con exito, Ahora defina los servicios que tendra", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnRegresar.Enabled = false;
                    button1.Enabled = false;

                    try
                    {
                        TabServicios = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter("select * from Servicio", cnx);
                        adapter.Fill(TabServicios);
                        dataGridView1.DataSource = TabServicios;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.AllowUserToAddRows = false;
                        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView1.ScrollBars = ScrollBars.Both;
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int Servicio = (int) TabServicios.DefaultView[dataGridView2.CurrentRow.Index]["SerciciosID"];

                SqlCommand cmd = new SqlCommand("spAgregarServicioExt", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServicioID", Servicio);
                cmd.Parameters.AddWithValue("@PaqueteID", PaqueteID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Servicio agregado al Paquete", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string query = "select s.serciciosID as ServicioID,s.Nombre,s.Precio from PaqueteServicio ps inner join Paquete p on p.PaqueteId = ps.PaqueteId inner join Servicio s on s.SerciciosId = ps.ServicioId where p.PaqueteId = @PaqueteID";
                SqlCommand cmd2 = new SqlCommand(query, cnx);
                cmd2.Parameters.AddWithValue("@PaqueteID", PaqueteID);
                TabDisponibles = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd2);
                adapter.Fill(TabDisponibles);
                dataGridView1.DataSource = TabDisponibles;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.ScrollBars = ScrollBars.Both;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnEliminarS_Click(object sender, EventArgs e)
        {
            try
            {
                String Nombre = TabDisponibles.DefaultView[dataGridView1.CurrentRow.Index]["Nombre"].ToString();
                DialogResult r = MessageBox.Show("Estas Seguro que quieres Eliminar el servicio" + Nombre + "?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (r == DialogResult.Yes)
                {
                    int ServicioID = (int)TabDisponibles.DefaultView[dataGridView1.CurrentRow.Index]["ServicioId"];

                    SqlCommand cmd = new SqlCommand("spEliminarServicioPaquete", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServicioID", ServicioID);
                    cmd.Parameters.AddWithValue("@PaqueteID", PaqueteID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Servicio Eliminado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string query = "select s.serciciosID as ServicioID,s.Nombre,s.Precio from PaqueteServicio ps inner join Paquete p on p.PaqueteId = ps.PaqueteId inner join Servicio s on s.SerciciosId = ps.ServicioId where p.PaqueteId = @PaqueteID";
                    SqlCommand cmd2 = new SqlCommand(query, cnx);
                    cmd2.Parameters.AddWithValue("@PaqueteID", PaqueteID);
                    TabDisponibles = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd2);
                    adapter.Fill(TabDisponibles);
                    dataGridView1.DataSource = TabDisponibles;
                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.ScrollBars = ScrollBars.Both;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int ServicioID = (int)TabServicios.DefaultView[dataGridView1.CurrentRow.Index]["SerciciosId"];

                SqlCommand cmd = new SqlCommand("spAgregarServicio", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServicioID", ServicioID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Servicio agregado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRegresar.Enabled = true;


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
