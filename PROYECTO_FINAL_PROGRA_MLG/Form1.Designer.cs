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
            groupBox1.SuspendLayout();
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
            groupBox1.Location = new Point(31, 39);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(403, 193);
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
            btnGuardarCliente.Location = new Point(146, 146);
            btnGuardarCliente.Name = "btnGuardarCliente";
            btnGuardarCliente.Size = new Size(121, 23);
            btnGuardarCliente.TabIndex = 6;
            btnGuardarCliente.Text = "Guardar cliente";
            btnGuardarCliente.UseVisualStyleBackColor = true;
            // 
            // grpAbastecimiento
            // 
            grpAbastecimiento.Location = new Point(31, 238);
            grpAbastecimiento.Name = "grpAbastecimiento";
            grpAbastecimiento.Size = new Size(403, 200);
            grpAbastecimiento.TabIndex = 1;
            grpAbastecimiento.TabStop = false;
            grpAbastecimiento.Text = "Abastecimiento";
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
    }
}
