using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Conticassa
{
    public partial class Finan_camion : Form1
    {
        // conexion a la base de datos
        string DB_CONN_STR = "server=" + Login.serv + ";port=" + Login.port + ";uid=" + Login.usua + ";pwd=" + Login.cont + ";database=" + Login.data +
            ";ConnectionLifeTime=" + Login.ctl + ";";
        // datos de la grilla
        internal DataTable dt_grillaE = new DataTable();
        //
        publicoConf conf = new publicoConf();
        AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();     // placas
        AutoCompleteStringCollection accd = new AutoCompleteStringCollection();     // ctas destino
        cajDestino Ocajd = new cajDestino();                                        // Objeto cada de destino - desde donde sale el dinero
        montos Omonto = new montos();                                               // Objeto monto
        monedas Omone = new monedas();
        Finan_Egres oFEgres = new Finan_Egres();
        string nomForm = "";
        int diasAtroya = 0;                                                         // dias atras hasta donde mostrará la grilla
        int limCols = 1;                                                            // limite de columnas que muestra la grilla

        public Finan_camion()
        {
            InitializeComponent();
            CargaINI(this);                         // colorea los objetos graficos
            CargaFormatos();                        // jala datos de combos y demas
            oFEgres.colorea(this, "#caf44d", "#d9f684", "#ecf8c8");    // pinta el mundo de colores!
            sololee("T");                           // T=todos los campos, "" ó "C" campos comunes
            jalainfo();                             // jala variables de tabla enlace
            initCampos();                           // pone maximos y upper case de campos texto

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
                /* if (Tx_ctaDes.Focused == true)
                {
                    para1 = "omg";  // : "personal";
                    para2 = "cuenta";
                    para3 = "activos";    // todos | activos
                    ayuda2 ayu2 = new ayuda2(para1, para2, para3, para4);
                    var result = ayu2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        if (!string.IsNullOrEmpty(ayu2.ReturnValue1))   // 0=codigo, 1=descripCorta, 2=descripLarga
                        {
                            Tx_ctaDes.Text = ayu2.ReturnValueA[1];
                            //eti_nomCaja.Text = ayu2.ReturnValueA[2];
                        }
                    }
                } */
                return true;    // indicate that you handled this keystroke
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }    // F1 

        private void jalaoc()
        {
            /*
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
            */
        }                                                   // muestra en el formulario los objetos de la clase Egresos
        private void CargaFormatos()
        {
            // cuenta destino
            accd = new AutoCompleteStringCollection();
            /*
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
            // monedas
            depar = Program.dt_definic.Select("idtabella='MON' and numero=1");
            cmb_mon.DataSource = depar.CopyToDataTable();
            cmb_mon.DisplayMember = "descrizionerid";
            cmb_mon.ValueMember = "idcodice";
            */
        }
        private void jalainfo()
        {
            nomForm = this.Name;
            DataRow[] row = Program.dt_enlaces.Select("formulario='" + nomForm + "' and campo='grillas' and param='diasAtras'");
            diasAtroya = int.Parse(row[0]["valor"].ToString());
            row = Program.dt_enlaces.Select("formulario='" + nomForm + "' and campo='grillas' and param='limCols'");
            limCols = int.Parse(row[0]["valor"].ToString());
        }
        private void initCampos()
        {
            /*
            Tx_catIngre.MaxLength = 20;
            Tx_catIngre.CharacterCasing = CharacterCasing.Upper;
            Tx_ctaDes.MaxLength = 20;
            Tx_ctaDes.CharacterCasing = CharacterCasing.Upper;
            tx_ctaGiro.MaxLength = 20;
            tx_ctaGiro.CharacterCasing = CharacterCasing.Upper;
            tx_descrip.MaxLength = 100;
            tx_idOper.MaxLength = 15;
            */
        }                                               // inicializa ancho de campos y upper case

        #region limpiadores, readonlys
        private void limpiaObj()
        {
            Omone.codigo = "";                                        // Objeto moneda
            Omone.nombre = "";
            Omone.siglas = "";
            Ocajd.codigo = "";                                        // Objeto cada de destino - desde donde sale el dinero
            Ocajd.nombre = "";
            Ocajd.largo = "";
            Omonto.codMOrige = "";                                    // Objeto monto
            Omonto.monDolar = 0;
            Omonto.monEuros = 0;
            Omonto.monOrige = 0;
            Omonto.monSoles = 0;
            Omonto.tipCDol = 0;
            Omonto.tipCOri = 0;
        }
        private void limpiaTE() // limpia textbox, etiquetas, combos
        {
            /*
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
            cmb_mon.SelectedIndex = -1; // no puede ser 0 porque el objeto moneda esta limpio 02/09/2024
            */
        }
        private void escribe(string quien)  // pones los campos necesarios en readonly = false
        {
            /*
            if (quien == "EDICION") tx_idOper.ReadOnly = true;
            Tx_fecha.ReadOnly = false;
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
            */
        }
        private void sololee(string quien)  //    // T=todos los campos, "" ó "C" campos comunes
        {
            /*
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
            */
        }
        #endregion

        #region Botones de comando
        // acá falta todo el asunto de leer los permisos del usuario
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            /*
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            escribe("");
            Tx_fecha.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            if (tx_tipcam.Text == "") tx_tipcam.Focus();
            else Tx_catIngre.Focus();
            */
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDICION";
            /*
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            escribe("EDICION");    // sololee("")
            pan_p.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            tx_idOper.Focus();
            */
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "BORRAR";
            /*
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            sololee("");
            pan_p.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            tx_idOper.Focus();
            */
        }
        private void Bt_ver_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "VISUALIZAR";
            /*
            rb_pers.Checked = true;
            rb_pers_Click(null, null);
            sololee("");
            pan_p.Enabled = true;
            rb_omg.Enabled = true;
            rb_pers.Enabled = true;
            tx_idOper.Focus();
            */
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
    }
}
