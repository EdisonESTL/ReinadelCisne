using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;

namespace ReinadelCisne.Models
{
    [Table("UserModel")]
    public class UserModel
    {
        [PrimaryKey]
        [Column("CiUser")]
        public string CiUser { get; set; }

        [Column("NameUser")]
        public string NameUser { get; set; }


        [Column("MailUser")]
        public string MailUser { get; set; }


        [Column("PhoneUSer"), MaxLength(10)]
        public string PhoneUser { get; set; }


        [Column("PasswordUSer"), MaxLength(12)]
        public string PasswordUser { get; set; }

        [Column("TypeUser")]
        public string TypeUser { get; set; }
    }
}
