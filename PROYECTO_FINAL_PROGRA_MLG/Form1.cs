using Newtonsoft.Json;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public partial class Form1 : Form
    {
        ControladorCentral controlador = new ControladorCentral();
        List<Cliente> listaClientes = new List<Cliente>();
        string rutaClientes = "clientes.json";

        double litrosSimulados = 0.0; // Para pruebas sin Arduino

        public Form1()
        {
            InitializeComponent();
            InitializeComponent();
            CargarClientes();
            lblPrecioActual.Text = "Q37.35"; // Precio fijo
        }

        void GuardarClientes()
        {
            string json = JsonConvert.SerializeObject(listaClientes, Formatting.Indented);
            File.WriteAllText(rutaClientes, json);
        }

        void CargarClientes()
        {
            if (File.Exists(rutaClientes))
            {
                string json = File.ReadAllText(rutaClientes);
                listaClientes = JsonConvert.DeserializeObject<List<Cliente>>(json);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblPrecioLitro_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarCliente_Click(object sender, EventArgs e)
        {
            Cliente c = new Cliente();

            if (chkConsumidorFinal.Checked)
            {
                c.Nombre = "Consumidor Final";
                c.NIT = "CF";
                c.ConsumidorFinal = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNIT.Text))
                {
                    MessageBox.Show("Debe ingresar nombre y NIT.");
                    return;
                }

                c.Nombre = txtNombre.Text;
                c.NIT = txtNIT.Text;
                c.ConsumidorFinal = false;
            }

            // Si ya existe, lo reemplaza
            listaClientes.RemoveAll(x => x.NIT == c.NIT);
            listaClientes.Add(c);

            GuardarClientes();

            MessageBox.Show("Cliente guardado correctamente.");
        }

        private void btnActualizarCliente_Click(object sender, EventArgs e)
        {
            string nit = txtNIT.Text;

            Cliente encontrado = listaClientes.FirstOrDefault(x => x.NIT == nit);

            if (encontrado == null)
            {
                MessageBox.Show("No existe un cliente con ese NIT.");
                return;
            }

            txtNombre.Text = encontrado.Nombre;
            chkConsumidorFinal.Checked = encontrado.ConsumidorFinal;

            MessageBox.Show("Cliente cargado.");
        }
    }
}
