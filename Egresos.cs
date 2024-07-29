using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conticassa
{
    public class Egresos
    {
        private string tipMovPrin;  // Tipo de movimiento, OMG o PERSONAL
        private int idOper;         // Id de la operación, solo para operaciones <> nuevo
        private string fechOper;    // fecha de la operación
        private decimal monto;      // Valor de la operación
        private decimal tipCamb;    // tipo de cambio de la operación si fue en moneda <> a sol
        private string descrip;     // descripcion de la operacion
        private catEgresos catEgreso;   // Categoría del egreso
        private monedas moneda;         // Moneda de la operación
        private cajDestino cajaDes;     // caja destino de la operación, caja desde donde sale el dinero
        private provees proveedor;      // provedor del servicio o bien, es opcional

        public Egresos()
        {

        }

        public string TipMovPrin { get => tipMovPrin; set => tipMovPrin = value; }
        public int IdOper { get => idOper; set => idOper = value; }
        public string FechOper { get => fechOper; set => fechOper = value; }
        public monedas Moneda { get => moneda; set => moneda = value; }
        public decimal Monto { get => monto; set => monto = value; }
        public decimal TipCamb { get => tipCamb; set => tipCamb = value; }
        public cajDestino CajaDes { get => cajaDes; set => cajaDes = value; }
        public provees Proveedor { get => proveedor; set => proveedor = value; }
        public string Descrip { get => descrip; set => descrip = value; }
        public catEgresos CatEgreso { get => catEgreso; set => catEgreso = value; }

        public void creaEgreso(string _tipMovPrin, string _fechOper, catEgresos _catEgreso, monedas _moneda, decimal _monto, decimal _tipCamb, cajDestino _cajaDes, provees _proveedor, string _descrip)
        {
            tipMovPrin = _tipMovPrin;
            fechOper = _fechOper;
            CatEgreso = _catEgreso;
            moneda = _moneda;
            monto = _monto;
            tipCamb = _tipCamb;
            cajaDes = _cajaDes;
            proveedor = _proveedor;
            descrip = _descrip;
        }
        public void grabaEgreso(MySqlConnection conn)
        {
            string consulta = "insert into () values ()";
            using (MySqlCommand micon = new MySqlCommand(consulta, conn))
            {

            }
        }
    }
}
