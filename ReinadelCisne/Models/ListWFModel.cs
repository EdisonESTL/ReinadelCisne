using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ListWFModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [OneToMany("ListWFModelId")]
        public List<WorkForceModel> WorkForces { get; set; }
    }
}
