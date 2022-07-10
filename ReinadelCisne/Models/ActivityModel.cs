using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ReinadelCisne.Models
{
    public class ActivityModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DateA { get; set; }
        public TimeSpan TimeI { get; set; }
        public TimeSpan TimeF { get; set; }
        public string Description { get; set; }
    }
}
