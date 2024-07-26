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
        libreria lnp = new libreria();
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
            this.Text = this.Text + " - " + para4 + " - " + para1 + " - " + para2;
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
            if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")    // articulos de la maestra
            {
                consulta = "";
                {
                    consulta = "select codig,nombr,medid,soles2018 " +
                    "from items order by codig";
                }
                // Acomodamos la grilla 891
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 4;
                dataGridView1.Columns[0].Name = "CODIGO";
                dataGridView1.Columns[0].Width = 170;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = "NOMBRE";
                dataGridView1.Columns[1].Width = 490;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = "MEDIDAS";
                dataGridView1.Columns[2].Width = 90;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = "PRECIO";
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[3].ReadOnly = true;
                //
                this.Width = dataGridView1.Width + 5;
                //
                ReturnValueA = new string[4] { "", "", "", ""};
            }
            if (para1 == "items_adic" && para2 == "todos" && para3 == "" && para4 == "")    // articulos de la maestra
            {
                consulta = "select codig,nombr,medid,precio,detporc " +
                    "from items_adic where bloqueado=0";                                    // el campo "detporc" es el % detraccion si fuera servicio
                // Acomodamos la grilla 891
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 5;
                dataGridView1.Columns[0].Name = "CODIGO";
                dataGridView1.Columns[0].Width = 170;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = "NOMBRE";
                dataGridView1.Columns[1].Width = 490;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = "MEDIDAS";
                dataGridView1.Columns[2].Width = 90;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = "PRECIO";
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].Width = 10;
                dataGridView1.Columns[4].Visible = false;
                //
                this.Width = dataGridView1.Width + 5;
                //
                ReturnValueA = new string[5] { "", "", "", "", "" };
            }
            if (para1 == "anag_cli" && para2 == "todos" && para3 == "" && para4 == "")   // maestra de clientes
            {
                consulta = "select idanagrafica,tipdoc,ruc,razonsocial,space(1),space(1),space(1) from anag_cli where estado=0";
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 7;
                dataGridView1.Columns[0].Name = " ID ";
                dataGridView1.Columns[0].Width = 35;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = " TDOC";
                dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = " #DOC";
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = " NOMBRE";
                dataGridView1.Columns[3].Width = 500;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                //
                this.Width = dataGridView1.Width + 5;
                //
                ReturnValueA = new string[4] { "", "", "", "" };
            }
            if (para1 == "contrat" && para4 == "")
            {
                if (para2 != "" && para3 == "" && para4 == "")
                {
                    consulta = "select a.id,a.cliente,a.contrato,b.razonsocial,a.status,ifnull(c.descrizionerid,''),a.tipoes,a.fecha " +
                        "from contrat a left join anag_cli b on b.idanagrafica=a.cliente " +
                        "left join desc_alm c on c.idcodice=a.tipoes " +
                        "where b.idanagrafica = @para2";
                }
                if (para2 == "" && para3 == "" && para4 == "")
                {
                    consulta = "select a.id,a.cliente,a.contrato,b.razonsocial,a.status,ifnull(c.descrizionerid,''),a.tipoes,a.fecha " +
                        "from contrat a left join anag_cli b on b.idanagrafica=a.cliente " +
                        "left join desc_alm c on c.idcodice=a.tipoes " +
                        "where a.status not in ('ANULAD', 'ENTREG') order by b.razonsocial";
                }
                if (para2 == "" && para3 == "saldo" && para4 == "")     // solo muestra contratos con saldo por pagar
                {
                    consulta = "select a.id,a.cliente,a.contrato,b.razonsocial,a.status,ifnull(c.descrizionerid,''),a.tipoes,a.fecha " +
                        "from contrat a left join anag_cli b on b.idanagrafica=a.cliente " +
                        "left join desc_alm c on c.idcodice=a.tipoes " +
                        "where a.status not in ('ANULAD', 'ENTREG') and a.saldo>0 order by b.razonsocial";
                }
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 8;
                dataGridView1.Columns[0].Name = " ID ";
                dataGridView1.Columns[0].Width = 35;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = " CLIENTE";
                dataGridView1.Columns[1].Width = 70;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = " CONTRATO";
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = " NOMBRE";
                dataGridView1.Columns[3].Width = 400;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].Name = " ESTADO";
                dataGridView1.Columns[4].Width = 80;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].Name = " DESTINO";
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Name = " tipoes";
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                //
                ReturnValueA = new string[8] { "", "", "", "", "", "", "", ""};
                this.Width = dataGridView1.Width + 5;
            }
            if (para1 == "detacon" && para2 != "" && para3 == "" && para4 == "")
            {
                consulta = "select a.iddetacon,a.item,a.cant,a.nombre,a.medidas,a.madera,if(trim(a.estado)='',substring(a.item,10,1),trim(a.estado)) as estado,a.saldo,a.coment,a.total,b.descrizionerid as acabado " +
                    "from detacon a left join desc_est b on b.idcodice=if(trim(a.estado)='',substring(a.item,10,1),trim(a.estado)) " +
                    "where a.contratoh=@para2 and a.saldo>0";
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 11;
                dataGridView1.Columns[0].Name = " ID ";
                dataGridView1.Columns[0].Width = 35;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = " CODIGO";
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = " CANT";
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = " NOMBRE";
                dataGridView1.Columns[3].Width = 300;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].Name = " MEDIDAS";
                dataGridView1.Columns[4].Width = 80;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].Name = " MADERA";
                dataGridView1.Columns[5].Width = 80;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].Name = " ESTADO";
                dataGridView1.Columns[6].Width = 60;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Name = " SALDO";
                dataGridView1.Columns[7].Width = 60;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Name = " COMENTARIO";
                dataGridView1.Columns[8].Width = 200;
                dataGridView1.Columns[8].ReadOnly = true;
                dataGridView1.Columns[9].Name = " TOTAL";
                dataGridView1.Columns[9].Width = 60;
                dataGridView1.Columns[9].ReadOnly = true;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Name = " ACABADO";
                dataGridView1.Columns[10].Width = 60;
                dataGridView1.Columns[10].ReadOnly = true;
                dataGridView1.Columns[10].Visible = false;
                //
                ReturnValueA = new string[11] { "", "", "", "", "", "", "", "", "", "", ""};
            }
            if (para1 == "pedidos" && para2 == "pend" && para3 != "" && para4 == "")
            {   // b.cant
                consulta = "select a.codped,a.origen,a.destino,trim(cl.razonsocial) as cliente," +
                    "b.saldo,b.item,b.nombre,b.medidas,b.madera,b.estado,b.precio,b.total," +
                    "m.descrizionerid as nomad,e.descrizionerid as acabado," +
                    "o.descrizionerid as nomorig,d.descrizionerid as nomdestin,a.contrato,a.fecha " +
                    "from pedidos a left join detaped b on b.pedidoh=a.codped " +
                    "left join desc_mad m on m.idcodice=b.madera " +
                    "left join desc_est e on e.idcodice=b.estado " +
                    "left join desc_loc o on o.idcodice=a.origen " +
                    "left join desc_alm d on d.idcodice=a.destino " +
                    "left join anag_cli cl on cl.idanagrafica=a.cliente " +
                    "where b.saldo>0 and a.tipoes=@para3 and a.status<>'ANULAD'";
                //"left join movim c on c.pedido=a.codped " +
                //"where c.pedido is null and a.tipoes=@para3";
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 18;
                dataGridView1.Columns[0].Name = " CODIGO";
                dataGridView1.Columns[0].Width = 70;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = " ORIGEN";
                dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = " DESTINO";
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = " CLIENTE";
                dataGridView1.Columns[3].Width = 100;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].Name = "SALDO";
                dataGridView1.Columns[4].Width = 50;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].Name = " ITEM";
                dataGridView1.Columns[5].Width = 100;
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Name = " NOMBRE";
                dataGridView1.Columns[6].Width = 100;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Name = " MEDIDAS";
                dataGridView1.Columns[7].Width = 80;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[8].Name = " MADERA";
                dataGridView1.Columns[8].Width = 50;
                dataGridView1.Columns[8].ReadOnly = true;
                dataGridView1.Columns[9].Visible = true;
                dataGridView1.Columns[9].Name = " ACAB";
                dataGridView1.Columns[9].Width = 40;
                dataGridView1.Columns[9].ReadOnly = true;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[10].Name = " PRECIO";
                dataGridView1.Columns[10].Width = 40;
                dataGridView1.Columns[10].ReadOnly = true;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[11].Name = " TOTAL";
                dataGridView1.Columns[11].Width = 40;
                dataGridView1.Columns[11].ReadOnly = true;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[12].Name = " NOMAD";
                dataGridView1.Columns[12].Width = 40;
                dataGridView1.Columns[12].ReadOnly = true;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[13].Name = " NOACA";
                dataGridView1.Columns[13].Width = 40;
                dataGridView1.Columns[13].ReadOnly = true;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[14].Name = " NORIG";
                dataGridView1.Columns[14].Width = 40;
                dataGridView1.Columns[14].ReadOnly = true;
                dataGridView1.Columns[15].Visible = false;
                dataGridView1.Columns[15].Name = " NODEST";
                dataGridView1.Columns[15].Width = 40;
                dataGridView1.Columns[15].ReadOnly = true;
                dataGridView1.Columns[16].Visible = false;
                dataGridView1.Columns[16].Name = "CONTRATO";
                dataGridView1.Columns[17].Visible = false;
                dataGridView1.Columns[17].Name = "FECHA";
                //
                ReturnValueA = new string[18] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            }
            if (para1 == "movim" && para2 == "pend" && para3 != "" && para4 == "")
            {
                consulta = "select a.pedido,cl.razonsocial as cliente,a.destino,ifnull(b.descrizionerid, '') as nomact,a.articulo," + 
                    "dp.nombre,a.med1,a.madera,ifnull(c.descrizionerid,'') as nomad,a.estado,ifnull(d.descrizionerid,'') as acabado,a.cant," +
                    "a.saldo,a.idmovim,pe.contrato,a.fechain from movim a " +
                    "left join pedidos pe on pe.codped=a.pedido and pe.tipoes=@para3 " +
                    "left join anag_cli cl on cl.idanagrafica=pe.cliente " +
                    "left join desc_alm b on b.idcodice=a.destino " +
                    "left join detaped dp on dp.pedidoh=a.pedido AND dp.item=a.articulo " +
                    "left join desc_mad c on c.idcodice=a.madera " +
                    "left join desc_est d on d.idcodice=a.estado " +
                    "where a.saldo>0";  // a.fventa is null or
                //
                dataGridView1.Rows.Clear();
                dataGridView1.ColumnCount = 16;
                dataGridView1.Columns[0].Name = "pedido";
                dataGridView1.Columns[0].Width = 70;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Name = "cliente";
                dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Name = "destino";
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].Name = "nomact";
                dataGridView1.Columns[3].Width = 100;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].Name = "articulo";
                dataGridView1.Columns[4].Width = 50;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].Name = "nombre";
                dataGridView1.Columns[5].Width = 100;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Name = "med1";
                dataGridView1.Columns[6].Width = 100;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Name = "madera";
                dataGridView1.Columns[7].Width = 80;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[8].Name = "nomad";
                dataGridView1.Columns[8].Width = 50;
                dataGridView1.Columns[8].ReadOnly = true;
                dataGridView1.Columns[9].Visible = true;
                dataGridView1.Columns[9].Name = "estado";
                dataGridView1.Columns[9].Width = 40;
                dataGridView1.Columns[9].ReadOnly = true;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[10].Name = "acabado";
                dataGridView1.Columns[10].Width = 40;
                dataGridView1.Columns[10].ReadOnly = true;
                dataGridView1.Columns[11].Visible = true;
                dataGridView1.Columns[11].Name = "cant";
                dataGridView1.Columns[12].Visible = true;
                dataGridView1.Columns[12].Name = "saldo";
                dataGridView1.Columns[13].Visible = true;
                dataGridView1.Columns[13].Name = "IdMov";
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[14].Name = "Contrato";
                dataGridView1.Columns[15].Visible = false;
                //
                ReturnValueA = new string[16] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            }
            // Se crea un MySqlAdapter para obtener los datos de la base
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        if (para3 == "serv")
                        {
                            if (para3 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@ser", para1);
                            if (para4 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@cor", int.Parse(para2));
                            if (para4 == "") mdaDatos.SelectCommand.Parameters.AddWithValue("@lser", para4);
                        }
                        else
                        {
                            if (para3 != "serv") mdaDatos.SelectCommand.Parameters.AddWithValue("@ser", para1);
                            if (para4 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@cor", int.Parse(para2));
                        }
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // 
                        {
                            DataRow row = dtDatos.Rows[li];
                            // (li + 1).ToString(),
                            dataGridView1.Rows.Add(
                                                row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString()
                                                );
                        }
                    }
                    if (para1 == "items_adic" && para2 == "todos" && para3 == "" && para4 == "")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        if (para3 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@ser", para1);
                        if (para4 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@cor", int.Parse(para2));
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // 
                        {
                            DataRow row = dtDatos.Rows[li];
                            // (li + 1).ToString(),
                            dataGridView1.Rows.Add(
                                                row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString()
                                                );
                        }
                    }
                    if (para1 == "anag_cli" && para2 == "todos" && para3 == "" && para4 == "")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        if (para1 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para1", para1);
                        if (para2 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // 
                        {
                            DataRow row = dtDatos.Rows[li];
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString()
                                                );
                        }
                    }
                    if (para1 == "contrat" && para4 == "")  // 
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        if(para2 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // 
                        {
                            DataRow row = dtDatos.Rows[li];
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString()
                                                );
                        }
                    }
                    if (para1 == "detacon" && para2 != "" && para3 == "" && para4 == "")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        if (para2 != "") mdaDatos.SelectCommand.Parameters.AddWithValue("@para2", para2);
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // iddetacon,item,cant,nombre,medidas,madera,estado,saldo,coment,total,acabado
                        {
                            DataRow row = dtDatos.Rows[li];
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString(),
                                                row.ItemArray[8].ToString(),
                                                row.ItemArray[9].ToString(),
                                                row.ItemArray[10].ToString()
                                                );
                        }
                    }
                    if (para1 == "pedidos" && para2 == "pend" && para3 != "" && para4 == "")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        mdaDatos.SelectCommand.Parameters.AddWithValue("@para3", para3);
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // iddetacon,item,cant,nombre,medidas,madera,estado,saldo,coment
                        {
                            DataRow row = dtDatos.Rows[li];
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString(),
                                                row.ItemArray[8].ToString(),
                                                row.ItemArray[9].ToString(),
                                                row.ItemArray[10].ToString(),
                                                row.ItemArray[11].ToString(),
                                                row.ItemArray[12].ToString(),
                                                row.ItemArray[13].ToString(),
                                                row.ItemArray[14].ToString(),
                                                row.ItemArray[15].ToString(),
                                                row.ItemArray[16].ToString(),
                                                row.ItemArray[17].ToString()
                                                );
                        }
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                    if (para1 == "movim" && para2 == "pend" && para3 != "" && para4 == "")
                    {
                        MySqlDataAdapter mdaDatos = new MySqlDataAdapter(consulta, conn);
                        mdaDatos.SelectCommand.Parameters.AddWithValue("@para3", para3);
                        mdaDatos.Fill(dtDatos);
                        int li = 0;   // contador de las lineas a llenar el datagrid
                        for (li = 0; li < dtDatos.Rows.Count; li++) // a.pedido,cliente,a.destino,nomact,a.articulo,dp.nombre,a.med1,a.madera,nomad,a.estado,acabado
                        {
                            DataRow row = dtDatos.Rows[li];
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString(),
                                                row.ItemArray[8].ToString(),
                                                row.ItemArray[9].ToString(),
                                                row.ItemArray[10].ToString(),
                                                row.ItemArray[11].ToString(),
                                                row.ItemArray[12].ToString(),
                                                row.ItemArray[13].ToString(),
                                                row.ItemArray[14].ToString(),
                                                row.ItemArray[15].ToString()
                                                );
                        }
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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
                ReturnValue0 = tx_id.Text;
                ReturnValue1 = tx_codigo.Text;
                ReturnValue2 = tx_nombre.Text;
                if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                }
                if (para1 == "items_adic" && para2 == "todos" && para3 == "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                }
                if (para1 == "anag_cli" && para2 == "todos" && para3 == "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // tipo doc
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // num doc
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // nombre
                }
                if (para1 == "contrat" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    ReturnValueA[4] = (dataGridView1.CurrentRow.Cells[4].Value == null) ? "" : dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    ReturnValueA[5] = (dataGridView1.CurrentRow.Cells[5].Value == null) ? "" : dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    ReturnValueA[6] = (dataGridView1.CurrentRow.Cells[6].Value == null) ? "" : dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    ReturnValueA[7] = (dataGridView1.CurrentRow.Cells[7].Value == null) ? "" : dataGridView1.CurrentRow.Cells[7].Value.ToString();
                }
                if (para1 == "detacon" && para2 != "" && para3 == "" && para4 == "")    // iddetacon,item,cant,nombre,medidas,madera,estado,saldo,coment,total,acabado
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                        ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                        ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                        ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                        ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                        ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                        ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                        ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                        ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                        ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();
                    }
                    else
                    {
                        ReturnValueA[0] = "";
                        ReturnValueA[1] = "";
                        ReturnValueA[2] = "";
                        ReturnValueA[3] = "";
                        ReturnValueA[4] = "";
                        ReturnValueA[5] = "";
                        ReturnValueA[6] = "";
                        ReturnValueA[7] = "";
                        ReturnValueA[8] = "";
                        ReturnValueA[9] = "";
                        ReturnValueA[10] = "";
                    }
                }
                if (para1 == "pedidos" && para2 == "pend" && para3 == "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                    ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();
                    ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();
                    ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();
                    ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();
                    ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();
                    ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();
                    ReturnValueA[16] = dataGridView1.CurrentRow.Cells[16].Value.ToString();
                    ReturnValueA[17] = dataGridView1.CurrentRow.Cells[17].Value.ToString();
                }
                if (para1 == "movim" && para2 == "pend" && para3 != "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                    ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();
                    ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();
                    ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();
                    ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();
                    ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();
                    ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();
                }
            }
            this.Close();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string cellva = "";
            if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = cellva;
                tx_id.Text = "";
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // codigo
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // nombre
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // precio
            }
            if (para1 == "items_adic" && para2 == "todos" && para3 == "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = cellva;
                tx_id.Text = "";
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
            if (para1 == "anag_cli" && para2 == "todos" && para3 == "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // tipo doc
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // num doc
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // nombre
            }
            if (para1 == "contrat" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();    // nombre del cliente
                cellva = dataGridView1.CurrentRow.Cells[1].Value.ToString();            // id del cliente
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();    // numero de contrato
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();        // id del contrato
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // codigo
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // 
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // 
                if (dataGridView1.CurrentRow.Cells[4].Value == null) ReturnValueA[4] = "";
                else ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // 
                if (dataGridView1.CurrentRow.Cells[5].Value == null) ReturnValueA[5] = "";
                else ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // destino
                if (dataGridView1.CurrentRow.Cells[6].Value == null) ReturnValueA[6] = "";
                else ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // cod destino
                ReturnValueA[7] = (dataGridView1.CurrentRow.Cells[7].Value == null)? "" : dataGridView1.CurrentRow.Cells[7].Value.ToString();
            }
            if (para1 == "detacon" && para2 != "" && para3 == "" && para4 == "")        // // iddetacon,item,cant,nombre,medidas,madera,estado,saldo,coment,total,acabado
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // codigo
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // cant
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // nombre
                ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // medidas
                ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // madera
                ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // estado codigo
                ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();   // saldo
                ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();   // coment
                ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();   // total
                ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // acabado
            }
            if (para1 == "pedidos" && para2 == "pend" && para3 != "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // 
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // 
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // 
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // 
                ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // 
                ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // 
                ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // 
                ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();   // 
                ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();   // 
                ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();   // 
                ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // 
                ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();   // 
                ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();   // 
                ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();   // 
                ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();   // 
                ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();   // 
                ReturnValueA[16] = dataGridView1.CurrentRow.Cells[16].Value.ToString();   // 
                ReturnValueA[17] = dataGridView1.CurrentRow.Cells[17].Value.ToString();   // 
            }
            if (para1 == "movim" && para2 == "pend" && para3 != "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[13].Value.ToString();
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // 
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // 
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // 
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // 
                ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // 
                ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // 
                ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // 
                ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();   // 
                ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();   // 
                ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();   // 
                ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // 
                ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();   // 
                ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();
                ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();
                ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();
                ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();
            }
            iOMG.Program.retorna1 = cellva;
            tx_codigo.Focus();
        }

        private void tx_codigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                ReturnValue1 = tx_codigo.Text;
                ReturnValue0 = tx_id.Text;
                ReturnValue2 = tx_nombre.Text;
                if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                }
                if (para1 == "items_adic" && para2 == "todos" && para3 == "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                }
                if (para1 == "anag_cli" && para2 == "todos" && para3 == "" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // tipo doc
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // num doc
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // nombre
                }
                if (para1 == "contrat" && para4 == "")
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                }
                if (para1 == "detacon" && para2 != "" && para3 == "" && para4 == "")    // iddetacon,item,cant,nombre,medidas,madera,estado,saldo,coment,total,acabado
                {
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // item
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // cant
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // nombre
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // medidas
                    ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // madera
                    ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // estado
                    ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();    // saldo
                    ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();    // coment
                    ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();    // total
                    ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // acabado
                }
                if (para1 == "pedidos" && para2 == "pend" && para3 != "" && para4 == "")
                {
                    tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    //cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    tx_codigo.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // 
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // 
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // 
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // 
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // 
                    ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // 
                    ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // 
                    ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();   // 
                    ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();   // 
                    ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();   // 
                    ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // 
                    ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();   // 
                    ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();   // 
                    ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();   // 
                    ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();   // 
                    ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();   // 
                    ReturnValueA[16] = dataGridView1.CurrentRow.Cells[16].Value.ToString();   // 
                    ReturnValueA[17] = dataGridView1.CurrentRow.Cells[17].Value.ToString();   // 
                }
                if (para1 == "movim" && para2 == "pend" && para3 != "" && para4 == "")
                {
                    tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    //cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    tx_codigo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    tx_id.Text = dataGridView1.CurrentRow.Cells[13].Value.ToString();
                    ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // 
                    ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // 
                    ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // 
                    ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // 
                    ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // 
                    ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // 
                    ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // 
                    ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();   // 
                    ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();   // 
                    ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();   // 
                    ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // 
                    ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();   // 
                    ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();
                    ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();
                    ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();
                    ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();
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
                    string cols4 = "items,items_adic";         // busqueda en columna 1
                    string cols5 = "anag_cli,contrat";         // busqueda en columna 3
                    string colst = "detacon";                  // 
                    string col16 = "pedidos";                  // 16 columnas
                    string col14 = "movim";             // 14 columnas
                    {
                        if (colst.Contains(para1))
                        {
                            dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString(),
                                                row.ItemArray[8].ToString(),
                                                row.ItemArray[9].ToString(),
                                                row.ItemArray[10].ToString()
                                                );
                        }
                        if (cols4.Contains(para1))
                        {
                            if (row.ItemArray[1].ToString().ToLower().Contains(tx_buscar.Text.Trim().ToLower()))
                            {
                                dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString());
                            }
                        }
                        if (cols5.Contains(para1))
                        {
                            if (row.ItemArray[3].ToString().ToLower().Contains(tx_buscar.Text.Trim().ToLower()))
                            {
                                dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString()
                                                );
                            }
                        }
                        if (col16.Contains(para1))
                        {
                            if (row.ItemArray[3].ToString().ToLower().Contains(tx_buscar.Text.Trim().ToLower()))
                            {
                                dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString(),
                                                row.ItemArray[8].ToString(),
                                                row.ItemArray[9].ToString(),
                                                row.ItemArray[10].ToString(),
                                                row.ItemArray[11].ToString(),
                                                row.ItemArray[12].ToString(),
                                                row.ItemArray[13].ToString(),
                                                row.ItemArray[14].ToString(),
                                                row.ItemArray[15].ToString(),
                                                row.ItemArray[16].ToString(),
                                                row.ItemArray[17].ToString()
                                                );
                            }
                        }
                        if (col14.Contains(para1))
                        {
                            if (row.ItemArray[1].ToString().ToLower().Contains(tx_buscar.Text.Trim().ToLower()))
                            {
                                dataGridView1.Rows.Add(row.ItemArray[0].ToString(),
                                                row.ItemArray[1].ToString(),
                                                row.ItemArray[2].ToString(),
                                                row.ItemArray[3].ToString(),
                                                row.ItemArray[4].ToString(),
                                                row.ItemArray[5].ToString(),
                                                row.ItemArray[6].ToString(),
                                                row.ItemArray[7].ToString(),
                                                row.ItemArray[8].ToString(),
                                                row.ItemArray[9].ToString(),
                                                row.ItemArray[10].ToString(),
                                                row.ItemArray[11].ToString(),
                                                row.ItemArray[12].ToString(),
                                                row.ItemArray[13].ToString(),
                                                row.ItemArray[14].ToString(),
                                                row.ItemArray[15].ToString()
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
            if (para1 == "items" && para2 == "todos" && para3 == "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = cellva;
                tx_id.Text = "";
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // codigo
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // nombre
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // precio
            }
            if (para1 == "items_adic" && para2 == "todos" && para3 == "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = cellva;
                tx_id.Text = "";
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
            if (para1 == "anag_cli" && para2 == "todos" && para3 == "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // tipo doc
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // num doc
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // nombre
            }
            if (para1 == "contrat" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();    // nombre del cliente
                cellva = dataGridView1.CurrentRow.Cells[1].Value.ToString();            // id del cliente
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();    // numero de contrato
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();        // id del contrato
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // id
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // codigo
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // 
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // 
                if (dataGridView1.CurrentRow.Cells[4].Value == null) ReturnValueA[4] = "";
                else ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // 
                if (dataGridView1.CurrentRow.Cells[5].Value == null) ReturnValueA[5] = "";
                else ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // destino
                if (dataGridView1.CurrentRow.Cells[6].Value == null) ReturnValueA[6] = "";
                else ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // cod destino
                ReturnValueA[7] = (dataGridView1.CurrentRow.Cells[7].Value == null) ? "" : dataGridView1.CurrentRow.Cells[7].Value.ToString();   // 
            }
            if (para1 == "detacon" && para2 != "" && para3 == "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            if (para1 == "pedidos" && para2 == "pend" && para3 != "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                //cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // codped
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // origen
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // destino
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // cliente
                ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // cant
                ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // item
                ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // nombre
                ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();   // medidas
                ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();   // madera
                ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();   // estado
                ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // precio
                ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();   // total
                ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();   // nomad
                ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();   // acabado
                ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();   // nomorig
                ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();   // nomdestin
                ReturnValueA[16] = dataGridView1.CurrentRow.Cells[16].Value.ToString();   // contrato
                ReturnValueA[17] = dataGridView1.CurrentRow.Cells[17].Value.ToString();   // FECHA PEDIDO
            }
            if (para1 == "movim" && para2 == "pend" && para3 != "" && para4 == "")
            {
                tx_nombre.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                //cellva = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_codigo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                tx_id.Text = dataGridView1.CurrentRow.Cells[13].Value.ToString();
                ReturnValueA[0] = dataGridView1.CurrentRow.Cells[0].Value.ToString();   // 
                ReturnValueA[1] = dataGridView1.CurrentRow.Cells[1].Value.ToString();   // 
                ReturnValueA[2] = dataGridView1.CurrentRow.Cells[2].Value.ToString();   // 
                ReturnValueA[3] = dataGridView1.CurrentRow.Cells[3].Value.ToString();   // 
                ReturnValueA[4] = dataGridView1.CurrentRow.Cells[4].Value.ToString();   // 
                ReturnValueA[5] = dataGridView1.CurrentRow.Cells[5].Value.ToString();   // 
                ReturnValueA[6] = dataGridView1.CurrentRow.Cells[6].Value.ToString();   // 
                ReturnValueA[7] = dataGridView1.CurrentRow.Cells[7].Value.ToString();   // 
                ReturnValueA[8] = dataGridView1.CurrentRow.Cells[8].Value.ToString();   // 
                ReturnValueA[9] = dataGridView1.CurrentRow.Cells[9].Value.ToString();   // 
                ReturnValueA[10] = dataGridView1.CurrentRow.Cells[10].Value.ToString();   // 
                ReturnValueA[11] = dataGridView1.CurrentRow.Cells[11].Value.ToString();   // 
                ReturnValueA[12] = dataGridView1.CurrentRow.Cells[12].Value.ToString();   // 
                ReturnValueA[13] = dataGridView1.CurrentRow.Cells[13].Value.ToString();   // 
                ReturnValueA[14] = dataGridView1.CurrentRow.Cells[14].Value.ToString();   // }
                ReturnValueA[15] = dataGridView1.CurrentRow.Cells[15].Value.ToString();
            }
            iOMG.Program.retorna1 = cellva;
            tx_codigo.Focus();
        }
    }
}
