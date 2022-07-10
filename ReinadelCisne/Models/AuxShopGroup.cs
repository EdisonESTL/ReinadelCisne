using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Models
{
    public class AuxShopGroup : List<AuxShopsRM>
    {
        public string IdShop { get; set; }
        public string Distribuidor { get; set; }
        public string Factura { get; set; }
        public string Total { get; set; }

        //public AuxShopGroup(string )
    }
}
