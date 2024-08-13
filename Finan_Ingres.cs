using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Conticassa
{
    public partial class Finan_Ingres : Form1
    {
        // conexion a la base de datos
        string DB_CONN_STR = "server=" + Login.serv + ";port=" + Login.port + ";uid=" + Login.usua + ";pwd=" + Login.cont + ";database=" + Login.data +
            ";ConnectionLifeTime=" + Login.ctl + ";";
        // datos de la grilla
        internal DataTable dt_grillaI = new DataTable();
        //
        publicoConf conf = new publicoConf();
        AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();     // categorias
        AutoCompleteStringCollection accd = new AutoCompleteStringCollection();     // ctas destino
        AutoCompleteStringCollection acgc = new AutoCompleteStringCollection();     // cta giroconto
        //
        catIngresos OcatIn = new catIngresos();                                       // Objeto categoría de egreso
        monedas Omone = new monedas();                                              // Objeto moneda
        cajDestino Ocajd = new cajDestino();                                        // Objeto cada de destino - desde donde sale el dinero
        provees Oprove = new provees();                                             // Objeto proveedor
        montos Omonto = new montos();                                               // Objeto monto
        //
        Ingresos Oingreso = new Ingresos();
        Finan_Egres oFEgres = new Finan_Egres();
        string nomForm = "";
        int diasAtroya = 0;                                                         // dias atras hasta donde mostrará la grilla
        int limCols = 1;                                                            // limite de columnas que muestra la grilla

        public Finan_Ingres()
        {
            InitializeComponent();                  // inicializa los objetos graficos
            CargaINI(this);                         // colorea los objetos graficos
            CargaDatos();                           // jala datos de combos y demas
            chk_giroC_CheckedChanged(null, null);   // 
            sololee("T");                           // T=todos los campos, "" ó "C" campos comunes
            jalainfo();                             // jala variables de tabla enlace
            initCampos();                           // limita maximos de ancho en campos y mayusculas
        }

        private void Finan_Ingres_KeyDown(object sender, KeyEventArgs e)
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
                if (Tx_ctaDes.Focused == true)
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
                if (Tx_catIngre.Focused == true)
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
                            Tx_catIngre.Text = ayu2.ReturnValueA[1];
                            eti_nomCat.Text = ayu2.ReturnValueA[2];
                            OcatIn.codigo = ayu2.ReturnValueA[0];
                            OcatIn.nombre = ayu2.ReturnValueA[1];
                            OcatIn.largo = ayu2.ReturnValueA[2];
                        }
                    }
                }
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }    // F1 
        private void CargaDatos()
        {
            // categorias
            acsc = new AutoCompleteStringCollection();
            Tx_catIngre.AutoCompleteCustomSource = acsc;
            Tx_catIngre.AutoCompleteMode = AutoCompleteMode.None;
            Tx_catIngre.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
        private void jalaoc()
        {
            tx_idOper.Text = Oingreso.IdMovim;
            selecFecha1.Value = DateTime.Parse(Oingreso.FechOper);
            Tx_catIngre.Text = Oingreso.CatIngreso.nombre;
            eti_nomCat.Text = Oingreso.CatIngreso.largo;
            cmb_mon.SelectedValue = Oingreso.Moneda.codigo;
            tx_monto.Text = Oingreso.Monto.monOrige.ToString("#0.00");
            tx_tipcam.Text = Oingreso.TipCamb.ToString("#0.000");
            Tx_ctaDes.Text = Oingreso.CajaDes.nombre;
            eti_nomCaja.Text = Oingreso.CajaDes.largo;
            tx_descrip.Text = Oingreso.Descrip;
        }                                                   // muestra en el formulario los objetos de la clase Egresos
        private void initCampos()
        {
            Tx_catIngre.MaxLength = 20;
            Tx_ctaDes.MaxLength = 20;
            tx_ctaGiro.MaxLength = 20;
            tx_descrip.MaxLength = 100;
            tx_idOper.MaxLength = 15;
        }

        #region Botones de comando
        // acá falta todo el asunto de leer los permisos del usuario
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            escribe("");
            Tx_catIngre.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDICION";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            escribe("EDICION");    // sololee("")
            pan_p.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            tx_idOper.Focus();
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "BORRAR";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            sololee("");
            pan_p.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            tx_idOper.Focus();
        }
        private void Bt_ver_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "VISUALIZAR";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            sololee("");
            pan_p.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            tx_idOper.Focus();
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
        private void limpiaObj()
        {
            OcatIn.codigo = "";                                       // Objeto categoría de ingreso
            OcatIn.nombre = "";
            OcatIn.largo = "";
            Omone.codigo = "";                                        // Objeto moneda
            Omone.nombre = "";
            Omone.siglas = "";
            Ocajd.codigo = "";                                        // Objeto cada de destino - desde donde sale el dinero
            Ocajd.nombre = "";
            Ocajd.largo = "";
            Oprove.codigo = "";                                       // Objeto proveedor
            Oprove.nombre = "";
            Omonto.codMOrige = "";                                    // Objeto monto
            Omonto.monDolar = 0;
            Omonto.monEuros = 0;
            Omonto.monOrige = 0;
            Omonto.monSoles = 0;
            Omonto.tipCDol = 0;
            Omonto.tipCOri = 0;
            Oingreso.limpia();
        }
        private void limpiaTE() // limpia textbox, etiquetas, combos
        {
            tx_idOper.Clear();
            Tx_catIngre.Clear();
            Tx_ctaDes.Clear();
            tx_ctaGiro.Clear();
            tx_descrip.Clear();
            tx_monto.Clear();
            tx_tipcam.Clear();
            //
            eti_nomCaja.Text = "";
            eti_nomCat.Text = "";
            eti_nomCtaGiro.Text = "";
            //
            cmb_mon.SelectedIndex = 0;
        }
        private void escribe(string quien)  // pones los campos necesarios en readonly = false
        {
            if (quien == "EDICION") tx_idOper.ReadOnly = true;
            Tx_catIngre.ReadOnly = false;
            Tx_ctaDes.ReadOnly = false;
            tx_ctaGiro.ReadOnly = false;
            tx_descrip.ReadOnly = false;
            tx_monto.ReadOnly = false;
            tx_tipcam.ReadOnly = false;
            //
            cmb_mon.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            chk_datSimil.Enabled = true;
            chk_giroC.Enabled = true;
            cmb_mon.Enabled = true;
            cmb_mon.SelectedIndex = -1;
            cmb_mon_SelectedIndexChanged(null, null);
        }
        private void sololee(string quien)  //    // T=todos los campos, "" ó "C" campos comunes
        {
            Tx_catIngre.ReadOnly = true;
            Tx_ctaDes.ReadOnly = true;
            tx_ctaGiro.ReadOnly = true;
            tx_descrip.ReadOnly = true;
            tx_monto.ReadOnly = true;
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
        private void Tx_catIngre_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (Tx_catIngre.Text.Length == 0)
            {
                hideResults();
                return;
            }
            foreach (String s in Tx_catIngre.AutoCompleteCustomSource)
            {
                if (s.ToUpper() == Tx_catIngre.Text.ToUpper())
                {
                    hideResults();
                    return;
                }
                else
                {
                    if (s.ToUpper().Contains(Tx_catIngre.Text.ToUpper()))
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
                Tx_catIngre.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                hideResults();
                DataRow[] nc = Program.dt_definic.Select("idtabella='CAM' and descrizionerid='" + Tx_catIngre.Text.Trim() + "'");
                eti_nomCat.Text = nc[0].ItemArray[2].ToString();
                OcatIn.codigo = nc[0].ItemArray[1].ToString();
                OcatIn.nombre = Tx_catIngre.Text;
                OcatIn.largo = eti_nomCat.Text;
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
                Ocajd.codigo = nc[0].ItemArray[1].ToString();
                Ocajd.nombre = Tx_ctaDes.Text;
                Ocajd.largo = eti_nomCaja.Text;
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
        private void Tx_catIngre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION")
            {
                if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
                {
                    if (Tx_catIngre.Text.Trim() != "")
                    {
                        if (oFEgres.Vali_CAM(Tx_catIngre.Text) == false) 
                        {
                            Tx_catIngre.Clear();
                            eti_nomCat.Text = "";
                            MessageBox.Show("No existe el nombre del egreso");
                        }
                    }
                }
            }
        }
        private void Tx_ctaDes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION")
            {
                if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
                {
                    if (Tx_ctaDes.Text.Trim() != "")
                    {
                        if (oFEgres.ValiCtaCon(Tx_ctaDes.Text) == false)
                        {
                            Tx_ctaDes.Clear();
                            eti_nomCaja.Text = "";
                            MessageBox.Show("No existe el nombre de la cuenta");
                        }
                    }
                }
            }
        }
        private void tx_idOper_Validating(object sender, CancelEventArgs e)
        {
            if (tx_idOper.Text.Trim() != "" && ("NUEVO,EDICION").Contains(Tx_modo.Text))
            {
                string[] retu = oFEgres.ValiIdOper();
                if (retu[0] == "")
                {
                    limpiaObj();
                    limpiaTE();
                    MessageBox.Show("No existe el código de operación");
                }
                else
                {
                    // asignamos los valores de retu[] a los objetos
                    string fecOp = "";              // fecha de operacion
                    decimal tipca = 0;              // tip cambio del monto origen
                    string descr = "";              // descripcion de la operacion
                    string idmov = "";              // id del movimiento
                    if (rb_omg.Checked == true)
                    {
                        // CASA,ID_MOVIM,FECHA,DESTINO,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,idgiroconto,CTA_DESTINO,usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDDestino,IDCategoria
                        fecOp = retu[2].Substring(0, 10);       // fecha
                        OcatIn.codigo = retu[18];               // aca debe ser id del ingreso
                        OcatIn.nombre = retu[4];                // aca debe ser ingreso
                        OcatIn.largo = retu[23];                // aca debe ser ingreso
                        Omone.codigo = retu[19];                // "codimon"
                        Omone.siglas = retu[5];                 // "MONEDA"
                        Omone.nombre = retu[20];                // "nombmon"
                        Omonto.codMOrige = retu[19];            // "codimon"
                        Omonto.monOrige = decimal.Parse(retu[6]);   // "MONTO"
                        Omonto.tipCOri = decimal.Parse(retu[21]);   // "TCMonOri"
                        Omonto.monDolar = decimal.Parse(retu[14]);  // "ImportoDU"
                        Omonto.tipCDol = decimal.Parse(retu[8]);    // "TIP_CAMBIO"
                        Omonto.monSoles = decimal.Parse(retu[15]);  // "ImportoSU"
                        tipca = decimal.Parse(retu[21]);            // "TCMonOri"
                        Ocajd.codigo = retu[17];                // "IDDestino"
                        Ocajd.nombre = retu[3];                 // "DESTINO"
                        Ocajd.largo = retu[22];                 // "DET_DESTINO"
                        Oprove.codigo = retu[16];               // "idanagrafica"
                        Oprove.nombre = retu[9];                // "PROVEEDOR"
                        descr = retu[7];                        // "DESCRIPCION"
                        idmov = retu[1];                        // "ID_MOVIM"
                    }
                    else
                    {
                        // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                        //usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
                        fecOp = retu[2].Substring(0, 10);       // "FECHA"
                        OcatIn.codigo = retu[18];               // aca debe ser id del ingreso
                        OcatIn.nombre = retu[4];                // aca debe ser ingresos
                        OcatIn.largo = retu[23];                // aca tambien debe ser ingresos
                        Omone.codigo = retu[19];                // "codimon"
                        Omone.siglas = retu[5];                 // "MONEDA"
                        Omone.nombre = retu[20];                // "nombmon"
                        Omonto.codMOrige = retu[19];            // "codimon"
                        Omonto.monOrige = decimal.Parse(retu[6]);   // "MONTO"
                        Omonto.tipCOri = decimal.Parse(retu[21]);   // "TCMonOri"
                        Omonto.monDolar = decimal.Parse(retu[14]);  // "ImportoDU"
                        Omonto.tipCDol = decimal.Parse(retu[8]);    // "TIP_CAMBIO"
                        Omonto.monSoles = decimal.Parse(retu[15]);  // "ImportoSU"
                        tipca = decimal.Parse(retu[21]);        // "TCMonOri"
                        Ocajd.codigo = retu[17];                // "IDConto"
                        Ocajd.nombre = retu[3];                 // "CUENTA"
                        Ocajd.largo = retu[22];                 // "DET_CUENTA"
                        Oprove.codigo = retu[16];               // "idanagrafica"
                        Oprove.nombre = retu[9];                // "PROVEEDOR"
                        descr = retu[7];                        // "DESCRIPCION"
                        idmov = retu[1];                        // "ID_MOVIM"
                    }
                    Oingreso.creaIngreso(pan_p.Tag.ToString(), fecOp, OcatIn, Omone, Omonto, tipca,
                            Ocajd, descr, idmov);
                    jalaoc();
                }
            }
        }     // busca en toda la base de datos
        private void tx_monto_Validating(object sender, CancelEventArgs e)
        {
            decimal monti = 0; decimal cambi = 0;
            decimal.TryParse(tx_monto.Text, out monti);
            decimal.TryParse(tx_tipcam.Text, out cambi);
            if (Tx_modo.Text == "NUEVO" && monti > 0)
            {
                Omonto.monOrige = monti;
                if (true)
                {
                    Omonto = oFEgres.calc_monedas(cmb_mon, monti, cambi);
                }
            }
        }
        private void tx_tipcam_Validating(object sender, CancelEventArgs e)
        {
            decimal monti = 0; decimal cambi = 0;
            decimal.TryParse(tx_monto.Text, out monti);
            decimal.TryParse(tx_tipcam.Text, out cambi);
            if (Tx_modo.Text == "NUEVO" && monti > 0)
            {
                Omonto.monOrige = monti;
                if (true)
                {
                    Omonto = oFEgres.calc_monedas(cmb_mon, monti, cambi);
                }
            }
        }

        #endregion

        #region combos
        private void cmb_mon_SelectedValueChanged(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "" && (cmb_mon.SelectedValue != null && cmb_mon.SelectedValue.ToString() != ""))
            {
                Omone.codigo = cmb_mon.SelectedValue.ToString();              // codigo de la moneda
                Omone.siglas = cmb_mon.Text;    // siglas de la moneda
                DataRow[] row = Program.dt_definic.Select("idtabella='MON' and idcodice='" + Omone.codigo + "'");
                Omone.nombre = row[0].ItemArray[2].ToString();
                if (tx_monto.Text != "" && tx_tipcam.Text != "") oFEgres.calc_monedas(cmb_mon, decimal.Parse(tx_monto.Text), decimal.Parse(tx_tipcam.Text));
            }
        }   // selección de moneda
        private void cmb_mon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_mon.SelectedIndex > -1 && (cmb_mon.SelectedValue != null && cmb_mon.SelectedValue.ToString() != ""))
            {
                Omone.codigo = cmb_mon.SelectedValue.ToString();              // codigo de la moneda
                Omone.siglas = cmb_mon.Text;    // siglas de la moneda
                DataRow[] row = Program.dt_definic.Select("idtabella='MON' and idcodice='" + Omone.codigo + "'");
                if (row.Length > 1) Omone.nombre = row[0].ItemArray[2].ToString();
            }
        }
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
                    // CASA,ID_MOVIM,FECHA,DESTINO,INGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,GIRO_CTA,idgiroconto,CTA_DESTINO,
                    // usuario,dia,ImportoDE,ImportoSE,IDDestino,IDCategoria,codimon,nombmon,TCMonOri,DET_DESTINO,DET_INGRESO
                    fecOp = dataGridView1.Rows[e.RowIndex].Cells["FECHA"].Value.ToString().Substring(0, 10);
                    OcatIn.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDCategoria"].Value.ToString();
                    OcatIn.nombre = dataGridView1.Rows[e.RowIndex].Cells["INGRESO"].Value.ToString();
                    OcatIn.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_INGRESO"].Value.ToString();    // debe ser ingreso
                    Omone.codigo = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omone.siglas = dataGridView1.Rows[e.RowIndex].Cells["MONEDA"].Value.ToString();
                    Omone.nombre = dataGridView1.Rows[e.RowIndex].Cells["nombmon"].Value.ToString();
                    Omonto.codMOrige = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omonto.monOrige = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["MONTO"].Value.ToString());
                    Omonto.tipCOri = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Omonto.monDolar = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoDE"].Value.ToString());
                    Omonto.tipCDol = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TIP_CAMBIO"].Value.ToString());
                    Omonto.monSoles = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoSE"].Value.ToString());
                    tipca = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Ocajd.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDDestino"].Value.ToString();
                    Ocajd.nombre = dataGridView1.Rows[e.RowIndex].Cells["DESTINO"].Value.ToString();
                    Ocajd.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_DESTINO"].Value.ToString();
                    descr = dataGridView1.Rows[e.RowIndex].Cells["DESCRIPCION"].Value.ToString();
                    idmov = dataGridView1.Rows[e.RowIndex].Cells["ID_MOVIM"].Value.ToString();
                }
                else
                {
                    // CASA,ID_MOVIM,FECHA,CUENTA,INGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,GIRO_CTA,IDGiroConto,CTA_DESTINO,
                    // usuario,dia,ImportoDE,ImportoSE,IDConto,IDCategoria,codimon,nombmon,TCMonOri,DET_CUENTA,DET_INGRESO 
                    fecOp = dataGridView1.Rows[e.RowIndex].Cells["FECHA"].Value.ToString().Substring(0, 10);
                    OcatIn.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDCategoria"].Value.ToString();
                    OcatIn.nombre = dataGridView1.Rows[e.RowIndex].Cells["INGRESO"].Value.ToString();
                    OcatIn.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_INGRESO"].Value.ToString(); 
                    Omone.codigo = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omone.siglas = dataGridView1.Rows[e.RowIndex].Cells["MONEDA"].Value.ToString();
                    Omone.nombre = dataGridView1.Rows[e.RowIndex].Cells["nombmon"].Value.ToString();
                    Omonto.codMOrige = dataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omonto.monOrige = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["MONTO"].Value.ToString());
                    Omonto.tipCOri = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Omonto.monDolar = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoDE"].Value.ToString());
                    Omonto.tipCDol = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TIP_CAMBIO"].Value.ToString());
                    Omonto.monSoles = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["ImportoSE"].Value.ToString());
                    tipca = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Ocajd.codigo = dataGridView1.Rows[e.RowIndex].Cells["IDConto"].Value.ToString();
                    Ocajd.nombre = dataGridView1.Rows[e.RowIndex].Cells["CUENTA"].Value.ToString();
                    Ocajd.largo = dataGridView1.Rows[e.RowIndex].Cells["DET_CUENTA"].Value.ToString();
                    descr = dataGridView1.Rows[e.RowIndex].Cells["DESCRIPCION"].Value.ToString();
                    idmov = dataGridView1.Rows[e.RowIndex].Cells["ID_MOVIM"].Value.ToString();
                }
                Oingreso.creaIngreso(pan_p.Tag.ToString(), fecOp, OcatIn, Omone, Omonto, tipca,
                        Ocajd, descr, idmov);
                jalaoc();
            }
        }
        private void insFilaEnDataG(string _casa, string _corre)
        {
            DataRow fila = dt_grillaI.NewRow();
            string fecOp = selecFecha1.Value.Date.ToShortDateString();
            if (rb_omg.Checked == true)
            {
                // CASA,ID_MOVIM,FECHA,DESTINO,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,idgiroconto,CTA_DESTINO,
                //usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDDestino,IDCategoria
                // , , Omone, Omonto, decimal.Parse(tx_tipcam.Text), Ocajd, Oprove, tx_descrip.Text, corre
                fila["CASA"] = _casa;
                fila["ID_MOVIM"] = _corre;
                fila["FECHA"] = fecOp;
                fila["DESTINO"] = Ocajd.nombre;     // nombre cuenta destino
                fila["INGRESO"] = OcatIn.nombre;     // nombre categoria egreso
                fila["MONEDA"] = Omone.siglas;      // siglas moneda origen
                fila["MONTO"] = Omonto.monOrige;    // valor origen
                fila["DESCRIPCION"] = tx_descrip.Text;
                fila["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                //fila["PROVEEDOR"] = Oprove.nombre;
                fila["GIRO_CTA"] = "";
                fila["idgiroconto"] = "";
                fila["CTA_DESTINO"] = "";
                fila["usuario"] = Program.vg_user;
                //fila["dia"] = "";
                fila["ImportoDE"] = Omonto.monDolar;
                fila["ImportoSE"] = Omonto.monSoles;
                //fila["idanagrafica"] = Oprove.codigo;
                fila["IDDestino"] = Ocajd.codigo;
                fila["IDCategoria"] = OcatIn.codigo;
            }
            if (rb_pers.Checked == true)
            {
                // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                // usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
                fila["CASA"] = _casa;
                fila["ID_MOVIM"] = _corre;
                fila["FECHA"] = fecOp;
                fila["CUENTA"] = Ocajd.nombre;
                fila["INGRESO"] = OcatIn.nombre;
                fila["MONEDA"] = Omone.siglas;
                fila["MONTO"] = Omonto.monOrige;
                fila["DESCRIPCION"] = tx_descrip.Text;
                fila["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                //fila["PROVEEDOR"] = Oprove.nombre;
                fila["GIRO_CTA"] = "";
                fila["IDGiroConto"] = "";
                fila["CTA_DESTINO"] = "";
                fila["usuario"] = Program.vg_user;
                //fila["dia"] = "";
                fila["ImportoDE"] = Omonto.monDolar;
                fila["ImportoSE"] = Omonto.monSoles;
                //fila["idanagrafica"] = Oprove.codigo;
                fila["IDConto"] = Ocajd.codigo;
                fila["IDCategoria"] = OcatIn.codigo;
                fila["codimon"] = Omone.codigo;
                fila["nombmon"] = Omone.nombre;
                fila["TCMonOri"] = Omonto.tipCOri;
            }
            dt_grillaI.Rows.InsertAt(fila, 0);
        }                                           // INSERTA en la grilla el registro nuevo despues de grabar en la B.D.
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
                            consulta = "ConIngre_cassaOmg";
                        }
                        if (ntabla == "cassaconti")
                        {
                            consulta = "ConIngre_cassaConti";
                        }
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.CommandType = CommandType.StoredProcedure;
                            micon.Parameters.AddWithValue("@Vdias", dAtras);
                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                dt_grillaI.Clear();
                                dt_grillaI.Columns.Clear();
                                da.Fill(dt_grillaI);
                                dataGridView1.DataSource = dt_grillaI;
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
                                        tx_tipcam.Text = Math.Round(dr.GetDecimal(0), 3).ToString();
                                        Omonto.tipCDol = Math.Round(dr.GetDecimal(0), 3);
                                        Omonto.tipCOri = Math.Round(dr.GetDecimal(1), 3);
                                        if (Omonto.tipCDol <= 0 || Omonto.tipCOri <= 0)
                                        {
                                            MessageBox.Show("El tipo de cambio Dólares es: " + Omonto.tipCDol.ToString() + Environment.NewLine +
                                                "El tipo de cambio Euros es: " + Omonto.tipCOri.ToString(), "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                                else
                                {
                                    var aa = MessageBox.Show("No existen tipos de cambio para la fecha actual" + Environment.NewLine +
                                        "Desea ingresarlos en este momento?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (aa == DialogResult.Yes)
                                    {
                                        // llamada a formulario de tipos de cambio
                                    }
                                }
                            }
                        }
                        oFEgres.armaGrilla(dataGridView1, limCols);      // cuadramos las columnas de la grilla
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
        }                      // muestra datos de la fecha actual hasta <dAtras> días atras 
        public void actFilaEnDataI(DataTable dt, string _casa, string _corre)
        {
            string fecOp = selecFecha1.Value.Date.ToShortDateString();
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                if (dr["ID_MOVIM"].ToString() == (_corre.Substring(0, 4) + oFEgres.CDerecha(_corre, 6)))
                {
                    if (rb_omg.Checked == true)
                    {
                        // CASA,ID_MOVIM,FECHA,DESTINO,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,idgiroconto,CTA_DESTINO,
                        //usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDDestino,IDCategoria
                        // , , Omone, Omonto, decimal.Parse(tx_tipcam.Text), Ocajd, Oprove, tx_descrip.Text, corre
                        dr["CASA"] = _casa;
                        dr["ID_MOVIM"] = _corre;
                        dr["FECHA"] = fecOp;
                        dr["DESTINO"] = Ocajd.nombre;     // nombre cuenta destino
                        dr["INGRESO"] = OcatIn.nombre;     // nombre categoria ingreso
                        dr["MONEDA"] = Omone.siglas;      // siglas moneda origen
                        dr["MONTO"] = Omonto.monOrige;    // valor origen
                        dr["DESCRIPCION"] = tx_descrip.Text;
                        dr["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                        //dr["PROVEEDOR"] = ;
                        dr["GIRO_CTA"] = "";
                        dr["idgiroconto"] = "";
                        dr["CTA_DESTINO"] = "";
                        dr["usuario"] = Program.vg_user;
                        //dr["dia"] = "";
                        dr["ImportoDE"] = Omonto.monDolar;
                        dr["ImportoSE"] = Omonto.monSoles;
                        //dr["idanagrafica"] = ;
                        dr["IDDestino"] = Ocajd.codigo;
                        dr["IDCategoria"] = OcatIn.codigo;
                    }
                    if (rb_pers.Checked == true)
                    {
                        // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                        // usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
                        dr["CASA"] = _casa;
                        dr["ID_MOVIM"] = _corre;
                        dr["FECHA"] = fecOp;
                        dr["CUENTA"] = Ocajd.nombre;
                        dr["INGRESO"] = OcatIn.nombre;
                        dr["MONEDA"] = Omone.siglas;
                        dr["MONTO"] = Omonto.monOrige;
                        dr["DESCRIPCION"] = tx_descrip.Text;
                        dr["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                        //dr["PROVEEDOR"] = ;
                        dr["GIRO_CTA"] = "";
                        dr["IDGiroConto"] = "";
                        dr["CTA_DESTINO"] = "";
                        dr["usuario"] = Program.vg_user;
                        //dr["dia"] = "";
                        dr["ImportoDE"] = Omonto.monDolar;
                        dr["ImportoSE"] = Omonto.monSoles;
                        //dr["idanagrafica"] = ;
                        dr["IDConto"] = Ocajd.codigo;
                        dr["IDCategoria"] = OcatIn.codigo;
                        dr["codimon"] = Omone.codigo;
                        dr["nombmon"] = Omone.nombre;
                        dr["TCMonOri"] = Omonto.tipCOri;
                    }
                    dr.AcceptChanges();
                }
            }
            dt.AcceptChanges();
        }                // ACTUALIZA la grilla despues de haber actualizado la tabla
        #endregion

        private void Bt_graba_Click(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                graba_nuevo();
            }
            if (Tx_modo.Text == "EDICION")
            {
                if (tx_idOper.Text == "")
                {
                    MessageBox.Show("No hay registro que Editar!", "Identificador en blanco", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                var aaa = MessageBox.Show("Confirma que desea EDITAR el Ingreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    graba_edicion();
                    limpiaObj();
                    limpiaTE();
                }
            }
            if (Tx_modo.Text == "BORRAR")
            {
                // validamos que exista registro que borrar
                if (tx_idOper.Text == "")
                {
                    MessageBox.Show("No hay registro que borrar!", "Identificador en blanco", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                var aaa = MessageBox.Show("Confirma que desea BORRAR el Ingreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    string tabla = "";
                    if (rb_omg.Checked == true) tabla = "cassaomg";
                    else tabla = "cassaconti";
                    oFEgres.graba_borrar(tabla, selecFecha1.Value.Year.ToString(), "000000000" + oFEgres.CDerecha(tx_idOper.Text, 6), dt_grillaI);
                    limpiaObj();
                    limpiaTE();
                }
            }
        }
        private void graba_nuevo()
        {
            // validamos datos esenciales
            if (Tx_catIngre.Text == "")
            {
                errorProvider1.SetError(Tx_catIngre, "Debe ingresar un tipo");
                Tx_catIngre.Focus();
                return;
            }
            errorProvider1.SetError(Tx_catIngre, "");
            if (Tx_ctaDes.Text == "")
            {
                errorProvider1.SetError(Tx_ctaDes, "Debe seleccionar la cuenta");
                Tx_ctaDes.Focus();
                return;
            }
            errorProvider1.SetError(Tx_ctaDes, "");
            if (cmb_mon.Text == "")
            {
                errorProvider1.SetError(cmb_mon, "Debe seleccionar la moneda");
                cmb_mon.Focus();
                return;
            }
            errorProvider1.SetError(cmb_mon, "");
            if (tx_monto.Text == "")
            {
                errorProvider1.SetError(tx_monto, "Debe ingresar un valor");
                tx_monto.Focus();
                return;
            }
            errorProvider1.SetError(tx_monto, "");

            var aaa = MessageBox.Show("Confirma que desea crear el Ingreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aaa == DialogResult.Yes)
            {
                string fecOp = selecFecha1.Value.Date.ToShortDateString();
                Ingresos Oingresos = new Ingresos();
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
                        if (Tx_modo.Text == "NUEVO")
                        {
                            string corre = oFEgres.correlativo(conn, ((rb_omg.Checked == true) ? "MCA" : "MCO"), selecFecha1.Value.Date.Year);
                            if (corre != "error" && corre != "")
                            {
                                try
                                {
                                    Oingresos.creaIngreso(pan_p.Tag.ToString(), fecOp, OcatIn, Omone, Omonto, decimal.Parse(tx_tipcam.Text),
                                        Ocajd, tx_descrip.Text, corre);
                                    Oingresos.grabaIngreso(conn);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error en grabar el Ingreso");
                                    return;
                                }
                                insFilaEnDataG("LIM", fecOp.Substring(6, 4) + oFEgres.CDerecha("00000" + corre, 6));       // inserta el registro nuevo en la grilla
                                limpiaObj();
                                limpiaTE();
                            }
                            else
                            {
                                MessageBox.Show("Error en grabar los datos del ingreso", "No se completo la operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }
        private void graba_edicion()
        {
            if (true)
            {
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
                        string fecOp = selecFecha1.Value.Date.ToShortDateString();
                        Oingreso.creaIngreso(pan_p.Tag.ToString(), fecOp, OcatIn, Omone, Omonto, decimal.Parse(tx_tipcam.Text),
                                        Ocajd, tx_descrip.Text, tx_idOper.Text);
                        Oingreso.EditaIngreso(conn, tx_idOper.Text.Substring(0, 4), ("000000000" + oFEgres.CDerecha(tx_idOper.Text, 6)));
                        actFilaEnDataI(dt_grillaI, "LIM", tx_idOper.Text);
                    }
                }
            }
        }

    }
}
