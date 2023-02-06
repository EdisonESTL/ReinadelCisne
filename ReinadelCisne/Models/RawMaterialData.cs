using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinadelCisne.Models
{
    public class RawMaterialData
    {
        public static IList<RawMaterialModel> RawMaterials { get; private set; }
        static RawMaterialData()
        {
            RawMaterials = new List<RawMaterialModel>();
            CargarRawMaterials();
        }

        private static void CargarRawMaterials()
        {
            List<KardexRMModel> KardexRawMaterials = App.Database.GetKardexsRM().Result;

            foreach (KardexRMModel kardexMaterial in KardexRawMaterials)
            {
                if (kardexMaterial.SaldosRMs.Count > 0 && kardexMaterial.RawMaterialModell != null)
                {
                    var SaldoxKardexMaterial = App.Database.GetSaldosxKardex(kardexMaterial).Result;
                    var ord = SaldoxKardexMaterial.OrderByDescending(x => x.Date).FirstOrDefault();

                    RawMaterialModel rrm = App.Database.GetOneRM(kardexMaterial.RawMaterialModell.Id).Result;
                    rrm.CantidadRM = ord.Cantidad;
                    rrm.CostoRM = (float)ord.ValorUnitario;
                    rrm.TotalCost = (float)ord.SaldoTotal;

                    RawMaterials.Add(rrm);
                }
            }
        }
    }
}
