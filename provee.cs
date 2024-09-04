﻿using ADGV;
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

namespace Conticassa
{
    public partial class provee : Form1
    {
        static string nomform = "provee";               // nombre del formulario
        string asd = Program.vg_user;                   // usuario conectado al sistema
        string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string v_noM1 = "";
        string v_noM2 = "";
        string v_noM3 = "";
        string v_noM4 = "";
        Finan_Egres OFegres = new Finan_Egres();
        publicoConf lp = new publicoConf();
        // string de conexion
        string DB_CONN_STR = "server=" + Login.serv + ";uid=" + Login.usua + ";pwd=" + Login.cont + ";database=" + Login.data + ";";
        DataTable dtg = new DataTable();
        DataTable dtm = new DataTable();
        public provee()
        {
            InitializeComponent();
        }
        private void provee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            //if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        private void provee_Load(object sender, EventArgs e)
        {
            /*
            ToolTip toolTipNombre = new ToolTip();           // Create the ToolTip and associate with the Form container.
            toolTipNombre.AutoPopDelay = 5000;
            toolTipNombre.InitialDelay = 1000;
            toolTipNombre.ReshowDelay = 500;
            toolTipNombre.ShowAlways = true;                 // Force the ToolTip text to be displayed whether or not the form is active.
            toolTipNombre.SetToolTip(toolStrip1, nomform);   // Set up the ToolTip text for the object
            */
            init();
            //toolboton();
            limpiar();
            sololee();
            dataload();
            this.KeyPreview = true;
            advancedDataGridView1.Enabled = false;
        }
        private void init()
        {
            jalainfo();


        }
        private void jalainfo()                 // obtiene datos de imagenes
        {
            try
            {
                for (int t = 0; t < Program.dt_enlaces.Rows.Count; t++)
                {
                    //DataRow row = Program.dt_enlaces.Rows[t];
                    //if (row["campo"].ToString() == "estado" && row["param"].ToString() == "anulado") vEstAnu = row["valor"].ToString().Trim();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
        }
        public void dataload()                  // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            string consulta = "select a.idanagrafica AS ID_PROV,a.ragionesociale AS NOMBRE,a.indirizzo1 AS DIRECCION,a.numerotel1 AS TELEFONO1,a.numerotel2 AS TELEFONO2,a.email AS CORREO,a.stato AS ESTADO from anag_for a";
            using (MySqlCommand micon = new MySqlCommand(consulta, conn))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                {
                    da.Fill(dtm);
                    advancedDataGridView1.DataSource = dtm;
                }
            }
            conn.Close();
            grilla();
        }
        private void grilla()                   // arma la grilla
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            advancedDataGridView1.Font = tiplg;
            advancedDataGridView1.DefaultCellStyle.Font = tiplg;
            advancedDataGridView1.RowTemplate.Height = 15;
            advancedDataGridView1.DataSource = dtm;
            // codigo proveedor 
            advancedDataGridView1.Columns[0].Visible = true;
            // nombre
            advancedDataGridView1.Columns[1].Visible = true;            // columna visible o no
            //advancedDataGridView1.Columns[1].HeaderText = "Fecha";    // titulo de la columna
            advancedDataGridView1.Columns[1].Width = 150;                // ancho
            advancedDataGridView1.Columns[1].ReadOnly = true;           // lectura o no
            //advancedDataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // direccion
            advancedDataGridView1.Columns[2].Visible = true;
            //advancedDataGridView1.Columns[2].HeaderText = v_noM1; // "mext1"
            advancedDataGridView1.Columns[2].Width = 60;
            advancedDataGridView1.Columns[2].ReadOnly = true;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[2].Tag = "validaNO";          // las celdas de esta columna se NO se validan
            //advancedDataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // telefono 1
            advancedDataGridView1.Columns[3].Visible = true;
            //advancedDataGridView1.Columns[3].HeaderText = v_noM2; // "mext2"
            advancedDataGridView1.Columns[3].Width = 60;
            advancedDataGridView1.Columns[3].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[3].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // telefono 2
            advancedDataGridView1.Columns[4].Visible = true;
            //advancedDataGridView1.Columns[4].HeaderText = v_noM2; // "mext2"
            advancedDataGridView1.Columns[4].Width = 60;
            advancedDataGridView1.Columns[4].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[4].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // correo
            advancedDataGridView1.Columns[5].Visible = true;
            //advancedDataGridView1.Columns[5].HeaderText = v_noM2; // "mext2"
            advancedDataGridView1.Columns[5].Width = 60;
            advancedDataGridView1.Columns[5].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[5].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // estado
            advancedDataGridView1.Columns[6].Visible = true;
            //advancedDataGridView1.Columns[6].HeaderText = v_noM2; // "mext2"
            advancedDataGridView1.Columns[6].Width = 60;
            advancedDataGridView1.Columns[6].ReadOnly = false;          // las celdas de esta columna pueden cambiarse
            advancedDataGridView1.Columns[6].Tag = "validaNO";          // las celdas de esta columna se validan
            advancedDataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        #region limpiadores_modos
        public void sololee()
        {
            lp.sololee(this);
        }
        public void escribe()
        {
            lp.escribe(this);
        }
        private void limpiar()
        {
            lp.limpiar(this);
        }
        private void limpiaPag(TabPage pag)
        {
            lp.limpiapag(pag);
        }
        public void limpia_chk()
        {
            lp.limpia_chk(this);
        }
        public void limpia_otros()
        {
            //checkBox1.Checked = false;
        }
        public void limpia_combos()
        {
            lp.limpia_cmb(this);
        }
        #endregion limpiadores_modos;

