using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ShoppingModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime ShoppingDate { get; set; }
        public string NameEstablishment { get; set; }
        public string InvoiceNumber { get; set; }
        public float TotalShop { get; set; }
        
        [OneToMany("ShoppingModelId")]
        public List<ShoppingListModel> ShoppingRaw { get; set; }
    }
}
