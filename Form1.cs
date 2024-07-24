using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conticassa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
            this.BackColor = Color.FromArgb(8,253,254,243);  // brillo,rojo,verde,azul rgba(253, 254, 243, 0.8)
            this.Text = "CONTICASSA V.3 - 2024";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
    public class generalTextBox : TextBox
    {
        publicoConf conf = new publicoConf();
        public generalTextBox()
        {
            Font = new Font(conf.nombreFont, conf.tamañoFont);
            BackColor = Color.FromName(conf.nombreFondo);
            ForeColor = Color.FromName(conf.colorFont);
            BorderStyle = BorderStyle.None;
        }
        bool allowSpace = false;
    }
    public class NumericTextBox : TextBox
    {
        publicoConf conf = new publicoConf();
        
        public NumericTextBox()
        {
            Font = new Font(conf.nombreFont, conf.tamañoFont);
            BackColor = Color.FromName(conf.nombreFondo); // Color.Aqua;
            ForeColor = Color.FromName(conf.colorFont);
            BorderStyle = BorderStyle.None;
        }
        bool allowSpace = false;
        // Restricts the entry of characters to digits (including hex), the negative sign,
        // the decimal point, and editing keystrokes (backspace).
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
             keyInput.Equals(negativeSign))
            {
                // Decimal separator is OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            //    {
            //     // Let the edit control handle control and alt key combinations
            //    }
            else if (this.allowSpace && e.KeyChar == ' ')
            {

            }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }
        /*
        public int IntValue
        {
            get
            {
                return Int32.Parse(this.Text);
            }
        }
        public decimal DecimalValue
        {
            get
            {
                return Decimal.Parse(this.Text);
            }
        }
        public bool AllowSpace
        {
            set
            {
                this.allowSpace = value;
            }

            get
            {
                return this.allowSpace;
            }
        }
        */
    }
    public class generalEtiqueta : Label
    {
        publicoConf conf = new publicoConf();
        public generalEtiqueta()
        {
            Font = new Font(conf.nombreFont, conf.tamañoFont);
            BackColor = Color.FromArgb(conf.fondoRojoE, conf.fondoVerdeE, conf.fondoAzulE);  // FromName(conf.fondoEtiq);
            ForeColor = Color.FromName(conf.colorFont);
        }
        bool allowSpace = false;
    }
    public class generalBoton : Button
    {
        publicoConf conf = new publicoConf();
        public generalBoton()
        {
            this.BackColor = Color.FromName(conf.colorfondoBoton);
            this.FlatStyle = FlatStyle.Popup;
            this.Text = "";
            this.Font = new Font(conf.nomFontBoton, conf.tamañoFontBoton);
        }
    }
    public class panelGeneral : Panel
    {
        publicoConf conf = new publicoConf();
        public panelGeneral()
        {
            BackColor = Color.FromName(conf.nombreFondo);
            ForeColor = Color.FromName(conf.colorFont);
            BorderStyle = BorderStyle.FixedSingle;
        }
    }
    public class radioBoton : RadioButton
    {
        publicoConf conf = new publicoConf();
        public radioBoton()
        {
            BackColor = Color.FromName(conf.nombreFondo);
            ForeColor = Color.FromName(conf.colorFont);
            Font = new Font(conf.nombreFont, conf.tamañoFont);
        }
    }
    public class selecFecha : DateTimePicker
    {
        public selecFecha()
        {
            Format = DateTimePickerFormat.Short;
        }
    }
}
