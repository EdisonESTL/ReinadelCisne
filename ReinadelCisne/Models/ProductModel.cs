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


        [ManyToMany(typeof(OrderModel))]
        public List<SaleModel> Sales { get; set; }


        [ForeignKey(typeof(ListRMModel))]
        public int ListRMModelId { get; set; }

        [OneToOne]
        public ListRMModel ListRMModel { get; set; }


        [ForeignKey(typeof(ListWFModel))]
        public int ListWFModelId { get; set; }

        [OneToOne]
        public ListWFModel ListWFModel { get; set; }


        [ForeignKey(typeof(ListOCModel))]
        public int ListOCModelId { get; set; }

        [OneToOne]
        public ListOCModel ListOCModel { get; set; }
    }
}
