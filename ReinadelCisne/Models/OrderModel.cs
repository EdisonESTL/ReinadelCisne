using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class OrderModel
    {
        [ForeignKey(typeof(SaleModel))]
        public int SaleModeld { get; set; }

        [ForeignKey(typeof(ProductModel))]
        public int ProductModelId { get; set; }
   
        public int AmountProduct { get; set; }
    }
}
