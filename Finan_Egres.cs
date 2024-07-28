﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Conticassa
{
    public partial class Finan_Egres : Form1
    {
        // conexion a la base de datos
        string DB_CONN_STR = "server=" + Login.serv + ";port=" + Login.port + ";uid=" + Login.usua + ";pwd=" + Login.cont + ";database=" + Login.data +
            ";ConnectionLifeTime=" + Login.ctl + ";";
        // datos de la grilla
        DataTable dt_grilla = new DataTable();
        //
        publicoConf conf = new publicoConf();
        AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();     // categorias
        AutoCompleteStringCollection accd = new AutoCompleteStringCollection();     // ctas destino
        AutoCompleteStringCollection acgc = new AutoCompleteStringCollection();     // cta giroconto
        public Finan_Egres()
        {
            InitializeComponent();
            CargaFormatos();
            chk_giroC_CheckedChanged(null, null);
            sololee("T");   // T=todos los campos, "" ó "C" campos comunes

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
                if (Tx_ctaDes.Focused == true)   // Tx_ctaDes.Focus() == true
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
                            Tx_ctaDes.Text = ayu2.ReturnValueA[1];
                            eti_nomCaja.Text = ayu2.ReturnValueA[2];
                        }
                    }
                }
                if (tx_ctaGiro.Focused == true)
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
                            tx_ctaGiro.Text = ayu2.ReturnValueA[1];
                            eti_nomCtaGiro.Text = ayu2.ReturnValueA[2];
                        }
                    }
                }
                if (Tx_catEgre.Focused == true)
                {
                    para1 = (rb_omg.Checked == true) ? "omg" : "personal";
                    para2 = "tEgresos";
                    para3 = "activos";    // todos | activos
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))   // 0=codigo, 1=descripCorta, 2=descripLarga
                        {
                            Tx_catEgre.Text = ayu2.ReturnValueA[1];
                            eti_nomCat.Text = ayu2.ReturnValueA[2];
                        }
                    }
                }
                if (tx_provee.Focused == true)
                {
                    para1 = "provee";
                    para2 = "";
                    para3 = "activos";    // todos | activos
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))   // 0=codigo, 1=descripCorta, 2=descripLarga
                        {
                            tx_provee.Text = ayu2.ReturnValueA[0];
                            eti_nomprovee.Text = ayu2.ReturnValueA[1];
                            SendKeys.Send("{Tab}");
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
            DataRow[] depar = Program.dt_definic.Select("idtabella='CAM' and numero=1");
            acsc.Clear();
            foreach (DataRow row in depar)
            {
                acsc.Add(row["descrizionerid"].ToString().Trim());
            }
            listBox1.Visible = false;
            // cuentas personales
            accd = new AutoCompleteStringCollection();
            Tx_ctaDes.AutoCompleteCustomSource = accd;
            Tx_ctaDes.AutoCompleteMode = AutoCompleteMode.None;
            Tx_ctaDes.AutoCompleteSource = AutoCompleteSource.CustomSource;
            depar = Program.dt_definic.Select("idtabella='CON' and numero=1");
            accd.Clear();
            foreach (DataRow row in depar)
            {
                accd.Add(row["descrizionerid"].ToString().Trim());
            }
            listBox2.Visible = false;
            // giroconto
            acgc = new AutoCompleteStringCollection();
            tx_ctaGiro.AutoCompleteCustomSource = acgc;
            tx_ctaGiro.AutoCompleteMode = AutoCompleteMode.None;
            tx_ctaGiro.AutoCompleteSource = AutoCompleteSource.CustomSource;
            depar = Program.dt_definic.Select("idtabella='CON' and numero=1");
            acgc.Clear();
            foreach (DataRow row in depar)
            {
                acgc.Add(row["descrizionerid"].ToString().Trim());
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

        private void jalaGrilla(int dAtras, string ntabla) // muestra datos de la fecha actual hasta <dAtras> días atras 
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string consulta = "";
                        if (ntabla == "cassaomg")
                        {
                            consulta = "select IDBanco,CONCAT(Anno,RIGHT(IDMovimento,6)) AS IDMovimento,DataMovimento,IDDestino,IDCategoria,ImportoDE,ImportoSE,ImportoDU," +
                                "ImportoSU,Cambio,Descrizione,IDGiroConto,monori,ctaori,ctades,usuario,dia,idanagrafica,idcassaomg,tipodesgiro " +
                                "from " + ntabla + " where datamovimento BETWEEN CURDATE() - INTERVAL @dias DAY and curdate() order by IDMovimento desc";
                        }
                        if (ntabla == "cassaconti")
                        {
                            consulta = "select IDBanco,CONCAT(Anno,RIGHT(IDMovimento,6)) AS IDMovimento,DataMovimento,IDConto,IDCategoria,ImportoDE,ImportoSE,ImportoDU," +
                                "ImportoSU,Cambio,Descrizione,IDGiroConto,monori,ctaori,ctades,usuario,dia,idanagrafica,idcassaconti,tipodesgiro " +
                                "from " + ntabla + " where datamovimento BETWEEN CURDATE() - INTERVAL @dias DAY and curdate() order by IDMovimento desc";
                        }
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.Parameters.AddWithValue("@dias", dAtras);
                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                dt_grilla.Clear();
                                dt_grilla.Columns.Clear();
                                da.Fill(dt_grilla);
                                dataGridView1.DataSource = dt_grilla;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
        }

        #region Botones de comando
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);   // .PerformClick(); <- no funca!
            //limpiaTE();
            escribe("");
            Tx_catEgre.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDICION";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);   // .PerformClick(); <- no funca!
            //limpiaTE();
            sololee("");
            tx_idOper.Focus();
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

        #region limpiadores, readonlys
        private void limpiaTE() // limpia textbox, etiquetas, combos
        {
            tx_idOper.Clear();
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
        private void escribe(string quien)  // pones los campos necesarios en readonly = false
        {
            tx_idOper.ReadOnly = true;
            Tx_catEgre.ReadOnly = false;
            Tx_ctaDes.ReadOnly = false;
            tx_ctaGiro.ReadOnly = false;
            tx_descrip.ReadOnly = false;
            tx_monto.ReadOnly = false;
            tx_provee.ReadOnly = false;
            tx_tipcam.ReadOnly = false;
            //
            cmb_mon.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            chk_datSimil.Enabled = true;
            chk_giroC.Enabled = true;
        }
        private void sololee(string quien)  //    // T=todos los campos, "" ó "C" campos comunes
        {
            Tx_catEgre.ReadOnly = true;
            Tx_ctaDes.ReadOnly = true;
            tx_ctaGiro.ReadOnly = true;
            tx_descrip.ReadOnly = true;
            tx_monto.ReadOnly = true;
            tx_provee.ReadOnly = true;
            tx_tipcam.ReadOnly = true;
            tx_idOper.ReadOnly = false;
            rb_omg.Enabled = false;
            rb_pers.Enabled = false;
            chk_datSimil.Enabled = false;
            chk_giroC.Enabled = false;
            cmb_mon.Enabled = false;
            if (quien == "T")
            {
                tx_idOper.ReadOnly = true;
            }
        }
        #endregion

        #region radiobotones y checks
        private void rb_omg_Click(object sender, EventArgs e)
        {
            if (rb_omg.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS OMG";
                limpiaTE();
                jalaGrilla(1, "cassaomg");  // muestra datos de un dias atras hasta hoy
            }
        }
        private void rb_pers_Click(object sender, EventArgs e)
        {
            if (rb_pers.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS PERSONALES";
                limpiaTE();
                jalaGrilla(1, "cassaconti");  // muestra datos de un dias atras hasta hoy
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
                SendKeys.Send("{TAB}");
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
                SendKeys.Send("{TAB}");
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
                SendKeys.Send("{TAB}");
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
        #region leaves
        private void Tx_provee_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (tx_provee.Text.Trim() != "")
                {
                    if (ValiProvee() == false)
                    {
                        eti_nomprovee.Text = "";
                        tx_provee.Text = "";
                        MessageBox.Show("No existe el código de proveedor");
                    }
                }
            }
        }
        private void tx_idOper_Validating(object sender, CancelEventArgs e)     // busca en toda la base de datos
        {
            if (tx_idOper.Text.Trim() != "" && ("NUEVO,EDICION").Contains(Tx_modo.Text))
            {
                if (ValiIdOper() == false)
                {
                    tx_idOper.Text = "";
                    MessageBox.Show("No existe el código de operación");
                }
            }
        }
        #endregion
        private bool ValiIdOper()
        {
            bool retorna = false;
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string consulta = "";
                        if (rb_omg.Checked == true)
                        {
                            consulta = "SELECT IDBanco,CONCAT(Anno,RIGHT(IDMovimento, 6)) AS IDMovimento,DataMovimento,IDCategoria,IDDestino,Descrizione,idanagrafica," +
                                "ImportoDE,ImportoSE,ImportoDU,ImportoSU,Cambio,unMisura,Quantita,Chiusura,monori,ctaori,ctades,usuario,dia,idcassaomg,IDGiroConto,tipodesgiro " +
                                "FROM cassaomg WHERE IDMovimento=@idm";
                        }
                        if (rb_pers.Checked == true)
                        {
                            consulta = "select IDBanco,CONCAT(Anno,RIGHT(IDMovimento,6)) AS IDMovimento,DataMovimento,IDConto,IDCategoria,ImportoDE,ImportoSE,ImportoDU," +
                                "ImportoSU,Cambio,Descrizione,IDGiroConto,monori,ctaori,ctades,usuario,dia,idanagrafica,idcassaconti,tipodesgiro " +
                                "from cassaconti WHERE IDMovimento=@idm";
                        }
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.Parameters.AddWithValue("@idm", tx_idOper.Text);
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                if (dr.HasRows == true)
                                {
                                    if (dr.Read())
                                    {
                                        if (dr[0] != null && dr[0].ToString() != "")
                                        {
                                            // me quede aca 27/07/2024
                                            retorna = true;
                                        }
                                    }
                                }
                                else
                                {
                                    retorna = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tx_provee.Text = "";
                    eti_nomprovee.Text = "";
                }
            }
            return retorna;
        }           // valida idOper, si hay jala datos, sino No
        private bool ValiProvee()
        {
            bool retorna = false;
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (MySqlCommand micon = new MySqlCommand("select ragionesociale from anag_for where idanagrafica=@codi", conn))
                        {
                            micon.Parameters.AddWithValue("@codi", tx_provee.Text.Trim());
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                if (dr.HasRows == true)
                                {
                                    if (dr.Read())
                                    {
                                        if (dr[0] != null && dr[0].ToString() != "") eti_nomprovee.Text = dr[0].ToString();
                                        retorna = true;
                                    }
                                }
                                else
                                {
                                    retorna = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tx_provee.Text = "";
                    eti_nomprovee.Text = "";
                }
            }
            return retorna;
        }           // valida existencia del proveedor
    }
}
