using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class SaleModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DateSale { get; set; }
        public double TotalSale { get; set; }
        public double Discount { get; set; }
        public string WayToPay { get; set; }
        //Relacion con OrderModel
        [OneToMany("SaleModeld", CascadeOperations = CascadeOperation.CascadeDelete)]
        public List<OrderModel> Orders {get; set;}

        [ForeignKey(typeof(ClientModel))]
        public int ClientModelId { get; set; }
        [ManyToOne]
        public ClientModel ClientModel { get; set; }
    }
}
