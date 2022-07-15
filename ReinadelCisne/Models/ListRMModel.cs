using System;
using System.Collections.Generic;
using System.Text;
using ReinadelCisne.Models;
using SQLiteNetExtensions.Attributes;
using SQLite;

namespace ReinadelCisne.Models
{
    [Table("ListRawMaterial")]
    public class ListRMModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public float Total { get; set; }
        
        [OneToMany("ListRMModelId")]
        public List<ItemsListRMModel> itemsListRMModels { get; set; }
    }
}
