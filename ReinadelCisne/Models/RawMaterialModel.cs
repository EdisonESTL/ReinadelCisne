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
        public DateTime DateTime { get; set; }
        public string NameRM { get; set; }     //Description
        public string DescriptionRM { get; set; }
        public string LocationRM { get; set; }
        public string ProductSupplier { get; set; }        
        public int MinimalExistence { get; set; }
        public int MaximumExistence { get; set; }
        //public double AmountRM { get; set; }     //Cantidad en Inventario
        public double CantidadRM { get; set; }
        public string UnitMeasurementRM { get; set; }
        public string TypeRM { get; set; }
        public float CostoRM { get; set; }      //Costo Unitario ponderado
        public float TotalCost { get; set; }

        //Relacion inversa con UMedidasRMModel
        [ForeignKey(typeof(UMedidasRMModel))]
        public int UMedidaModel { get; set; }
        [ManyToOne]
        public UMedidasRMModel UMedidaRM { get; set; }

        //Relacion inversa con GroupsRMModel
        [ForeignKey(typeof(GroupsRMModel))]
        public int UGroupModel { get; set; }
        [ManyToOne]
        public GroupsRMModel GroupRM { get; set; }

        /*[OneToMany("RawMaterialModelId")]
        public List<ShoppingListModel> shoppingList { get; set; }*/

        /*[OneToMany("RawMaterialModelId")]
        public List<ItemsListRMModel> itemsListRMs { get; set; }*/
    }
}
