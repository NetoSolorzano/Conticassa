using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Conticassa
{
    public partial class ayuda2 : Form
    {
        public string para1 = "";
        public string para2 = "";
        public string para3 = "";
        public string para4 = "";
        // Se crea un DataTable que almacenará los datos desde donde se cargaran los datos al DataGridView
        DataTable dtDatos = new DataTable();
        // string de conexion
        static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        static string usua = ConfigurationManager.AppSettings["user"].ToString();
        static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";

        public ayuda2(string param1,string param2,string param3,string param4)
        {
            para1 = param1;              // 
            para2 = param2;              //
            para3 = param3;              //
            para4 = param4;              // 
            InitializeComponent();
        }
        private void ayuda2_Load(object sender, EventArgs e)
        {
            loadgrids();    // datos del grid
            this.Text = "AYUDA";
        }
        private void ayuda2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public string ReturnValue1 { get; set; }
        public string ReturnValue0 { get; set; }
        public string ReturnValue2 { get; set; }
        public string[] ReturnValueA { get; set; }

        public void loadgrids()
        {
            dtDatos.Clear();
            // DATOS DE LA GRILLA
            string consulta = "";
            if (para1 == "omg" && para2 == "cuenta" && para3 == "activos" && para4 == "")
            {
                consulta = "select idcodice,descrizionerid,descrizione " +
                "from desc_ where numero=0";
            }
            if (para1 == "personal" && para2 == "cuenta" && para3 == "activos" && para4 == "")    // CUENTAS PERSONALES
            {
                consulta = "select idcodice,descrizionerid,descrizione " +
                    "from desc_con where numero=0";
            }
            if (para1 != "" && para2 == "tEgresos" && para3 == "activos" && para4 == "")
            {
                consulta = "select idcodice,descrizionerid,descrizione " +
                    "from desc_cam where numero=1";
            }
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "CODIGO";
            dataGridView1.Columns[0].Width = 75;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].Name = "NOMBRE";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].Name = "DETALLE";
            dataGridView1.Columns[2].Width = 320;
            dataGridView1.Columns[2].ReadOnly = true;
            //
            ReturnValueA = new string[4] { "", "", "", "" };
            // Se crea un MySqlAdapter para obtener los datos de la base
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // 
                        {
                            DataRow row = dtDatos.Rows[li];
                            dataGridView1.Rows.Add(
                                                row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString()
                                                );
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en consulta de datos");
                    Application.Exit();
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tx_codigo.Text.Trim() != "")
            {
                ReturnValue0 = tx_codigo.Text;  // tx_id.Text
                ReturnValue1 = tx_nombre.Text;
                ReturnValue2 = "";
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = "";
                }
            }
            this.Close();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string cellva = "";
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = cellva;
                tx_id.Text = "";
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // codigo
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // nombre
                ReturnValueA[3] = "";
            }
            //Program.retorna1 = cellva;
            tx_codigo.Focus();
        }

        private void tx_codigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                ReturnValue1 = tx_codigo.Text;
                ReturnValue0 = tx_id.Text;
                ReturnValue2 = tx_nombre.Text;
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = "";
                }
                this.Close();
            }
        }

        private void tx_buscar_Leave(object sender, EventArgs e)
        {
            if (tx_buscar.Text != "")
            {
                dataGridView1.Rows.Clear();
                int li = 0;   // contador de las lineas a llenar el datagrid
                for (li = 0; li < dtDatos.Rows.Count; li++) // 
                {
                    DataRow row = dtDatos.Rows[li];
                    string cols3 = "omg,personal,tEgresos";         // busqueda en columna 1
                    string col14 = "????";             // 14 columnas
                    {
                        if (true)   // cols3.Contains(para1)
                        {
                            if (row.ItemArray[1].ToString().ToLower().Contains(tx_buscar.Text.Trim().ToLower()))
                            {
                                dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString()
                                                );
                            }
                        }
                    }
                }
            }
            else
            {
                dataGridView1.Rows.Clear();
                loadgrids();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string cellva = "";
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = cellva;
                tx_id.Text = "";
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // codigo
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // nombre
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // descripcion
                ReturnValueA[3] = "";
            }
            tx_codigo.Focus();
        }
    }
}
