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
        private int userID, Index;

        string Articulo, PrecioBase;

        private Dictionary<int, string> map = new Dictionary<int, string>();


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
            CargarDatos();
            FiltrarYAsignarDatos();
        }

        private void CargarDatos( )
        {
            string url = "Server= 3.128.144.165;" +
                             "DataBase= DB20222000953;" +
                             "User ID= david.rodriguez;" +
                             "password= DR20222000953;";


            using (SqlConnection cnx= new SqlConnection(url))
            {
                SqlCommand cmd = new SqlCommand("select * from vInventarioTodos",cnx);
                
                cnx.Open();
                SqlDataReader data = cmd.ExecuteReader();

                int index = 0;
                while (data.Read())
                {
                    
                    map.Add(index, data["Articulo"].ToString());
                    index++;
                }
                data.Close();
            }
        }
        private void FiltrarYAsignarDatos()
        {
            // Ejemplo: ocultar el ítem en el índice 1
            List<int> indicesAExcluir = new List<int> { 1,2 ,3};
            var indicesVisibles = map.Where(x => !indicesAExcluir.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

            // Asignar los valores filtrados al ListBox
            listBox1.DataSource = new BindingSource(indicesVisibles, null);
            listBox1.DisplayMember = "Value";  // Lo que se muestra en el ListBox
            listBox1.ValueMember = "Key";      // El índice original
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var itemSeleccionado = (KeyValuePair<int, string>)listBox1.SelectedItem;
                int indiceOriginal = itemSeleccionado.Key;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }



        private void VentaArticulo_Load(object sender, EventArgs e)
        {
            //cmd = new SqlCommand("vInventarioTodos", cnx);
            //cmd.CommandType = CommandType.StoredProcedure;
            //data = cmd.ExecuteReader();


        }



    private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                Index=listBox1.SelectedIndex +1;
                cmd = new SqlCommand("spMostrarDetaArt1", cnx);
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
