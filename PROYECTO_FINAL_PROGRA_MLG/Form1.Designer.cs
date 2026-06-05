namespace PROYECTO_FINAL_PROGRA_MLG
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            grpCliente = new GroupBox();
            btnGuardarCliente = new Button();
            btnActualizarCliente = new Button();
            chkConsumidorFinal = new CheckBox();
            txtNIT = new TextBox();
            label2 = new Label();
            txtNombre = new TextBox();
            label1 = new Label();
            grpAbastecimiento = new GroupBox();
            buttonDetener = new Button();
            btnCobrar = new Button();
            lblTotalCobrar = new Label();
            label5 = new Label();
            lblLitrosServidos = new Label();
            label4 = new Label();
            btnIniciarAbastecimiento = new Button();
            lblPrecioActual = new Label();
            lblPrecioLitro = new Label();
            txtMonto = new TextBox();
            lblMonto = new Label();
            rdbTanqueLleno = new RadioButton();
            rdbPrepago = new RadioButton();
            cboBomba = new ComboBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            dtpFechaReporte = new DateTimePicker();
            btnReporteBombaMenosUsada = new Button();
            btnReporteBombaMasUsada = new Button();
            btnReporteTanqueLleno = new Button();
            btnReportePrepago = new Button();
            btnReporteDiario = new Button();
            dgvReportes = new DataGridView();
            grpCliente.SuspendLayout();
            grpAbastecimiento.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReportes).BeginInit();
            SuspendLayout();
            // 
            // grpCliente
            // 
            grpCliente.Controls.Add(btnGuardarCliente);
            grpCliente.Controls.Add(btnActualizarCliente);
            grpCliente.Controls.Add(chkConsumidorFinal);
            grpCliente.Controls.Add(txtNIT);
            grpCliente.Controls.Add(label2);
            grpCliente.Controls.Add(txtNombre);
            grpCliente.Controls.Add(label1);
            grpCliente.Location = new Point(12, 84);
            grpCliente.Name = "grpCliente";
            grpCliente.Size = new Size(311, 176);
            grpCliente.TabIndex = 0;
            grpCliente.TabStop = false;
            grpCliente.Text = "Datos de Cliente";
            // 
            // btnGuardarCliente
            // 
            btnGuardarCliente.Location = new Point(104, 145);
            btnGuardarCliente.Name = "btnGuardarCliente";
            btnGuardarCliente.Size = new Size(121, 23);
            btnGuardarCliente.TabIndex = 6;
            btnGuardarCliente.Text = "Guardar cliente";
            btnGuardarCliente.UseVisualStyleBackColor = true;
            btnGuardarCliente.Click += btnGuardarCliente_Click;
            // 
            // btnActualizarCliente
            // 
            btnActualizarCliente.Location = new Point(231, 80);
            btnActualizarCliente.Name = "btnActualizarCliente";
            btnActualizarCliente.Size = new Size(75, 23);
            btnActualizarCliente.TabIndex = 5;
            btnActualizarCliente.Text = "Actualizar";
            btnActualizarCliente.UseVisualStyleBackColor = true;
            btnActualizarCliente.Click += btnActualizarCliente_Click;
            // 
            // chkConsumidorFinal
            // 
            chkConsumidorFinal.AutoSize = true;
            chkConsumidorFinal.Location = new Point(66, 109);
            chkConsumidorFinal.Name = "chkConsumidorFinal";
            chkConsumidorFinal.Size = new Size(45, 19);
            chkConsumidorFinal.TabIndex = 4;
            chkConsumidorFinal.Text = "C/F";
            chkConsumidorFinal.UseVisualStyleBackColor = true;
            chkConsumidorFinal.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // txtNIT
            // 
            txtNIT.Location = new Point(66, 80);
            txtNIT.Name = "txtNIT";
            txtNIT.Size = new Size(159, 23);
            txtNIT.TabIndex = 3;
            txtNIT.TextChanged += txtNIT_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 88);
            label2.Name = "label2";
            label2.Size = new Size(29, 15);
            label2.TabIndex = 2;
            label2.Text = "NIT:";
            label2.Click += label2_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(66, 27);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(159, 23);
            txtNombre.TabIndex = 1;
            txtNombre.TextChanged += txtNombre_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 30);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 0;
            label1.Text = "Nombre:";
            // 
            // grpAbastecimiento
            // 
            grpAbastecimiento.Controls.Add(buttonDetener);
            grpAbastecimiento.Controls.Add(btnCobrar);
            grpAbastecimiento.Controls.Add(lblTotalCobrar);
            grpAbastecimiento.Controls.Add(label5);
            grpAbastecimiento.Controls.Add(lblLitrosServidos);
            grpAbastecimiento.Controls.Add(label4);
            grpAbastecimiento.Controls.Add(btnIniciarAbastecimiento);
            grpAbastecimiento.Controls.Add(lblPrecioActual);
            grpAbastecimiento.Controls.Add(lblPrecioLitro);
            grpAbastecimiento.Controls.Add(txtMonto);
            grpAbastecimiento.Controls.Add(lblMonto);
            grpAbastecimiento.Controls.Add(rdbTanqueLleno);
            grpAbastecimiento.Controls.Add(rdbPrepago);
            grpAbastecimiento.Controls.Add(cboBomba);
            grpAbastecimiento.Location = new Point(375, 55);
            grpAbastecimiento.Name = "grpAbastecimiento";
            grpAbastecimiento.Size = new Size(397, 309);
            grpAbastecimiento.TabIndex = 1;
            grpAbastecimiento.TabStop = false;
            grpAbastecimiento.Text = "Abastecimiento";
            // 
            // buttonDetener
            // 
            buttonDetener.Location = new Point(114, 260);
            buttonDetener.Name = "buttonDetener";
            buttonDetener.Size = new Size(146, 23);
            buttonDetener.TabIndex = 17;
            buttonDetener.Text = "Detener";
            buttonDetener.UseVisualStyleBackColor = true;
            buttonDetener.Click += buttonDetener_Click;
            // 
            // btnCobrar
            // 
            btnCobrar.Location = new Point(212, 182);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new Size(146, 23);
            btnCobrar.TabIndex = 16;
            btnCobrar.Text = "Cobrar ";
            btnCobrar.UseVisualStyleBackColor = true;
            btnCobrar.Click += button1_Click_1;
            // 
            // lblTotalCobrar
            // 
            lblTotalCobrar.AutoSize = true;
            lblTotalCobrar.Location = new Point(114, 215);
            lblTotalCobrar.Name = "lblTotalCobrar";
            lblTotalCobrar.Size = new Size(34, 15);
            lblTotalCobrar.TabIndex = 15;
            lblTotalCobrar.Text = "0.000";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 215);
            label5.Name = "label5";
            label5.Size = new Size(102, 15);
            label5.TabIndex = 14;
            label5.Text = "Total a cobrar (Q):";
            // 
            // lblLitrosServidos
            // 
            lblLitrosServidos.AutoSize = true;
            lblLitrosServidos.Location = new Point(100, 190);
            lblLitrosServidos.Name = "lblLitrosServidos";
            lblLitrosServidos.Size = new Size(34, 15);
            lblLitrosServidos.TabIndex = 13;
            lblLitrosServidos.Text = "0.000";
            lblLitrosServidos.Click += lblLitrosServidos_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 190);
            label4.Name = "label4";
            label4.Size = new Size(86, 15);
            label4.TabIndex = 12;
            label4.Text = "Litros Servidos:";
            // 
            // btnIniciarAbastecimiento
            // 
            btnIniciarAbastecimiento.Location = new Point(212, 145);
            btnIniciarAbastecimiento.Name = "btnIniciarAbastecimiento";
            btnIniciarAbastecimiento.Size = new Size(146, 23);
            btnIniciarAbastecimiento.TabIndex = 11;
            btnIniciarAbastecimiento.Text = "Iniciar abastecimiento";
            btnIniciarAbastecimiento.UseVisualStyleBackColor = true;
            btnIniciarAbastecimiento.Click += btnIniciarAbastecimiento_Click;
            // 
            // lblPrecioActual
            // 
            lblPrecioActual.AutoSize = true;
            lblPrecioActual.Location = new Point(100, 19);
            lblPrecioActual.Name = "lblPrecioActual";
            lblPrecioActual.Size = new Size(43, 15);
            lblPrecioActual.TabIndex = 10;
            lblPrecioActual.Text = "Q37.35";
            lblPrecioActual.Click += label3_Click;
            // 
            // lblPrecioLitro
            // 
            lblPrecioLitro.AutoSize = true;
            lblPrecioLitro.Location = new Point(6, 19);
            lblPrecioLitro.Name = "lblPrecioLitro";
            lblPrecioLitro.Size = new Size(88, 15);
            lblPrecioLitro.TabIndex = 9;
            lblPrecioLitro.Text = "Precio por litro:";
            lblPrecioLitro.Click += lblPrecioLitro_Click;
            // 
            // txtMonto
            // 
            txtMonto.Location = new Point(78, 110);
            txtMonto.Name = "txtMonto";
            txtMonto.Size = new Size(100, 23);
            txtMonto.TabIndex = 8;
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Location = new Point(6, 118);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new Size(66, 15);
            lblMonto.TabIndex = 7;
            lblMonto.Text = "Monto (Q):";
            // 
            // rdbTanqueLleno
            // 
            rdbTanqueLleno.AutoSize = true;
            rdbTanqueLleno.Location = new Point(6, 149);
            rdbTanqueLleno.Name = "rdbTanqueLleno";
            rdbTanqueLleno.Size = new Size(93, 19);
            rdbTanqueLleno.TabIndex = 2;
            rdbTanqueLleno.TabStop = true;
            rdbTanqueLleno.Text = "Tanque lleno";
            rdbTanqueLleno.UseVisualStyleBackColor = true;
            // 
            // rdbPrepago
            // 
            rdbPrepago.AutoSize = true;
            rdbPrepago.Location = new Point(6, 78);
            rdbPrepago.Name = "rdbPrepago";
            rdbPrepago.Size = new Size(128, 19);
            rdbPrepago.TabIndex = 1;
            rdbPrepago.TabStop = true;
            rdbPrepago.Text = "Prepago (monto Q)";
            rdbPrepago.UseVisualStyleBackColor = true;
            // 
            // cboBomba
            // 
            cboBomba.FormattingEnabled = true;
            cboBomba.Items.AddRange(new object[] { "1", "2" });
            cboBomba.Location = new Point(6, 49);
            cboBomba.Name = "cboBomba";
            cboBomba.Size = new Size(121, 23);
            cboBomba.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(-4, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(892, 518);
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(grpCliente);
            tabPage1.Controls.Add(grpAbastecimiento);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(884, 490);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dtpFechaReporte);
            tabPage2.Controls.Add(btnReporteBombaMenosUsada);
            tabPage2.Controls.Add(btnReporteBombaMasUsada);
            tabPage2.Controls.Add(btnReporteTanqueLleno);
            tabPage2.Controls.Add(btnReportePrepago);
            tabPage2.Controls.Add(btnReporteDiario);
            tabPage2.Controls.Add(dgvReportes);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(884, 490);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            tabPage2.Click += tabPage2_Click;
            // 
            // dtpFechaReporte
            // 
            dtpFechaReporte.Format = DateTimePickerFormat.Short;
            dtpFechaReporte.Location = new Point(181, 43);
            dtpFechaReporte.Name = "dtpFechaReporte";
            dtpFechaReporte.Size = new Size(142, 23);
            dtpFechaReporte.TabIndex = 8;
            dtpFechaReporte.ValueChanged += dtpFechaReporte_ValueChanged;
            // 
            // btnReporteBombaMenosUsada
            // 
            btnReporteBombaMenosUsada.Location = new Point(567, 106);
            btnReporteBombaMenosUsada.Name = "btnReporteBombaMenosUsada";
            btnReporteBombaMenosUsada.Size = new Size(142, 28);
            btnReporteBombaMenosUsada.TabIndex = 7;
            btnReporteBombaMenosUsada.Text = "Bomba menos usada";
            btnReporteBombaMenosUsada.UseVisualStyleBackColor = true;
            btnReporteBombaMenosUsada.Click += btnReporteBombaMenosUsada_Click;
            // 
            // btnReporteBombaMasUsada
            // 
            btnReporteBombaMasUsada.Location = new Point(567, 72);
            btnReporteBombaMasUsada.Name = "btnReporteBombaMasUsada";
            btnReporteBombaMasUsada.Size = new Size(142, 28);
            btnReporteBombaMasUsada.TabIndex = 6;
            btnReporteBombaMasUsada.Text = "Bomba más usada";
            btnReporteBombaMasUsada.UseVisualStyleBackColor = true;
            btnReporteBombaMasUsada.Click += btnReporteBombaMasUsada_Click;
            // 
            // btnReporteTanqueLleno
            // 
            btnReporteTanqueLleno.Location = new Point(567, 43);
            btnReporteTanqueLleno.Name = "btnReporteTanqueLleno";
            btnReporteTanqueLleno.Size = new Size(142, 23);
            btnReporteTanqueLleno.TabIndex = 5;
            btnReporteTanqueLleno.Text = "Reporte tanque lleno";
            btnReporteTanqueLleno.UseVisualStyleBackColor = true;
            btnReporteTanqueLleno.Click += btnReporteTanqueLleno_Click;
            // 
            // btnReportePrepago
            // 
            btnReportePrepago.Location = new Point(181, 105);
            btnReportePrepago.Name = "btnReportePrepago";
            btnReportePrepago.Size = new Size(142, 25);
            btnReportePrepago.TabIndex = 3;
            btnReportePrepago.Text = "Reporte prepago";
            btnReportePrepago.UseVisualStyleBackColor = true;
            btnReportePrepago.Click += btnReportePrepago_Click;
            // 
            // btnReporteDiario
            // 
            btnReporteDiario.Location = new Point(181, 72);
            btnReporteDiario.Name = "btnReporteDiario";
            btnReporteDiario.Size = new Size(142, 27);
            btnReporteDiario.TabIndex = 2;
            btnReporteDiario.Text = "Reporte diario";
            btnReporteDiario.UseVisualStyleBackColor = true;
            btnReporteDiario.Click += btnReporteDiario_Click;
            // 
            // dgvReportes
            // 
            dgvReportes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReportes.Location = new Point(12, 143);
            dgvReportes.Name = "dgvReportes";
            dgvReportes.Size = new Size(846, 301);
            dgvReportes.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(883, 530);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            grpCliente.ResumeLayout(false);
            grpCliente.PerformLayout();
            grpAbastecimiento.ResumeLayout(false);
            grpAbastecimiento.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReportes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpCliente;
        private Label label2;
        private TextBox txtNombre;
        private Label label1;
        private CheckBox chkConsumidorFinal;
        private TextBox txtNIT;
        private Button btnGuardarCliente;
        private Button btnActualizarCliente;
        private GroupBox grpAbastecimiento;
        private ComboBox cboBomba;
        private TextBox txtMonto;
        private Label lblMonto;
        private RadioButton rdbTanqueLleno;
        private RadioButton rdbPrepago;
        private Label lblPrecioLitro;
        private Label lblPrecioActual;
        private Label lblLitrosServidos;
        private Label label4;
        private Button btnIniciarAbastecimiento;
        private Button btnCobrar;
        private Label lblTotalCobrar;
        private Label label5;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button1;
        private Button btnReportePrepago;
        private Button btnReporteDiario;
        private DataGridView dgvReportes;
        private Button btnReporteBombaMasUsada;
        private Button btnReporteTanqueLleno;
        private Button btnReporteBombaMenosUsada;
        private DateTimePicker dtpFechaReporte;
        private Button buttonDetener;
    }
}
