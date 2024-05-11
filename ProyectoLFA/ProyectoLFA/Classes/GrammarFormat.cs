using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProyectoLFA.Classes
{
    public class GrammarFormat
    {

        /// <Libraries>
        /// We used a library, thats because without a Three Expression, we can't verificate the correct status of the Regular Expressions.
        /// We create a Reglular Expression, and we used Regex.Match to verificate every line on the file.
        /// Library used: System.Text.RegularExpressions. obtained from: https://learn.microsoft.com/es-es/dotnet/api/system.text.regularexpressions.regex?view=net-7.0
        /// </Libraries>

        //Regular expression to evaluate SETS
        private static string SETS = @"^(\s*([A-Z])+\s*=\s*((((\'([A-Z]|[a-z]|[0-9]|_)\'\.\.\'([A-Z]|[a-z]|[0-9]|_)\')\+)*(\'([A-z]|[a-z]|[0-9]|_)\'\.+\'([A-z]|[a-z]|[0-9]|_)\')*(\'([A-z]|[a-z]|[0-9]|_)\')+)|(CHR\(+([0-9])+\)+\.\.CHR\(+([0-9])+\)+)+)\s*)";

        //Regular expression to evaluate TOKENS
        private static string TOKENS = @"^(\s*TOKEN\s*[0-9]+\s*=\s*(([A-Z]+)|((\'*)([a-z]|[A-Z]|[1-9]|(\<|\>|\=|\+|\-|\*|\(|\)|\{|\}|\[|\]|\.|\,|\:|\;))(\'))+|((\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9]|\')*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9])*\s*\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*\{*\s*([A-Z]|[a-z]|[0-9])*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*)+)+)";

        //Regular expression to evaluate ACTIONS and ERRORS
        private static string ACTIONSANDERRORS
            = @"^((\s*RESERVADAS\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*)+|}+\s*|(\s*([A-Z]|[a-z]|[0-9])\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*|}+\s)*(\s*ERROR\s*=\s*[0-9]+\s*))$";

        //This are for 3rd fase. When we have the AFD. 
        private static string expresionSET
            = " *[A-Z]+ *= *((('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\)))( *. *. *(('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\))))?)+ *" +
              "( *\\+ *((('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\)))( *. *. *(('([A-Z]|[a-z]|[0-9]|[Simbolo])+')|(CHR\\([0-9]+\\))))?))* *#";
        private static string expresionTOKEN
            = "( *TOKEN *[0-9]+ *= *((([A-Z]+)|('([Simbolo]|[A-Z]|[a-z]|[0-9]) *' *)|(\\(+( *([A-Z]|[Simbolo]) *)+\\))| |\\?|\\(|\\)|\\||\\*|\\+|({ *[A-Z]+\\(\\) *}))|( *\\( *((([A-Z]+)|('([Simbolo]|[A-Z]|[a-z]|[0-9]) *' *)|(\\(( *([A-Z]|[Simbolo]) *)+\\))| |\\?|\\(|\\)|\\||\\*|\\+|({ *[A-Z]+\\(\\) *})))+ *\\)+ *))+ *)+#";
        private static string expresionACTIONSYERROR
            = "( *ACTIONS +RESERVADAS *\\( *\\) *{( *[0-9]+ *= *'([A-Z]|[a-z]|[0-9]|[Simbolo])+')+ *} *([A-Z]+ *\\( *\\) *{( *[0-9]+ *= *'([A-Z]|[a-z]|[0-9]|[Simbolo])+')+ *})*)( *[A-Z]+ *= *[0-9]+)+ *#";

        public static Dictionary<int, string> actionReference = new Dictionary<int, string>();

        public static string AnalyseFile(string data, ref int line)
        {
            //It helps changing jumps for spaces and remove all them with "TrimStart" and "TrimEnd"
            //data = data.Replace('\r', ' ');
            //data = data.Replace('\t', ' ');

            //data = data.TrimStart();
            //data = data.TrimEnd();

            string mensaje = "";


            bool first = true;
            bool setExists = false;
            bool tokenExists = false;
            bool actionExists = false;

            int actionCount = 0;
            int actionsError = 0;
            int tokenCount = 0;
            int setCount = 0;

            string[] lines = data.Split('\n');
            int count = 0;

            foreach (var item in lines)
            {
                count++;
                if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrEmpty(item))
                {
                    if (first)
                    {
                        first = false;
                        if (item.Contains("SETS"))
                        {
                            setExists = true;
                            mensaje = "Formato Correcto";
                        }
                        else if (item.Contains("TOKENS"))
                        {
                            tokenExists = true;
                            mensaje = "Formato Correcto";
                        }
                        else
                        {
                            line = 1;
                            return "Error en linea 1: Se esperaba SETS o TOKENS";
                        }
                    }
                    else if (setExists)
                    {
                        Match setMatch = Regex.Match(item, SETS);
                        if (item.Contains("TOKENS"))
                        {
                            if (setCount < 1)
                            {
                                line = count;
                                return "Error: Se esperaba almenos un SET";
                            }
                            setExists = false;
                            tokenExists = true;
                        }
                        else
                        {
                            if (!setMatch.Success)
                            {
                                return $"Error en linea: {count}";
                            }
                            tokenCount++;
                        }

                        setCount++;
                    }
                    else if (tokenExists)
                    {
                        Match m = Regex.Match(item, TOKENS);
                        if (item.Contains("ACTIONS"))
                        {
                            if (tokenCount < 1)
                            {
                                line = count;
                                return "Error: Se esperaba almenos un TOKEN";
                            }
                            actionCount++;
                            tokenExists = false;
                            actionExists = true;
                        }
                        else
                        {
                            if (!m.Success)
                            {
                                return $"Error en linea: {count}";
                            }
                            tokenCount++;
                        }
                    }
                    else if (actionExists)
                    {
                        if (item.Contains("ERROR"))
                        {
                            actionsError++;
                        }
                        Match actMatch = Regex.Match(item, ACTIONSANDERRORS);
                        if (!actMatch.Success)
                        {
                            return $"Error en linea: {count}";
                        }
                    }
                }
            }
            if (actionCount < 1)
            {
                return $"Error: Se esperaba la sección de ACTIONS";
            }
            if (actionsError < 1)
            {
                return $"Error: Se esperaba una la sección de ERROR";
            }
            line = count;
            return mensaje;
        }

        public static ET GetExpressionTree(string text)
        {
            Dictionary<string, string[]> sets = new Dictionary<string, string[]>();
            List<Action> actionsList = new List<Action>();

            //List with the number of the tokens
            List<int> tokensList = new List<int>();

            string token = ""; //Each token will be concatenated

            text = text.Replace('\r', ' ');
            text = text.Replace('\t', ' ');

            text = text.TrimStart();
            text = text.TrimEnd();

            //Position in the file
            bool inicio = true;
            bool setActive = false;
            bool tokenActive = false;
            bool actionActive = true;

            string[] lineas = text.Split('\n');

            //Traverse the file
            foreach (var item in lineas)
            {
                if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrEmpty(item))
                {
                    if (inicio)
                    {
                        inicio = false;
                        if (item.Contains("SETS"))
                        {
                            setActive = true;
                        }
                        else if (item.Contains("TOKENS"))
                        {
                            tokenActive = true;
                        }
                        else
                        {
                            throw new Exception("Formato incorrecto.");
                        }
                    }
                    else if (setActive)
                    {
                        if (item.Contains("TOKENS")) //End of section 'SETS'
                        {
                            setActive = false;
                            tokenActive = true;
                        }
                        else //There are still sets in the file 
                        {
                            AddNewSET(ref sets, item);
                        }
                    }
                    else if (tokenActive)
                    {
                        if (item.Contains("ACTIONS")) //End of section 'TOKENS'
                        {
                            tokenActive = false;
                            actionActive = true;
                        }
                        else
                        {
                            AddNewTOKEN(ref tokensList, ref token, item);
                        }
                    }
                }
            }

            //Check for repeated token numbers
            CheckForRepeatedTokens(tokensList, actionsList);

            //Create tree
            if (token != "")
            {
                return new ET(token, sets, tokensList);
            }
            else
            {
                throw new Exception("Se esperaba mas tokens");
            }
        }

        private static void CheckForRepeatedTokens(List<int> tokens, List<Action> actionsList) //Just validate that the tokenNumber, an identifier to each token read, it's not repeated
        {
            List<int> repeated = new List<int>();

            foreach (var action in actionsList)
            {
                foreach (var item in action.ActionValues.Keys)
                {
                    if (repeated.Contains(item) || tokens.Contains(item))
                    {
                        throw new Exception($"Error: El token {item} aparece más de una vez");
                    }
                    else
                    {
                        repeated.Add(item);
                    }
                }
            }
            //If it ends, it is correct
        }

        //SET reader
        private static void AddNewSET(ref Dictionary<string, string[]> sets, string text)
        {
            List<string> asciiValues = new List<string>();
            string setName = "";

            string[] line = text.Split('=');

            setName = line[0].Trim();//this is the set name
            line[1] = line[1].Replace(" ", "");//this are the values

            string[] values = line[1].Split('+');

            foreach (var item in values)
            {
                string[] tmpLimits = item.Split('.');

                List<string> Limits = new List<string>();

                //format
                foreach (var i in tmpLimits)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        Limits.Add(i);
                    }
                }

                if (Limits.Count == 2)
                {
                    int lowerLimit = formatSET(Limits[0]);
                    int upperLimit = formatSET(Limits[1]); ;

                    //Add range of values
                    asciiValues.Add($"{lowerLimit},{upperLimit}");
                }
                else if (Limits.Count == 1)
                {
                    int character = formatSET(Limits[0]);

                    asciiValues.Add(character.ToString());
                }
            }

            if (setName.Length > 1)
            {
                sets.Add(setName, asciiValues.ToArray());
            }
            else
            {
                throw new Exception($"El nombre del SET {setName} debe ser mas largo.");
            }

        }

        private static int formatSET(string token)
        {
            int result;

            if (token.Contains("CHR")) //Ex. CHR(90)
            {
                string value = token.Replace("CHR", "");
                value = value.Replace("(", "");
                value = value.Replace(")", "");
                value = value.Replace(" ", "");

                result = Convert.ToInt16(value);
            }
            else //Ex. 'A'
            {
                result = Encoding.ASCII.GetBytes(token)[1];
            }

            return result;
        }

        //TOKEN reader
        private static void AddNewTOKEN(ref List<int> tokenNumbers, ref string tokens, string text)
        {
            text = text.Replace("TOKEN", "");
            text = text.Trim();
            int tokenNumber = 0;

            string[] line = SplitToken(text);

            //Validate token number
            if (int.TryParse(line[0].Trim(), out tokenNumber)) //This validates that the tokenNumber, an identifier to each token read, it's not repeated
            {
                if (!tokenNumbers.Contains(tokenNumber))
                {
                    tokenNumbers.Add(tokenNumber);
                }
                else
                {
                    throw new Exception($"El TOKEN {tokenNumber} aparece mas de una vez.");
                }
            }
            else
            {
                throw new Exception($"El nombre del TOKEN {line[0]} no es valido.");
            }

            string newToken = line[1].Trim();//this are the values

            if (string.IsNullOrEmpty(tokens) | string.IsNullOrWhiteSpace(tokens))
            {
                tokens = $"({newToken})";
            }
            else
            {
                tokens += $"|({newToken})";
            }
        }

        private static string[] SplitToken(string expression)
        {
            string functionName = "";//When Token Contains { function() }
            string[] token = { "", "" };

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] != '=')
                {
                    token[0] += expression[i]; //joins the expression when it doesn't have "="
                }
                else //Deletes de "=" from the expression and the white space characters
                {
                    string tmp = "";

                    for (int j = i + 1; j < expression.Length; j++)
                    {
                        tmp += expression[j];
                    }

                    token[1] = removeActionsFromExpression(tmp, ref functionName); //Deletes the actions
                    token[1] = token[1].Trim();
                    break;
                }
            }

            //Validate token number
            if (!string.IsNullOrEmpty(functionName))
            {
                if (int.TryParse(token[0].Trim(), out int tokenNumber))
                {
                    actionReference.Add(tokenNumber, functionName.Trim());
                }
                else
                {
                    throw new Exception($"El nombre del TOKEN {token[0]} no es valido.");
                }

            }

            return token;
        }

        private static string removeActionsFromExpression(string text, ref string functionName)
        {
            //Remove everything contained within {}
            string result = "";

            if (text.Contains('{') && text.Contains('}'))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '\'')
                    {
                        result += $"'{text[i + 1]}'";
                        i += 2;
                    }
                    else if (text[i] == '{')
                    {
                        int counter = 0;
                        char? actualChar = text[i];

                        while (actualChar != '}')
                        {
                            counter++;
                            actualChar = text[i + counter];
                            functionName += actualChar;
                        }

                        functionName = functionName.Replace("}", "");
                        functionName = functionName.Replace(" ", "");

                        i += counter;
                    }
                    else
                    {
                        result += text[i];
                    }
                }
                return result;
            }

            return text;
        }


    }
}
