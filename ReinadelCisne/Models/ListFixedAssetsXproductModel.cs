using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ListFixedAssetsXproductModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TipoUso { get; set; }
        public double CantidadUso { get; set; }
        public double PrecioXuso { get; set; }

        [ForeignKey(typeof(FixedAssetsModel))]
        public int FixedAssetId { get; set; }

        [OneToOne]
        public FixedAssetsModel FixedAssets { get; set; }

        [ForeignKey(typeof(ListFAxProductModel))]
        public int ListFAId { get; set; }
        [ManyToOne]
        public ListFAxProductModel ListFAxProduct { get; set; }
    }
}
