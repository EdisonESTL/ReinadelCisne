using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReinadelCisne.Models
{
    public class WorkForceModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tasks { get; set; }
        public string Type { get; set; }

        /*[ForeignKey(typeof(ListWFModel))]
        public int ListWFModelId { get; set; }
        [ManyToOne]
        public ListWFModel ListWF { get; set; }*/

        /*[ForeignKey(typeof(PersonalModel))]
        public int PersonalModelId { get; set; }

        [OneToOne]
        public PersonalModel PersonalModel { get; set; }*/

        [OneToMany]
        public List<PersonalModel> Personals { get; set; }
    }
}
