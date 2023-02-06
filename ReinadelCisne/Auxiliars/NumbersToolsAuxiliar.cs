using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Auxiliars
{
    public static class NumbersToolsAuxiliar
    {        public static string GetNember(float number)
        {
            if (number >= 1000000)
            {
                //return $"{Math.Round(number / 1000000, MidpointRounding.ToEven)} M €";
                return $"{Math.Round(number / 1000000, MidpointRounding.ToEven)}";
            }

            if (number >= 1000)
            {
                //return $"{Math.Round(number / 1000, MidpointRounding.ToEven)} K €";
                return $"{Math.Round(number / 1000, MidpointRounding.ToEven)}";
            }

            //return $"{Math.Round(number, MidpointRounding.ToEven)} €";
            return $"{Math.Round(number, MidpointRounding.ToEven)}";
        }
    }
}
