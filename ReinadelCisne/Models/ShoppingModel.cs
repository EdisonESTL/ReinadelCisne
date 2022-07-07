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
        public DateTime DateShop { get; set; }
        public string NameEstablishment { get; set; }
        public string InvoiceNumber { get; set; }
        public float TotalShop { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
        public List<RawMaterialShModel> rawMaterialShops { get; set; }
    }
}
