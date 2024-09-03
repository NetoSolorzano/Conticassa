using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ADGV;
using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Conticassa
{
    public partial class Finan_Egres : Form1
    {
        // conexion a la base de datos
        string DB_CONN_STR = "server=" + Login.serv + ";port=" + Login.port + ";uid=" + Login.usua + ";pwd=" + Login.cont + ";database=" + Login.data +
            ";ConnectionLifeTime=" + Login.ctl + ";";
        // datos de la grilla
        internal DataTable dt_grillaE = new DataTable();
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
        giroConto Ogiro = new giroConto();                                          // Objeto giroconto
        //
        Ingresos Oingresos = new Ingresos();
        Egresos Oegreso = new Egresos();
        string nomForm = "";
        int diasAtroya = 0;                                                         // dias atras hasta donde mostrará la grilla
        int limCols = 1;                                                            // limite de columnas que muestra la grilla
        public Finan_Egres()
        {
            InitializeComponent();                  // inicializa los objetos graficos
            CargaINI(this);                         // colorea los objetos graficos
            CargaFormatos();                        // jala datos de combos y demas
            colorea(this, "#e35b46", "#f58f7f", "#fae0dc");    // pinta el mundo de colores!
            chk_giroC_CheckedChanged(null, null);   // 
            sololee("T");                           // T=todos los campos, "" ó "C" campos comunes
            jalainfo();                             // jala variables de tabla enlace
            initCampos();                           // pone maximos y upper case de campos texto
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
                            tx_dat_giro.Text = ayu2.ReturnValueA[0];
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
                            OcatEg.codigo = ayu2.ReturnValueA[0];
                            OcatEg.nombre = ayu2.ReturnValueA[1];
                            OcatEg.largo = ayu2.ReturnValueA[2];
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
                            tx_dat_provee.Text = ayu2.ReturnValueA[0];
                            tx_provee.Text = ayu2.ReturnValueA[1];
                            eti_nomprovee.Text = ayu2.ReturnValueA[1]; // 31/08/2024 ya no usamos
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
            // categorias
            acsc = new AutoCompleteStringCollection();
            Tx_catEgre.AutoCompleteCustomSource = acsc;
            Tx_catEgre.AutoCompleteMode = AutoCompleteMode.None;
            Tx_catEgre.AutoCompleteSource = AutoCompleteSource.CustomSource;
            DataRow[] depar = Program.dt_definic.Select("idtabella='CAM' and numero=1");
            acsc.Clear();
            foreach (DataRow row in depar)
            {
                acsc.Add(row["descrizionerid"].ToString().Trim().ToUpper());
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
                accd.Add(row["descrizionerid"].ToString().Trim().ToUpper());
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
                acgc.Add(row["descrizionerid"].ToString().Trim().ToUpper());
            }
            listBox3.Visible = false;
            // monedas
            depar = Program.dt_definic.Select("idtabella='MON' and numero=1");
            cmb_mon.DataSource = depar.CopyToDataTable();
            cmb_mon.DisplayMember = "descrizionerid";
            cmb_mon.ValueMember = "idcodice";
        }
        public void colorea(Form este, string fuerte, string normal, string suave)
        {
            este.BackColor = ColorTranslator.FromHtml(normal); // cuando usamos FromHtml NO da error por fondo transparente
            foreach (Control oControl in este.Controls)
            {
                if (oControl is TextBox)
                {
                    oControl.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                }
                if (oControl is MaskedTextBox)
                {
                    oControl.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                }
                if (oControl is Label)
                {
                    if (oControl.Name == "eti_tituloForm")
                    {
                        oControl.BackColor = ColorTranslator.FromHtml(fuerte); // cuando usamos FromHtml NO da error por fondo transparente
                    }
                    else
                    {
                        oControl.BackColor = ColorTranslator.FromHtml(normal); // cuando usamos FromHtml NO da error por fondo transparente
                    }
                }
                if (oControl is CheckBox)
                {
                    oControl.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                }
                if (oControl is RadioButton)
                {
                    oControl.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                }
                if (oControl is ListBox)
                {
                    oControl.Font = new System.Drawing.Font(conf.nombreFont, conf.tamañoFont);
                    oControl.ForeColor = System.Drawing.Color.FromName(conf.colorFont);
                    oControl.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                }
                if (oControl is ComboBox)
                {
                    oControl.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                }
                if (oControl is Panel)
                {
                    oControl.BackColor = ColorTranslator.FromHtml(suave);
                    foreach (Control control in oControl.Controls)
                    {
                        if (control is TextBox)
                        {
                            control.BackColor = ColorTranslator.FromHtml(suave); // Color.FromArgb(1, 186, 218, 169);
                        }
                        if (oControl is MaskedTextBox)
                        {
                            control.BackColor = ColorTranslator.FromHtml(suave); // Color.FromArgb(1, 186, 218, 169);
                        }
                        if (control is Label)
                        {
                            control.BackColor = ColorTranslator.FromHtml(normal); // Color.FromArgb(1, 186, 218, 169);
                        }
                        if (control is CheckBox)
                        {
                            control.BackColor = ColorTranslator.FromHtml(suave); // Color.FromArgb(1, 186, 218, 169);
                        }
                        if (control is RadioButton)
                        {
                            control.BackColor = ColorTranslator.FromHtml(suave); // Color.FromArgb(1, 186, 218, 169);
                        }
                        if (control is ListBox)
                        {
                            control.Font = new System.Drawing.Font(conf.nombreFont, conf.tamañoFont);
                            control.ForeColor = System.Drawing.Color.FromName(conf.colorFont);
                            oControl.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                        }
                        if (control is ComboBox)
                        {
                            control.BackColor = ColorTranslator.FromHtml(suave); // cuando usamos FromHtml NO da error por fondo transparente
                        }
                    }
                }
            }
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
            tx_idOper.Text = Oegreso.IdMovim;
            selecFecha1.Value = DateTime.Parse(Oegreso.FechOper);
            Tx_catEgre.Text = Oegreso.CatEgreso.nombre;
            eti_nomCat.Text = Oegreso.CatEgreso.largo;
            cmb_mon.SelectedValue = Oegreso.Moneda.codigo;
            tx_monto.Text = Oegreso.Monto.monOrige.ToString("#0.00");
            tx_tipcam.Text = Oegreso.TipCamb.ToString("#0.000");
            Tx_ctaDes.Text = Oegreso.CajaDes.nombre;
            eti_nomCaja.Text = Oegreso.CajaDes.largo;
            //tx_provee.Text = Oegreso.Proveedor.codigo;    
            tx_dat_provee.Text = Oegreso.Proveedor.codigo;
            tx_provee.Text = Oegreso.Proveedor.nombre;
            eti_nomprovee.Text = Oegreso.Proveedor.nombre;  // 31/08/2024 ya no es necesario
            tx_descrip.Text = Oegreso.Descrip;
        }                                                   // muestra en el formulario los objetos de la clase Egresos
        private void initCampos()
        {
            Tx_catEgre.MaxLength = 20;
            Tx_catEgre.CharacterCasing = CharacterCasing.Upper;
            Tx_ctaDes.MaxLength = 20;
            Tx_ctaDes.CharacterCasing = CharacterCasing.Upper;
            tx_ctaGiro.MaxLength = 20;
            tx_ctaGiro.CharacterCasing = CharacterCasing.Upper;
            tx_descrip.MaxLength = 100;
            tx_idOper.MaxLength = 15;
        }                                               // inicializa ancho de campos y upper case

        #region Botones de comando
        // acá falta todo el asunto de leer los permisos del usuario
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            escribe("");
            Tx_fecha.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            if (tx_tipcam.Text == "") tx_tipcam.Focus();
            else Tx_catEgre.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDICION";
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            //sololee("");
            escribe("EDICION");
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
            OcatEg.codigo = "";                                       // Objeto categoría de egreso
            OcatEg.nombre = "";
            OcatEg.largo = "";
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
            Ogiro.ctades = "";
            Ogiro.tipodes = "";
            Oegreso.limpia();
        }
        private void limpiaTE() // limpia textbox, etiquetas, combos
        {
            tx_idOper.Clear();
            Tx_catEgre.Clear();
            Tx_ctaDes.Clear();
            tx_ctaGiro.Clear();
            tx_descrip.Clear();
            tx_monto.Clear();
            tx_provee.Clear();
            tx_dat_provee.Clear();
            tx_tipcam.Clear();
            tx_dat_giro.Clear();
            //
            eti_nomCaja.Text = "";
            eti_nomCat.Text = "";
            eti_nomCtaGiro.Text = "";
            eti_nomprovee.Text = "";
            //
            cmb_mon.SelectedIndex = -1; // no debe ser cero 02/09/2024 porque el objeto moneda esta limpio
            chk_giroC.Checked = false;
        }
        private void escribe(string quien)  // pones los campos necesarios en readonly = false
        {
            if (quien == "EDICION") tx_idOper.ReadOnly = false;
            else tx_idOper.ReadOnly = true;
            Tx_catEgre.ReadOnly = false;
            Tx_ctaDes.ReadOnly = false;
            tx_ctaGiro.ReadOnly = false;
            tx_descrip.ReadOnly = false;
            tx_monto.ReadOnly = false;
            tx_provee.ReadOnly = true; // false; 31/08/2024 solo se jala con F1, no se puede validar por nombre
            tx_tipcam.ReadOnly = false;
            //
            cmb_mon.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            chk_datSimil.Enabled = true;
            chk_giroC.Enabled = true;
            cmb_mon.SelectedIndex = -1;
            cmb_mon_SelectedIndexChanged(null, null);
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
        private void chk_datSimil_CheckStateChanged(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (chk_datSimil.CheckState == CheckState.Checked)
                {
                    // Si los campos principales estan en blanco, jalamos el ultimo del dia,casa y tipo
                    if (Tx_catEgre.Text == "" && Tx_ctaDes.Text == "" && tx_provee.Text == "" )
                    {
                        // al 02/09/2024 no estoy seguro de hacer esto
                        jala_ultimo("EGRESO", ((rb_omg.Checked == true) ? rb_omg.Text : rb_pers.Text), Tx_fecha.Text);
                    }
                    else
                    {
                        // si los campos principales no estan en blanco, no jala nada 
                    }
                }

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
                OcatEg.codigo = nc[0].ItemArray[1].ToString();
                OcatEg.nombre = Tx_catEgre.Text;
                OcatEg.largo = eti_nomCat.Text;
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
                tx_dat_giro.Text = nc[0].ItemArray[1].ToString();
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
                        if (Vali_CAM(Tx_catEgre.Text) == false)
                        {
                            Tx_catEgre.Clear();
                            eti_nomCat.Text = "";
                            MessageBox.Show("No existe el nombre del egreso");
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
                        if (ValiCtaCon(Tx_ctaDes.Text) == false)
                        {
                            Tx_ctaDes.Clear();
                            eti_nomCaja.Text = "";
                            MessageBox.Show("No existe el nombre de la cuenta");
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
                    Oprove = ValiProvee(tx_dat_provee.Text);  // tx_provee.Text
                    if (Oprove.nombre == "")
                    {
                        eti_nomprovee.Text = "";
                        tx_provee.Text = "";
                        MessageBox.Show("No existe el código de proveedor");
                    }
                    else
                    {
                        tx_provee.Text = Oprove.nombre;
                        eti_nomprovee.Text = Oprove.nombre; // 31/08/2024 ya no lo usamos
                    }
                }
                else
                {
                    eti_nomprovee.Text = "";
                    tx_provee.Text = "";
                }
            }
        }
        private void tx_idOper_Validating(object sender, CancelEventArgs e)       // busca en toda la base de datos
        {
            if (tx_idOper.Text.Trim() != "" && ("NUEVO,EDICION").Contains(Tx_modo.Text))
            {
                string[] retu = ValiIdOper();
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
                        OcatEg.codigo = retu[18];               // IDCategoria
                        OcatEg.nombre = retu[4];                // EGRESO
                        OcatEg.largo = retu[23];                // "DET_EGRESO"
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
                        Ogiro.ctades = retu[10];                // 
                        Ogiro.tipodes = retu[11];   //(tx_ctaGiro.Text.Trim() == "") ? "" : (rb_omg.Checked == true) ? "OMG" : "PER";
                    }
                    else
                    {
                        // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                        //usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
                        fecOp = retu[2].Substring(0, 10);       // "FECHA"
                        OcatEg.codigo = retu[18];               // "IDCategoria"
                        OcatEg.nombre = retu[4];                // "EGRESO"
                        OcatEg.largo = retu[23];                // "DET_EGRESO"
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
                        Ogiro.ctades = retu[10];                // 
                        Ogiro.tipodes = retu[11];   // (tx_ctaGiro.Text.Trim() == "") ? "" : (rb_omg.Checked == true) ? "OMG" : "PER";
                    }
                    Oegreso.creaEgreso(pan_p.Tag.ToString(), fecOp, OcatEg, Omone, Omonto, tipca,
                            Ocajd, Oprove, descr, idmov, Ogiro);
                    jalaoc();
                }
            }
        }
        private void tx_monto_Validating(object sender, CancelEventArgs e)
        {
            decimal monti = 0; decimal cambi = 0;
            decimal.TryParse(tx_monto.Text, out monti);
            tx_monto.Text = Math.Round(monti, 2).ToString("#,##0.00");
            decimal.TryParse(tx_tipcam.Text, out cambi);
            if (Tx_modo.Text == "NUEVO" && monti > 0)
            {
                Omonto.monOrige = monti;
                if (true)
                {
                    calc_monedas(cmb_mon, monti, cambi);
                }
            }
        }
        private void tx_tipcam_Validating(object sender, CancelEventArgs e)
        {
            decimal monti = 0; decimal cambi = 0;
            decimal.TryParse(tx_monto.Text, out monti);
            decimal.TryParse(tx_tipcam.Text, out cambi);
            tx_tipcam.Text = Math.Round(cambi, 3).ToString("#0.000");
            if (Tx_modo.Text == "NUEVO" && monti > 0)
            {
                Omonto.monOrige = monti;
                if (true)
                {
                    calc_monedas(cmb_mon, monti, cambi);
                }
            }
        }
        public string[] ValiIdOper()
        {
            string[] retorna = { "", "", "", "", "", "", "", "", "", "",
                                "", "", "", "", "", "", "", "", "", "", 
                                "", "", "", ""};           // bool retorna = false
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
                                "ImportoDE,ImportoSE,ImportoDU,ImportoSU,Cambio,unMisura,Quantita,Chiusura,monori,ctaori,ctades,usuario,dia,idcassaomg,IDGiroConto,tipodesgiro AS GIRO_CTA " +
                                "FROM cassaomg WHERE CONCAT(Anno,RIGHT(IDMovimento,6))=@idm";
                        }
                        if (pan_p.Tag.ToString() == "personal")    // rb_pers.Checked == true
                        {
                            consulta = "select a.IDBanco AS CASA,CONCAT(a.Anno, RIGHT(a.IDMovimento, 6)) AS ID_MOVIM, DATE(a.DataMovimento) AS FECHA," +
                                "ifnull(dc.Descrizionerid, '') AS CUENTA, ifnull(ca.Descrizionerid, '') AS EGRESO, a.monori AS MONEDA,a.valorOrig AS MONTO,a.Descrizione AS DESCRIPCION," +
                                "a.Cambio AS TIP_CAMBIO,ifnull(af.ragionesociale, '') AS PROVEEDOR, a.tipodesgiro AS GIRO_CTA,a.IDGiroConto,IFNULL(gc.Descrizione, '') AS CTA_DESTINO," +
                                "a.usuario,a.dia,round(a.ImportoDU, 2) as ImportoDU,round(a.ImportoSU, 2) as ImportoSU," +
                                "a.idanagrafica,a.IDConto,a.IDCategoria,a.codimon,a.nombmon,a.TCMonOri,ifnull(dc.Descrizione, '') AS DET_CUENTA, ifnull(ca.Descrizione, '') AS DET_EGRESO " +
                                "from cassaconti a " +
                                "LEFT JOIN desc_con dc ON dc.IDCodice = a.IDConto " +
                                "LEFT JOIN desc_cam ca ON ca.IDCodice = a.IDCategoria " +
                                "LEFT JOIN anag_for af ON af.idanagrafica = a.IDAnagrafica " +
                                "LEFT JOIN desc_con gc ON gc.IDCodice = a.IDGiroConto " +
                                "LEFT JOIN desc_mon mo ON mo.IDCodice = a.monori " +
                                "WHERE CONCAT(Anno,RIGHT(a.IDMovimento,6))=@idm";
                            //consulta = "select IDBanco,CONCAT(Anno,RIGHT(IDMovimento,6)) AS IDMovimento,DataMovimento,IDConto,IDCategoria,ImportoDE,ImportoSE,ImportoDU," +
                            //    "ImportoSU,Cambio,Descrizione,IDGiroConto,monori,ctaori,ctades,usuario,dia,idanagrafica,idcassaconti,tipodesgiro " +
                            //    "from cassaconti WHERE CONCAT(Anno,RIGHT(IDMovimento,6))=@idm";
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
                                            //retorna = true;
                                            retorna[0] = dr["CASA"].ToString();
                                            retorna[1] = dr["ID_MOVIM"].ToString();
                                            retorna[2] = dr["FECHA"].ToString();
                                            retorna[3] = dr["CUENTA"].ToString();
                                            retorna[4] = dr["EGRESO"].ToString();
                                            retorna[5] = dr["MONEDA"].ToString();
                                            retorna[6] = dr["MONTO"].ToString();
                                            retorna[7] = dr["DESCRIPCION"].ToString();
                                            retorna[8] = dr["TIP_CAMBIO"].ToString();
                                            retorna[9] = dr["PROVEEDOR"].ToString();
                                            retorna[10] = dr["GIRO_CTA"].ToString();        // tipo cta destino OMG o PER
                                            retorna[11] = dr["IDGiroConto"].ToString();     // cuenta destino del giro
                                            retorna[12] = dr["CTA_DESTINO"].ToString();
                                            retorna[13] = dr["usuario"].ToString();
                                            retorna[14] = dr["ImportoDU"].ToString();
                                            retorna[15] = dr["ImportoSU"].ToString();
                                            retorna[16] = dr["idanagrafica"].ToString();
                                            retorna[17] = dr["IDConto"].ToString();
                                            retorna[18] = dr["IDCategoria"].ToString(); 
                                            retorna[19] = dr["codimon"].ToString();
                                            retorna[20] = dr["nombmon"].ToString();
                                            retorna[21] = dr["TCMonOri"].ToString();
                                            retorna[22] = dr["DET_CUENTA"].ToString();
                                            retorna[23] = dr["DET_EGRESO"].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // limpiamos ???  no, no, limpiamos en el llamador de la funcion
                }
            }
            return retorna;
        }                                           // valida idOper, si hay jala datos, sino No
        public provees ValiProvee(string idAnag)
        {
            provees retona = new provees();
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (MySqlCommand micon = new MySqlCommand("select ragionesociale from anag_for where TRIM(idanagrafica)=@codi", conn))
                        {
                            micon.Parameters.AddWithValue("@codi", idAnag.Trim());
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                if (dr.HasRows == true)
                                {
                                    if (dr.Read())
                                    {
                                        if (dr[0] != null && dr[0].ToString() != "")
                                        {
                                            retona.codigo = idAnag;
                                            retona.nombre = dr[0].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    retona.codigo = "";
                                    retona.nombre = "";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    retona.codigo = "";
                    retona.nombre = "";
                }
            }
            return retona;
        }                               // valida existencia del proveedor
        public bool ValiCtaCon(string _nombre)
        {
            // validamos la existencia del nombre en ... descrizionerid
            bool retorna = false;
            DataRow[] row = Program.dt_definic.Select("idtabella='CON' and descrizionerid='" + _nombre + "'");
            foreach (DataRow dat in row)
            {
                retorna = true;
            }
            return retorna;
        }                                 // valida existencia de la cuenta destino
        public bool Vali_CAM(string _nombre)
        {
            // validamos la existencia del nombre en ... descrizionerid
            bool retorna = false;
            DataRow[] row = Program.dt_definic.Select("descrizionerid='" + _nombre + "' and idtabella='CAM'");
            foreach (DataRow dat in row)
            {
                retorna = true;
            }
            return retorna;
        }
        private void tx_tipcam_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION")
            {
                tx_idOper.Focus();
            }
        }
        private void Tx_fecha_Click(object sender, EventArgs e)
        {
            var mtb = (MaskedTextBox)sender;
            mtb.Select(0, 0);
            mtb.Focus();
        }
        private void selecFecha1_ValueChanged(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDICION")
            {
                Tx_fecha.Text = selecFecha1.Value.Date.ToString("dd/MM/yyyy");
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
                if (tx_monto.Text != "" && tx_tipcam.Text != "") calc_monedas(cmb_mon, decimal.Parse(tx_monto.Text), decimal.Parse(tx_tipcam.Text));
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

        #region datagridview - Grilla
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
                                dt_grillaE.Clear();
                                dt_grillaE.Columns.Clear();
                                da.Fill(dt_grillaE);
                                advancedDataGridView1.DataSource = dt_grillaE;
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
                        armaGrilla(advancedDataGridView1, limCols);      // cuadramos las columnas de la grilla
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión al servidor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
        }            // muestra datos de la fecha actual hasta <dAtras> días atras 
        private void armaGrilla(AdvancedDataGridView dgv_, int filasLim) // DataGridView dgv_, int filasLim
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
        }                 // ajusta el ancho de las columnas y muestra hasta el limite
        private void advancedadvancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Tx_modo.Text != "NUEVO")
            {
                string fecOp = "";              // fecha de operacion
                decimal tipca = 0;              // tip cambio del monto origen
                string descr = "";              // descripcion de la operacion
                string idmov = "";              // id del movimiento
                if (rb_omg.Checked == true)
                {
                    // CASA,ID_MOVIM,FECHA,DESTINO,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,idgiroconto,CTA_DESTINO,
                    // usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDDestino,IDCategoria,tipodesgiro
                    fecOp = advancedDataGridView1.Rows[e.RowIndex].Cells["FECHA"].Value.ToString().Substring(0, 10);
                    OcatEg.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["IDCategoria"].Value.ToString();
                    OcatEg.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["EGRESO"].Value.ToString();
                    OcatEg.largo = advancedDataGridView1.Rows[e.RowIndex].Cells["DET_EGRESO"].Value.ToString();
                    Omone.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omone.siglas = advancedDataGridView1.Rows[e.RowIndex].Cells["MONEDA"].Value.ToString();
                    Omone.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["nombmon"].Value.ToString();
                    Omonto.codMOrige = advancedDataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omonto.monOrige = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["MONTO"].Value.ToString());
                    Omonto.tipCOri = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Omonto.monDolar = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["ImportoDU"].Value.ToString());
                    Omonto.tipCDol = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["TIP_CAMBIO"].Value.ToString());
                    Omonto.monSoles = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["ImportoSU"].Value.ToString());
                    tipca = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Ocajd.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["IDDestino"].Value.ToString();
                    Ocajd.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["DESTINO"].Value.ToString();
                    Ocajd.largo = advancedDataGridView1.Rows[e.RowIndex].Cells["DET_DESTINO"].Value.ToString();
                    Oprove.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["idanagrafica"].Value.ToString();
                    Oprove.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["PROVEEDOR"].Value.ToString();
                    descr = advancedDataGridView1.Rows[e.RowIndex].Cells["DESCRIPCION"].Value.ToString();
                    idmov = advancedDataGridView1.Rows[e.RowIndex].Cells["ID_MOVIM"].Value.ToString();
                    Ogiro.ctades = advancedDataGridView1.Rows[e.RowIndex].Cells["IDGiroConto"].Value.ToString();
                    Ogiro.tipodes = advancedDataGridView1.Rows[e.RowIndex].Cells["tipodesgiro"].Value.ToString();
                }
                else
                {
                    // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                    //usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri,tipodesgiro
                    fecOp = advancedDataGridView1.Rows[e.RowIndex].Cells["FECHA"].Value.ToString().Substring(0, 10);
                    OcatEg.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["IDCategoria"].Value.ToString();
                    OcatEg.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["EGRESO"].Value.ToString();
                    OcatEg.largo = advancedDataGridView1.Rows[e.RowIndex].Cells["DET_EGRESO"].Value.ToString();
                    Omone.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omone.siglas = advancedDataGridView1.Rows[e.RowIndex].Cells["MONEDA"].Value.ToString();
                    Omone.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["nombmon"].Value.ToString();
                    Omonto.codMOrige = advancedDataGridView1.Rows[e.RowIndex].Cells["codimon"].Value.ToString();
                    Omonto.monOrige = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["MONTO"].Value.ToString());
                    Omonto.tipCOri = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Omonto.monDolar = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["ImportoDU"].Value.ToString());
                    Omonto.tipCDol = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["TIP_CAMBIO"].Value.ToString());
                    Omonto.monSoles = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["ImportoSU"].Value.ToString());
                    tipca = decimal.Parse(advancedDataGridView1.Rows[e.RowIndex].Cells["TCMonOri"].Value.ToString());
                    Ocajd.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["IDConto"].Value.ToString();
                    Ocajd.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["CUENTA"].Value.ToString();
                    Ocajd.largo = advancedDataGridView1.Rows[e.RowIndex].Cells["DET_CUENTA"].Value.ToString();
                    Oprove.codigo = advancedDataGridView1.Rows[e.RowIndex].Cells["idanagrafica"].Value.ToString();
                    Oprove.nombre = advancedDataGridView1.Rows[e.RowIndex].Cells["PROVEEDOR"].Value.ToString();
                    descr = advancedDataGridView1.Rows[e.RowIndex].Cells["DESCRIPCION"].Value.ToString();
                    idmov = advancedDataGridView1.Rows[e.RowIndex].Cells["ID_MOVIM"].Value.ToString();
                    Ogiro.ctades = advancedDataGridView1.Rows[e.RowIndex].Cells["IDGiroConto"].Value.ToString();
                    Ogiro.tipodes = advancedDataGridView1.Rows[e.RowIndex].Cells["tipodesgiro"].Value.ToString();
                }
                Oegreso.creaEgreso(pan_p.Tag.ToString(), fecOp, OcatEg, Omone, Omonto, tipca,
                        Ocajd, Oprove, descr, idmov, Ogiro);
                jalaoc();
            }
        }
        private void insFilaEnDataG(string _casa, string _corre)
        {
            DataRow fila = dt_grillaE.NewRow();
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
                fila["EGRESO"] = OcatEg.nombre;     // nombre categoria egreso
                fila["MONEDA"] = Omone.siglas;      // siglas moneda origen
                fila["MONTO"] = Omonto.monOrige;    // valor origen
                fila["DESCRIPCION"] = tx_descrip.Text;
                fila["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                fila["PROVEEDOR"] = Oprove.nombre;
                fila["GIRO_CTA"] = "";
                fila["idgiroconto"] = "";
                fila["CTA_DESTINO"] = "";
                fila["usuario"] = Program.vg_user;
                //fila["dia"] = "";
                fila["ImportoDU"] = Omonto.monDolar;
                fila["ImportoSU"] = Omonto.monSoles;
                fila["idanagrafica"] = Oprove.codigo;
                fila["IDDestino"] = Ocajd.codigo;
                fila["IDCategoria"] = OcatEg.codigo;
            }
            if (rb_pers.Checked == true)
            {
                // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                // usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
                fila["CASA"] = _casa;
                fila["ID_MOVIM"] = _corre;
                fila["FECHA"] = fecOp;
                fila["CUENTA"] = Ocajd.nombre;
                fila["EGRESO"] = OcatEg.nombre;
                fila["MONEDA"] = Omone.siglas;
                fila["MONTO"] = Omonto.monOrige;
                fila["DESCRIPCION"] = tx_descrip.Text;
                fila["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                fila["PROVEEDOR"] = Oprove.nombre;
                fila["GIRO_CTA"] = "";
                fila["IDGiroConto"] = "";
                fila["CTA_DESTINO"] = "";
                fila["usuario"] = Program.vg_user;
                //fila["dia"] = "";
                fila["ImportoDU"] = Omonto.monDolar;
                fila["ImportoSU"] = Omonto.monSoles;
                fila["idanagrafica"] = Oprove.codigo;
                fila["IDConto"] = Ocajd.codigo;
                fila["IDCategoria"] = OcatEg.codigo;
                fila["codimon"] = Omone.codigo;
                fila["nombmon"] = Omone.nombre;
                fila["TCMonOri"] = Omonto.tipCOri;
            }
            dt_grillaE.Rows.InsertAt(fila, 0); //.Add(fila);
        }                // INSERTA en la grilla el registro nuevo despues de grabar en la B.D.
        public void actFilaEnDataG(DataTable dt, string _casa, string _corre)
        {
            string fecOp = selecFecha1.Value.Date.ToShortDateString();
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                if (dr["ID_MOVIM"].ToString() == (_corre.Substring(0, 4) + CDerecha(_corre, 6)))
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
                        dr["EGRESO"] = OcatEg.nombre;     // nombre categoria egreso
                        dr["MONEDA"] = Omone.siglas;      // siglas moneda origen
                        dr["MONTO"] = Omonto.monOrige;    // valor origen
                        dr["DESCRIPCION"] = tx_descrip.Text;
                        dr["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                        dr["PROVEEDOR"] = Oprove.nombre;
                        dr["GIRO_CTA"] = "";
                        dr["idgiroconto"] = "";
                        dr["CTA_DESTINO"] = "";
                        dr["usuario"] = Program.vg_user;
                        //dr["dia"] = "";
                        dr["ImportoDU"] = Omonto.monDolar;
                        dr["ImportoSU"] = Omonto.monSoles;
                        dr["idanagrafica"] = Oprove.codigo;
                        dr["IDDestino"] = Ocajd.codigo;
                        dr["IDCategoria"] = OcatEg.codigo;
                    }
                    if (rb_pers.Checked == true)
                    {
                        // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
                        // usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
                        dr["CASA"] = _casa;
                        dr["ID_MOVIM"] = _corre;
                        dr["FECHA"] = fecOp;
                        dr["CUENTA"] = Ocajd.nombre;
                        dr["EGRESO"] = OcatEg.nombre;
                        dr["MONEDA"] = Omone.siglas;
                        dr["MONTO"] = Omonto.monOrige;
                        dr["DESCRIPCION"] = tx_descrip.Text;
                        dr["TIP_CAMBIO"] = decimal.Parse(tx_tipcam.Text);
                        dr["PROVEEDOR"] = Oprove.nombre;
                        dr["GIRO_CTA"] = "";
                        dr["IDGiroConto"] = "";
                        dr["CTA_DESTINO"] = "";
                        dr["usuario"] = Program.vg_user;
                        //dr["dia"] = "";
                        dr["ImportoDU"] = Omonto.monDolar;
                        dr["ImportoSU"] = Omonto.monSoles;
                        dr["idanagrafica"] = Oprove.codigo;
                        dr["IDConto"] = Ocajd.codigo;
                        dr["IDCategoria"] = OcatEg.codigo;
                        dr["codimon"] = Omone.codigo;
                        dr["nombmon"] = Omone.nombre;
                        dr["TCMonOri"] = Omonto.tipCOri;
                    }
                    dr.AcceptChanges();
                }
            }
            dt.AcceptChanges();
        }                // ACTUALIZA la grilla despues de haber actualizado la tabla
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            DataTable dtg = (DataTable)advancedDataGridView1.DataSource;
            dtg.DefaultView.Sort = advancedDataGridView1.SortString;
        }
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            DataTable dtg = (DataTable)advancedDataGridView1.DataSource;
            dtg.DefaultView.RowFilter = advancedDataGridView1.FilterString;
        }
        #endregion

        #region botones Grabar, nuevo prov.
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
                var aaa = MessageBox.Show("Confirma que desea EDITAR el Egreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    graba_edicion(dt_grillaE);
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
                var aaa = MessageBox.Show("Confirma que desea BORRAR el Egreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    string tabla = "";
                    if (rb_omg.Checked == true) tabla = "cassaomg";
                    else tabla = "cassaconti";
                    graba_borrar(tabla, selecFecha1.Value.Year.ToString(), "000000000" + CDerecha(tx_idOper.Text, 6), dt_grillaE);
                    limpiaObj();
                    limpiaTE();
                }
            }
        }
        private void graba_nuevo()
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
            if (tx_tipcam.Text.Trim() == "0" || tx_tipcam.Text.Trim() == "")
            {
                errorProvider1.SetIconAlignment(tx_tipcam, ErrorIconAlignment.TopLeft);
                errorProvider1.SetError(tx_tipcam, "Debe ingresar el tipo de cambio");
                tx_tipcam.Focus();
                return;
            }
            errorProvider1.SetError(tx_tipcam, "");
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
            if (chk_giroC.CheckState == CheckState.Checked)
            {
                if (tx_ctaGiro.Text.Trim() == "")
                {
                    MessageBox.Show("Debe ingresar la cuenta destino del giro","Atención",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    tx_ctaGiro.Focus();
                    return;
                }
            }
            var aaa = MessageBox.Show("Confirma que desea crear el Egreso?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                        if (true)   // Tx_modo.Text == "NUEVO"
                        {
                            string corre = correlativo(conn, ((rb_omg.Checked == true) ? "MCA" : "MCO"), selecFecha1.Value.Date.Year);
                            if (corre != "error" && corre != "")
                            {
                                try
                                {
                                    Oegresos.creaEgreso(pan_p.Tag.ToString(), fecOp, OcatEg, Omone, Omonto, decimal.Parse(tx_tipcam.Text),
                                        Ocajd, Oprove, tx_descrip.Text, corre, Ogiro);
                                    Oegresos.grabaEgreso(conn);
                                    // si esta marcado el giro, hacemos el movimiento inverso
                                    if (chk_giroC.CheckState == CheckState.Checked)
                                    {
                                        catIngresos OcatIn = new catIngresos();
                                        OcatIn.codigo = OcatEg.codigo;
                                        OcatIn.nombre = OcatEg.nombre;
                                        OcatIn.largo = OcatEg.largo;
                                        cajDestino _desgiro = new cajDestino();
                                        _desgiro.codigo = tx_dat_giro.Text;
                                        _desgiro.nombre = tx_ctaGiro.Text;
                                        _desgiro.largo = eti_nomCtaGiro.Text;
                                        corre = correlativo(conn, ((rb_omg.Checked == true) ? "MCA" : "MCO"), selecFecha1.Value.Date.Year);
                                        Oingresos.creaIngreso(pan_p.Tag.ToString(), fecOp, OcatIn, Omone, Omonto, decimal.Parse(tx_tipcam.Text),
                                        _desgiro, tx_descrip.Text, corre, Ogiro);
                                        Oingresos.grabaIngreso(conn); 
                                        OcatIn.codigo = "";
                                        OcatIn.nombre = "";
                                        OcatIn.largo = "";
                                        _desgiro.codigo = "";
                                        _desgiro.nombre = "";
                                        _desgiro.largo = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error en grabar Egreso");
                                    return;
                                }   // CONCAT(a.Anno, RIGHT(a.IDMovimento, 6))
                                insFilaEnDataG("LIM", fecOp.Substring(6, 4) + CDerecha("00000" + corre, 6));       // inserta el registro nuevo en la grilla
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
                if (chk_datSimil.CheckState == CheckState.Checked)
                {
                    jala_ultimo("EGRESO", ((rb_omg.Checked == true) ? rb_omg.Text : rb_pers.Text), Tx_fecha.Text);
                }
            }
        }
        public void graba_edicion(DataTable dgv)
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
                        Oegreso.creaEgreso(pan_p.Tag.ToString(), fecOp, OcatEg, Omone, Omonto, decimal.Parse(tx_tipcam.Text),
                                        Ocajd, Oprove, tx_descrip.Text, tx_idOper.Text, Ogiro);
                        Oegreso.EditaEgreso(conn, tx_idOper.Text.Substring(0, 4), ("000000000" + CDerecha(tx_idOper.Text, 6)));
                        actFilaEnDataG(dt_grillaE, "LIM", tx_idOper.Text);
                    }
                }
            }
        }
        public void graba_borrar(string tabla, string year, string idmov, DataTable dgv)
        {
            if (true)
            {
                // borra en la tabla
                // borra en la grilla
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
                        string consulta = "delete from " + tabla + " where anno=@year and idmovimento=@corre";
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.Parameters.AddWithValue("@year", year);
                            micon.Parameters.AddWithValue("@corre", idmov);
                            micon.ExecuteNonQuery();
                        }
                        for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dgv.Rows[i];
                            if (dr["ID_MOVIM"].ToString() == (year + CDerecha(idmov, 6)))
                                dr.Delete();
                        }
                        dgv.AcceptChanges();
                    }
                }
            }
        }
        private void bt_Pnuevo_Click(object sender, EventArgs e)
        {
            // me quede aca 31/08/2024
            if (Tx_modo.Text == "NUEVO" && bt_Pnuevo.Enabled == true)
            {
                string para1 = "provee";
                string para2 = "";
                string para3 = "";
                string para4 = "";    // todos | activos
                ayuda1 ayu1 = new ayuda1(para1, para2, para3, para4);
                var result = ayu1.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    if (!string.IsNullOrEmpty(ayu1.ReturnValue1))
                    {
                        tx_dat_provee.Text = ayu1.ReturnValue0;
                        tx_provee.Text = ayu1.ReturnValue1;
                        eti_nomprovee.Text = ayu1.ReturnValue1; // 31/08/2024 ya no usamos
                        SendKeys.Send("{Tab}");
                    }
                }
            }
        }

        #endregion

        public montos calc_monedas(ComboBox combo, decimal valOri, decimal tipCam)
        {
            if (valOri <= 0) return Omonto;
            if (tipCam <= 0) return Omonto;
            if (combo.SelectedValue == null) return Omonto;
            Omonto.codMOrige = combo.SelectedValue.ToString();              // codigo de la moneda
            Omonto.monOrige = valOri;
            if (combo.SelectedValue.ToString() == "MON001") // Soles
            {
                Omonto.monSoles = valOri;
                Omonto.tipCDol = tipCam;
                Omonto.monDolar = Math.Round((valOri / tipCam), 2);
                Omonto.tipCOri = tipCam;
            }
            if (combo.SelectedValue.ToString() == "MON002") // Dolares
            {
                Omonto.tipCDol = tipCam;
                Omonto.monDolar = valOri;
                Omonto.monSoles = Math.Round((valOri * tipCam), 2);
                Omonto.tipCOri = tipCam;
            }
            if (combo.SelectedValue.ToString() == "MON003") // Euros
            {
                Omonto.tipCDol = 0;
                Omonto.monEuros = valOri;
                Omonto.tipCOri = tipCam;
                Omonto.monSoles = Math.Round((valOri * tipCam), 2);
            }
            return Omonto;
        }
        public string correlativo(MySqlConnection conn, string idcont, int year)
        {
            string retorna = "";
            int contador = 0;
            string consulta = "select numero from contatori where idbanco='LIM' and anno=@year and idcontatore=@idcont";
            using (MySqlCommand micon = new MySqlCommand(consulta, conn))
            {
                micon.Parameters.AddWithValue("@year", year);
                micon.Parameters.AddWithValue("@idcont", idcont);
                using (MySqlDataReader dr = micon.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            contador = dr.GetInt32(0) + 1;
                            retorna = CDerecha("00000000000000" + contador.ToString(), 15);
                        }
                    }
                    else
                    {
                        retorna = "error";
                    }
                }
            }
            if (retorna != "error" && retorna != "")
            {
                using (MySqlCommand micon = new MySqlCommand("update contatori set numero=@contador where idbanco='LIM' and anno=@year and idcontatore=@idcont", conn))
                {
                    micon.Parameters.AddWithValue("@year", year);
                    micon.Parameters.AddWithValue("@idcont", idcont);
                    micon.Parameters.AddWithValue("@contador", contador);
                    micon.ExecuteNonQuery();
                }
            }
            return retorna;
        }
        public string CDerecha(string sValue, int iMaxLength)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }
            else if (sValue.Length > iMaxLength)
            {
                sValue = sValue.Substring(sValue.Length - iMaxLength, iMaxLength);
            }
            return sValue;
        }                  // devuelve los ultimos n caractares desde la derecha
        private string[] jala_ultimo(string tipo, string tcuenta, string fecha)
        {
            // los datos deben jalarse de la grilla actual porque ahi estan los datos! 
            string [] retorna = new string[2];

            DataRow[] row = dt_grillaE.Select("FECHA='" + Tx_fecha.Text + "'", "ID_MOVIM DESC");

            // CASA,ID_MOVIM,FECHA,CUENTA,EGRESO,MONEDA,MONTO,DESCRIPCION,TIP_CAMBIO,PROVEEDOR,GIRO_CTA,a.IDGiroConto,CTA_DESTINO,
            //usuario,dia,ImportoDU,ImportoSU,idanagrafica,IDConto,IDCategoria,codimon,nombmon,TCMonOri
            DataRow[] cam = Program.dt_definic.Select("idcodice='" + row[0].ItemArray[19].ToString() + "' and idtabella='CAM'");
            OcatEg.codigo = row[0].ItemArray[19].ToString();
            eti_nomCat.Text = cam[0].ItemArray[3].ToString();
            OcatEg.nombre = cam[0].ItemArray[3].ToString();
            OcatEg.largo = cam[0].ItemArray[2].ToString();
            Tx_catEgre.Text = OcatEg.nombre;
            
            cam = Program.dt_definic.Select("idcodice='" + row[0].ItemArray[18].ToString() + "' and idtabella='CON'");
            eti_nomCaja.Text = cam[0].ItemArray[3].ToString();
            Ocajd.codigo = row[0].ItemArray[18].ToString();
            Ocajd.nombre = cam[0].ItemArray[3].ToString();
            Ocajd.largo = cam[0].ItemArray[2].ToString();
            Tx_ctaDes.Text = Ocajd.nombre;

            tx_dat_provee.Text = row[0].ItemArray[17].ToString();
            if (tx_dat_provee.Text != "")
            {
                Tx_provee_Leave(null, null);

            }
            return retorna;
        }   // jala el ultimo registro OMG/Personal, Egreso/Ingreso, Fecha
    }
}
