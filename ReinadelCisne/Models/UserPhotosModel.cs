using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using Xamarin.Forms;


namespace ReinadelCisne.Models
{
    public class UserPhotosModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Photo")]
        public Image Image { get; set; }

        [Column("Name")]
        public string Name { get; set; }
        public UserPhotosModel()
        {
            Image = new Image();
        }

        [Column("IdForeign")]
        public int IdForeign { get; set; }
        [Column("SringForeign")]
        public string StringForeign { get; set; }
    }
}
