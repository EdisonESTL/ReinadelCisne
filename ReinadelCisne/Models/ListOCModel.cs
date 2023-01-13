using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ListOCModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public float Total { get; set; }

        [ForeignKey(typeof(ProductModel))]
        public int ProductID { get; set; }
        [OneToOne]
        public ProductModel Product { get; set; }

        [OneToMany("ListOCModelId")]
        public List<OtherCostModel> OtherCostsxProduct { get; set; }
    }
}
