using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ItemsListRMModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Amount { get; set; }    //Cantidad necesaria 
        public double UnitCost { get; set; }     //Unidades necesarias
        public double TotalCost { get; set; }

        [ForeignKey(typeof(ListRMModel))]
        public int ListRMModelId { get; set; }
        [ManyToOne]
        public ListRMModel ListRMModel { get; set; }

        [ForeignKey(typeof(KardexRMModel))]
        public int RawMaterialModelId { get; set; }
        [ManyToOne]
        public KardexRMModel KardexRMModel { get; set; }


    }
}
