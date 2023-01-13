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
        public double ValorUnitario { get; set; }
        public double Cantidad { get; set; }
        public double Valor { get; set; }

        //Relacion con materia prima
        [ForeignKey(typeof(RawMaterialModel))]
        public int IdRawMaterial { get; set; }
        [OneToOne]
        public RawMaterialModel RawMaterialModell { get; set; }
        
        [OneToMany("KardexRMModelId")]
        public List<ShoppingListModel> ShoppingModell { get; set; }        

        //Relación con Consumo materia prima
        [OneToMany("RawMaterialModelId")]
        public List<ItemsListRMModel> itemsListRMs { get; set; }

        [OneToMany("KardexRMModelId")]
        public List<SaldosRMModel> SaldosRMs { get; set; }

    }
}
