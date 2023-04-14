using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class SaldosKardexProductModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double ValorUnitario { get; set; }
        public double Cantidad { get; set; }
        public double SaldoTotal { get; set; }

        [ForeignKey(typeof(KardexModel))]
        public int KardexProductModelId { get; set; }
        [ManyToOne]
        public KardexModel KardexProductModel { get; set; }

        //VAlores de reconocimiento
        public int IdReconcimiento { get; set; }
        public string NombreReconocimiento { get; set; }
    }
}
