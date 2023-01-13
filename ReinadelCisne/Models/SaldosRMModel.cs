using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class SaldosRMModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double ValorUnitario { get; set; }
        public double Cantidad { get; set; }
        public double SaldoTotal { get; set; }

        [ForeignKey(typeof(KardexRMModel))]
        public int KardexRMModelId { get; set; }
        [ManyToOne]
        public KardexRMModel KardexRMModel { get; set; }

        //VAlores de reconocimiento
        public int IdReconcimiento { get; set; }
        public string NombreReconocimiento { get; set; }
    }
}
