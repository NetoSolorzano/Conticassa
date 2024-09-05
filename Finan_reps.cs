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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Conticassa
{
    public partial class Finan_reps : Form1
    {
        Finan_Egres oFEgres = new Finan_Egres();
        DataTable dt_ctaPer = new DataTable();      // cuentas personales
        DataTable dt_ctaOmg = new DataTable();      // cuentas omg
        DataTable dt_provee = new DataTable();      // proveedores

        // conexion a la base de datos
        string DB_CONN_STR = "server=" + Login.serv + ";port=" + Login.port + ";uid=" + Login.usua + ";pwd=" + Login.cont + ";database=" + Login.data +
            ";ConnectionLifeTime=" + Login.ctl + ";";

        public Finan_reps()
        {
            InitializeComponent();
            oFEgres.colorea(this, "#38c497", "#7de4c3", "#dff5ee");   // pinta el mundo de colores
            cargaDatos();                                             // carga data de los combos
            pan_menu.Enabled = false;
            pan_repos.Enabled = false;

        }
        private void Finan_reps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        #region combos
        private void cargaDatos()
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
                    // monedas
                    DataRow[] depar = Program.dt_definic.Select("idtabella='MON' and numero=1");
                    cmb_moneda.DataSource = depar.CopyToDataTable();
                    cmb_moneda.DisplayMember = "descrizionerid";
                    cmb_moneda.ValueMember = "idcodice";

                    // casa o sede
                    cmb_sede.Items.Add("OMG");
                    cmb_sede.Items.Add("PER");

                    // cta personal
                    dt_ctaPer.Columns.Add("idcodice");
                    dt_ctaPer.Columns.Add("descrizionerid");
                    DataRow[] _pers = Program.dt_definic.Select("idtabella='CON' and numero=1");
                    foreach (DataRow row in _pers)
                    {
                        dt_ctaPer.Rows.Add(row.ItemArray[1].ToString(), row.ItemArray[3].ToString());
                    }

                    // cta omg
                    dt_ctaOmg.Columns.Add("idcodice");
                    dt_ctaOmg.Columns.Add("descrizionerid");
                    DataRow[] _omgs = Program.dt_definic.Select("idtabella='DES' and numero=1");
                    foreach (DataRow row in _omgs)
                    {
                        dt_ctaOmg.Rows.Add(row.ItemArray[1].ToString(), row.ItemArray[3].ToString());
                    }

                    // proveedor
                    using (MySqlCommand micon = new MySqlCommand("select trim(idanagrafica) as idanagrafica,trim(ragionesociale) as ragionesociale from anag_for", conn))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            da.Fill(dt_provee);
                            cmb_prov.DataSource = dt_provee;
                            cmb_prov.DisplayMember = "ragionesociale";
                            cmb_prov.ValueMember = "idanagrafica";
                        }
                    }
                    // categorias de egreso/ingreso
                    DataRow[] cate = Program.dt_definic.Select("idtabella='CAM' and numero=1");
                    cmb_categ.DataSource = cate.CopyToDataTable();
                    cmb_categ.DisplayMember = "descrizionerid";
                    cmb_categ.ValueMember = "idcodice";
                }
            }
        }
        private void cmb_sede_SelectedValueChanged(object sender, EventArgs e)
        {
            /* if (cmb_sede.SelectedValue != null)
            {
                if (cmb_sede.SelectedValue.ToString() == "OMG")
                {
                    cmb_destin.DataSource = dt_ctaOmg;
                }
                if (cmb_sede.SelectedValue.ToString() == "PER")
                {
                    cmb_destin.DataSource = dt_ctaPer;
                }
            } */
        }
        private void cmb_sede_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_sede.SelectedIndex > -1)
            {
                if (cmb_sede.Text == "OMG")
                {
                    cmb_destin.DataSource = dt_ctaOmg;
                }
                if (cmb_sede.Text == "PER")
                {
                    cmb_destin.DataSource = dt_ctaPer;
                }
                cmb_destin.DisplayMember = "descrizionerid";
                cmb_destin.ValueMember = "idcodice";
            }
        }

        #endregion

        #region Botones de comando
        // acá falta todo el asunto de leer los permisos del usuario
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
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
            pan_menu.Enabled = true;
            //pan_repos.Enabled = true;
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

        private void Finan_reps_Load(object sender, EventArgs e)
        {
            //
        }

        #region radiobuttons
        private void rb_ctaPers_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_ctaPers.Checked == true)
            {
                pan_repos.Enabled = true;
                cmb_sede.Enabled = true;
                cmb_destin.Enabled = false; cmb_destin.SelectedIndex = -1;
                cmb_categ.Enabled = false; cmb_categ.SelectedIndex = -1;
                cmb_prov.Enabled = false; cmb_prov.SelectedIndex = -1;
                cmb_moneda.SelectedIndex = 0;
            }
        }
        private void rb_movCaja_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_movCaja.Checked == true)
            {
                pan_repos.Enabled = true;
                cmb_sede.Enabled = true;
                cmb_destin.Enabled = true; cmb_destin.SelectedIndex = -1;
                cmb_categ.Enabled = true; cmb_categ.SelectedIndex = -1;
                cmb_prov.Enabled = false; cmb_prov.SelectedIndex = -1;
                cmb_moneda.SelectedIndex = 0;
            }
        }
        private void rb_globOmg_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_globOmg.Checked == true)
            {
                pan_repos.Enabled = true; 
                cmb_sede.Enabled = false; cmb_sede.SelectedIndex = 0;
                cmb_destin.Enabled = true; cmb_destin.SelectedIndex = -1;
                cmb_categ.Enabled = true; cmb_categ.SelectedIndex = -1;
                cmb_prov.Enabled = true; cmb_prov.SelectedIndex = -1;
                cmb_moneda.SelectedIndex = 0;
            }
        }
        private void rb_gasCam_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_gasCam.Checked == true)
            {
                pan_repos.Enabled = true;
                cmb_sede.Enabled = false; cmb_sede.SelectedIndex = 0;
                cmb_destin.Enabled = true; cmb_destin.SelectedIndex = -1;
                cmb_categ.Enabled = true; cmb_categ.SelectedIndex = -1;
                cmb_prov.Enabled = true; cmb_prov.SelectedIndex = -1;
                cmb_moneda.SelectedIndex = 0;
            }
        }

        #endregion

        #region validaciones
        private void Tx_fecha_Click(object sender, EventArgs e)
        {
            var mtb = (MaskedTextBox)sender;
            mtb.Select(0, 0);
            mtb.Focus();
        }
        #endregion

        private void bt_genera_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (conn.State == ConnectionState.Open)
                {
                    if (rb_ctaPers.Checked == true)     // Rep 1
                    {

                    }
                    if (rb_movCaja.Checked == true)     // Rep 2
                    {
                        DataTable dt_s = new DataTable();
                        DataTable dt_d = new DataTable();
                        string va01 = "LIM";
                        string va05 = cmb_destin.SelectedValue.ToString();
                        string va02 = Tx_fecha1.Text.Substring(6, 4) + "-" + Tx_fecha1.Text.Substring(3, 2) + "-" + Tx_fecha1.Text.Substring(0, 2);
                        string va03 = Tx_fecha2.Text.Substring(6, 4) + "-" + Tx_fecha2.Text.Substring(3, 2) + "-" + Tx_fecha2.Text.Substring(0, 2);
                        string va02f = Tx_fecha1.Text.Substring(6, 4) + "-" + Tx_fecha1.Text.Substring(3, 2) + "-" + Tx_fecha1.Text.Substring(0, 2);
                        string va04 = ""; // aca va la categoria
                        string consulta = "reps_saldoIni";
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.CommandType = CommandType.StoredProcedure;
                            micon.Parameters.AddWithValue("@v_tabla", "cassaconti");
                            micon.Parameters.AddWithValue("@v_a01", va01);  // idconti = 'LIM'
                            micon.Parameters.AddWithValue("@v_a02", va02);  // fecha ini
                            micon.Parameters.AddWithValue("@v_a03", va03);  // fecha fin
                            micon.Parameters.AddWithValue("@v_a02f", va02f); // fecha 
                            micon.Parameters.AddWithValue("@v_a04", va04);  // categoria
                            micon.Parameters.AddWithValue("@v_a05", va05);  // cuenta

                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                da.Fill(dt_s);
                            }
                        }
                        consulta = "reps_cuenta";
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.CommandType = CommandType.StoredProcedure;
                            micon.Parameters.AddWithValue("@v_tabla", "cassaconti");
                            micon.Parameters.AddWithValue("@v_a01", va01);  // idconti = 'LIM'
                            micon.Parameters.AddWithValue("@v_a02", va02);  // fecha ini
                            micon.Parameters.AddWithValue("@v_a03", va03);  // fecha fin
                            micon.Parameters.AddWithValue("@v_a02f", va02f); // fecha 
                            micon.Parameters.AddWithValue("@v_a04", va04);  // categoria
                            micon.Parameters.AddWithValue("@v_a05", va05);  // cuenta

                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                da.Fill(dt_d);
                            }
                        }

                    }
                }
            }
        }
    }
}
