using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    // Obtiene, guarda y construye una expresión regular simple para un AFD [3ra fase]
    class RegularExpression : CharSET
    {
        // Constructor
        public RegularExpression(string exp)
        {
            exp = simplifyExpression(exp);
        }

        // Método para simplificar la expresión regular (actualmente no hace nada)
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

            //bool isValid = AFD.isValidString(text, ref message, ref characters);

            message = message.Replace(MayusChar, AbrevLetrasMayus);
            message = message.Replace(MinusChar, AbrevLetrasMinus);
            message = message.Replace(Numbers, AbrevNumbers);
            message = message.Replace(Symbols, AbrevSymbols);

            return message;
        }
    }
}
