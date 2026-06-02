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
    }
}
