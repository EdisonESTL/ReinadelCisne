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
        public int Id { get; set; }

        public Image Image { get; set; }

        public string Name { get; set; }

        public byte[] ByteImage { get; set; }
        public UserPhotosModel()
        {
            Image = new Image();
        }
    }
}
