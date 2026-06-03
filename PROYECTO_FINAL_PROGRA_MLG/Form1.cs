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
            this.FormClosing += Form1_FormClosing;
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
            if (abastecimientoActual == null)
            {
                MessageBox.Show("No hay abastecimiento activo.");
                return;
            }

            controlador.Registro.Agregar(abastecimientoActual);

            // Liberar bomba
            controlador.Bombas[abastecimientoActual.NumeroBomba - 1].Finalizar();

            MessageBox.Show("Abastecimiento guardado.");

            // Limpiar
            abastecimientoActual = null;
            lblLitrosServidos.Text = "0.00";
            lblTotalCobrar.Text = "0.00";
            txtMonto.Clear();
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






        private Abastecimiento abastecimientoActual;

        private void btnIniciarAbastecimiento_Click(object sender, EventArgs e)
        {
            if (cboBomba.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una bomba.");
                return;
            }

            int numeroBomba = int.Parse(cboBomba.SelectedItem.ToString());
            Bomba bomba = controlador.Bombas[numeroBomba - 1];

            if (bomba.Ocupada)
            {
                MessageBox.Show("La bomba está ocupada.");
                return;
            }

            // Obtener cliente
            Cliente cliente;

            if (chkConsumidorFinal.Checked)
            {
                cliente = new Cliente { Nombre = "Consumidor Final", NIT = "CF", ConsumidorFinal = true };
            }
            else
            {
                cliente = listaClientes.FirstOrDefault(x => x.NIT == txtNIT.Text);

                if (cliente == null)
                {
                    MessageBox.Show("Debe guardar el cliente primero.");
                    return;
                }
            }

            // Crear abastecimiento
            if (rdbPrepago.Checked)
            {
                if (!double.TryParse(txtMonto.Text, out double monto))
                {
                    MessageBox.Show("Monto inválido.");
                    return;
                }

                abastecimientoActual = new AbastecimientoPrepago
                {
                    Cliente = cliente,
                    NumeroBomba = numeroBomba,
                    MontoPagado = monto
                };
                // Asignar Id del abastecimiento al NIT del cliente


                abastecimientoActual.Id = cliente.NIT;
            }
            else if (rdbTanqueLleno.Checked)
            {
                abastecimientoActual = new AbastecimientoTanqueLleno
                {
                    Cliente = cliente,
                    NumeroBomba = numeroBomba
                };
                // Asignar Id del abastecimiento al NIT del cliente
                abastecimientoActual.Id = cliente.NIT;
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de abastecimiento.");
                return;
            }

            bomba.Iniciar();
            litrosSimulados = 0;
            lblLitrosServidos.Text = "0.00";
            lblTotalCobrar.Text = "0.00";

            MessageBox.Show("Abastecimiento iniciado.");
        }

        private void btnSimularMasLitros_Click(object sender, EventArgs e)
        {
            litrosSimulados += 0.1;
            lblLitrosServidos.Text = litrosSimulados.ToString("0.00");

            if (abastecimientoActual is AbastecimientoPrepago prepago)
            {
                double litrosMax = prepago.MontoPagado / 37.35;

                if (litrosSimulados >= litrosMax)
                {
                    litrosSimulados = litrosMax;
                    lblLitrosServidos.Text = litrosSimulados.ToString("0.00");
                    MessageBox.Show("Prepago completado.");
                }
            }
        }

        private void btnSimularFinalizar_Click(object sender, EventArgs e)
        {
            if (abastecimientoActual == null)
            {
                MessageBox.Show("No hay abastecimiento en curso.");
                return;
            }

            abastecimientoActual.LitrosServidos = litrosSimulados;
            abastecimientoActual.Procesar();

            lblTotalCobrar.Text = abastecimientoActual.TotalCobrado.ToString("0.00");

            MessageBox.Show("Simulación finalizada.");
        }

        private void btnEnviarJsonSimulado_Click(object sender, EventArgs e)
        {
            if (abastecimientoActual == null)
            {
                MessageBox.Show("No hay abastecimiento activo.");
                return;
            }

            var json = $@"
    {{
        ""bomba"": {abastecimientoActual.NumeroBomba},
        ""litrosServidos"": {litrosSimulados.ToString("0.00")},
        ""finalizado"": true
    }}";

            controlador.RecibirRespuestaJSON(json);

            lblTotalCobrar.Text = abastecimientoActual.TotalCobrado.ToString("0.00");

            MessageBox.Show("JSON simulado enviado.");
        }

        private void btnReporteDiario_Click(object sender, EventArgs e)
        {
            DateTime fecha = dtpFechaReporte.Value.Date;

            var lista = controlador.Registro.FiltrarPorDia(fecha);

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = lista;
        }

        private void btnReportePrepago_Click(object sender, EventArgs e)
        {
            var lista = controlador.Registro.ReportePrepago();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = lista;
        }

        private void btnReporteTanqueLleno_Click(object sender, EventArgs e)
        {
            var lista = controlador.Registro.ReporteTanqueLleno();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = lista;
        }

        private void btnReporteBombaMasUsada_Click(object sender, EventArgs e)
        {
            var bomba = controlador.Registro.BombaMasUsada(controlador.Bombas);

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = new List<Bomba> { bomba };
        }

        private void btnReporteBombaMenosUsada_Click(object sender, EventArgs e)
        {
            var bomba = controlador.Registro.BombaMenosUsada(controlador.Bombas);

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = new List<Bomba> { bomba };
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNIT_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object? sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // Si hay un abastecimiento en curso, procesarlo y guardarlo antes de cerrar
            if (abastecimientoActual != null)
            {
                abastecimientoActual.LitrosServidos = Math.Round(litrosSimulados, 2);
                abastecimientoActual.Procesar();

                controlador.Registro.Agregar(abastecimientoActual);

                // Liberar la bomba si es posible
                try
                {
                    controlador.Bombas[abastecimientoActual.NumeroBomba - 1].Finalizar();
                }
                catch { }

                abastecimientoActual = null;
            }

            // Asegurar que el registro esté guardado
            controlador.Registro.GuardarEnArchivo();
        }
    }
}
