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
        AutoCompleteStringCollection acgc = new AutoCompleteStringCollection();     // cta giroconto
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            string para4 = "";
            if (keyData == Keys.F1 && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION"))
            {
                if (Tx_ctaDes.Focus())
                {
                    para1 = (rb_omg.Checked == true) ? "omg" : "personal";
                    para2 = "cuenta";
                    para3 = "activos";    // todos | activos
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))   // 0=codigo, 1=descripCorta, 2=descripLarga
                        {
                            Tx_ctaDes.Text = ayu2.ReturnValue1;
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void CargaFormatos()
        {
            pan_p.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
            eti_cuenta.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
            // categorias
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
            // cuentas personales
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
            // giroconto
            acgc = new AutoCompleteStringCollection();
            tx_ctaGiro.AutoCompleteCustomSource = acgc;
            tx_ctaGiro.AutoCompleteMode = AutoCompleteMode.None;
            tx_ctaGiro.AutoCompleteSource = AutoCompleteSource.CustomSource;
            depar = Program.dt_definic.Select("idtabella='CON'");
            acgc.Clear();
            foreach (DataRow row in depar)
            {
                acgc.Add(row["descrizionerid"].ToString());
            }
            listBox3.Visible = false;
            // monedas
            Dictionary<string, string> mons = new Dictionary<string, string>();
            mons.Add("MON001", "SOL");
            mons.Add("MON002", "US$");
            mons.Add("MON003", "EUR");
            cmb_mon.DataSource = new BindingSource(mons, null);
            cmb_mon.DisplayMember = "Value";
            cmb_mon.ValueMember = "Key";
            cmb_mon.SelectedIndex = 0;
        }

        #region Botones de comando
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            rb_pers.PerformClick();
            limpiaTE();
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

        #region limpiadores
        private void limpiaTE() // limpia textbox, etiquetas, combos
        {
            Tx_catEgre.Clear();
            Tx_ctaDes.Clear();
            tx_ctaGiro.Clear();
            tx_descrip.Clear();
            tx_monto.Clear();
            tx_provee.Clear();
            tx_tipcam.Clear();
            //
            eti_nomCaja.Text = "";
            eti_nomCat.Text = "";
            eti_nomCtaGiro.Text = "";
            eti_nomprovee.Text = "";
            //
            cmb_mon.SelectedIndex = 0;
        }
        #endregion

        #region radiobotones y checks
        private void rb_omg_Click(object sender, EventArgs e)
        {
            if (rb_omg.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS OMG";
                limpiaTE();
            }
        }
        private void rb_pers_Click(object sender, EventArgs e)
        {
            if (rb_pers.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS PERSONALES";
                limpiaTE();
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
        private void tx_ctaGiro_TextChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            if (tx_ctaGiro.Text.Length == 0)
            {
                hideResults();
                return;
            }
            foreach (String s in tx_ctaGiro.AutoCompleteCustomSource)
            {
                if (s.ToUpper().Contains(tx_ctaGiro.Text.ToUpper()))
                {
                    listBox3.Items.Add(s);
                    listBox3.Visible = true;
                }
            }
        }

        private void hideResults()
        {
            listBox1.Visible = false;
            listBox2.Visible = false;
            listBox3.Visible = false;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                Tx_catEgre.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                hideResults();
                DataRow[] nc = Program.dt_definic.Select("idtabella='CAM' and descrizionerid='" + Tx_catEgre.Text.Trim() + "'");
                eti_nomCat.Text = nc[0].ItemArray[2].ToString();
            }
        }
        private void listBox1_Leave(object sender, EventArgs e)
        {
            hideResults();
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1)
            {
                Tx_ctaDes.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
                hideResults();
                DataRow[] nc = Program.dt_definic.Select("idtabella='CON' and descrizionerid='" + Tx_ctaDes.Text.Trim() + "'");
                eti_nomCaja.Text = nc[0].ItemArray[2].ToString();
            }
        }
        private void listBox2_Leave(object sender, EventArgs e)
        {
            hideResults();
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex > -1)
            {
                tx_ctaGiro.Text = listBox3.Items[listBox3.SelectedIndex].ToString();
                hideResults();
                DataRow[] nc = Program.dt_definic.Select("idtabella='CON' and descrizionerid='" + tx_ctaGiro.Text.Trim() + "'");
                eti_nomCtaGiro.Text = nc[0].ItemArray[2].ToString();
            }
        }
        private void listBox3_Leave(object sender, EventArgs e)
        {
            hideResults();
        }
        #endregion

        private void Bt_graba_Click(object sender, EventArgs e)
        {
            // validamos datos esenciales
            if (Tx_catEgre.Text == "")
            {
                errorProvider1.SetError(Tx_catEgre, "Debe ingresar un tipo");
                Tx_catEgre.Focus();
                return;
            }
            errorProvider1.SetError(Tx_catEgre, "");
            if (Tx_ctaDes.Text == "")
            {
                errorProvider1.SetError(Tx_ctaDes, "Debe seleccionar la cuenta");
                Tx_ctaDes.Focus();
                return;
            }
            errorProvider1.SetError(Tx_ctaDes, "");
            if (tx_monto.Text == "")
            {
                errorProvider1.SetError(tx_monto, "Debe ingresar un valor");
                tx_monto.Focus();
                return;
            }
            errorProvider1.SetError(tx_monto, "");
        }

    }
}
