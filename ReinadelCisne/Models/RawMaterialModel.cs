using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class RawMaterialModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }           
        public double AmountRM { get; set; }     //Cantidad en Inventario
        public string NameRM { get; set; }     //Description
        public string UnitMeasurementRM { get; set; }
        public float CostoRM { get; set; }      //Costo Unitario ponderado
        public float TotalCost { get; set; }
        public string TypeRM { get; set; }

        [OneToMany("RawMaterialModelId")]
        public List<ShoppingListModel> shoppingList { get; set; }

        [OneToMany("RawMaterialModelId")]
        public List<ItemsListRMModel> itemsListRMs { get; set; }
    }
}
