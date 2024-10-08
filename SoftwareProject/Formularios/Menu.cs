﻿using SoftwareProject.Formularios;
using SoftwareProject.Formularios.Formularios_de_DELETE;
using SoftwareProject.Formularios.CRUD_Clientes;
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
using SoftwareProject.Formularios.Inventario;


namespace SoftwareProject
{
    public partial class Menu : Form
    {
        private SqlConnection cnx;
        private int userID;
        private int ClienteID;
        
        public Menu(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            Design();
            cnx= conexion;
            userID = usuario;
        }

        #region Metodos

        private void Design()
        {

            subMenuClie.Visible = false;
            subMenuEmpleado.Visible = false;
            subMenuInventario.Visible = false;
            subMenuS.Visible = false;
            subMenuP.Visible = false;
            subMenuFinanza.Visible = false;
            subMenuF.Visible = false;
        }

        private void RecuperarClienteID()
        {
            try
            {
                if (cnx.State == System.Data.ConnectionState.Closed)
                {
                    cnx.Open();
                }

                SqlCommand command = new SqlCommand("SELECT dbo.RecuperarClienteID(@UserID) AS ClienteID", cnx);
                command.Parameters.AddWithValue("@UserID", userID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ClienteID")))
                    {
                        int clienteID = reader.GetInt32(reader.GetOrdinal("ClienteID"));
                    }
                    else
                    {
                        MessageBox.Show("ClienteID es nulo o no se encontró.");
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recuperar ClienteID: " + ex.Message);
            }
        }

        private void Ocultar()
        { 
            if (subMenuClie.Visible == true)
            {
                subMenuClie.Visible = false;
            }

            if (subMenuEmpleado.Visible == true)
            {
                subMenuEmpleado.Visible = false;
            }

            if (subMenuInventario.Visible == true)
            {
                subMenuInventario.Visible = false;
            }

            if (subMenuS.Visible == true)
            {
                subMenuS.Visible = false;
            }

            if(subMenuP.Visible == true)
            {
                subMenuP.Visible = false;
            }

            if (subMenuFinanza.Visible == true)
            {
                subMenuFinanza.Visible = false;
            }

            if (subMenuF.Visible== true)
            {
               subMenuF.Visible = false; 
            }

        }


        private void Mostrar(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                Ocultar();
                subMenu.Visible = true;
            }
            else 
                subMenu.Visible = false; 
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuEmpleado);
        }

        private void btnAggUser_Click(object sender, EventArgs e)
        {
            OpenChildForm(new RegistroEmpleados(cnx, userID));
            Ocultar();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DeleteEmpleados(cnx));
            Ocultar();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuClie);
        }

        private void btnAggClie_Click(object sender, EventArgs e)
        {
            //probando Git
            OpenChildForm(new ConsultarClientes(cnx));
            Ocultar();
        }

      

        private Form activeForm = null;
        public void OpenChildForm (Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelForms.Controls.Add(childForm);
            panelForms.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private int TipoAutorizado(SqlConnection conexion, int usuario)
        {
            int quien = 10; // Valor por defecto
            try
            {
                SqlCommand cmd = new SqlCommand("spJefes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", usuario);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    // Imprimir el valor de jefeId para depuración
                    var jefeId = reader["jefeId"];
                    Console.WriteLine("jefeId: " + (jefeId == DBNull.Value ? "NULL" : jefeId.ToString()));

                    if (jefeId == DBNull.Value)
                    {
                        quien = 1;
                    }
                    else
                    {
                        quien = 2;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found."); // Depuración
                    quien = 0; // No hay filas, usuario no autorizado
                }

                reader.Close();
                return quien;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrió un Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return quien;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine(TipoAutorizado(cnx, userID));
            if (TipoAutorizado(cnx, userID) == 1)
           { 
               btnActividad.Visible = false;
               btnComprarA.Visible = false;
                button1.Visible = false;
                
           }

            if (TipoAutorizado(cnx, userID) == 0)
            { 
                btnEmpleados.Visible = false;
                btnClientes.Visible = false;
                btnInventario.Visible = false;
                btnAgregarS.Visible = false;
                btnCuentas.Visible = false;
                btnVerSolicitudes.Visible = false;
                btnFacturas.Visible = false;
            }

            if (TipoAutorizado(cnx, userID) == 2)
            { 
                btnEmpleados.Visible=false;    
                button1.Visible = false;
                btnComprarA.Visible=false;
               
            }
                
                
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuInventario);
        }

        private void btnCompraInv_Click(object sender, EventArgs e)
        {
            OpenChildForm(new NuevoExistente(cnx, userID));
            Ocultar();
        }

        private void btnInfoInv_Click(object sender, EventArgs e)
        {
            OpenChildForm(new InformacionInv(cnx,userID));
            Ocultar();
        }

        #endregion Metodos


        private void button1_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuS);
        }

        private void btnAgregarS_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CrearServicios(cnx, userID));
            Ocultar();
        }

        private void btnPaquetes_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuP);
        }

        private void btnAgregarP_Click(object sender, EventArgs e)
        {
           
            Ocultar();
        }

        private void btnInfoP_Click(object sender, EventArgs e)
        {
            OpenChildForm(new VerPaquetes(cnx,userID));
            Ocultar();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(new SolicitarServicio(cnx, userID));
            Ocultar();
        }

        private void btnFinanzas_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuFinanza);
        }

        private void btnCuentas_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CuentasB(cnx));
            Ocultar();
        }

        private void btnActividad_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Actividad(cnx, userID));
            Ocultar();
        }

        private void btnVerSolicitudes_Click(object sender, EventArgs e)
        {
            OpenChildForm(new VerSolicitudes(cnx, userID));
            Ocultar();
        }

        private void btnComprarA_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CompraArtC(cnx, userID));
            Ocultar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void btnFacturas_Click(object sender, EventArgs e)
        {
            Mostrar(subMenuF);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Ocultar();
            FacturasDetalle fd = new FacturasDetalle(cnx);
                fd.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ocultar();
            OpenChildForm(new crud_Medidas(cnx));
        }
    }
}
