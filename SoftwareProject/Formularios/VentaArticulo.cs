using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareProject.Formularios
{
    public partial class VentaArticulo : Form
    {
        private SqlConnection cnx;
        private int userID,Index;

        string Articulo, PrecioBase;

        SqlCommand cmd;
        SqlCommand cmd2;
        SqlDataReader data;
        SqlDataReader data1;

        DetalleArtVenta frm;
        public VentaArticulo()
        {
            InitializeComponent();
        }

        public VentaArticulo(SqlConnection conexion, int usuario)
        {
            InitializeComponent();
            cnx = conexion;
            userID = usuario;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void VentaArticulo_Load(object sender, EventArgs e)
        {
               

            cmd = new SqlCommand("select * from vInventario", cnx);
            data = cmd.ExecuteReader();

            if (data.HasRows)
            {
                while (data.Read())
                {
                    this.listBox1.Items.Add(data.GetString(1));
                }
                data.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                Index=listBox1.SelectedIndex +1;
                cmd = new SqlCommand("spMostrarDetaArt", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ArticuloId", listBox1.SelectedIndex+1);
                data1 = cmd.ExecuteReader();

                if (data1.HasRows) 
                { 
                    while (data1.Read())
                    {
                        Articulo = data1["nombre"].ToString();
                        PrecioBase = data1["precioBase"].ToString();
                    }
                data1.Close();
                }
                else
                {
                    MessageBox.Show("Ocurrio Un error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                data1.Close();
                DetalleArtVenta frm= new DetalleArtVenta(cnx,userID,Articulo,PrecioBase,Index);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor seleccione el articulo que desea comprar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
