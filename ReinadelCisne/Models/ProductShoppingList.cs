using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Models
{
    public class ProductShoppingList
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }    //Cantidad comprada 
        public double ValorUnitario { get; set; }     //Unidades compradas
        public double TotalCost { get; set; }

        [ForeignKey(typeof(ProductModel))]
        public int ProductModelId { get; set; }
        [ManyToOne]
        public ProductModel ProductModel { get; set; }

        [ForeignKey(typeof(ProductShoppingModel))]
        public int ShoppingModelId { get; set; }
        [ManyToOne]
        public ProductShoppingModel ProductShopping { get; set; }
    }
}
