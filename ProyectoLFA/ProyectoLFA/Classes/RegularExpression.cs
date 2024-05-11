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
            expression = expression.Replace(AbrevLetrasMayus, MayusChar);
            expression = expression.Replace(AbrevLetrasMinus, MinusChar);
            expression = expression.Replace(AbrevNumbers, Numbers);
            expression = expression.Replace(AbrevSymbols, Symbols);

            return expression;
        }

        public string ValidateString(string text)
        {
            //It verifies if the grammar has the correct format
           

            string message = "";
            int characters = 0;

            //bool isValid = AFD.isValidString(text, ref message, ref characters);

            message = message.Replace(MayusChar, AbrevLetrasMayus);
            message = message.Replace(MinusChar, AbrevLetrasMinus);
            message = message.Replace(Numbers, AbrevNumbers);
            message = message.Replace(Symbols, AbrevSymbols);

            return message;
        }

    }
}
