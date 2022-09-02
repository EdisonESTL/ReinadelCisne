using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class OtherCostModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string DescriptionOC { get; set; }
        public double CostOC { get; set; }

        [ForeignKey(typeof(ListOCModel))]
        public int ListOCModelId { get; set; }
        [ManyToOne]
        public ListOCModel ListOCModel { get; set; }
    }
}
