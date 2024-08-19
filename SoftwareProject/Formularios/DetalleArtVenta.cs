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
    public partial class DetalleArtVenta : Form
    {
        string Articulo, PrecioBase;
        int Index;

        private SqlConnection cnx;
        private int userID;

        SqlCommand cmd;
        public DetalleArtVenta()
        {
            InitializeComponent();
        }

        public DetalleArtVenta(SqlConnection conexion, int usuario,string articulo,string precioBase, int index)
        {
            InitializeComponent();
            cnx = conexion;
            userID = usuario;
            Articulo = articulo;
            PrecioBase = precioBase;
            Index = index;
        }

        double precioTotal() 
        {
            double precio, precioTotal;

            precio = double.Parse(PrecioBase);
            precioTotal = precio * 1.20;

            return precioTotal; 
        }
        private void DetalleArtVenta_Load(object sender, EventArgs e)
        {
            Console.WriteLine(userID);

            txtArticulo.ReadOnly = true;
            txtPrecioTotal.ReadOnly = true;

            txtArticulo.Text = Articulo;
            txtPrecioTotal.Text = precioTotal().ToString();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad))
            {
                double precioTotal1 = cantidad * precioTotal();
                txtPrecioTotal.Text = precioTotal1.ToString(); 
            }
            else
            {
                txtPrecioTotal.Text = precioTotal().ToString();
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números en este campo.", "Entrada no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            try
            {

            cmd = new SqlCommand("spCompraArtCliente", cnx);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ArticuloId", Index);
            cmd.Parameters.AddWithValue("@UsuarioId", userID);
            cmd.Parameters.AddWithValue("@precio", txtPrecioTotal.Text);
            cmd.Parameters.AddWithValue("@cantidad", txtCantidad.Text);
            cmd.ExecuteNonQuery();

            txtCantidad.Clear();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
