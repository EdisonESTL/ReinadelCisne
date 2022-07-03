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

        [OneToMany("ListOCModelId")]
        public List<OtherCostModel> OtherCosts { get; set; }
    }
}
