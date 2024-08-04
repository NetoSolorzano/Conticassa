using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        //
        catEgresos OcatEg = new catEgresos();                                       // Objeto categoría de egreso
        monedas Omone = new monedas();                                              // Objeto moneda
        cajDestino Ocajd = new cajDestino();                                        // Objeto cada de destino - desde donde sale el dinero
        provees Oprove = new provees();                                             // Objeto proveedor
        montos Omonto = new montos();                                               // Objeto monto
        //
        Egresos Oegreso = new Egresos();
        string nomForm = "";
        int diasAtroya = 0;                                                         // dias atras hasta donde mostrará la grilla
        int limCols = 1;                                                            // limite de columnas que muestra la grilla
        public Finan_Egres()
        {
            InitializeComponent();
            CargaFormatos();
            chk_giroC_CheckedChanged(null, null);
            sololee("T");   // T=todos los campos, "" ó "C" campos comunes
            jalainfo();
        }
        private void Finan_Egres_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
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
        }    // F1 
        private void CargaFormatos()
        {
            pan_p.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
            //eti_cuenta.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
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
            depar = Program.dt_definic.Select("idtabella='MON' and numero=1");
            cmb_mon.DataSource = depar.CopyToDataTable();
            cmb_mon.DisplayMember = "descrizionerid";
            cmb_mon.ValueMember = "idcodice";
        }
        private void jalainfo()
        {
            // 31/07/2024 .. variabilizamos los datos que vamos a necesitar
            nomForm = this.Name;
            DataRow[] row = Program.dt_enlaces.Select("formulario='" + nomForm + "' and campo='grillas' and param='diasAtras'");
            diasAtroya = int.Parse(row[0]["valor"].ToString());
            row = Program.dt_enlaces.Select("formulario='" + nomForm + "' and campo='grillas' and param='limCols'");
            limCols = int.Parse(row[0]["valor"].ToString());
        }
        private void jalaGrilla(int dAtras, string ntabla)
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
                            consulta = "ConEgre_cassaOmg";
                        }
                        if (ntabla == "cassaconti")
                        {
                            consulta = "ConEgre_cassaConti";
                        }
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.CommandType = CommandType.StoredProcedure;
                            micon.Parameters.AddWithValue("@Vdias", dAtras);
                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                dt_grilla.Clear();
                                dt_grilla.Columns.Clear();
                                da.Fill(dt_grilla);     // me quede aca, error al pasar de omg a personal
                                dataGridView1.DataSource = dt_grilla;
                            }
                        }
                        // buscamos tipo de cambio del día
                        using (MySqlCommand micon = new MySqlCommand("select ifnull(Cambio1,0),ifnull(Cambio2,0) from cambi where date(datavaluta)=@fec", conn))  // dolares,euros
                        {
                            string fcv = selecFecha1.Value.ToString().Substring(6, 4) + "-" + selecFecha1.Value.ToString().Substring(3, 2) + "-" + selecFecha1.Value.ToString().Substring(0, 2);
                            micon.Parameters.AddWithValue("@fec", fcv);
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    if (dr.Read())
                                    {
                                        tx_tipcam.Text = Math.Round(dr.GetDecimal(0), 3).ToString(); //.GetDecimal(0).ToString("#0.00"); //    .ToString("#0.000"); //dr.GetString(0);   // tipo de cambio dolares
                                        Omonto.tipCDol = Math.Round(dr.GetDecimal(0), 3); // Math.Round((decimal)dr.GetFloat(0),3)
                                        Omonto.tipCOri = Math.Round(dr.GetDecimal(1), 3); // Math.Round((decimal)dr.GetFloat(1),3);  // tipo de cambio euro
                                        if (Omonto.tipCDol <= 0 || Omonto.tipCOri <= 0)
                                        {
                                            MessageBox.Show("El tipo de cambio Dólares es: " + Omonto.tipCDol.ToString() + Environment.NewLine +
                                                "El tipo de cambio Euros es: " + Omonto.tipCOri.ToString(), "Alerta",MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                                else
                                {
                                    var aa = MessageBox.Show("No existen tipos de cambio para la fecha actual" + Environment.NewLine +
                                        "Desea ingresarlos en este momento?","Atención",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (aa == DialogResult.Yes)
                                    {
                                        // llamada a formulario de tipos de cambio
                                    }
                                }
                            }
                        }
                        armaGrilla(dataGridView1, limCols);      // cuadramos las columnas de la grilla
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
        }                      // muestra datos de la fecha actual hasta <dAtras> días atras 
        private void jalaoc()
        {
            tx_idOper.Text = Oegreso.IdMovim;
            selecFecha1.Value = DateTime.Parse(Oegreso.FechOper);
            Tx_catEgre.Text = Oegreso.CatEgreso.nombre;
            eti_nomCat.Text = Oegreso.CatEgreso.largo;
            cmb_mon.SelectedValue = Oegreso.Moneda.codigo;
            tx_monto.Text = Oegreso.Monto.monOrige.ToString("#0.00");
            tx_tipcam.Text = Oegreso.TipCamb.ToString("#0.000");
            Tx_ctaDes.Text = Oegreso.CajaDes.nombre;
            eti_nomCaja.Text = Oegreso.CajaDes.largo;
            tx_provee.Text = Oegreso.Proveedor.codigo;
            eti_nomprovee.Text = Oegreso.Proveedor.nombre;
            tx_descrip.Text = Oegreso.Descrip;
        }                                                   // muestra en el formulario los objetos de la clase Egresos
        public void armaGrilla(DataGridView dgv_, int filasLim)
        {
            if (dgv_.Rows.Count > 0)
            {
                for (int i = 0; i < dgv_.Columns.Count; i++)
                {
                    if (i > filasLim)
                    {
                        dgv_.Columns[i].Visible = false;
                    }
                    else
                    {
                        dgv_.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        _ = decimal.TryParse(dgv_.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                        if (vd != 0) dgv_.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                int b = 0;
                for (int i = 0; i < dgv_.Columns.Count; i++)
                {
                    int a = dgv_.Columns[i].Width;
                    b += a;
                    dgv_.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgv_.Columns[i].Width = a;
                }
                if (b < dgv_.Width) dgv_.Width = b - 20;
                dgv_.ReadOnly = true;
            }
            else
            {

            }
        }               // ajusta el ancho de las columnas y muestra hasta el limite

        #region Botones de comando
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            escribe("");
            Tx_catEgre.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDICION";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            sololee("");
            pan_p.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
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

        #region radiobotones y checks   // me quede aca revisando si pasamos a clase o hacerlo publico
        private void rb_omg_Click(object sender, EventArgs e)
        {
            if (rb_omg.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS OMG";
                pan_p.Tag = "omg";
                limpiaTE();
                jalaGrilla(diasAtroya, "cassaomg");  // muestra datos de un dias atras hasta hoy
            }
        }
        private void rb_pers_Click(object sender, EventArgs e)
        {
            if (rb_pers.Checked == true)
            {
                eti_tituloForm.Text = eti_tituloForm.Tag.ToString() + "DE CUENTAS PERSONALES";
                pan_p.Tag = "personal";
                limpiaTE();
                jalaGrilla(diasAtroya, "cassaconti");  // muestra datos de un dias atras hasta hoy
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
                if (s.ToUpper() == Tx_catEgre.Text.ToUpper())
                {
                    hideResults();
                    return;
                }
                else
                {
                    if (s.ToUpper().Contains(Tx_catEgre.Text.ToUpper()))
                    {
                        listBox1.Items.Add(s);
                        listBox1.Visible = true;
                    }
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
                if (s.ToUpper() == Tx_ctaDes.Text.ToUpper())
                {
                    hideResults();
                    return;
                }
                else
                {
                    if (s.ToUpper().Contains(Tx_ctaDes.Text.ToUpper()))
                    {
                        listBox2.Items.Add(s);
                        listBox2.Visible = true;
                    }
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
                OcatEg.codigo = Tx_catEgre.Text;
                OcatEg.nombre = eti_nomCat.Text;
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
                Ocajd.codigo = Tx_ctaDes.Text;
                Ocajd.nombre = eti_nomCaja.Text;
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
                // objetos de la cuenta giro
                SendKeys.Send("{TAB}");
            }
        }
        private void listBox3_Leave(object sender, EventArgs e)
        {
            hideResults();
        }
        #endregion

        #region leaves y validaciones
        private void Tx_catEgre_Leave(object sender, EventArgs e)
        {
            /* No hacemos la validación aqui porque el leave dispara cuando se sale hacia el listbox */
        }
        private void Tx_catEgre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION")
            {
                if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
                {
                    if (Tx_catEgre.Text.Trim() != "")
                    {
                        if (ValiEgreso(Tx_catEgre.Text) == false)
                        {
                            Tx_catEgre.Clear();
                            eti_nomCat.Text = "";
                        }
                    }
                }
            }
        }
        private void Tx_ctaDes_Leave(object sender, EventArgs e)
        {
            /* No hacemos la validación aqui porque el leave dispara cuando se sale hacia el listbox */

        }
        private void Tx_ctaDes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION")
            {
                if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
                {
                    if (Tx_ctaDes.Text.Trim() != "")
                    {
                        if (ValiCtaDes(Tx_ctaDes.Text) == false)
                        {
                            Tx_ctaDes.Clear();
                            eti_nomCaja.Text = "";
                        }
                    }
                }
            }
        }
        private void Tx_provee_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION")
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
        private void tx_monto_Validating(object sender, CancelEventArgs e)
        {
            decimal monti = 0;
            decimal.TryParse(tx_monto.Text, out monti);
            if (Tx_modo.Text == "NUEVO" && monti > 0)
            {
                Omonto.monOrige = monti;
                if (true)
                {
                    // acá tenemos que ver el asunto de los cambios 
                    // a los moneda soles dolares y euros
                }
            }
        }

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
                        if (pan_p.Tag.ToString() == "omg")         // rb_omg.Checked == true
                        {
                            consulta = "SELECT IDBanco,CONCAT(Anno,RIGHT(IDMovimento, 6)) AS IDMovimento,DataMovimento,IDCategoria,IDDestino,Descrizione,idanagrafica," +
                                "ImportoDE,ImportoSE,ImportoDU,ImportoSU,Cambio,unMisura,Quantita,Chiusura,monori,ctaori,ctades,usuario,dia,idcassaomg,IDGiroConto,tipodesgiro " +
                                "FROM cassaomg WHERE IDMovimento=@idm";
                        }
                        if (pan_p.Tag.ToString() == "personal")    // rb_pers.Checked == true
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
        private bool ValiProvee()   // esto si puede ir a publico, todos estos validadors
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
                                        Oprove.codigo = tx_provee.Text;
                                        Oprove.nombre = eti_nomprovee.Text;
                                        retorna = true;
                                    }
                                }
                                else
                                {
                                    Oprove.codigo = "";
                                    Oprove.nombre = "";
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
        private bool ValiCtaDes(string _nombre)
        {
            // validamos la existencia del nombre en ... descrizionerid
            bool retorna = false;
            DataRow[] row = Program.dt_definic.Select("idtabella='CON' and descrizionerid='" + _nombre + "'");
            foreach (DataRow dat in row)
            {
                retorna = true;
            }
            if (retorna == false)
            {
                Tx_ctaDes.Clear();
                eti_nomCaja.Text = "";
                MessageBox.Show("No existe el nombre de la cuenta");
            }
            else
            {
                retorna = true;
            }
            return retorna;
        }           // valida existencia de la cuenta destino
        private bool ValiEgreso(string _nombre)
        {
            // validamos la existencia del nombre en ... descrizionerid
            bool retorna = false;
            DataRow[] row = Program.dt_definic.Select("descrizionerid='" + _nombre + "' and idtabella='CAM'");
            foreach (DataRow dat in row)
            {
                retorna = true;
            }
            if (retorna == false)
            {
                Tx_catEgre.Clear();
                eti_nomCat.Text = "";
                MessageBox.Show("No existe el nombre del egreso");
            }
            return retorna;
        }
        #endregion

        #region combos
        private void cmb_mon_SelectedValueChanged(object sender, EventArgs e)
        {
            Omone.codigo = cmb_mon.SelectedValue.ToString();              // codigo de la moneda
            Omone.siglas = cmb_mon.Text;    // siglas de la moneda
            Omone.nombre = "";              // nombre de la moneda

            // buscamos su tipo de cambio

            Omonto.codMOrige = cmb_mon.SelectedValue.ToString();              // codigo de la moneda
            Omonto.monDolar = 0;        // estos importes 
            Omonto.monEuros = 0;        // serán calculados en
            Omonto.monSoles = 0;        // el valid del campo monto
        }   // selección de moneda

        #endregion

        #region datagridview
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")
            {
                string fecOp = "";              // fecha de operacion
                decimal tipca = 0;              // tip cambio del monto origen
                string descr = "";              // descripcion de la operacion
                string idmov = "";              // id del movimiento
                if (rb_omg.Checked == true)
                {
                    // CASA,ID_MOVIM,FECHA,DESTINO,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,idgiroconto,CTA_DESTINO,usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDDestino,IDCategoria
                    fecOp = dataGridView1.Rows[e.RowIndex].Cells["FECHA"].Value.ToString().Substring(0, 10);
                    OcatEg.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDCategoria"].Value.ToString();
                    OcatEg.nombre = dataGridView1.Rows[e.RowIndex].Cells["EGRESO"].Value.ToString();
                    OcatEg.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_EGRESO"].Value.ToString();
                    Omone.codigo = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omone.siglas = dataGridView1.Rows[e.RowIndex].Cells["MONEDA"].Value.ToString();
                    Omone.nombre = dataGridView1.Rows[e.RowIndex].Cells["nombmon"].Value.ToString();
                    Omonto.codMOrige = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omonto.monOrige = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["MONTO"].Value.ToString());
                    Omonto.tipCOri = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Omonto.monDolar = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoDU"].Value.ToString());
                    Omonto.tipCDol = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TIP_CAMBIO"].Value.ToString());
                    Omonto.monSoles = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoSU"].Value.ToString());
                    tipca = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Ocajd.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDDestino"].Value.ToString();
                    Ocajd.nombre = dataGridView1.Rows[e.RowIndex].Cells["DESTINO"].Value.ToString();
                    Ocajd.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_DESTINO"].Value.ToString();
                    Oprove.codigo = dataGridView1.Rows[e.RowIndex].Cells["idanagrafica"].Value.ToString();
                    Oprove.nombre = dataGridView1.Rows[e.RowIndex].Cells["PROVEEDOR"].Value.ToString();
                    descr = dataGridView1.Rows[e.RowIndex].Cells["DESCRIPCION"].Value.ToString();
                    idmov = dataGridView1.Rows[e.RowIndex].Cells["ID_MOVIM"].Value.ToString();
                }
                else
                {
                    // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                    //usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
                    fecOp = dataGridView1.Rows[e.RowIndex].Cells["FECHA"].Value.ToString().Substring(0, 10);
                    OcatEg.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDCategoria"].Value.ToString();
                    OcatEg.nombre = dataGridView1.Rows[e.RowIndex].Cells["EGRESO"].Value.ToString();
                    OcatEg.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_EGRESO"].Value.ToString();
                    Omone.codigo = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omone.siglas = dataGridView1.Rows[e.RowIndex].Cells["MONEDA"].Value.ToString();
                    Omone.nombre = dataGridView1.Rows[e.RowIndex].Cells["nombmon"].Value.ToString();
                    Omonto.codMOrige = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omonto.monOrige = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["MONTO"].Value.ToString());
                    Omonto.tipCOri = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Omonto.monDolar = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoDU"].Value.ToString());
                    Omonto.tipCDol = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TIP_CAMBIO"].Value.ToString());
                    Omonto.monSoles = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoSU"].Value.ToString());
                    tipca = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Ocajd.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDConto"].Value.ToString();
                    Ocajd.nombre = dataGridView1.Rows[e.RowIndex].Cells["CUENTA"].Value.ToString();
                    Ocajd.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_CUENTA"].Value.ToString();
                    Oprove.codigo = dataGridView1.Rows[e.RowIndex].Cells["idanagrafica"].Value.ToString();
                    Oprove.nombre = dataGridView1.Rows[e.RowIndex].Cells["PROVEEDOR"].Value.ToString();
                    descr = dataGridView1.Rows[e.RowIndex].Cells["DESCRIPCION"].Value.ToString();
                    idmov = dataGridView1.Rows[e.RowIndex].Cells["ID_MOVIM"].Value.ToString();
                }
                Oegreso.creaEgreso(pan_p.Tag.ToString(), fecOp, OcatEg, Omone, Omonto, tipca,
                        Ocajd, Oprove, descr, idmov);
                jalaoc();
            }
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

            var aaa = MessageBox.Show("Confirma que desea crear el egreso?","Confirme por favor",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aaa == DialogResult.Yes)
            {
                string fecOp = selecFecha1.Value.Date.ToShortDateString();
                Egresos Oegresos = new Egresos();
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                        return;
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        Oegresos.creaEgreso(pan_p.Tag.ToString(), fecOp, OcatEg, Omone, Omonto, decimal.Parse(tx_tipcam.Text), 
                            Ocajd, Oprove, tx_descrip.Text, "");
                        Oegresos.grabaEgreso(conn);
                    }
                }
            }
        }

    }
}
