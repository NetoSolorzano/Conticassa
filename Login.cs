using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;
using System.Security.Cryptography;
using MySql.Data;

namespace Conticassa
{
    public partial class Login : Form1
    {
        publicoConf conf = new publicoConf();
        // conexion a la base de datos
        public static string serv = ConfigurationManager.AppSettings["serv"].ToString();     // Decrypt(ConfigurationManager.AppSettings["serv"].ToString(), true);
        public static string port = ConfigurationManager.AppSettings["port"].ToString();
        public static string usua = ConfigurationManager.AppSettings["user"].ToString(); 
        public static string cont = ConfigurationManager.AppSettings["pass"].ToString();     // Decrypt(ConfigurationManager.AppSettings["pass"].ToString(), true);
        public static string data = ConfigurationManager.AppSettings["data"].ToString();
        public static string ctl = ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";


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
        private static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("pass",typeof(String));   // SecurityKey
            string key = "8312@Sorol";
            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider
                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
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
            this.generalBoton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.generalBoton1.Font = new System.Drawing.Font("Arial", 11F);
            this.generalBoton1.Location = new System.Drawing.Point(159, 131);
            this.generalBoton1.Name = "generalBoton1";
            this.generalBoton1.Size = new System.Drawing.Size(129, 37);
            this.generalBoton1.TabIndex = 4;
            this.generalBoton1.Text = "Acceder";
            this.generalBoton1.UseVisualStyleBackColor = false;
            this.generalBoton1.Click += new System.EventHandler(this.generalBoton1_Click);
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

        private void generalBoton1_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
        }
    }
}
