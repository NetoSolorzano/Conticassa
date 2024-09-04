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
        public Finan_reps()
        {
            InitializeComponent();
            oFEgres.colorea(this, "#38c497", "#7de4c3", "#dff5ee");   // pinta el mundo de colores
            cargaDatos();
            pan_menu.Enabled = false;
            pan_repos.Enabled = false;

        }
        #region combos
        private void cargaDatos()
        {
            // casa o sede
            cmb_sede.Items.Add("OMG");
            cmb_sede.Items.Add("PER");
            // monedas
            DataRow[] depar = Program.dt_definic.Select("idtabella='MON' and numero=1");
            cmb_moneda.DataSource = depar.CopyToDataTable();
            cmb_moneda.DisplayMember = "descrizionerid";
            cmb_moneda.ValueMember = "idcodice";
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

    }
}
