using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class KardexRMModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        //Relacion con materia prima
        [ForeignKey(typeof(RawMaterialModel))]
        public int IdRawMaterial { get; set; }
        [OneToOne]
        public RawMaterialModel RawMaterialModell { get; set; }

        //Relacion con compras
        /*[ForeignKey(typeof(ShoppingListModel))]
        public int IdShopping { get; set; }*/
        [OneToMany]
        public List<ShoppingListModel> ShoppingModell { get; set; }

        //Relacion con ventas
        /*[ForeignKey(typeof(OrderModel))]
        public int IdOrder { get; set; }
        [OneToOne]
        public OrderModel OrderModel { get; set; }*/

        //Relación con Consumo materia prima
        [OneToMany("RawMaterialModelId")]
        public List<ItemsListRMModel> itemsListRMs { get; set; }

        public double ValorUnitario { get; set; }
        public double Cantidad { get; set; }
        public double Valor { get; set; }
    }
}
