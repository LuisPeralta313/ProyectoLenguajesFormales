using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    /// <summary>
    /// Clase que define constantes para caracteres especiales utilizados en expresiones regulares.
    /// </summary>
    public class CharSET
    {
        public const string QuestionMark = "?"; // Símbolo de interrogación
        public const string Grouping_Open = "("; // Paréntesis de apertura
        public const string Grouping_Close = ")"; // Paréntesis de cierre
        public const string EndCharacter = "#"; // Carácter de fin
        public const string Epsilon = "ε"; // Epsilon
        public const string Concatenation = "●"; // Símbolo de concatenación
        public const string Alternation = "|"; // Símbolo de alternancia
        public const string Escape = "\\"; // Carácter de escape
        public const string Star = "*"; // Operador de cero o más repeticiones
        public const string Plus = "+"; // Operador de una o más repeticiones

        /////////////////////////////////////////////////////////

        public const string Char_Separator = "'"; // Separador de caracteres

        public const string AbrevLetrasMinus = "[a-z]"; // Abreviatura para letras minúsculas
        public const string MinusChar = "©"; // Representación de letras minúsculas

        public const string AbrevLetrasMayus = "[A-Z]"; // Abreviatura para letras mayúsculas
        public const string MayusChar = "®"; // Representación de letras mayúsculas

        public const string AbrevNumbers = "[0-9]"; // Abreviatura para números
        public const string Numbers = "Ø"; // Representación de números

        public const string AbrevSymbols = "[Simbolo]"; // Abreviatura para símbolos
        public const string Symbols = "ƒ"; // Representación de símbolos
    }
}
