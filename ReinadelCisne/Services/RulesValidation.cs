using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Services
{
    public class RulesValidation
    {
        public int ValidarCiPassword(string Ci, string Password)
        {
            int resp;
            //Ci es nulo o vacio
            if (string.IsNullOrWhiteSpace(Ci) ||
                string.IsNullOrWhiteSpace(Password))
            {
                resp = 2;
            }
            else
            {
                //Ci es válido
                resp = ValidarCi(Ci) ? 1 : 3;
            }
            
            return resp;
        }

        private bool ValidarCi(string ci)
        {
            string[] digitos = {"00","01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","30"};
            bool res = false;
            var g = ci.Substring(0, 2);

            foreach(var i in digitos)
            {
                if (i.Contains(g))
                {
                    res = true;
                }
            }
            
            return res;
        }

        public int ValidarRegisterCamps(string ci, string name, string mail, string phone, string password, string passwordd, string type)
        {
            int resp = 0;
            if (CampsNullsEmpty(ci, name, mail, phone, password, passwordd, type))
            {
                if (ValidarCi(ci))
                {
                    if (string.Equals(password, passwordd))
                    {
                        resp = 1;
                    }
                    else
                    {
                        resp = 3;
                    }
                }
                else
                {
                    resp = 2;
                }
                
            }
            return resp;
        }

        public bool CampsNullsEmpty(string ci, string name, string mail, string phone, string password, string passwordd, string type)
        {
            if (
                string.IsNullOrWhiteSpace(mail) &&
                string.IsNullOrWhiteSpace(phone) &&
                string.IsNullOrWhiteSpace(passwordd) &&
                string.IsNullOrWhiteSpace(type))
            {
                return false;
            }
            else { return true; }
        }
    }
}
