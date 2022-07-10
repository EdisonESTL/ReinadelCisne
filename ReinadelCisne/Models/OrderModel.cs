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

        //Relacion con SaleModel
        [ForeignKey(typeof(SaleModel))]
        public int SaleModeld { get; set; }

        //Relacion con ProductModel
        [ForeignKey(typeof(ProductModel))]
        public int ProductModelId { get; set; }
   
        public int AmountProduct { get; set; }
    }
}
