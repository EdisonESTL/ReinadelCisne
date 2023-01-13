using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class PaymentsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double XIII { get; set; }
        public double XIV { get; set; }
        public double AporteIESS { get; set; }
        public double FondoReserva { get; set; }
        public double TotalIngreso { get; set; }
        public double EgresoIESS { get; set; }
        public double Anticipo { get; set; }
        public double HorasExtras { get; set; }
        public double TotalEgreso { get; set; }
        public double ARecibir { get; set; }
        public double EstadoPago { get; set; }

        [ForeignKey(typeof(PersonalModel))]
        public int PersonalModelId { get; set; }

        [ManyToOne]
        public PersonalModel PersonalModel { get; set; }
    }
}
