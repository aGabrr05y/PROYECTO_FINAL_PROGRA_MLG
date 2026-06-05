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
        //Variables de control y comunicación con Arduino
        ControladorCentral controlador = new ControladorCentral();
        private SerialPort serialArduino;
        private ManualResetEventSlim ackEvent = new ManualResetEventSlim(false);
        private volatile bool ackReceived = false;
        private volatile int lastAckBomba = 0;
        private double lastAckLimite = 0.0;

        // M: Eventos para manejo de parada manual de bombas
        private ManualResetEventSlim stopEvent = new ManualResetEventSlim(false);
        private volatile bool stopReceived = false;
        private volatile int lastStopBomba = 0;

        List<Cliente> listaClientes = new List<Cliente>();
        string rutaClientes = "clientes.json";
        public Form1()
        {
 
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;

            // G: Cargo clientes guardados previamente y arranco comunicación
            CargarClientes();
            InicializarSerial();

            lblPrecioActual.Text = "Q37.35";
        }
        //Lo usamos para comunicarnos con el Arduino, enviarle las órdenes de
        //inicio de abastecimiento y recibir los datos de litros servidos en tiempo real.
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


        //L Leemos el JSON y cargamos los datos que tenga dentro
        void CargarClientes()
        {
            if (File.Exists(rutaClientes))
            {
                string json = File.ReadAllText(rutaClientes);
                listaClientes = JsonConvert.DeserializeObject<List<Cliente>>(json);
            }
        }



        // G: Manejo del checkbox de Consumidor Final
        // Cuando se activa, deshabilito campos y asigno valores por defecto
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
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void lblPrecioLitro_Click(object sender, EventArgs e)
        {
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
            // Limpiar campos de la interfaz
            lblLitrosServidos.Text = "0.00";
            lblTotalCobrar.Text = "0.00";
            txtMonto.Clear();
        }







        private void btnGuardarCliente_Click(object sender, EventArgs e)
        {
            // Crear nuevo cliente con los datos del formulario
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
            // Reemplazar si ya existe cliente con el mismo NIT

            listaClientes.RemoveAll(x => x.NIT == c.NIT);
            listaClientes.Add(c);
            GuardarClientes();
            MessageBox.Show("Cliente guardado correctamente.");

        }





        private void btnActualizarCliente_Click(object sender, EventArgs e)
        {
            // Buscar cliente por NIT
            string nit = txtNIT.Text;
            Cliente encontrado = listaClientes.FirstOrDefault(x => x.NIT == nit);
            if (encontrado == null) { MessageBox.Show("No existe un cliente con ese NIT."); return; }
            // Cargar datos del cliente en el formulario
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
            // VALIDACIÓN 1: Verificar que se haya seleccionado una bomba
            if (cboBomba.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una bomba.");
                return;
            }

            // Obtener número de bomba y verificar que no esté ocupada
            int numeroBomba = int.Parse(cboBomba.SelectedItem.ToString());
            Bomba bomba = controlador.Bombas[numeroBomba - 1];

            if (bomba.Ocupada)
            {
                MessageBox.Show("La bomba está ocupada.");
                return;
            }

            // OBTENER CLIENTE: Crear consumidor final o buscar por NIT
            Cliente cliente;
            if (chkConsumidorFinal.Checked)
                cliente = new Cliente { Nombre = "Consumidor Final", NIT = "CF", ConsumidorFinal = true };
            else
            {
                cliente = listaClientes.FirstOrDefault(x => x.NIT == txtNIT.Text);
                if (cliente == null)
                {
                    MessageBox.Show("Debe guardar el cliente primero.");
                    return;
                }
            }

            // VARIABLE: Litros en escala "app" (1 litro app = Q37.35 = 50ml reales)
            double limiteLitrosApp = 0.0;

            // CREAR ABASTECIMIENTO según el tipo seleccionado
            if (rdbPrepago.Checked)
            {
                // Leer monto ingresado por el usuario
                if (!double.TryParse(txtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto)
                    && !double.TryParse(txtMonto.Text, out monto))
                {
                    MessageBox.Show("Monto inválido.");
                    return;
                }

                // Calcular cuántos litros "app" equivalen al monto pagado
                // Ejemplo: Q100 / Q37.35 = 2.68 litros app
                limiteLitrosApp = monto / Config.PrecioPorLitro;

                // Crear objeto de tipo Prepago
                var ap = new AbastecimientoPrepago
                {
                    Cliente = cliente,
                    NumeroBomba = numeroBomba,
                    MontoPagado = monto,
                    LimiteLitrosRequested = limiteLitrosApp,  // Litros que debe despachar
                    Calibrado = false
                };
                abastecimientoActual = ap;
            }
            else if (rdbTanqueLleno.Checked)
            {
                // Crear objeto de tipo Tanque Lleno (sin límite)
                abastecimientoActual = new AbastecimientoTanqueLleno
                {
                    Cliente = cliente,
                    NumeroBomba = numeroBomba
                };
                // Nota: En tanque lleno, limiteLitrosApp queda en 0.0
                //       El Arduino recibe {"bomba":1, "tanqueLleno":true}
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de abastecimiento.");
                return;
            }

            // Asignar ID y guardar en registro
            abastecimientoActual.Id = cliente.NIT;
            controlador.Registro.Agregar(abastecimientoActual);

            // GENERAR ORDEN JSON para enviar al Arduino
            // El ControladorCentral aplica internamente ESCALA_LITROS (0.05)
            // convirtiendo litros app → litros reales
            string json = controlador.EnviarOrdenJSON(abastecimientoActual, limiteLitrosApp);

            // VERIFICAR PUERTO SERIE: Si no está abierto, intentar abrirlo
            if (serialArduino == null || !serialArduino.IsOpen)
                InicializarSerial();

            // ENVIAR ORDEN al Arduino por puerto serie
            if (serialArduino != null && serialArduino.IsOpen)
            {
                // Resetear eventos de espera para recibir confirmación (ACK)
                ackEvent.Reset();
                ackReceived = false;
                try
                {
                    Debug.WriteLine($"Enviando: {json}");
                    serialArduino.WriteLine(json);  // <--- AQUÍ SE ENVÍA LA ORDEN
                                                    // El Arduino recibe algo como: {"bomba":1,"limiteLitros":0.134}
                                                    // o {"bomba":1,"tanqueLleno":true}
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al escribir en puerto: {ex.Message}");
                }
            }
            else
                MessageBox.Show("Puerto serie no disponible. Orden no enviada.");

            // ACTUALIZAR ESTADO LOCAL
            bomba.Iniciar();                    // Marcar bomba como ocupada
            lblLitrosServidos.Text = "0.00";   // Reiniciar visualización
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
                // Leer una línea JSON enviada por el Arduino (ej: {"bomba":1,"litrosServidos":0.023})
                string json = serialArduino.ReadLine();

                // Invocar en el hilo de la UI para actualizar controles de forma segura
                this.Invoke(new Action(() =>
                {
                    // --- DETECTAR ACK (confirmación de inicio) ---
                    // El Arduino responde con {"ok":true, "bomba":1, "limiteLitros":0.5}
                    // cuando recibió correctamente la orden de inicio
                    try
                    {
                        var j = JObject.Parse(json);
                        if (j["ok"] != null && j.Value<bool>("ok"))
                        {
                            lastAckBomba = j.Value<int?>("bomba") ?? 0;           // Guardar bomba que confirmó
                            lastAckLimite = j.Value<double?>("limiteLitros") ?? 0.0; // Guardar límite enviado
                            ackReceived = true;      // Marcar que se recibió ACK
                            ackEvent.Set();          // Desbloquear hilo que esperaba confirmación
                            return;                  // Salir, este mensaje ya fue procesado
                        }

                        // --- DETECTAR FINALIZACIÓN DEL DESPACHO ---
                        // El Arduino envía {"bomba":1, "finalizado":true} cuando terminó de despachar
                        if (j["finalizado"] != null && j.Value<bool>("finalizado"))
                        {
                            lastStopBomba = j.Value<int?>("bomba") ?? 0;  // Guardar bomba que finalizó
                            stopReceived = true;    // Marcar que se recibió señal de parada
                            stopEvent.Set();        // Desbloquear hilo que esperaba finalización
                        }
                    }
                    catch { }  // Si no se puede parsear, continuar con otros procesamientos

                    // --- PROCESAMIENTO PRINCIPAL ---
                    // Delegar al controlador la lógica de actualizar registro y convertir escalas
                    controlador.RecibirRespuestaJSON(json);

                    // --- ACTUALIZAR INTERFAZ DE USUARIO ---
                    try
                    {
                        var j2 = JObject.Parse(json);

                        // Ignorar mensajes de ACK (ya procesados arriba)
                        if (j2["ok"] != null) return;

                        int bombaId = j2.Value<int?>("bomba") ?? 0;

                        // Verificar que este mensaje corresponde al abastecimiento actual
                        if (abastecimientoActual == null || abastecimientoActual.NumeroBomba != bombaId) return;

                        // --- ACTUALIZAR LITROS EN TIEMPO REAL ---
                        // Cuando llega {"bomba":1, "litrosServidos": 0.023}
                        // ya vino convertido de reales a app por RecibirRespuestaJSON
                        if (j2.TryGetValue("litrosServidos", out var lt) && lt.Type != JTokenType.Null)
                        {
                            // Mostrar en la interfaz los litros servidos hasta el momento
                            lblLitrosServidos.Text = abastecimientoActual.LitrosServidos.ToString("0.00");
                            // Mostrar el total acumulado (litros × precio)
                            lblTotalCobrar.Text = abastecimientoActual.TotalCobrado.ToString("0.00");
                            // Cálculo alternativo en tiempo real (no se usa en UI)
                            double totalTiempoReal = abastecimientoActual.LitrosServidos * abastecimientoActual.PrecioPorLitro;
                        }

                        // --- PROCESAR FINALIZACIÓN DEL DESPACHO ---
                        // Cuando llega {"bomba":1, "finalizado":true}
                        if (j2.TryGetValue("finalizado", out var tf) && tf.Type == JTokenType.Boolean && tf.Value<bool>())
                        {
                            Abastecimiento aFinalizado;
                            lock (_lockAbastecimiento)  // Bloquear para evitar conflictos con hilos
                            {
                                aFinalizado = abastecimientoActual;
                                abastecimientoActual = null;  // Limpiar porque ya terminó
                            }

                            // Si ya fue procesado por botón Detener, ignorar
                            if (aFinalizado == null) return;

                            // Calcular totales finales (litros × precio)
                            aFinalizado.Procesar();
                            // Liberar la bomba para que otro cliente pueda usarla
                            controlador.Bombas[bombaId - 1].Finalizar();
                            // Guardar como pendiente de cobro (el usuario debe presionar "Cobrar")
                            abastecimientoPendienteCobro = aFinalizado;

                            // Actualizar UI con valores finales
                            lblLitrosServidos.Text = aFinalizado.LitrosServidos.ToString("0.00");
                            lblTotalCobrar.Text = aFinalizado.TotalCobrado.ToString("0.00");

                            // Notificar al usuario que el despacho terminó
                            MessageBox.Show(
                                $"Abastecimiento finalizado.\n" +
                                $"Litros: {aFinalizado.LitrosServidos:0.00}\n" +
                                $"Total: Q{aFinalizado.TotalCobrado:0.00}\n" +
                                $"Presione Cobrar para finalizar.",
                                "Finalizado");
                        }
                    }
                    catch { }  // Errores de UI no detienen el procesamiento
                }));
            }
            catch { }  // Errores de lectura del puerto serie se ignoran silenciosamente
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
                abastecimientoActual = null;
            }

            // Enviar comando parar al Arduino
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

            // AGREGA ESTO: marcar como detenido manualmente si es prepago
            if (aDetener is AbastecimientoPrepago prepago)
            {
                prepago.DetenidoManualmente = true;
            }

            // Procesar con la lógica correcta según el tipo
            aDetener.Procesar();
            controlador.Bombas[aDetener.NumeroBomba - 1].Finalizar();
            abastecimientoPendienteCobro = aDetener;

            lblLitrosServidos.Text = aDetener.LitrosServidos.ToString("0.00");
            lblTotalCobrar.Text = aDetener.TotalCobrado.ToString("0.00");

            // CAMBIA el MessageBox para mostrar la información correcta:
            if (aDetener is AbastecimientoPrepago prepagoDet)
            {
                double montoOriginal = prepagoDet.MontoPagado;
                double montoACobrar = aDetener.TotalCobrado;
                double litrosServidos = aDetener.LitrosServidos;
                double reembolso = Math.Round(montoOriginal - montoACobrar, 2);

                MessageBox.Show(
                    $"Abastecimiento detenido manualmente.\n\n" +
                    $"Cliente: {aDetener.Cliente?.Nombre}\n" +
                    $"Monto ingresado originalmente: Q{montoOriginal:0.00}\n" +
                    $"Litros consumidos: {litrosServidos:0.00} L\n" +
                    $"Monto a cobrar (por lo consumido): Q{montoACobrar:0.00}\n" +
                    $"Diferencia a devolver al cliente: Q{reembolso:0.00}\n\n" +
                    $"Presione Cobrar para finalizar.",
                    "Servicio Detenido");
            }
            else
            {
                // Tanque lleno detenido: cobrar por lo que salió
                MessageBox.Show(
                    $"Abastecimiento detenido.\n\n" +
                    $"Cliente: {aDetener.Cliente?.Nombre}\n" +
                    $"Litros consumidos: {aDetener.LitrosServidos:0.00} L\n" +
                    $"Total a cobrar: Q{aDetener.TotalCobrado:0.00}\n\n" +
                    $"Presione Cobrar para finalizar.",
                    "Servicio Detenido");
            }
        }

        private void lblLitrosServidos_Click(object sender, EventArgs e)
        {

        }
    }
}
