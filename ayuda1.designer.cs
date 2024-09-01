namespace Conticassa
{
    partial class ayuda1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ayuda1));
            this.button1 = new System.Windows.Forms.Button();
            this.generalEtiqueta1 = new Conticassa.generalEtiqueta();
            this.generalEtiqueta2 = new Conticassa.generalEtiqueta();
            this.Tx_codigo = new Conticassa.generalTextBox();
            this.Tx_nombre = new Conticassa.generalTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(359, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 27);
            this.button1.TabIndex = 3;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // generalEtiqueta1
            // 
            this.generalEtiqueta1.AutoSize = true;
            this.generalEtiqueta1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(243)))), ((int)(((byte)(219)))));
            this.generalEtiqueta1.Font = new System.Drawing.Font("Verdana", 8F);
            this.generalEtiqueta1.ForeColor = System.Drawing.Color.Black;
            this.generalEtiqueta1.Location = new System.Drawing.Point(10, 61);
            this.generalEtiqueta1.Name = "generalEtiqueta1";
            this.generalEtiqueta1.Size = new System.Drawing.Size(131, 13);
            this.generalEtiqueta1.TabIndex = 5;
            this.generalEtiqueta1.Text = "Código del proveedor";
            // 
            // generalEtiqueta2
            // 
            this.generalEtiqueta2.AutoSize = true;
            this.generalEtiqueta2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(243)))), ((int)(((byte)(219)))));
            this.generalEtiqueta2.Font = new System.Drawing.Font("Verdana", 8F);
            this.generalEtiqueta2.ForeColor = System.Drawing.Color.Black;
            this.generalEtiqueta2.Location = new System.Drawing.Point(10, 90);
            this.generalEtiqueta2.Name = "generalEtiqueta2";
            this.generalEtiqueta2.Size = new System.Drawing.Size(135, 13);
            this.generalEtiqueta2.TabIndex = 6;
            this.generalEtiqueta2.Text = "Nombre o razón social";
            // 
            // Tx_codigo
            // 
            this.Tx_codigo.BackColor = System.Drawing.Color.White;
            this.Tx_codigo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Tx_codigo.Font = new System.Drawing.Font("Verdana", 8F);
            this.Tx_codigo.ForeColor = System.Drawing.Color.Black;
            this.Tx_codigo.Location = new System.Drawing.Point(148, 61);
            this.Tx_codigo.Name = "Tx_codigo";
            this.Tx_codigo.Size = new System.Drawing.Size(58, 13);
            this.Tx_codigo.TabIndex = 7;
            this.Tx_codigo.Validating += new System.ComponentModel.CancelEventHandler(this.Tx_codigo_Validating);
            // 
            // Tx_nombre
            // 
            this.Tx_nombre.BackColor = System.Drawing.Color.White;
            this.Tx_nombre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Tx_nombre.Font = new System.Drawing.Font("Verdana", 8F);
            this.Tx_nombre.ForeColor = System.Drawing.Color.Black;
            this.Tx_nombre.Location = new System.Drawing.Point(148, 90);
            this.Tx_nombre.Name = "Tx_nombre";
            this.Tx_nombre.Size = new System.Drawing.Size(306, 13);
            this.Tx_nombre.TabIndex = 8;
            // 
            // ayuda1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(468, 202);
            this.Controls.Add(this.Tx_nombre);
            this.Controls.Add(this.Tx_codigo);
            this.Controls.Add(this.generalEtiqueta2);
            this.Controls.Add(this.generalEtiqueta1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ayuda1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PROVEEDOR NUEVO";
            this.Load += new System.EventHandler(this.ayuda1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ayuda1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private generalEtiqueta generalEtiqueta1;
        private generalEtiqueta generalEtiqueta2;
        private generalTextBox Tx_codigo;
        private generalTextBox Tx_nombre;
    }
}
