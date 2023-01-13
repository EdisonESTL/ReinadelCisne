using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class PersonalModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CI { get; set; }
        public string Telephone { get; set; }
        public string Mail { get; set; }
        public DateTime Date { get; set; }
        public float Pay { get; set; }
        public string TypeContract { get; set; }
        public DateTime DateInicio { get; set; }
        public DateTime DateFinal { get; set; }
        public double CantDiauHora { get; set; }
        public string DescriptionObra { get; set; }

        //Relacion con el puesto laboral que ocupa
        [ForeignKey(typeof(WorkForceModel))]
        public int WorkForceModelId { get; set; }
        [ManyToOne]
        public WorkForceModel WorkForce { get; set; }

        //Relación con ListWModel que se involucra
        [ForeignKey(typeof(ListWFModel))]
        public int ListWFModelId { get; set; }
        [ManyToOne]
        public ListWFModel ListWF { get; set; }

        //Relaciòn con pagos
        [OneToMany]
        public List<PaymentsModel> Payments { get; set; } 

    }
}
