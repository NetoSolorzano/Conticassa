using System;
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Forms;

namespace Conticassa
{
    public partial class frmvizoper : Form
    {
        conClie _datosReporte;

        private frmvizoper()
        {
            InitializeComponent();
        }

        public frmvizoper(conClie datos): this()
        {
            _datosReporte = datos;
        }

        private void frmvizoper_Load(object sender, EventArgs e)
        {
            if (_datosReporte.cuadreCaja_cab.Rows.Count > 0)
            {
                string nf = _datosReporte.cuadreCaja_cab.Rows[0].ItemArray[27].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);   // rpt.Load("formatos/cuadreCaja1.rpt");
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.pendCob.Rows.Count > 0)
            {
                string nf = _datosReporte.pendCob.Rows[0].ItemArray[28].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);  // rpt.Load("formatos/pendCob1.rpt");
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.repCobsem.Rows.Count > 0)
            {
                string nf = _datosReporte.repCobsem.Rows[0].ItemArray[10].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.placar_cab.Rows.Count > 0)
            {
                string nf = _datosReporte.placar_cab.Rows[0].ItemArray[0].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);    // rpt.Load("formatos/plancarga2.rpt");
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.gr_ind_cab.Rows.Count > 0)
            {
                string nf = _datosReporte.gr_ind_cab.Rows[0].ItemArray[0].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.ctacteclte.Rows.Count > 0)
            {
                string nf = _datosReporte.ctacteclte.Rows[0].ItemArray[7].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.gr_ind_cab.Rows.Count > 0)
            {
                string nf = _datosReporte.gr_ind_cab.Rows[0].ItemArray[0].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.cVta_cab.Rows.Count > 0)
            {
                string nf = _datosReporte.cVta_cab.Rows[0].ItemArray[0].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
            if (_datosReporte.cNot_cred.Rows.Count > 0)
            {
                string nf = _datosReporte.cNot_cred.Rows[0].ItemArray[1].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
        }
    }
}
