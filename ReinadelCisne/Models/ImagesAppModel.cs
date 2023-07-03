using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using Xamarin.Forms;

namespace ReinadelCisne.Models
{
    public class ImagesAppModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string Rute { get; set; }
        public int IdForeing { get; set; }
        public string NameForeing { get; set; }
    }
}
