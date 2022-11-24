using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Models
{
    public class ProductShoppingModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime ShoppingDate { get; set; }
        public string NameEstablishment { get; set; }
        public string InvoiceNumber { get; set; }
        public float TotalShop { get; set; }

        [OneToMany("ShoppingModelId")]
        public List<ProductShoppingList> ShoppingLists { get; set; }
    }
}
