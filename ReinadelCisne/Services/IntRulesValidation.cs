using System;
using System.Collections.Generic;
using System.Text;

namespace ReinadelCisne.Services
{
    interface IntRulesValidation
    {
        bool Vacio { get; set; }

        
        void CampsNullsEmpty(List<string> val);        
    }
}
