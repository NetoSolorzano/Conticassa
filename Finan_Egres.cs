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
        AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();     // categorias
        AutoCompleteStringCollection accd = new AutoCompleteStringCollection();     // ctas destino
        public Finan_Egres()
        {
            InitializeComponent();
            CargaFormatos();
            chk_giroC_CheckedChanged(null, null);
        }
        private void Finan_Egres_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        private void CargaFormatos()
        {
            pan_p.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
            eti_cuenta.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
            acsc = new AutoCompleteStringCollection();
            Tx_catEgre.AutoCompleteCustomSource = acsc;
            Tx_catEgre.AutoCompleteMode = AutoCompleteMode.None;
            Tx_catEgre.AutoCompleteSource = AutoCompleteSource.CustomSource;
            DataRow[] depar = Program.dt_definic.Select("idtabella='CAM'");
            acsc.Clear();
            foreach (DataRow row in depar)
            {
                acsc.Add(row["descrizionerid"].ToString());
            }
            listBox1.Visible = false;
            //
            accd = new AutoCompleteStringCollection();
            Tx_ctaDes.AutoCompleteCustomSource = accd;
            Tx_ctaDes.AutoCompleteMode = AutoCompleteMode.None;
            Tx_ctaDes.AutoCompleteSource = AutoCompleteSource.CustomSource;
            depar = Program.dt_definic.Select("idtabella='CON'");
            accd.Clear();
            foreach (DataRow row in depar)
            {
                accd.Add(row["descrizionerid"].ToString());
            }
            listBox2.Visible = false;
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

        #region autocompletado cat.egresos y cta.destino
        private void Tx_catEgre_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (Tx_catEgre.Text.Length == 0)
            {
                hideResults();
                return;
            }
            foreach (String s in Tx_catEgre.AutoCompleteCustomSource)
            {
                if (s.ToUpper().Contains(Tx_catEgre.Text.ToUpper()))
                {
                    listBox1.Items.Add(s);
                    listBox1.Visible = true;
                }
            }
        }
        private void Tx_ctaDes_TextChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            if (Tx_ctaDes.Text.Length == 0)
            {
                hideResults();
                return;
            }
            foreach (String s in Tx_ctaDes.AutoCompleteCustomSource)
            {
                if (s.ToUpper().Contains(Tx_ctaDes.Text.ToUpper()))
                {
                    listBox2.Items.Add(s);
                    listBox2.Visible = true;
                }
            }
        }
        private void hideResults()
        {
            listBox1.Visible = false;
            listBox2.Visible = false;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tx_catEgre.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
            hideResults();
        }
        private void listBox1_Leave(object sender, EventArgs e)
        {
            hideResults();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tx_ctaDes.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
            hideResults();
        }
        private void listBox2_Leave(object sender, EventArgs e)
        {
            hideResults();
        }
        #endregion

    }
}
