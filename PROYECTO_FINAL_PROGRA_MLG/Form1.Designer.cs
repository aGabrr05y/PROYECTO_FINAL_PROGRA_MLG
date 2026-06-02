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
            groupBox1 = new GroupBox();
            label1 = new Label();
            txtNombre = new TextBox();
            label2 = new Label();
            txtNIT = new TextBox();
            chkConsumidorFinal = new CheckBox();
            btnActualizarCliente = new Button();
            btnGuardarCliente = new Button();
            grpAbastecimiento = new GroupBox();
            cboBomba = new ComboBox();
            rdbPrepago = new RadioButton();
            rdbTanqueLleno = new RadioButton();
            lblMonto = new Label();
            txtMonto = new TextBox();
            lblPrecioLitro = new Label();
            label3 = new Label();
            btnIniciarAbastecimiento = new Button();
            label4 = new Label();
            lblLitrosServidos = new Label();
            groupBox1.SuspendLayout();
            grpAbastecimiento.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnGuardarCliente);
            groupBox1.Controls.Add(btnActualizarCliente);
            groupBox1.Controls.Add(chkConsumidorFinal);
            groupBox1.Controls.Add(txtNIT);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(31, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(403, 168);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos de Cliente";
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
            // txtNombre
            // 
            txtNombre.Location = new Point(66, 27);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(159, 23);
            txtNombre.TabIndex = 1;
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
            // txtNIT
            // 
            txtNIT.Location = new Point(66, 80);
            txtNIT.Name = "txtNIT";
            txtNIT.Size = new Size(159, 23);
            txtNIT.TabIndex = 3;
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
            // btnActualizarCliente
            // 
            btnActualizarCliente.Location = new Point(231, 80);
            btnActualizarCliente.Name = "btnActualizarCliente";
            btnActualizarCliente.Size = new Size(75, 23);
            btnActualizarCliente.TabIndex = 5;
            btnActualizarCliente.Text = "Actualizar";
            btnActualizarCliente.UseVisualStyleBackColor = true;
            // 
            // btnGuardarCliente
            // 
            btnGuardarCliente.Location = new Point(130, 137);
            btnGuardarCliente.Name = "btnGuardarCliente";
            btnGuardarCliente.Size = new Size(121, 23);
            btnGuardarCliente.TabIndex = 6;
            btnGuardarCliente.Text = "Guardar cliente";
            btnGuardarCliente.UseVisualStyleBackColor = true;
            // 
            // grpAbastecimiento
            // 
            grpAbastecimiento.Controls.Add(lblLitrosServidos);
            grpAbastecimiento.Controls.Add(label4);
            grpAbastecimiento.Controls.Add(btnIniciarAbastecimiento);
            grpAbastecimiento.Controls.Add(label3);
            grpAbastecimiento.Controls.Add(lblPrecioLitro);
            grpAbastecimiento.Controls.Add(txtMonto);
            grpAbastecimiento.Controls.Add(lblMonto);
            grpAbastecimiento.Controls.Add(rdbTanqueLleno);
            grpAbastecimiento.Controls.Add(rdbPrepago);
            grpAbastecimiento.Controls.Add(cboBomba);
            grpAbastecimiento.Location = new Point(37, 195);
            grpAbastecimiento.Name = "grpAbastecimiento";
            grpAbastecimiento.Size = new Size(397, 243);
            grpAbastecimiento.TabIndex = 1;
            grpAbastecimiento.TabStop = false;
            grpAbastecimiento.Text = "Abastecimiento";
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
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Location = new Point(6, 118);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new Size(66, 15);
            lblMonto.TabIndex = 7;
            lblMonto.Text = "Monto (Q):";
            // 
            // txtMonto
            // 
            txtMonto.Location = new Point(78, 110);
            txtMonto.Name = "txtMonto";
            txtMonto.Size = new Size(100, 23);
            txtMonto.TabIndex = 8;
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
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(100, 19);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 10;
            label3.Text = "Q37.35";
            // 
            // btnIniciarAbastecimiento
            // 
            btnIniciarAbastecimiento.Location = new Point(225, 182);
            btnIniciarAbastecimiento.Name = "btnIniciarAbastecimiento";
            btnIniciarAbastecimiento.Size = new Size(151, 23);
            btnIniciarAbastecimiento.TabIndex = 11;
            btnIniciarAbastecimiento.Text = "Iniciar abastecimiento";
            btnIniciarAbastecimiento.UseVisualStyleBackColor = true;
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
            // lblLitrosServidos
            // 
            lblLitrosServidos.AutoSize = true;
            lblLitrosServidos.Location = new Point(100, 190);
            lblLitrosServidos.Name = "lblLitrosServidos";
            lblLitrosServidos.Size = new Size(28, 15);
            lblLitrosServidos.TabIndex = 13;
            lblLitrosServidos.Text = "0.00";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(grpAbastecimiento);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            grpAbastecimiento.ResumeLayout(false);
            grpAbastecimiento.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
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
        private Label label3;
        private Label lblLitrosServidos;
        private Label label4;
        private Button btnIniciarAbastecimiento;
    }
}
