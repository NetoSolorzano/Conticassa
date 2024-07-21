using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conticassa
{
    public class Program
    {
        #region Variables globales
        public string vg_user = ""; // codigo del usuario
        public string vg_nuse = ""; // nombre del usuario
        public string cliente = ""; // nombre del cliente usuario del sistema
        #endregion
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
