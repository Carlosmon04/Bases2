﻿using SoftwareProject.Formularios.Inventario;
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
    public partial class NuevoExistente : Form
    {
        private SqlConnection cnx;
        private int userID;
        public NuevoExistente()
        {
            InitializeComponent();
        }
        public NuevoExistente(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            cnx = conexion;
            userID = usuario;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

            if (form1 != null)
            {
                form1.OpenChildForm(new Compra(cnx, userID));
            }
        }

        private void btnExistente_Click(object sender, EventArgs e)
        {
            Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

            if (form1 != null)
            {
                form1.OpenChildForm(new InformacionInv(cnx, userID));
            }
        }

        private void NuevoExistente_Load(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
