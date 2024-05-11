using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Classes
{   
    //MODEL to define the possible characters
    public class CharSET
    {
        public const string Concatenation = "●";
        public const string Alternation = "|";
        public const string Escape = "\\";
        public const string Star = "*";
        public const string Plus = "+";
        public const string QuestionMark = "?";
        public const string Grouping_Open = "(";
        public const string Grouping_Close = ")";
        public const string EndCharacter = "#";
        public const string Epsilon = "ε";

        //Division for tokens
        public const string Char_Separator = "'";

        public const string AbrevLetrasMinus = "[a-z]";
        public const string MinusChar = "©";

        public const string AbrevLetrasMayus = "[A-Z]";
        public const string MayusChar = "®";

        public const string AbrevNumbers = "[0-9]";
        public const string Numbers = "Ø";

        public const string AbrevSymbols = "[Simbolo]";
        //"(\\#|[|]|{|}|\\(|\\)|\\\\|$|@|!|%|^|&|\\*|\\+|-|_|.|:|/|;|<|>|,|\"|"|`|~|\\||=)";
        public const string Symbols = "ƒ";
    }
}
