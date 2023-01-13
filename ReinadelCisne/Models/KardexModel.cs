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
        public double Valor { get; set; }
        public int Cantidad { get; set; }
        public double ValorPromPond { get; set; }


        //Relacion con producto
        [ForeignKey(typeof(ProductModel))]
        public int IdProduct { get; set; }
        [ManyToOne]
        public ProductModel ProductModel { get; set; }

        //Relacion con ventas
        [ForeignKey(typeof(OrderModel))]
        public int IdOrder { get; set; }
        [OneToOne]
        public OrderModel OrderModel { get; set; }

        
    }
}
