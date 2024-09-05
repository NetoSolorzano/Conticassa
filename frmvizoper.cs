using System;
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Forms;

namespace Conticassa
{
    public partial class frmvizoper : Form
    {
        DataSet1 _datosReporte;

        private frmvizoper()
        {
            InitializeComponent();
        }

        public frmvizoper(DataSet1 datos): this()
        {
            _datosReporte = datos;
        }

        private void frmvizoper_Load(object sender, EventArgs e)
        {
            if (_datosReporte.repSaldoIni_.Rows.Count > 0)
            {
                string nf = _datosReporte.repSaldoIni_.Rows[0].ItemArray[0].ToString();
                ReportDocument rpt = new ReportDocument();
                rpt.Load(nf);   // nf = ruta y nombre del CR
                rpt.SetDataSource(_datosReporte);
                crystalReportViewer1.ReportSource = rpt;
            }
        }
    }
}
