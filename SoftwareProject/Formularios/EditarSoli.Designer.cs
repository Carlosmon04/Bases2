namespace SoftwareProject.Formularios
{
    partial class EditarSoli
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRegresar = new System.Windows.Forms.Button();
            this.Calendar1 = new System.Windows.Forms.MonthCalendar();
            this.cmxHoras = new System.Windows.Forms.ComboBox();
            this.btnConfirmarHora = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(50)))));
            this.panel2.Controls.Add(this.btnConfirmarHora);
            this.panel2.Controls.Add(this.cmxHoras);
            this.panel2.Controls.Add(this.Calendar1);
            this.panel2.Controls.Add(this.btnRegresar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1213, 591);
            this.panel2.TabIndex = 4;
            // 
            // btnRegresar
            // 
            this.btnRegresar.BackColor = System.Drawing.SystemColors.Info;
            this.btnRegresar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRegresar.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegresar.ForeColor = System.Drawing.Color.Black;
            this.btnRegresar.Location = new System.Drawing.Point(82, 504);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(113, 33);
            this.btnRegresar.TabIndex = 9;
            this.btnRegresar.Text = "Regresar";
            this.btnRegresar.UseVisualStyleBackColor = false;
            this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
            // 
            // Calendar1
            // 
            this.Calendar1.Location = new System.Drawing.Point(253, 65);
            this.Calendar1.MinDate = new System.DateTime(2024, 8, 20, 0, 0, 0, 0);
            this.Calendar1.Name = "Calendar1";
            this.Calendar1.TabIndex = 10;
            this.Calendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.Calendar1_DateChanged);
            // 
            // cmxHoras
            // 
            this.cmxHoras.FormattingEnabled = true;
            this.cmxHoras.Items.AddRange(new object[] {
            "8:00",
            "9:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00"});
            this.cmxHoras.Location = new System.Drawing.Point(748, 113);
            this.cmxHoras.Name = "cmxHoras";
            this.cmxHoras.Size = new System.Drawing.Size(249, 24);
            this.cmxHoras.TabIndex = 11;
            this.cmxHoras.SelectedIndexChanged += new System.EventHandler(this.cmxHoras_SelectedIndexChanged);
            // 
            // btnConfirmarHora
            // 
            this.btnConfirmarHora.BackColor = System.Drawing.SystemColors.Info;
            this.btnConfirmarHora.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConfirmarHora.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmarHora.ForeColor = System.Drawing.Color.Black;
            this.btnConfirmarHora.Location = new System.Drawing.Point(600, 504);
            this.btnConfirmarHora.Name = "btnConfirmarHora";
            this.btnConfirmarHora.Size = new System.Drawing.Size(113, 33);
            this.btnConfirmarHora.TabIndex = 12;
            this.btnConfirmarHora.Text = "Confirmar";
            this.btnConfirmarHora.UseVisualStyleBackColor = false;
            this.btnConfirmarHora.Click += new System.EventHandler(this.btnConfirmarHora_Click);
            // 
            // EditarSoli
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 591);
            this.Controls.Add(this.panel2);
            this.Name = "EditarSoli";
            this.Text = "EditarSoli";
            this.Load += new System.EventHandler(this.EditarSoli_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRegresar;
        private System.Windows.Forms.ComboBox cmxHoras;
        private System.Windows.Forms.MonthCalendar Calendar1;
        private System.Windows.Forms.Button btnConfirmarHora;
    }
}