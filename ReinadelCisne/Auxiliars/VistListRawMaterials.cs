using System;
using System.Collections.Generic;
using System.Text;
using ReinadelCisne.Models;
using Xamarin.Forms;

namespace ReinadelCisne.Auxiliars
{
    public class VistListRawMaterials
    {
        public RawMaterialModel Raw { get; set; }
        public Image Image { get; set; }
        public VistListRawMaterials()
        {
            Image = new Image();
        }
    }
}
