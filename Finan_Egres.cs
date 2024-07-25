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
    public partial class Finan_Egres : Form1
    {
        publicoConf conf = new publicoConf();
        public Finan_Egres()
        {
            InitializeComponent();
            CargaFormatos();
            chk_giroC_CheckedChanged(null, null);
        }
        private void CargaFormatos()
        {
            pan_p.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
            eti_cuenta.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
            //
            Tx_catEgre.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Tx_catEgre.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        #region Botones de comando
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            rb_pers.PerformClick();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDICION";
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "BORRAR";
        }
        private void Bt_ver_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "VISUALIZAR";
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "IMPRIMIR";
        }
        private void Bt_ini_Click(object sender, EventArgs e)
        {
            // GO TOP
        }
        private void Bt_sig_Click(object sender, EventArgs e)
        {
            // SKIP 1
        }
        private void Bt_ret_Click(object sender, EventArgs e)
        {
            // SKIP -1
        }
        private void Bt_fin_Click(object sender, EventArgs e)
        {
            // GO BOTT
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region radiobotones y checks
        private void rb_omg_Click(object sender, EventArgs e)
        {
            if (rb_omg.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS OMG";
            }
        }
        private void rb_pers_Click(object sender, EventArgs e)
        {
            if (rb_pers.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS PERSONALES";
            }
        }
        private void chk_giroC_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_giroC.CheckState == CheckState.Checked)
            {
                tx_ctaGiro.Visible = true;
                eti_nomCtaGiro.Visible = true;
            }
            else
            {
                tx_ctaGiro.Visible = false;
                eti_nomCtaGiro.Visible = false;
            }
        }
        #endregion

        private void Tx_catEgre_TextChanged(object sender, EventArgs e)
        {
            TextBox t = sender as TextBox;
            if (t != null)
            {
                if (t.Text.Length > 2)
                {
                    string[] arr = SuggestStrings(t.Text);
                    AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                    collection.AddRange(arr);
                    Tx_catEgre.AutoCompleteCustomSource = collection;
                }
            }
        }
        private string[] SuggestStrings(string t)
        {
            DataRow[] retu = Program.dt_definic.Select("idtabella='CAM' and descrizionerid LIKE '%a%'");
            StringBuilder output = new StringBuilder();
            //string[] retorna;
            foreach (DataRow row in retu)
            {
                output.AppendFormat("{0}", row[3].ToString().Trim());
                output.AppendLine();
            }
            string[] retorna = output.ToString().Split(Environment.NewLine.ToCharArray());
            //string[] retorna = { "" };
            return retorna;
        }
    }
}
