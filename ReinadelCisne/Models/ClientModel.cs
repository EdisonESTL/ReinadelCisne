using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class ClientModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CellphoneNumber { get; set; }

        [OneToMany("ClientModelId")]
        public List<SaleModel> Sales { get; set; }
    }
}
