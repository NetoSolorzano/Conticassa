using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conticassa
{
    public class camiones
    {
        private string placa;
        private int idOper;
        private string fechOper;        // fecha de la operación
        private cajDestino cajaDes;     // caja destino de la operación, caja desde donde sale el dinero
        private string bcoOrigen;
        private string bcoDestin;
        private monedas moneda;         // Moneda de la operación
        private decimal tipCamb;    // tipo de cambio de la operación si fue en moneda <> a sol
        private string descrip;     // descripcion de la operacion
        private decimal combust;    // importe gasto combustible
        private decimal viaticos;   // importe gasto viativos
        private decimal respuest;
        private decimal varios;
        private decimal honorar;    // gastos de honorarios 
        private decimal impuests;   // gastos por impuestos del camion
        private decimal totalS;     // gran total en soles

        public camiones()
        {

        }

        public string Placa { get => placa; set => placa = value; }
        public int IdOper { get => idOper; set => idOper = value; }
        public string FechOper { get => fechOper; set => fechOper = value; }
        public cajDestino CajaDes { get => cajaDes; set => cajaDes = value; }
        public string BcoOrigen { get => bcoOrigen; set => bcoOrigen = value; }
        public string BcoDestin { get => bcoDestin; set => bcoDestin = value; }
        public monedas Moneda { get => moneda; set => moneda = value; }
        public decimal TipCamb { get => tipCamb; set => tipCamb = value; }
        public string Descrip { get => descrip; set => descrip = value; }
        public decimal Combust { get => combust; set => combust = value; }
        public decimal Viaticos { get => viaticos; set => viaticos = value; }
        public decimal Respuest { get => respuest; set => respuest = value; }
        public decimal Varios { get => varios; set => varios = value; }
        public decimal Honorar { get => honorar; set => honorar = value; }
        public decimal Impuests { get => impuests; set => impuests = value; }
        public decimal TotalS { get => totalS; set => totalS = value; }

        public void creaCamion(string _placa, int _idoper, string _fecha, cajDestino _destino,
            string _bcoOrigen, string _bcoDestin, monedas _moneda, decimal _tipCamb, string _descrip,
            decimal _combust, decimal _viaticos, decimal _respuest, decimal _varios, decimal _honorar, 
            decimal _impuests, decimal _totalS)
        {
            placa = _placa;
            idOper = _idoper;
            fechOper = _fecha;
            cajaDes = _destino;
            bcoOrigen = _bcoOrigen;
            bcoDestin = _bcoDestin;
            moneda = _moneda;
            tipCamb = _tipCamb;
            descrip = _descrip;
            combust = _combust;
            viaticos = _viaticos;
            respuest = _respuest;
            varios = _varios;
            honorar = _honorar;
            impuests = _impuests;
            totalS = _totalS;
        }
        public void limpia()
        {
            placa = "";
            idOper = 0;
            fechOper = "";        // fecha de la operación
            cajaDes = null;     // caja destino de la operación, caja desde donde sale el dinero
            bcoOrigen = "";
            bcoDestin = "";
            moneda = null;         // Moneda de la operación
            tipCamb = 0;    // tipo de cambio de la operación si fue en moneda <> a sol
            descrip = "";     // descripcion de la operacion
            combust = 0;    // importe gasto combustible
            viaticos = 0;   // importe gasto viativos
            respuest = 0;
            varios = 0;
            honorar = 0;    // gastos de honorarios 
            impuests = 0;   // gastos por impuestos del camion
            totalS = 0;
        }
    public void grabaCamion(MySqlConnection conn) 
        {

        }
    }
}
