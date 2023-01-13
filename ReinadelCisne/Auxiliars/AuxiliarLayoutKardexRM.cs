using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Auxiliars
{
    public class AuxiliarLayoutKardexRM
    {
        public DateTime Date { get; set; }
        public string  Detail { get; set; }
        public double Discount { get; set; }
        public double EntradaCantidad { get; set; }
        public double EntradaValorUnitario { get; set; }
        public double EntradaValorTotal { get; set; }
        public double SalidaCantidad { get; set; }
        public double SalidaValorUnitario { get; set; }
        public double SalidaValorTotal { get; set; }
        public double SaldoCantidad { get; set; }
        public double SaldoValorUnitario { get; set; }
        public double SaldoTotal { get; set; }
    }
}
