using ADGV;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conticassa
{
    public partial class tipcam : Form1
    {
        static string nomform = "tipcam";               // nombre del formulario
        string asd = Program.vg_user;                   // usuario conectado al sistema
        string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string v_noM1 = "";
        string v_noM2 = "";
        string v_noM3 = "";
        string v_noM4 = "";
        Finan_Egres OFegres = new Finan_Egres();
        publicoConf lp = new publicoConf();
        // string de conexion
        string DB_CONN_STR = "server=" + Login.serv + ";uid=" + Login.usua + ";pwd=" + Login.cont + ";database=" + Login.data + ";";
        DataTable dtg = new DataTable();
        DataTable dtm = new DataTable();
        public tipcam()
        {
            InitializeComponent();
        }
        private void tipcam_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        private void tipcam_Load(object sender, EventArgs e)
        {
            /*
            ToolTip toolTipNombre = new ToolTip();           // Create the ToolTip and associate with the Form container.
            toolTipNombre.AutoPopDelay = 5000;
            toolTipNombre.InitialDelay = 1000;
            toolTipNombre.ReshowDelay = 500;
            toolTipNombre.ShowAlways = true;                 // Force the ToolTip text to be displayed whether or not the form is active.
            toolTipNombre.SetToolTip(toolStrip1, nomform);   // Set up the ToolTip text for the object
            */
            init();
            //toolboton();
            limpiar();
            sololee();
            dataload();
            //grilla();
            this.KeyPreview = true;
            advancedDataGridView1.Enabled = false;
        }
        private void init()
        {
            jalainfo();
            Bt_add.Image = (Image)Resource1.ResourceManager.GetObject("new_tab20"); // Image.FromFile(img_btN);
            Bt_edit.Image = (Image)Resource1.ResourceManager.GetObject("edit20");
            Bt_anul.Image = (Image)Resource1.ResourceManager.GetObject("delete20");
            //Bt_print.Image = ;
            Bt_ver.Image = (Image)Resource1.ResourceManager.GetObject("search_left20");
            Bt_close.Image = (Image)Resource1.ResourceManager.GetObject("close20");
            Bt_ini.Image = (Image)Resource1.ResourceManager.GetObject("arrow_in_left20");
            Bt_sig.Image = (Image)Resource1.ResourceManager.GetObject("arrow_right20");
            Bt_ret.Image = (Image)Resource1.ResourceManager.GetObject("arrow_left20");
            Bt_fin.Image = (Image)Resource1.ResourceManager.GetObject("arrow_in_right20");
            // año y mes
            dtp_yea.Format = DateTimePickerFormat.Custom;
            dtp_yea.CustomFormat = "yyyy";
            dtp_yea.ShowUpDown = true;
            //
            dtp_mes.Format = DateTimePickerFormat.Custom;
            dtp_mes.CustomFormat = "MM";
            dtp_mes.ShowUpDown = true;
        }
        private void jalainfo()                 // obtiene datos de imagenes
        {
            try
            {
                for (int t = 0; t < Program.dt_enlaces.Rows.Count; t++)
                {
                    //DataRow row = Program.dt_enlaces.Rows[t];
                    //if (row["campo"].ToString() == "estado" && row["param"].ToString() == "anulado") vEstAnu = row["valor"].ToString().Trim();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
        }
        public void dataload()                  // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            string consulta = "select idcodice,descrizionerid,descrizione from desc_mon";
            using (MySqlCommand micon = new MySqlCommand(consulta, conn))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                {
                    da.Fill(dtm);
                }
            }
            if (dtm.Rows.Count == 2) v_noM1 = dtm.Rows[1].ItemArray[2].ToString();
            if (dtm.Rows.Count == 3)
            {
                v_noM1 = dtm.Rows[1].ItemArray[2].ToString();
                v_noM2 = dtm.Rows[2].ItemArray[2].ToString();
            }
            conn.Close();
        }
        #region limpiadores_modos
        public void sololee()
        {
            lp.sololee(this);
        }
        public void escribe()
        {
            lp.escribe(this);
        }
        private void limpiar()
        {
            lp.limpiar(this);
        }
        private void limpiaPag(TabPage pag)
        {
            lp.limpiapag(pag);
        }
        public void limpia_chk()
        {
            lp.limpia_chk(this);
        }
        public void limpia_otros()
        {
            //checkBox1.Checked = false;
        }
        public void limpia_combos()
        {
            lp.limpia_cmb(this);
        }
        #endregion limpiadores_modos;


    }
}
