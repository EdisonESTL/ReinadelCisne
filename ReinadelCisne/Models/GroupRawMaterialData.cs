using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Models
{
    public class GroupRawMaterialData
    {
        public static IList<GroupsRMModel> GroupsRMs { get; private set; }

        static GroupRawMaterialData()
        {
            GroupsRMs = new List<GroupsRMModel>();
            CargarGroupsRM();
        }

        private static void CargarGroupsRM()
        {
            var GruposDataBase = App.Database.GetGroupRM();
            foreach(var grupo in GruposDataBase.Result)
            {
                GroupsRMs.Add(grupo);
            }
        }
    }
}
