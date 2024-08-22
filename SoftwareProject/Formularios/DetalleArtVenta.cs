using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareProject.Formularios
{
    public partial class DetalleArtVenta : Form
    {
        string Articulo, PrecioBase;
        private SqlConnection cnx;
        private int userID, ArticuloID;

        SqlCommand cmd;
        public DetalleArtVenta()
        {
            InitializeComponent();
        }

        public DetalleArtVenta(SqlConnection conexion, int usuario,string articulo,string precioBase, int AID)
        {
            InitializeComponent();
            cnx = conexion;
            userID = usuario;
            Articulo = articulo;
            PrecioBase = precioBase;
            ArticuloID = AID;
         
        }

        double precioTotal() 
        {
            double precio, precioTotal;

            precio = double.Parse(PrecioBase);
            precioTotal = precio * 1.15;

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

            Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

            if (form1 != null)
            {
                form1.OpenChildForm(new CompraArtC(cnx,userID));
            }
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            try
            {

            cmd = new SqlCommand("spCompraArtCliente", cnx);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ArticuloId", ArticuloID);
            cmd.Parameters.AddWithValue("@UsuarioId", userID);
            cmd.Parameters.AddWithValue("@precio", txtPrecioTotal.Text);
            cmd.Parameters.AddWithValue("@cantidad", txtCantidad.Text);
            cmd.ExecuteNonQuery();

            
                MessageBox.Show("Muchas Gracias por su compra", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Menu form1 = Application.OpenForms.OfType<Menu>().FirstOrDefault();

                if (form1 != null)
                {
                    form1.OpenChildForm(new CompraArtC(cnx, userID));
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrio un Error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
