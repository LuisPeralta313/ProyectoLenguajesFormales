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
            return expression;
        }
    }
}
