using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ListFAxProductModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public double ValorTotalMaquinas { get; set; }
        public double ValorTotalDepreciaciones { get; set; }

        [ForeignKey(typeof(ProductModel))]
        public int ProductModelId { get; set; }
        [OneToOne]
        public ProductModel Product { get; set; }
                
        [OneToMany]
        public List<ListFixedAssetsXproductModel> ListFixeds { get; set; }
    }
}
