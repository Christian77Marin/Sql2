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



namespace Sql2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Conexion
        private SqlConnection ConexionConLaBD = new SqlConnection(@"Server = (localDB)\MSSQLlocalDB; database=Empleados; Integrated Security = true");

        //Rellena el DataGridView
        void RellenarCuadricula()
        {
            string CadenaConsulta = "SELECT * FROM Personas ORDER BY DNI";
            SqlDataAdapter InstruccionesParaLaBD = new SqlDataAdapter(CadenaConsulta, ConexionConLaBD);

            DataTable Personas = new DataTable();
            InstruccionesParaLaBD.Fill(Personas);
            dtgvPersonas.DataSource = Personas;

        }

        //Ejecuta la sentencia
        void EjecutarSentencia(SqlCommand Comando)
        {
            ConexionConLaBD.Open();
            Comando.ExecuteNonQuery();
            ConexionConLaBD.Close();
            RellenarCuadricula();
            LimpiarTextBox();
        }


        //Se ejecuta al iniciar el formulario
        private void Form1_Load(object sender, EventArgs e)
        {
            RellenarCuadricula();
            this.Text = "Empleados";

            ResizeDataGridView();



        }
        //Boton Agregar, Ejecuta la sentencia INSERT
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            bool Comprobar;
            String CadenaConsulta = "IF NOT EXISTS(SELECT * FROM Personas WHERE Dni = @Dni) BEGIN INSERT INTO Personas (Dni, Nombre, Apellido1, Apellido2,Edad) VALUES(@Dni, @Nombre, @Apellido1, @Apellido2,@Edad)END";
            SqlCommand Insertar = new SqlCommand(CadenaConsulta, ConexionConLaBD);
            Insertar.Parameters.AddWithValue("@Dni", tbDni.Text);
            Insertar.Parameters.AddWithValue("@Nombre", tbNombre.Text);
            Insertar.Parameters.AddWithValue("@Apellido1", tbApellido1.Text);
            Insertar.Parameters.AddWithValue("@Apellido2", tbApellido2.Text);
            Insertar.Parameters.AddWithValue("@Edad", tbEdad.Text);
            if (Comprobar = int.TryParse(tbEdad.Text, out int n))
            {
                EjecutarSentencia(Insertar);
            }

        }

        //Boton Buscar, Busca en la base de datos con un reader
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            String CadenaConsulta = "SELECT * FROM Personas WHERE DNI = @Dni";
            SqlCommand Buscar = new SqlCommand(CadenaConsulta, ConexionConLaBD);
            Buscar.Parameters.AddWithValue("@Dni", tbDni.Text);

            ConexionConLaBD.Open();
            SqlDataReader reader = Buscar.ExecuteReader();

            if (reader.Read())
            {
                tbDni.Text = reader["Dni"].ToString();
                tbNombre.Text = reader["Nombre"].ToString();
                tbApellido1.Text = reader["Apellido1"].ToString();
                tbApellido2.Text = reader["Apellido2"].ToString();
                tbEdad.Text = reader["Edad"].ToString();
            }
            ConexionConLaBD.Close();
        }

        //Boton Modificar, Sentencia Update, Referenciando el DNI Modifica los demas parametros relacionados con un dni
        private void btnModificar_Click(object sender, EventArgs e)
        {
            bool Comprobar;
            String CadenaConsulta = "UPDATE Personas set Dni = @Dni, Nombre = @Nombre, Apellido1=@Apellido1 , Apellido2 = @Apellido2, Edad = @Edad WHERE Dni = @Dni";

            SqlCommand Modificar = new SqlCommand(CadenaConsulta, ConexionConLaBD);

            Modificar.Parameters.AddWithValue("@Dni", tbDni.Text);
            Modificar.Parameters.AddWithValue("@Nombre", tbNombre.Text);
            Modificar.Parameters.AddWithValue("@Apellido1", tbApellido1.Text);
            Modificar.Parameters.AddWithValue("@Apellido2", tbApellido2.Text);
            Modificar.Parameters.AddWithValue("@Edad", tbEdad.Text);

            if (Comprobar = int.TryParse(tbEdad.Text, out int n))
            {
                EjecutarSentencia(Modificar);
            }


        }

        //Boton Eliminar, Sentencia DELETE, Utiliza el DNI para eliminar todos los datos relacionados con ese Dni
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            String CadenaConsulta = "DELETE Personas WHERE DNI=@Dni";

            SqlCommand Borrar = new SqlCommand(CadenaConsulta, ConexionConLaBD);
            Borrar.Parameters.AddWithValue("@Dni", tbDni.Text);

            EjecutarSentencia(Borrar);
        }

        //Boton Limpiar, Limpia los TextBox
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarTextBox();
        }

        //Limpia los TextBox
        void LimpiarTextBox()
        {
            tbDni.Text = "";
            tbNombre.Text = "";
            tbApellido1.Text = "";
            tbApellido2.Text = "";
            tbEdad.Text = "";


        }

        //Boton Salir , Cierra el programa
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Evento de el textbox tbEdad cuando el texto se cambia mira si el texto puede ser convertido a int, si no es asi, manda un error con un "ErrorProvider"
        private void tbEdad_TextChanged(object sender, EventArgs e)
        {
            bool Comprobar;
            if (Comprobar = int.TryParse(tbEdad.Text, out int n) || tbEdad.Text == " " || tbEdad.Text == "")
            {
                epError.Clear();
            }
            else
            {
                epError.SetError(tbEdad, "Solo Admite numeros");
            }
        }
        //Adapta el DataGridView Al tamaño de la pantalla
        void ResizeDataGridView()
        {
            dtgvPersonas.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtgvPersonas.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtgvPersonas.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtgvPersonas.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtgvPersonas.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}

