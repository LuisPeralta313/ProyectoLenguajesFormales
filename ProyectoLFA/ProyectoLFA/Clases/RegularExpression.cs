using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Classes
{
    //Gets, save and build a simple regular expression for AFD [3rd fase]
    class RegularExpression : CharSET
    {
        //Constructor
        public RegularExpression(string exp)
        {
            exp = simplifyExpression(exp);
        }

        private string simplifyExpression(string expression)
        {
            return expression;
        }


    }
}
