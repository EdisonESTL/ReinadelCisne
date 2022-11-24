using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class GroupsRMModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }

        [OneToMany("UGroupModel")]
        public List<RawMaterialModel> rawMaterialModels { get; set; }
    }
}
