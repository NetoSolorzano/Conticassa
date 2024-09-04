﻿using ADGV;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

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
        DataTable dtm = new DataTable();    // datos de la grilla
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
            tx_idOper.MaxLength = 6;
            Tx_nombre.MaxLength = 50;
            Tx_direc.MaxLength = 30;
            tx_tele1.MaxLength = 15;
            tx_tele2.MaxLength = 15;
            tx_correo.MaxLength = 50;
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
            string consulta = "select trim(a.idanagrafica) AS ID_PROV,trim(a.ragionesociale) AS NOMBRE,trim(a.indirizzo1) AS DIRECCION,trim(a.numerotel1) AS TELEFONO1,trim(a.numerotel2) AS TELEFONO2,trim(a.email) AS CORREO,a.stato AS ESTADO from anag_for a";
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
            advancedDataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.FromName("LightYellow");
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
        private void jalaoc(string[] fila)                   // jala los datos de la grilla
        {
            // ID_PROV,NOMBRE,DIRECCION,TELEFONO1,TELEFONO2,CORREO,ESTADO
            tx_idOper.Text = fila[0];
            Tx_nombre.Text = fila[1];
            Tx_direc.Text = fila[2];
            tx_tele1.Text = fila[3];
            tx_tele2.Text = fila[4];
            tx_correo.Text = fila[5];
            tx_estado.Text = fila[6];
        }                                                   // muestra en el formulario los objetos de la clase Egresos

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
            tx_idOper.Focus();
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
            tx_idOper.Focus();
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
            tx_idOper.Focus();
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
            if (e.ColumnIndex == 0)
            {
                string idr = "";    // código del proveedor
                string rin = "";    // indice en la grilla
                idr = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                rin = advancedDataGridView1.CurrentRow.Index.ToString();
                string[] fila = {"", "", "", "", "", "", "" };
                fila[0] = advancedDataGridView1.CurrentRow.Cells[0].Value.ToString();
                fila[1] = advancedDataGridView1.CurrentRow.Cells[1].Value.ToString();
                fila[2] = advancedDataGridView1.CurrentRow.Cells[2].Value.ToString();
                fila[3] = advancedDataGridView1.CurrentRow.Cells[3].Value.ToString();
                fila[4] = advancedDataGridView1.CurrentRow.Cells[4].Value.ToString();
                fila[5] = advancedDataGridView1.CurrentRow.Cells[5].Value.ToString();
                fila[6] = advancedDataGridView1.CurrentRow.Cells[6].Value.ToString();
                limpiar();
                limpia_otros();
                limpia_combos();
                jalaoc(fila);
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
            /* if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
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
            } */
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

        #region leaves y validaciones
        private void tx_idOper_Validating(object sender, CancelEventArgs e)
        {
            if (true)   // tx_idOper.Text.Trim() != "" && ("NUEVO,EDICION").Contains(Tx_modo.Text)
            {
                DataRow[] row = dtm.Select("ID_PROV = '" + tx_idOper.Text.Trim() + "'");
                if (row.Length > 0)
                {
                    if (Tx_modo.Text != "NUEVO")
                    {
                        // ID_PROV,NOMBRE,DIRECCION,TELEFONO1,TELEFONO2,CORREO,ESTADO
                        string[] fila = { "", "", "", "", "", "", "" };
                        fila[0] = row[0].ItemArray[0].ToString();
                        fila[1] = row[0].ItemArray[1].ToString();
                        fila[2] = row[0].ItemArray[2].ToString();
                        fila[3] = row[0].ItemArray[3].ToString();
                        fila[4] = row[0].ItemArray[4].ToString();
                        fila[5] = row[0].ItemArray[5].ToString();
                        fila[6] = row[0].ItemArray[6].ToString();
                        jalaoc(fila);
                    }
                    else
                    {
                        MessageBox.Show("Ya existe el proveedor!", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        tx_idOper.Text = "";
                        return;
                    }
                }
                else
                {
                    if (Tx_modo.Text != "NUEVO")
                    {
                        MessageBox.Show("No existe el proveedor", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        tx_idOper.Text = "";
                        return;
                    }
                }
            }
        }     // busca en toda la base de datos
        private void tx_estado_Validating(object sender, CancelEventArgs e)
        {
            if (tx_estado.Text != "0" && tx_estado.Text != "1")
            {
                errorProvider1.SetError(tx_estado, "Debe ingresar: " + Environment.NewLine +
                    "1 para Activo " + Environment.NewLine +
                    "0 para Inactivo");
                tx_estado.Focus();
                return;
            }
        }
        #endregion

        private void Bt_graba_Click(object sender, EventArgs e)
        {
            // valida campos minimos
            if (tx_idOper.Text == "")
            {
                errorProvider1.SetError(tx_idOper, "Debe ingresar un código");
                tx_idOper.Focus();
                return;
            }
            errorProvider1.SetError(tx_idOper, "");
            if (Tx_nombre.Text == "")
            {
                errorProvider1.SetError(Tx_nombre, "Debe ingresar un nombre");
                Tx_nombre.Focus();
                return;
            }
            errorProvider1.SetError(Tx_nombre, "");
            if (tx_estado.Text.Trim() == "")
            {
                errorProvider1.SetError(tx_estado, "Debe ingresar: " + Environment.NewLine +
                    "1 para Activo " + Environment.NewLine +
                    "0 para Inactivo");
                tx_estado.Focus();
                return;
            }
            errorProvider1.SetError(tx_estado, "");

            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                var aa = MessageBox.Show("Confirma que desea grabar?","Atención",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (aa == DialogResult.No)
                {
                    tx_correo.Focus();
                    return;
                }

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
                        // inserta en tabla
                        string cins = "insert into anagrafiche (idanagrafica,ragionesociale,indirizzo1,numerotel1,numerotel2,email,stato,idcategoria) " +
                            "values (@ida,@nom,@dir,@tel1,@tel2,@corr,@esta,'FOR')";
                        using (MySqlCommand micon = new MySqlCommand(cins, conn))
                        {
                            micon.Parameters.AddWithValue("@ida", tx_idOper.Text.Trim());
                            micon.Parameters.AddWithValue("@nom", Tx_nombre.Text.Trim());
                            micon.Parameters.AddWithValue("@dir", Tx_direc.Text.Trim());
                            micon.Parameters.AddWithValue("@tel1", tx_tele1.Text.Trim());
                            micon.Parameters.AddWithValue("@tel2", tx_tele2.Text.Trim());
                            micon.Parameters.AddWithValue("@corr", tx_correo.Text.Trim());
                            micon.Parameters.AddWithValue("@esta", tx_estado.Text);
                            micon.ExecuteNonQuery();
                        }
                        // inserta en grilla
                        DataRow fila = dtm.NewRow();
                        fila[0] = tx_idOper.Text;
                        fila[1] = Tx_nombre.Text;
                        fila[2] = Tx_direc.Text;
                        fila[3] = tx_tele1.Text;
                        fila[4] = tx_tele2.Text;
                        fila[5] = tx_correo.Text;
                        fila[6] = tx_estado.Text;
                        dtm.Rows.InsertAt(fila, 0);
                    }
                    if (Tx_modo.Text == "EDITAR")
                    {
                        // actualiza la tabla
                        string cins = "update anagrafiche set ragionesociale=@nom,indirizzo1=@dir,numerotel1=@tel1,numerotel2=@tel2,email=@corr,stato=@esta " +
                            "where idanagrafica=@ida";
                        using (MySqlCommand micon = new MySqlCommand(cins, conn))
                        {
                            micon.Parameters.AddWithValue("@ida", tx_idOper.Text.Trim());
                            micon.Parameters.AddWithValue("@nom", Tx_nombre.Text.Trim());
                            micon.Parameters.AddWithValue("@dir", Tx_direc.Text.Trim());
                            micon.Parameters.AddWithValue("@tel1", tx_tele1.Text.Trim());
                            micon.Parameters.AddWithValue("@tel2", tx_tele2.Text.Trim());
                            micon.Parameters.AddWithValue("@corr", tx_correo.Text.Trim());
                            micon.Parameters.AddWithValue("@esta", tx_estado.Text);
                            micon.ExecuteNonQuery();
                        }
                        // actualiza la grilla
                        DataRow fila = dtm.Select("ID_PROV = '" + tx_idOper.Text.Trim() + "'").FirstOrDefault(); // dtm.NewRow();
                        if (fila != null)
                        {
                            //fila[0] = tx_idOper.Text;
                            fila[1] = Tx_nombre.Text;
                            fila[2] = Tx_direc.Text;
                            fila[3] = tx_tele1.Text;
                            fila[4] = tx_tele2.Text;
                            fila[5] = tx_correo.Text;
                            fila[6] = tx_estado.Text;
                        }
                    }
                }
            }
            limpiar();
            limpia_otros();
            limpia_combos();
        }

    }
}
