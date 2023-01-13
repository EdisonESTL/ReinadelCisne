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
        public float Total { get; set; }

        [ForeignKey(typeof(ProductModel))]
        public int ProducID { get; set; }
        [OneToOne]
        public ProductModel Product { get; set; }

        [OneToMany("ListWFModelId")]
        public List<PersonalModel> PersonalxProduct { get; set; }
    }
}
