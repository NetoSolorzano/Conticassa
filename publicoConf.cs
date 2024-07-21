using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conticassa
{
    public class publicoConf
    {
        // color de fondo principal
        public int fondoPrinBrilloE = 8;  // rgba(253, 254, 243, 0.8)
        public int fondoPrinRojoE = 253;
        public int fondoPrinVerdeE = 254;
        public int fondoPriAzulE = 243;
        #region font de etiquetas y textbox
        public string nombreFont = "Verdana";
        public string nombreFondo = "White";
        public string colorFont = "Blue";
        //public string fondoEtiq = "Aquamarine";
        // etiquetas          // Color.FromArgb(142,242,243,219);  // brillo,rojo,verde,azul
        public int fondoBrilloE = 142;  
        public int fondoRojoE = 242;
        public int fondoVerdeE = 243;
        public int fondoAzulE = 219;
        public int tamañoFont = 9;
        #endregion
        #region font de botones
        public string nomFontBoton = "Arial";
        public string colorFontBoton = "Black";
        public int tamañoFontBoton = 11;
        public string colorfondoBoton = "Green";
        public int fondoBrilloB = 55;
        public int fondoRojoB = 168;
        public int fondoVerdeB = 239;
        public int fondoAzulB = 131;

        #endregion
        
    }
}