        #region botones_de_comando_y_permisos  
        public void toolboton()
        {
            /*
            DataTable mdtb = new DataTable();
            const string consbot = "select * from permisos where formulario=@nomform and usuario=@use";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    MySqlCommand consulb = new MySqlCommand(consbot, conn);
                    consulb.Parameters.AddWithValue("@nomform", nomform);
                    consulb.Parameters.AddWithValue("@use", asd);
                    MySqlDataAdapter mab = new MySqlDataAdapter(consulb);
                    mab.Fill(mdtb);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, " Error ");
                    return;
                }
                finally { conn.Close(); }
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            if (mdtb.Rows.Count > 0)
            {
                DataRow row = mdtb.Rows[0];
                if (Convert.ToString(row["btn1"]) == "S")
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                if (Convert.ToString(row["btn4"]) == "S")
                {
                    this.Bt_ver.Visible = true;
                }
                else { this.Bt_ver.Visible = false; }
                if (Convert.ToString(row["btn5"]) == "S")
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
            }
            */
        }
        #region botones
        private void Bt_add_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            escribe();
            Tx_modo.Text = "NUEVO";
            limpiar();
            limpia_otros();
            limpia_combos();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = false;
            escribe();
            Tx_modo.Text = "EDITAR";
            limpiar();
            limpia_otros();
            limpia_combos();
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            sololee();
            this.Tx_modo.Text = "IMPRIMIR";
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            //Tx_modo.Text = "ANULAR";
        }
        private void Bt_ver_Click(object sender, EventArgs e)
        {
            advancedDataGridView1.Enabled = true;
            advancedDataGridView1.ReadOnly = true;
            escribe();
            Tx_modo.Text = "VISUALIZAR";
            limpiar();
            limpia_otros();
            limpia_combos();
        }
        private void Bt_first_Click(object sender, EventArgs e)
        {
            limpiar();
            limpia_chk();
            limpia_combos();
        }
        private void Bt_back_Click(object sender, EventArgs e)
        {
            limpia_chk();
            limpia_combos();
            limpiar();
        }
        private void Bt_next_Click(object sender, EventArgs e)
        {
            limpia_chk();
            limpia_combos();
            limpiar();
        }
        private void Bt_last_Click(object sender, EventArgs e)
        {
            limpiar();
            limpia_chk();
            limpia_combos();
        }
        #endregion botones;
        // permisos para habilitar los botones de comando
        #endregion botones_de_comando  ;

        #region advancedatagridview
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            dtg.DefaultView.RowFilter = advancedDataGridView1.FilterString;
        }
        private void advancedDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)            // almacena valor previo al ingresar a la celda
        {
            advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                /*string idr,rin;
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                rin = advancedDataGridView1.CurrentRow.Index.ToString();
                limpiar();
                limpia_otros();
                limpia_combos();
                jalaoc("tx_idr"); */
            }
        }
        private void advancedDataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            tb.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress);
            e.Control.KeyPress += new KeyPressEventHandler(dataGridViewTextBox_KeyPress);
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // valida cambios en valor de la celda
        {
            if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
            {
                if (e.RowIndex > -1 && e.ColumnIndex > 1
                    && advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != e.FormattedValue.ToString())
                {
                    string campo = advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString();
                    //
                    var aaa = MessageBox.Show("Confirma que desea cambiar el valor?",
                        "Columna: " + advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aaa == DialogResult.Yes)
                    {
                        using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                        {
                            conn.Open();
                            if (conn.State == ConnectionState.Open)
                            {
                                string actua = "update cambi set " + campo + " = " + e.FormattedValue +
                                    " where id=@idr";    // advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()
                                using (MySqlCommand micon = new MySqlCommand(actua, conn))
                                {
                                    micon.Parameters.AddWithValue("@idr", advancedDataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                                    micon.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        SendKeys.Send("{ESC}");
                    }
                }
            }
        }
        private void dataGridViewTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        #endregion


    }
}
