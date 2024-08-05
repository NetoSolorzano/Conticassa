using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Finan_Ingres()
        {
            InitializeComponent();
            CargaFormatos();
            chk_giroC_CheckedChanged(null, null);
            sololee("T");   // T=todos los campos, "" ó "C" campos comunes
            jalainfo();
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
            this.BackColor = Color.FromArgb(1, 150, 174, 101); // rgba(150, 174, 101, 0.8)
            pan_p.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE);
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

    }
}
