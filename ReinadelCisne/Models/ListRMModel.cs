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

        [ForeignKey(typeof(ProductModel))]
        public int ProducID { get; set; }
        [OneToOne]
        public ProductModel Product { get; set; }

        [OneToMany("ListRMModelId")]
        public List<ItemsListRMModel> ListMaterialxProduct { get; set; }
    }
}
