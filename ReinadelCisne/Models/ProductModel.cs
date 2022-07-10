using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    [Table("Product")]
    public class ProductModel
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public int UnitProduct { get; set; }
        public double UtilityProduct { get; set; }
        public double PriceProduct { get; set; }

        //Relacion con OrderModel
        [OneToMany("ProductModelId")]
        public List<OrderModel> Orders { get; set; }

        //Relacion con ListRMModel
        [ForeignKey(typeof(ListRMModel))]
        public int ListRMModelId { get; set; }
        [OneToOne]
        public ListRMModel ListRMModel { get; set; }

        //Relación con ListWFModel
        [ForeignKey(typeof(ListWFModel))]
        public int ListWFModelId { get; set; }
        [OneToOne]
        public ListWFModel ListWFModel { get; set; }

        //Relacion con ListOCModel
        [ForeignKey(typeof(ListOCModel))]
        public int ListOCModelId { get; set; }
        [OneToOne]
        public ListOCModel ListOCModel { get; set; }
    }
}
