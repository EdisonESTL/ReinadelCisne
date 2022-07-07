using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Models
{
    public class RawMaterialShModel
    {
        public double Amount { get; set; }
        public string Measurament { get; set; }
        public string Description { get; set; }
        public string UnitCost { get; set; }
        public float TotalCost { get; set; }
    }
}
