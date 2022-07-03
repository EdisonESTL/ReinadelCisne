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

        [ForeignKey(typeof(ListRMModel))]
        public int ListRMModelId { get; set; }

        public string NameRM { get; set; }
        public string UnitMeasurementRM { get; set; }
        public float CostoRM { get; set; }
        public double AmountRM { get; set; }
        public string TypeRM { get; set; }
    }
}
