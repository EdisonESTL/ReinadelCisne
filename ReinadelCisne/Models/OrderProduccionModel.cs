using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace ReinadelCisne.Models
{
    public class OrderProduccionModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime FechaOrder { get; set; }
        //public ProductModel Product { get; set; }
        public string TiempoProduccion { get; set; }
        public double CantTiempoProduccion { get; set; }
        public string EstadoOrder { get; set; }

        [ForeignKey(typeof(ProductModel))]
        public int ProductId { get; set; }
        [OneToOne]
        public ProductModel Product { get; set; }
    }
}
