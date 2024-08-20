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
    public partial class EditarSoli : Form
    {
        DateTime Visita;
        private int year;
        private int month;
        private int day;
        private int hour;
        private String Hora;
        private SqlConnection cnx;
        private int ClienteID;
        private int SolicitudID;
            
        public EditarSoli(SqlConnection conxion,int cliente ,int solicitud)
        {
            InitializeComponent();
            cnx = conxion;
            ClienteID = cliente;
            SolicitudID = solicitud;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmarHora_Click(object sender, EventArgs e)
        {

            try
            {

                SqlCommand cmd = new SqlCommand("spEditarSolicitudes", cnx);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SolicitudID", SolicitudID);
                cmd.Parameters.AddWithValue("@ClienteID", ClienteID);
                cmd.Parameters.AddWithValue("@HoraEntrada", FechaQuerida());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cita Actualizada ", "Listo", MessageBoxButtons.OK, MessageBoxIcon.None);

            }
            catch (SqlException ex) {
            MessageBox.Show("Ocurrio un ERROR" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

        }

        private void EditarSoli_Load(object sender, EventArgs e)
        {

        }


        private DateTime FechaQuerida()
        {
            if (cmxHoras.SelectedItem.ToString() == "8:00") Hora = "8";
            if (cmxHoras.SelectedItem.ToString() == "9:00") Hora = "9";
            if (cmxHoras.SelectedItem.ToString() == "10:00") Hora = "10";
            if (cmxHoras.SelectedItem.ToString() == "11:00") Hora = "11";
            if (cmxHoras.SelectedItem.ToString() == "12:00") Hora = "12";
            if (cmxHoras.SelectedItem.ToString() == "13:00") Hora = "13";
            if (cmxHoras.SelectedItem.ToString() == "14:00") Hora = "14";
            if (cmxHoras.SelectedItem.ToString() == "15:00") Hora = "15";
            if (cmxHoras.SelectedItem.ToString() == "16:00") Hora = "16";
            if (cmxHoras.SelectedItem.ToString() == "17:00") Hora = "17";
            if (cmxHoras.SelectedItem.ToString() == "18:00") Hora = "18";
            DateTime r = Calendar1.SelectionEnd;
            year = r.Year;
            month = r.Month;
            day = r.Day;
            hour = Convert.ToInt32(Hora);

            Visita = new DateTime(year, month, day, hour, 0, 0);
            return Visita;
        }

        private void cmxHoras_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime max = DateTime.Now;
            Console.WriteLine(max);
            Console.WriteLine(FechaQuerida());

            if (max > FechaQuerida())
            {
                int x = max.Hour;

                String y = cmxHoras.SelectedIndex.ToString();
                int z = Convert.ToInt32(y);

                if (z < x)
                {
                    MessageBox.Show("Esta hora ya no esta disponible por Hoy ", "Hora Fuera Rango", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void Calendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (e.Start.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Cerrado Dias Domingo", "Cerrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
