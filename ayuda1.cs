using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Conticassa
{
    public partial class ayuda1 : Form1
    {
        publicoConf conf = new publicoConf();
        public string para1 = "";
        public string para2 = "";
        public string para3 = "";
        public string para4 = "";
        // Se crea un DataTable que almacenará los datos desde donde se cargaran los datos al DataGridView
        DataTable dtDatos = new DataTable();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        public ayuda1(string param1,string param2,string param3,string param4)
        {
            para1 = param1;              // 
            para2 = param2;              //
            para3 = param3;              //
            para4 = param4;              // 
            InitializeComponent();
        }
        private void ayuda1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE); // conf.fondoPrinBrilloE, 
            this.Text = "PROVEEDOR NUEVO";
            Tx_codigo.MaxLength = 6;
            Tx_codigo.CharacterCasing = CharacterCasing.Upper;
            generalEtiqueta1.BackColor = ColorTranslator.FromHtml("#9df5b9");
            Tx_codigo.BackColor = ColorTranslator.FromHtml("#d5f2de");
            Tx_nombre.MaxLength = 50;
            Tx_nombre.CharacterCasing = CharacterCasing.Upper;
            generalEtiqueta2.BackColor = ColorTranslator.FromHtml("#9df5b9");
            Tx_nombre.BackColor = ColorTranslator.FromHtml("#d5f2de");
        }
        private void ayuda1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public string ReturnValue0 { get; set; }
        public string ReturnValue2 { get; set; }
        public string[] ReturnValueA { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Tx_codigo.Text != "" && Tx_nombre.Text != "")
            {
                var aa = MessageBox.Show("Confirma que desea grabar?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    graba();
                    ReturnValue0 = Tx_codigo.Text;
                    ReturnValue1 = Tx_nombre.Text;
                    ReturnValue0 = Tx_codigo.Text;
                }
            }
            this.Close();
        }

        private void Tx_codigo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Tx_codigo.Text != "")
            {
                bool existe;
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Error de conexión",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    using (MySqlCommand micon = new MySqlCommand("select * from anag_for where idanagrafica=@codi", conn))
                    {
                        micon.Parameters.AddWithValue("@codi", Tx_codigo.Text.Trim());
                        using (MySqlDataReader dr = micon.ExecuteReader())
                        {
                            if (dr.HasRows == true) existe = true;
                            else existe = false;
                        }
                    }
                }
                if (existe == true)
                {
                    MessageBox.Show("Ya existe el código", "Error en código", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Tx_codigo.Clear();
                    Tx_nombre.Clear();
                    Tx_nombre.Focus();
                }
            }
        }
        private void graba()
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                using (MySqlCommand micon = new MySqlCommand("insert into anagrafiche (IDAnagrafica,RagioneSociale,IDCategoria,stato) values (@codi,@nomb,'FOR',1)", conn))
                {
                    micon.Parameters.AddWithValue("@codi", Tx_codigo.Text.Trim());
                    micon.Parameters.AddWithValue("@nomb", Tx_nombre.Text.Trim());
                    micon.ExecuteNonQuery();
                }
            }
        }
    }
}
