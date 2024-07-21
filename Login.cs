using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conticassa
{
    public class Login : Form1
    {
        publicoConf conf = new publicoConf();

        private generalTextBox generalTextBox1;
        private generalEtiqueta generalEtiqueta1;
        private generalEtiqueta generalEtiqueta2;
        private generalBoton generalBoton1;
        private NumericTextBox numericTextBox1;

        public Login() 
        {
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
            CargaINI();
            MinimizeBox = false;
            MaximizeBox = false;
            Width = 450;
            Height = 250;
            CenterToParent();
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void InitializeComponent()
        {
            this.numericTextBox1 = new Conticassa.NumericTextBox();
            this.generalTextBox1 = new Conticassa.generalTextBox();
            this.generalEtiqueta1 = new Conticassa.generalEtiqueta();
            this.generalEtiqueta2 = new Conticassa.generalEtiqueta();
            this.generalBoton1 = new Conticassa.generalBoton();
            this.SuspendLayout();
            // 
            // numericTextBox1
            // 
            this.numericTextBox1.BackColor = System.Drawing.Color.White;
            this.numericTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericTextBox1.Font = new System.Drawing.Font("Verdana", 9F);
            this.numericTextBox1.ForeColor = System.Drawing.Color.Blue;
            this.numericTextBox1.Location = new System.Drawing.Point(212, 85);
            this.numericTextBox1.Name = "numericTextBox1";
            this.numericTextBox1.Size = new System.Drawing.Size(100, 15);
            this.numericTextBox1.TabIndex = 0;
            // 
            // generalTextBox1
            // 
            this.generalTextBox1.BackColor = System.Drawing.Color.White;
            this.generalTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.generalTextBox1.Font = new System.Drawing.Font("Verdana", 9F);
            this.generalTextBox1.ForeColor = System.Drawing.Color.Blue;
            this.generalTextBox1.Location = new System.Drawing.Point(200, 30);
            this.generalTextBox1.Name = "generalTextBox1";
            this.generalTextBox1.Size = new System.Drawing.Size(100, 15);
            this.generalTextBox1.TabIndex = 1;
            // 
            // generalEtiqueta1
            // 
            this.generalEtiqueta1.AutoSize = true;
            this.generalEtiqueta1.BackColor = System.Drawing.Color.Aquamarine;
            this.generalEtiqueta1.Font = new System.Drawing.Font("Verdana", 9F);
            this.generalEtiqueta1.ForeColor = System.Drawing.Color.Blue;
            this.generalEtiqueta1.Location = new System.Drawing.Point(67, 31);
            this.generalEtiqueta1.Name = "generalEtiqueta1";
            this.generalEtiqueta1.Size = new System.Drawing.Size(131, 14);
            this.generalEtiqueta1.TabIndex = 2;
            this.generalEtiqueta1.Text = "Campo tipo general";
            // 
            // generalEtiqueta2
            // 
            this.generalEtiqueta2.AutoSize = true;
            this.generalEtiqueta2.BackColor = System.Drawing.Color.Aquamarine;
            this.generalEtiqueta2.Font = new System.Drawing.Font("Verdana", 9F);
            this.generalEtiqueta2.ForeColor = System.Drawing.Color.Blue;
            this.generalEtiqueta2.Location = new System.Drawing.Point(70, 86);
            this.generalEtiqueta2.Name = "generalEtiqueta2";
            this.generalEtiqueta2.Size = new System.Drawing.Size(140, 14);
            this.generalEtiqueta2.TabIndex = 3;
            this.generalEtiqueta2.Text = "Campo solo números";
            // 
            // generalBoton1
            // 
            this.generalBoton1.BackColor = System.Drawing.Color.White;
            //this.generalBoton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generalBoton1.Font = new System.Drawing.Font("Arial", 11F);
            this.generalBoton1.Location = new System.Drawing.Point(159, 131);
            this.generalBoton1.Name = "generalBoton1";
            this.generalBoton1.Size = new System.Drawing.Size(129, 37);
            this.generalBoton1.TabIndex = 4;
            this.generalBoton1.Text = "generalBoton1";
            this.generalBoton1.UseVisualStyleBackColor = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(420, 192);
            this.Controls.Add(this.generalBoton1);
            this.Controls.Add(this.generalEtiqueta2);
            this.Controls.Add(this.generalEtiqueta1);
            this.Controls.Add(this.generalTextBox1);
            this.Controls.Add(this.numericTextBox1);
            this.Name = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void CargaINI()
        {
            numericTextBox1.Font = new System.Drawing.Font(conf.nombreFont, conf.tamañoFont);
            numericTextBox1.ForeColor = System.Drawing.Color.FromName(conf.colorFont);

            generalTextBox1.Font = new System.Drawing.Font(conf.nombreFont, conf.tamañoFont);
            generalTextBox1.ForeColor = System.Drawing.Color.FromName(conf.colorFont);

            generalEtiqueta1.BackColor = System.Drawing.Color.FromArgb(conf.fondoBrilloE,conf.fondoRojoE,conf.fondoVerdeE,conf.fondoAzulE);  // FromName(conf.nombreFondo);
            generalEtiqueta1.Font = new System.Drawing.Font(conf.nombreFont, conf.tamañoFont);
            generalEtiqueta1.ForeColor = System.Drawing.Color.FromName(conf.colorFont);

            generalEtiqueta2.BackColor = System.Drawing.Color.FromArgb(conf.fondoBrilloE, conf.fondoRojoE, conf.fondoVerdeE, conf.fondoAzulE);  // FromName(conf.nombreFondo)
            generalEtiqueta2.Font = new System.Drawing.Font(conf.nombreFont, conf.tamañoFont);
            generalEtiqueta2.ForeColor = System.Drawing.Color.FromName(conf.colorFont);

            this.generalBoton1.BackColor = System.Drawing.Color.FromArgb(conf.fondoBrilloB, conf.fondoRojoB, conf.fondoVerdeB, conf.fondoAzulB);    //FromName(conf.colorfondoBoton);
            //this.generalBoton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generalBoton1.Font = new System.Drawing.Font(conf.nomFontBoton, conf.tamañoFontBoton);

        }
    }
}
