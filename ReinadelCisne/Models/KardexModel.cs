using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class KardexModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Valor { get; set; } //Valor Total
        public int Cantidad { get; set; }
        public double ValorPromPond { get; set; } //Valor unitario

        //Relacion con producto
        [ForeignKey(typeof(ProductModel))]
        public int IdProduct { get; set; }
        [OneToOne]
        public ProductModel ProductModel { get; set; }

        //Relacion con ventas
        [OneToMany("KardexProductModelId")]
        public List<OrderModel> OrderModel { get; set; }

        //Relacion con saldos
        [OneToMany("KardexProductModelId")]
        public List<SaldosKardexProductModel> SaldosProducts { get; set; }
    }
}
