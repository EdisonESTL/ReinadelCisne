using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class SaleModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DateSale { get; set; }
        public double TotalSale { get; set; }

        [ManyToMany(typeof(OrderModel))]
        public List<ProductModel> Orders {get; set;}
    }
}
