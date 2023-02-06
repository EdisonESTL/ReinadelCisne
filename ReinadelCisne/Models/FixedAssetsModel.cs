using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace ReinadelCisne.Models
{
    public class FixedAssetsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public double ValorUnit { get; set; }
        public double ValorTotal { get; set; }
        public string Grupo { get; set; }

        [ForeignKey(typeof(GroupsFixedAssetsModel))]
        public int GroupFixedAssets { get; set; }
        [ManyToOne]
        public GroupsFixedAssetsModel GroupsFixed { get; set; }

        [ForeignKey(typeof(ListFixedAssetsXproductModel))]
        public int ListFixedAssetsId { get; set; }
        [ManyToOne]
        public ListFixedAssetsXproductModel ListFixedAssets { get; set; }
    }
}
