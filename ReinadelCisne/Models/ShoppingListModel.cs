using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ShoppingListModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }        
        public double Amount { get; set; }    //Cantidad comprada 
        public double UnitCost { get; set; }     //Unidades compradas
        public double TotalCost { get; set; }

        [ForeignKey(typeof(ShoppingModel))]
        public int ShoppingModelId { get; set; }
        [ManyToOne]
        public ShoppingModel ShoppingModel { get; set; }

        [ForeignKey(typeof(RawMaterialModel))]
        public int RawMaterialModelId { get; set; }
        [ManyToOne]
        public RawMaterialModel RawMaterial { get; set; }
    }
}
