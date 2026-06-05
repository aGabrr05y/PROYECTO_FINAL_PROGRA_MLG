using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

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
        private ManualResetEventSlim stopEvent = new ManualResetEventSlim(false);
        private volatile bool stopReceived = false;
        private volatile int lastStopBomba = 0;
        List<Cliente> listaClientes = new List<Cliente>();
        string rutaClientes = "clientes.json";

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
                if (ports != null && ports.Length > 0)
                {
                    string chosen = ports[0];
                    for (int i = 0; i < ports.Length; i++)
                        if (ports[i].ToUpper() == "COM5") { chosen = ports[i]; break; }
                    serialArduino.PortName = chosen;
                }
                else serialArduino.PortName = "COM5";

                serialArduino.BaudRate = 9600;
                serialArduino.NewLine = "\n";
                serialArduino.DataReceived += serialArduino_DataReceived;
                serialArduino.DtrEnable = false;
                serialArduino.Open();
                Thread.Sleep(1200);
                try { serialArduino.DiscardInBuffer(); } catch { }
                try { serialArduino.DiscardOutBuffer(); } catch { }
                Debug.WriteLine($"Serial abierto en {serialArduino.PortName}");
                MessageBox.Show($"Puerto serie abierto: {serialArduino.PortName}", "Puerto serie");
            }
            catch (Exception ex)
            {
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




        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
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
            Abastecimiento aCobrar;
            lock (_lockAbastecimiento)
            {
                aCobrar = abastecimientoPendienteCobro ?? abastecimientoActual;
                if (aCobrar == null)
                {
                    MessageBox.Show("No hay abastecimiento para cobrar.");
                    return;
                }
                abastecimientoPendienteCobro = null;
                abastecimientoActual = null;
            }

            // Asegurar que la bomba quede libre siempre
            try { controlador.Bombas[aCobrar.NumeroBomba - 1].Finalizar(); } catch { }

            controlador.Registro.GuardarEnArchivo();

            MessageBox.Show(
                $"Cobro realizado.\n" +
                $"Cliente: {aCobrar.Cliente?.Nombre}\n" +
                $"Litros: {aCobrar.LitrosServidos:0.00}\n" +
                $"Total: Q{aCobrar.TotalCobrado:0.00}",
                "Cobro exitoso");

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
            if (encontrado == null) { MessageBox.Show("No existe un cliente con ese NIT."); return; }
            txtNombre.Text = encontrado.Nombre;
            chkConsumidorFinal.Checked = encontrado.ConsumidorFinal;
            MessageBox.Show("Cliente cargado.");
        }







        private Abastecimiento abastecimientoActual;
        private Abastecimiento abastecimientoPendienteCobro;
        private readonly object _lockAbastecimiento = new object();
        //ARDUINO: Al iniciar un abastecimiento, se crea el objeto correspondiente (prepago o tanque lleno) y se asigna a esta variable.
        //Luego, al recibir datos del Arduino, se actualizan los litros servidos y total cobrado en la interfaz. Al finalizar el abastecimiento, se procesa y guarda el registro.
        private void btnIniciarAbastecimiento_Click(object sender, EventArgs e)
        {
            if (cboBomba.SelectedIndex == -1) { MessageBox.Show("Seleccione una bomba."); return; }

            int numeroBomba = int.Parse(cboBomba.SelectedItem.ToString());
            Bomba bomba = controlador.Bombas[numeroBomba - 1];

            if (bomba.Ocupada) { MessageBox.Show("La bomba está ocupada."); return; }

            Cliente cliente;
            if (chkConsumidorFinal.Checked)
                cliente = new Cliente { Nombre = "Consumidor Final", NIT = "CF", ConsumidorFinal = true };
            else
            {
                cliente = listaClientes.FirstOrDefault(x => x.NIT == txtNIT.Text);
                if (cliente == null) { MessageBox.Show("Debe guardar el cliente primero."); return; }
            }

            double limiteLitrosApp = 0.0; // litros en escala "app" (1 litro app = Q37.35 = 50ml reales)

            if (rdbPrepago.Checked)
            {
                if (!double.TryParse(txtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto)
                    && !double.TryParse(txtMonto.Text, out monto))
                {
                    MessageBox.Show("Monto inválido.");
                    return;
                }

                // Calcular cuántos litros "app" corresponden al monto
                limiteLitrosApp = monto / Config.PrecioPorLitro;

                var ap = new AbastecimientoPrepago
                {
                    Cliente = cliente,
                    NumeroBomba = numeroBomba,
                    MontoPagado = monto,
                    LimiteLitrosRequested = limiteLitrosApp,
                    Calibrado = false
                };
                abastecimientoActual = ap;
            }
            else if (rdbTanqueLleno.Checked)
            {
                abastecimientoActual = new AbastecimientoTanqueLleno
                {
                    Cliente = cliente,
                    NumeroBomba = numeroBomba
                };
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de abastecimiento.");
                return;
            }

            abastecimientoActual.Id = cliente.NIT;
            controlador.Registro.Agregar(abastecimientoActual);

            // Generar JSON — ControladorCentral escala internamente los litros app → reales
            string json = controlador.EnviarOrdenJSON(abastecimientoActual, limiteLitrosApp);

            if (serialArduino == null || !serialArduino.IsOpen) InicializarSerial();

            if (serialArduino != null && serialArduino.IsOpen)
            {
                ackEvent.Reset();
                ackReceived = false;
                try
                {
                    Debug.WriteLine($"Enviando: {json}");
                    serialArduino.WriteLine(json);
                }
                catch (Exception ex) { MessageBox.Show($"Error al escribir en puerto: {ex.Message}"); }

                bool got = ackEvent.Wait(2000);
                if (!got)
                    MessageBox.Show("No se recibió confirmación del Arduino (timeout).");
                else if (!ackReceived || lastAckBomba != abastecimientoActual.NumeroBomba)
                    MessageBox.Show("Confirmación no coincide con la bomba esperada.");
            }
            else
                MessageBox.Show("Puerto serie no disponible. Orden no enviada.");

            bomba.Iniciar();
            lblLitrosServidos.Text = "0.00";
            lblTotalCobrar.Text = "0.00";
            MessageBox.Show("Abastecimiento iniciado.");
        }









        private void btnReporteDiario_Click(object sender, EventArgs e)
        {
            DateTime fecha = dtpFechaReporte.Value.Date;
            dgvReportes.DataSource = null;
            dgvReportes.DataSource = controlador.Registro.FiltrarPorDia(fecha);

        }



        private void btnReportePrepago_Click(object sender, EventArgs e)
        {
            dgvReportes.DataSource = null;
            dgvReportes.DataSource = controlador.Registro.ReportePrepago();

        }



        private void btnReporteTanqueLleno_Click(object sender, EventArgs e)
        {
            dgvReportes.DataSource = null;
            dgvReportes.DataSource = controlador.Registro.ReporteTanqueLleno();

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
                this.Invoke(new Action(() =>
                {
                    // Detectar ACK de inicio
                    try
                    {
                        var j = JObject.Parse(json);
                        if (j["ok"] != null && j.Value<bool>("ok"))
                        {
                            lastAckBomba = j.Value<int?>("bomba") ?? 0;
                            lastAckLimite = j.Value<double?>("limiteLitros") ?? 0.0;
                            ackReceived = true;
                            ackEvent.Set();
                            return; // No procesar más este mensaje
                        }

                        // Detectar finalización
                        if (j["finalizado"] != null && j.Value<bool>("finalizado"))
                        {
                            lastStopBomba = j.Value<int?>("bomba") ?? 0;
                            stopReceived = true;
                            stopEvent.Set();
                        }
                    }
                    catch { }

                    // Pasar al controlador para actualizar registro
                    controlador.RecibirRespuestaJSON(json);

                    // Actualizar UI
                    try
                    {
                        var j2 = JObject.Parse(json);

                        // Ignorar ACKs
                        if (j2["ok"] != null) return;

                        int bombaId = j2.Value<int?>("bomba") ?? 0;

                        if (abastecimientoActual == null || abastecimientoActual.NumeroBomba != bombaId) return;

                        if (j2.TryGetValue("litrosServidos", out var lt) && lt.Type != JTokenType.Null)
                        {
                            // LitrosServidos ya fue convertido a escala app por RecibirRespuestaJSON
                            lblLitrosServidos.Text = abastecimientoActual.LitrosServidos.ToString("0.00");
                            lblTotalCobrar.Text = abastecimientoActual.TotalCobrado.ToString("0.00");
                        }

                        if (j2.TryGetValue("finalizado", out var tf) && tf.Type == JTokenType.Boolean && tf.Value<bool>())
                        {
                            Abastecimiento aFinalizado;
                            lock (_lockAbastecimiento)
                            {
                                aFinalizado = abastecimientoActual;
                                abastecimientoActual = null;
                            }

                            // Puede ser null si buttonDetener ya lo procesó — ignorar
                            if (aFinalizado == null) return;

                            aFinalizado.Procesar();
                            controlador.Bombas[bombaId - 1].Finalizar();
                            abastecimientoPendienteCobro = aFinalizado;

                            lblLitrosServidos.Text = aFinalizado.LitrosServidos.ToString("0.00");
                            lblTotalCobrar.Text = aFinalizado.TotalCobrado.ToString("0.00");

                            MessageBox.Show(
                                $"Abastecimiento finalizado.\n" +
                                $"Litros: {aFinalizado.LitrosServidos:0.00}\n" +
                                $"Total: Q{aFinalizado.TotalCobrado:0.00}\n" +
                                $"Presione Cobrar para finalizar.",
                                "Finalizado");
                        }
                    }
                    catch { }
                }));
            }
            catch { }
        }




        private void Form1_FormClosing(object? sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (abastecimientoActual != null)
            {
                abastecimientoActual.Procesar();
                if (!controlador.Registro.Lista.Contains(abastecimientoActual))
                    controlador.Registro.Agregar(abastecimientoActual);
                try { controlador.Bombas[abastecimientoActual.NumeroBomba - 1].Finalizar(); } catch { }
                abastecimientoActual = null;
            }
            controlador.Registro.GuardarEnArchivo();
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

        private void buttonDetener_Click(object sender, EventArgs e)
        {
            Abastecimiento aDetener;
            lock (_lockAbastecimiento)
            {
                aDetener = abastecimientoActual;
                if (aDetener == null)
                {
                    MessageBox.Show("No hay un abastecimiento en curso.");
                    return;
                }
                // Marcar como null inmediatamente para evitar race condition
                abastecimientoActual = null;
            }

            // Enviar comando parar al Arduino (sin bloquear el hilo UI con Wait)
            if (serialArduino != null && serialArduino.IsOpen)
            {
                try
                {
                    string stopJson = controlador.EnviarComandoParada(aDetener.NumeroBomba);
                    serialArduino.WriteLine(stopJson);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al enviar STOP: {ex.Message}");
                }
            }

            // Procesar y cobrar localmente sin esperar respuesta del Arduino
            aDetener.Procesar();
            controlador.Bombas[aDetener.NumeroBomba - 1].Finalizar();

            abastecimientoPendienteCobro = aDetener;

            lblLitrosServidos.Text = aDetener.LitrosServidos.ToString("0.00");
            lblTotalCobrar.Text = aDetener.TotalCobrado.ToString("0.00");

            MessageBox.Show(
                $"Abastecimiento detenido.\n" +
                $"Litros: {aDetener.LitrosServidos:0.00}\n" +
                $"Total: Q{aDetener.TotalCobrado:0.00}\n" +
                $"Presione Cobrar para finalizar.",
                "Detenido");
        }

        private void lblLitrosServidos_Click(object sender, EventArgs e)
        {

        }
    }
}
