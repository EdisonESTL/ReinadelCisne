using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class GroupsProductModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }

        [OneToMany("GroupProductId")]
        public List<ProductModel> Products { get; set; }
    }
}
