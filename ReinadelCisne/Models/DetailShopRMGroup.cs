using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Models
{
    public class DetailShopRMGroup : List<DetailShopRM>
    {
        public string NameRM { get; set; }
        public DetailShopRMGroup(string Name, List<DetailShopRM> detailShops) : base(detailShops)
        {
            NameRM = Name;
        }
    }
}
