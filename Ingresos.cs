﻿using MySql.Data.MySqlClient;
using System;

namespace Conticassa
{
    class Ingresos
    {
        private string tipMovPrin;  // Tipo de movimiento, OMG o PERSONAL
        private int idOper;         // Id de la operación, solo para operaciones <> nuevo
        private string fechOper;    // fecha de la operación
        private decimal tipCamb;    // tipo de cambio de la operación si fue en moneda <> a sol
        private string descrip;     // descripcion de la operacion
        private catEgresos catIngreso;   // Categoría del Ingreso
        private monedas moneda;         // Moneda de la operación
        private cajDestino cajaDes;     // caja destino de la operación, caja desde donde sale el dinero
        private string idMovim;         // idmovimiento
        private montos monto;           // Valor de la operación

        public Ingresos()
        {

        }

        public string TipMovPrin { get => tipMovPrin; set => tipMovPrin = value; }
        public int IdOper { get => idOper; set => idOper = value; }
        public string FechOper { get => fechOper; set => fechOper = value; }
        public monedas Moneda { get => moneda; set => moneda = value; }
        public montos Monto { get => monto; set => monto = value; }
        public decimal TipCamb { get => tipCamb; set => tipCamb = value; }
        public cajDestino CajaDes { get => cajaDes; set => cajaDes = value; }
        public string Descrip { get => descrip; set => descrip = value; }
        public catEgresos CatIngreso { get => catIngreso; set => catIngreso = value; }
        public string IdMovim { get => idMovim; set => idMovim = value; }

        public void creaIngreso(string _tipMovPrin, string _fechOper, catEgresos _catIngreso, monedas _moneda, montos _monto, decimal _tipCamb,
            cajDestino _cajaDes, string _descrip, string _IdMovim)
        {
            tipMovPrin = _tipMovPrin;
            fechOper = _fechOper;
            catIngreso = _catIngreso;
            moneda = _moneda;
            monto = _monto;
            tipCamb = _tipCamb;
            cajaDes = _cajaDes;
            descrip = _descrip;
            idMovim = _IdMovim;
        }

        public void limpia()
        {
            tipMovPrin = "";
            fechOper = "";
            catIngreso = null;
            moneda = null;
            monto = null;
            tipCamb = 0;
            cajaDes = null;
            descrip = "";
            idMovim = "";
        }

        public void grabaIngreso(MySqlConnection conn)
        {
            string tabla = "";
            if (tipMovPrin == "omg") tabla = "cassaomg";
            else tabla = "cassaconti";
            string consulta = "insert into @tab (IDBanco,Anno,IDMovimento,DataMovimento,IDConto,IDCategoria,ImportoDE,ImportoSE,ImportoDU," +
                                "ImportoSU,Cambio,Descrizione,IDGiroConto,monori,ctaori,ctades,usuario,dia,idanagrafica,tipodesgiro) values (" +
                                "@IDB,@Ann,@IDM,@DMo,@IDCo,@IDCa,@IDE,@ISE,@IDU,@ISU,@Cam,@Des,@IDG,@mon,@ctao,@ctad,@usua,now(),@idan,@tidgiro)";
            using (MySqlCommand micon = new MySqlCommand(consulta, conn))
            {
                micon.Parameters.AddWithValue("@tab", tabla);
                micon.Parameters.AddWithValue("@IDB", "LIM");   // este campo viene de donde ??? arreglar
                micon.Parameters.AddWithValue("@Ann", DateTime.Now.Year);   // tabla "contatori" debe autoiniciarse al cambiar el año, poner disparador en el login
                micon.Parameters.AddWithValue("@IDM", IdMovim); // este dato viene del metodo "grabar", '00'+contador de la tabla contatori
                micon.Parameters.AddWithValue("@DMo", fechOper);
                micon.Parameters.AddWithValue("@IDCo", cajaDes.codigo);
                micon.Parameters.AddWithValue("@IDCa", catIngreso.codigo);
                micon.Parameters.AddWithValue("@IDE", 0);                   // importe en dolares entrada 
                micon.Parameters.AddWithValue("@ISE", 0);                   // importe en soles entrada
                micon.Parameters.AddWithValue("@IEE", 0);                   // importe en euros entrada
                micon.Parameters.AddWithValue("@IDU", monto.monDolar);    // importe en dolares salida 
                micon.Parameters.AddWithValue("@ISU", monto.monSoles);    // importe en soles salida
                micon.Parameters.AddWithValue("@IEU", monto.monEuros);    // importe en euros salida 
                micon.Parameters.AddWithValue("@Cam", tipCamb);             // tipo de cambio
                micon.Parameters.AddWithValue("@Des", descrip);
                micon.Parameters.AddWithValue("@IDG", "");  // luego vemos esto
                micon.Parameters.AddWithValue("@mon", monto.codMOrige);    // codigo de la moneda origen de la operación
                micon.Parameters.AddWithValue("@ctao", ""); // esto va con el giroconto creo
                micon.Parameters.AddWithValue("@ctad", ""); // esto va con el giroconto creo
                micon.Parameters.AddWithValue("@usua", Program.vg_user);
                micon.Parameters.AddWithValue("@tidgiro", "");   // esto va con el giroconto creo
                micon.ExecuteNonQuery();
            }
        }
    }
}
