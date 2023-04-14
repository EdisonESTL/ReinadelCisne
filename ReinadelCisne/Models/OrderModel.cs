using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class OrderModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AmountProduct { get; set; }
        public double ValorUnitario { get; set; }
        public double Valor { get; set; }

        //Relacion con SaleModel
        [ForeignKey(typeof(SaleModel))]
        public int SaleModeld { get; set; }
        [ManyToOne]
        public SaleModel SaleModel { get; set; }

        //Relacion con ProductModel
        [ForeignKey(typeof(KardexModel))]
        public int KardexProductModelId { get; set; }
        [ManyToOne]
        public KardexModel KardexModel {get;set;}
   
    }
}
