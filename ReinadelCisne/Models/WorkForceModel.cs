using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class WorkForceModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Profesional { get; set; }
        public int Amount { get; set; }
        public double UnitSalary { get; set; }

        [ForeignKey(typeof(ListWFModel))]
        public int ListWFModelId { get; set; }
    }
}
