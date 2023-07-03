using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using Xamarin.Forms;

namespace ReinadelCisne.Models
{
    [Table("UserModel")]
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("CiUser")]
        public string CiUser { get; set; }

        [Column("NameUser")]
        public string NameUser { get; set; }

        [Column("NegocioUser")]
        public string NegocioUser { get; set; }

        [Column("MailUser")]
        public string MailUser { get; set; }


        [Column("PhoneUser"), MaxLength(10)]
        public string PhoneUser { get; set; }


        [Column("PasswordUser"), MaxLength(12)]
        public string PasswordUser { get; set; }

        [Column("PinUser"), MaxLength(12)]
        public string PinUser { get; set; }

        [Column("TypeUser")]
        public string TypeUser { get; set; }

        /*[Column("Photo")]
        public Image Image { get; set; }

        public UserModel()
        {
            Image = new Image();
        }*/
    }
}
