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
        public Actividad(SqlConnection conexion , int usuario)
        {
            InitializeComponent();
            cnx = conexion;
            UserID = usuario;
        }
    }
}
