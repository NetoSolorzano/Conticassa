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
    public partial class Main : Form1
    {
        publicoConf conf = new publicoConf();
             
        #region variables
        string img_log1 = @"C:\omg-peru\imAGENES\images6.jpg";
        //string img_sali = @"C:\iOMG - factur\recursos\exit24.png";
        //string img_fina = @"C:\iOMG - factur\recursos\excel32.png";
        //string img_cami = @"C:\iOMG - factur\recursos\process24.png";
        //string img_vali = @"C:\iOMG - factur\recursos\report16.png";
        //string img_maes = @"C:\iOMG - factur\recursos\Product-doc32.png";
        //string img_pcon = @"C:\iOMG - factur\recursos\service_manager.png";
        #endregion

        public Main()
        {
            InitializeComponent();
            jalainfo();                                 // jalamos los parametros, variables, etc
            Main_Load();                                // carga configuración del form
            cuadre();                                   // acomodo de botones verticales
            coloracion();                               // colores de botones y otros
        }
        private void Main_Load()
        {
            this.BackColor = Color.FromArgb(conf.fondoPrinRojoE, conf.fondoPrinVerdeE, conf.fondoPriAzulE); // conf.fondoPrinBrilloE, 

            Image logo1 = Image.FromFile(img_log1);     // logo del cliente - arriba a la izquierda
            Image salir = Resource1.shut_down40; // Image.FromFile(img_sali);     // icono del boton salir
            Image finan = Resource1.inbox_done40;  // Image.FromFile(img_fina);     // icono del boton finanzas
            Image webser = Resource1.wifi40;   // Image.FromFile(img_cami);     // icono del boton camiones
            Image valid = Resource1.fileboard_checklist40;   // Image.FromFile(img_vali);     // icono del boton validacion
            Image maest = Resource1.database_system40;  // Image.FromFile(img_maes);     // icono del boton maestras
            Image panel = Resource1.settings40;   // Image.FromFile(img_pcon);     // icono del boton panel de control
            pictureBox1.Image = logo1;
            bt_salir.Image = salir;
            bt_finan.Image = finan;
            bt_serWeb.Image = webser;
            bt_validac.Image = valid;
            bt_maestras.Image = maest;
            bt_pcontrol.Image = panel;
            //
            tx_user.Text = "Falta ";  // Program.vg_user;                     // código de usuario
            tx_nuser.Text = "Falta implementar";  //Program.vg_nuse;                    // nombre de usuario
            tx_empresa.Text = "Falta implementar";   // Program.cliente;                 // nombre de la organización
            //
            //pn_phor.Controls.Add(pn_menu);
            //pn_menu.Width = pn_phor.Width;  // - pn_acciones.Width;
            menuStrip1.Visible = true;
            pn_menu.Controls.Add(menuStrip1);
            menuStrip1.Dock = DockStyle.Top;
        }
        private void jalainfo()
        {

        }
        private void cuadre()
        {
            ControlBox = true;
            MaximizeBox = true;
            MinimizeBox = true;
            FormBorderStyle = FormBorderStyle.Sizable;  // FormBorderStyle.FixedSingle
            //Text = Program.tituloF;
            Left = Screen.PrimaryScreen.Bounds.Left;
            Top = Screen.PrimaryScreen.Bounds.Top;
            //Width = Screen.PrimaryScreen.Bounds.Width;
            //Height = Screen.PrimaryScreen.Bounds.Height;
            //
            bt_finan.Top = pictureBox1.Top + pictureBox1.Height + 0;
            bt_serWeb.Top = bt_finan.Top + bt_finan.Height + 0;
            bt_validac.Top = bt_serWeb.Top + bt_serWeb.Height + 0;
            //bt_almacen.Top = bt_pedidos.Top + bt_pedidos.Height + 0;
            bt_maestras.Top = bt_validac.Top + bt_validac.Height + 0;
            bt_pcontrol.Top = bt_maestras.Top + bt_maestras.Height + 0;
        }
        private void coloracion()
        {
            pn_centro.BackColor = Color.FromArgb(conf.fondoPrinRojoE,conf.fondoPrinVerdeE, conf.fondoPriAzulE); // conf.fondoPrinBrilloE,
            pn_pver.BackColor = Color.White; // Color.FromArgb(242, 243, 219);  // RGB
            bt_finan.BackColor = Color.White;
            bt_salir.BackColor = Color.White;
            bt_serWeb.BackColor = Color.White;
            bt_validac.BackColor = Color.White;
            bt_maestras.BackColor = Color.White;
            bt_pcontrol.BackColor = Color.White;
            pn_user.BackColor = Color.White;
            pn_menu.BackColor = Color.White;
            //pn_acciones.BackColor = Color.White;
        }

        #region botones_click   // menus
        #region finanzas
        private void bt_finan_Click(object sender, EventArgs e)
        {
            /*
            Image img_F1 = Image.FromFile(imgF1);
            Image img_F2 = Image.FromFile(imgF2);
            Image img_F3 = Image.FromFile(imgF3);
            Image img_F4 = Image.FromFile(imgF4);
            Image img_F5 = Image.FromFile(imgF5);
            */
            pic_icon_menu.Image = Resource1.inbox_done20;
            menuStrip1.Items.Clear();
            menuStrip1.Items.Add("Ingresos", Resource1.plus_circle20, fin_ingresos_Click);           // img_F1
            menuStrip1.Items.Add("Egresos", Resource1.minus_circle20, fin_egresos_Click);             // img_F2
            menuStrip1.Items.Add("Gastos Camiones", Resource1.truck_round20, fin_camion_Click);      // img_F3
            menuStrip1.Items.Add("Reportes", Resource1.file_arrow_down20, fin_reportes_Click);           // img_F5
            //
            menuStrip1.Visible = true;
        }
        private void fin_ingresos_Click(object sender, EventArgs e)
        {
            /*Form2 ffe1 = new Form2();
            ffe1.TopLevel = false;
            ffe1.Parent = this;
            //ffe1.Top = pn_phor.Top + pn_phor.Height + 1;
            ffe1.Left = pn_pver.Left + pn_pver.Width + 1;
            pn_centro.Controls.Add(ffe1);
            ffe1.Show();
            */
        }
        private void fin_egresos_Click(object sender, EventArgs e)
        {
            Finan_Egres ffe1 = new Finan_Egres();
            ffe1.TopLevel = false;
            ffe1.Parent = this;
            ffe1.Left = pn_pver.Left + pn_pver.Width + 1;
            pn_centro.Controls.Add(ffe1);
            ffe1.Show();
        }
        private void fin_camion_Click(object sender, EventArgs e)
        {

        }
        private void fin_reportes_Click(object sender, EventArgs e)
        {

        }
        #endregion
        private void bt_salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void Main_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var aa = MessageBox.Show("Realmente desea salir del sistema?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
