using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public partial class Form1 : Form
    {
        ControladorCentral controlador = new ControladorCentral();
        private SerialPort serialArduino;
        private ManualResetEventSlim ackEvent = new ManualResetEventSlim(false);
        private volatile bool ackReceived = false;
        private volatile int lastAckBomba = 0;
        private double lastAckLimite = 0.0;
        List<Cliente> listaClientes = new List<Cliente>();
        string rutaClientes = "clientes.json";

        double litrosSimulados = 0.0;


        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
            CargarClientes();
            InicializarSerial();
            lblPrecioActual.Text = "Q37.35";
        }

        void InicializarSerial()
        {
            try
            {
                serialArduino = new SerialPort();

                var ports = SerialPort.GetPortNames();
                // Preferir COM3 si existe (usuario indicó que el Arduino está en COM3)
                if (ports != null && ports.Length > 0)
                {
                    string chosen = ports[0];
                    for (int i = 0; i < ports.Length; i++)
                    {
                        if (ports[i].ToUpper() == "COM3") { chosen = ports[i]; break; }
                    }
                    serialArduino.PortName = chosen;
                }
                else
                {
                    serialArduino.PortName = "COM3"; // fallback
                }
                serialArduino.BaudRate = 9600;
                serialArduino.NewLine = "\n";
                serialArduino.DataReceived += serialArduino_DataReceived;

                // Evitar reinicio automático en algunos adaptadores: deshabilitar DTR antes de abrir
                serialArduino.DtrEnable = false;
                serialArduino.Open();

                // Dar tiempo al Arduino para reiniciarse/estabilizarse si ocurre
                Thread.Sleep(1200);
                try { serialArduino.DiscardInBuffer(); } catch { }
                try { serialArduino.DiscardOutBuffer(); } catch { }
                Debug.WriteLine($"Serial abierto en {serialArduino.PortName}");
                MessageBox.Show($"Puerto serie abierto: {serialArduino.PortName}", "Puerto serie");
            }
            catch (Exception ex)
            {
                // Si no se puede abrir el puerto, continuar en modo simulado
                MessageBox.Show($"No se pudo abrir el puerto serie: {ex.Message}");
            }
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

        // Métodos requeridos por el Designer (stubs para evitar errores de compilación)
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Si se marca Consumidor Final, completar los campos y deshabilitarlos
            try
            {
                if (chkConsumidorFinal.Checked)
                {
                    txtNombre.Text = "Consumidor Final";
                    txtNIT.Text = "CF";
                    txtNombre.Enabled = false;
                    txtNIT.Enabled = false;
                }
                else
                {
                    txtNombre.Enabled = true;
                    txtNIT.Enabled = true;
                }
            }
            catch { }
        }

        private void txtNIT_TextChanged(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // No-op: handler vacío requerido por Designer.
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            // No-op: handler vacío requerido por Designer.
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // No-op
        }

        private void lblPrecioLitro_Click(object sender, EventArgs e)
        {
            // No-op
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
     
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (abastecimientoActual == null)
            {
                MessageBox.Show("No hay abastecimiento activo.");
                return;
            }

            // Liberar bomba (el registro ya fue agregado al iniciar el abastecimiento)
            controlador.Bombas[abastecimientoActual.NumeroBomba - 1].Finalizar();

            MessageBox.Show("Abastecimiento guardado.");

            // Limpiar
            abastecimientoActual = null;
            lblLitrosServidos.Text = "0.00";
            lblTotalCobrar.Text = "0.00";
            txtMonto.Clear();
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

            // Agregar al registro para que la respuesta del Arduino pueda actualizarlo
            controlador.Registro.Agregar(abastecimientoActual);

            // Enviar orden JSON al Arduino (si está disponible)
            try
            {
                string json = controlador.EnviarOrdenJSON(abastecimientoActual);

                // Si el puerto no está inicializado o cerrado, intentar reabrir
                if (serialArduino == null || !serialArduino.IsOpen)
                {
                    InicializarSerial();
                    // pequeña espera para que el puerto se estabilice
                    Thread.Sleep(100);
                }

                if (serialArduino != null && serialArduino.IsOpen)
                {
                    // Preparar espera de ACK
                    ackEvent.Reset();
                    ackReceived = false;
                    try
                    {
                        Debug.WriteLine($"Enviando a {serialArduino.PortName}: {json}");
                        serialArduino.WriteLine(json);
                        MessageBox.Show($"Enviado al puerto {serialArduino.PortName}: {json}", "Depuración");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al escribir en el puerto {serialArduino.PortName}: {ex.Message}");
                    }

                    // Esperar ACK hasta 2s
                    bool got = ackEvent.Wait(2000);
                    if (!got)
                    {
                        MessageBox.Show("No se recibió confirmación del Arduino (timeout).");
                    }
                    else
                    {
                        if (ackReceived && lastAckBomba == abastecimientoActual.NumeroBomba)
                        {
                            // ACK válido recibido
                        }
                        else
                        {
                            MessageBox.Show("Confirmación recibida pero no coincide con la bomba esperada.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Puerto serie no disponible. Orden no enviada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enviando orden por serie: {ex.Message}");
            }

            bomba.Iniciar();
            litrosSimulados = 0;
            lblLitrosServidos.Text = "0.00";
            lblTotalCobrar.Text = "0.00";

            MessageBox.Show("Abastecimiento iniciado.");
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

        private void serialArduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string json = serialArduino.ReadLine();
                // Procesar en hilo UI via Invoke
                this.Invoke(new Action(() =>
                {
                    // Intentar parsear ACK primero
                    try
                    {
                        var j = JObject.Parse(json);
                        if (j["ok"] != null)
                        {
                            bool ok = j.Value<bool>("ok");
                            if (ok)
                            {
                                lastAckBomba = j.Value<int?>("bomba") ?? 0;
                                lastAckLimite = j.Value<double?>("limiteLitros") ?? 0.0;
                                ackReceived = true;
                                ackEvent.Set();
                            }
                        }
                    }
                    catch { }

                    controlador.RecibirRespuestaJSON(json);

                    if (abastecimientoActual != null)
                    {
                        lblLitrosServidos.Text = abastecimientoActual.LitrosServidos.ToString("0.00");
                        lblTotalCobrar.Text = abastecimientoActual.TotalCobrado.ToString("0.00");
                    }
                }));
            }
            catch { }
        }


        private void Form1_FormClosing(object? sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // Si hay un abastecimiento en curso, procesarlo y guardarlo antes de cerrar
            if (abastecimientoActual != null)
            {
                abastecimientoActual.LitrosServidos = Math.Round(litrosSimulados, 2);
                abastecimientoActual.Procesar();

                if (!controlador.Registro.Lista.Contains(abastecimientoActual))
                {
                    controlador.Registro.Agregar(abastecimientoActual);
                }

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

            // Cerrar puerto serie si está abierto
            try
            {
                if (serialArduino != null)
                {
                    if (serialArduino.IsOpen) serialArduino.Close();
                    serialArduino.Dispose();
                    serialArduino = null;
                }
            }
            catch { }
        }

        private void dtpFechaReporte_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
