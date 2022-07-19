using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ReinadelCisne.Services
{
    public class LoginAcces
    {
        public static string sstrUser = "";
        public static string sstrPasword = "";
        public LoginAcces(string pstrlUser, string pdtrPassw)
        {

            if (Application.Current.Properties.ContainsKey("UserLogged") & Application.Current.Properties.ContainsKey("PassLoged"))
            {
                var vUserLogged = Application.Current.Properties["UserLogged"];
                var vPassLoged = Application.Current.Properties["PassLoged"];
                sstrUser = vUserLogged.ToString();
                sstrPasword = vPassLoged.ToString();
            }
        }
        public string AuthentifyLogin()
        {
            return sstrUser;
        }

        public void LogoutLogin()
        {
            Application.Current.Properties["IsLoggedIn"] = false;
            Application.Current.Properties["UserLogged"] = null;
            Application.Current.Properties["PassLoged"] = null;
        }
    }
}
