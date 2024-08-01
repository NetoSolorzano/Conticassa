using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace Conticassa
{
    public class Program
    {
        #region Variables globales
        public static string vg_user = ""; // codigo del usuario
        public static string vg_nuse = ""; // nombre del usuario

        public static DataTable dt_definic = new DataTable();    // definiciones
        public static DataTable dt_enlaces = new DataTable();    // enlaces
        #endregion
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
