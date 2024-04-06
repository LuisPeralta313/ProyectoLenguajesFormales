using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProyectoLFA.Clases
{
    public class GrammarFormat
    {

        /// <Libraries>
        /// We used a library, thats because without a Three Expression, we can't verificate the correct status of the Regular Expressions.
        /// We create a Reglular Expression, and we used Regex.Match to verificate every line on the file.
        /// Library used: System.Text.RegularExpressions. obtained from: https://learn.microsoft.com/es-es/dotnet/api/system.text.regularexpressions.regex?view=net-7.0
        /// </Libraries>

        // SETS
        private static string SETS = @"^(\s*([A-Z])+\s*=\s*((((\'([A-Z]|[a-z]|[0-9]|_)\'\.\.\'([A-Z]|[a-z]|[0-9]|_)\')\+)*(\'([A-z]|[a-z]|[0-9]|_)\'\.+\'([A-z]|[a-z]|[0-9]|_)\')*(\'([A-z]|[a-z]|[0-9]|_)\')+)|(CHR\(+([0-9])+\)+\.\.CHR\(+([0-9])+\)+)+)\s*)";

        // TOKENS
        private static string TOKENS = @"^(\s*TOKEN\s*[0-9]+\s*=\s*(([A-Z]+)|((\'*)([a-z]|[A-Z]|[1-9]|(\<|\>|\=|\+|\-|\*|\(|\)|\{|\}|\[|\]|\.|\,|\:|\;))(\'))+|((\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9]|\')*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9])*\s*\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*\{*\s*([A-Z]|[a-z]|[0-9])*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*)+)+)";

        // ACTIONS y ERRORS
        private static string ACTIONSANDERRORS
            = @"^((\s*RESERVADAS\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*)+|}+\s*|(\s*([A-Z]|[a-z]|[0-9])\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*|}+\s)*(\s*ERROR\s*=\s*[0-9]+\s*))$";

        // Aquí deberían ir las Expresiones regulares para la tercera 
 

        public static Dictionary<int, string> actionReference = new Dictionary<int, string>();

        public static string AnalyseFile(string data, ref int line)
        {
            // Ayuda a cambiar saltos de línea por espacios y los elimina todos con "TrimStart" y "TrimEnd"
            //data = data.Replace('\r', ' ');
            //data = data.Replace('\t', ' ');

            // Elimina los espacios al principio y al final de la cadena
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


        /// <summary>
        /// Obtiene el árbol de expresiones a partir del texto proporcionado.
        /// </summary>
        /// <param name="text">Texto que contiene la definición de conjuntos, tokens y acciones.</param>
        /// <returns>El árbol de expresiones generado.</returns>
        public static ET GetExpressionTree(string text)
        {
            Dictionary<string, string[]> sets = new Dictionary<string, string[]>(); // Diccionario para almacenar los conjuntos
            List<Action> actionsList = new List<Action>(); // Lista para almacenar las acciones

            // Lista con el número de los tokens
            List<int> tokensList = new List<int>();

            string token = ""; // Cada token se concatenará aquí

            text = text.Replace('\r', ' '); // Reemplaza los saltos de línea con espacios
            text = text.Replace('\t', ' '); // Reemplaza las tabulaciones con espacios

            text = text.TrimStart(); // Elimina los espacios en blanco al principio de la cadena
            text = text.TrimEnd(); // Elimina los espacios en blanco al final de la cadena

            bool inicio = true; // Indica si es el comienzo del archivo
            bool setActive = false; // Indica si estamos en la sección de conjuntos (SETS)
            bool tokenActive = false; // Indica si estamos en la sección de tokens (TOKENS)
            bool actionActive = true; // Indica si estamos en la sección de acciones (ACTIONS)

            string[] lineas = text.Split('\n'); // Divide el texto en líneas

            // Recorre el archivo línea por línea
            foreach (var item in lineas)
            {
                if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrEmpty(item)) // Verifica que la línea no esté en blanco
                {
                    if (inicio) // Si es el comienzo del archivo
                    {
                        inicio = false;
                        if (item.Contains("SETS")) // Comprueba si la línea contiene la palabra "SETS"
                        {
                            setActive = true; // Establece setActive en true para indicar que estamos en la sección de conjuntos
                        }
                        else if (item.Contains("TOKENS")) // Comprueba si la línea contiene la palabra "TOKENS"
                        {
                            tokenActive = true; // Establece tokenActive en true para indicar que estamos en la sección de tokens
                        }
                        else
                        {
                            throw new Exception("Formato incorrecto."); // Lanza una excepción si el formato es incorrecto
                        }
                    }
                    else if (setActive) // Si estamos en la sección de conjuntos
                    {
                        if (item.Contains("TOKENS")) // Comprueba si la línea contiene la palabra "TOKENS" (fin de la sección de conjuntos)
                        {
                            setActive = false; // Desactiva la sección de conjuntos
                            tokenActive = true; // Activa la sección de tokens
                        }
                        else // Si todavía hay conjuntos en el archivo
                        {
                            AddNewSET(ref sets, item); // Agrega un nuevo conjunto al diccionario de conjuntos
                        }
                    }
                    else if (tokenActive) // Si estamos en la sección de tokens
                    {
                        if (item.Contains("ACTIONS")) // Comprueba si la línea contiene la palabra "ACTIONS" (fin de la sección de tokens)
                        {
                            tokenActive = false; // Desactiva la sección de tokens
                            actionActive = true; // Activa la sección de acciones
                        }
                        else // Si todavía hay tokens en el archivo
                        {
                            AddNewTOKEN(ref tokensList, ref token, item); // Agrega un nuevo token a la lista de tokens
                        }
                    }
                }
            }

            // Verifica si hay números de token repetidos
            CheckForRepeatedTokens(tokensList, actionsList);

            // Crea el árbol de expresiones
            if (token != "")
            {
                return new ET(token, sets, tokensList); // Retorna un nuevo árbol de expresiones con los conjuntos, tokens y lista de tokens
            }
            else
            {
                throw new Exception("Se esperaban más tokens"); // Lanza una excepción si se esperaban más tokens pero no se encontraron
            }
        }

        /// <summary>
        /// Verifica si hay tokens repetidos.
        /// </summary>
        /// <param name="tokens">Lista de números de token.</param>
        /// <param name="actionsList">Lista de acciones.</param>
        private static void CheckForRepeatedTokens(List<int> tokens, List<Action> actionsList)
        {
            List<int> repeated = new List<int>(); // Lista para almacenar tokens repetidos

            foreach (var action in actionsList) // Recorre la lista de acciones
            {
                foreach (var item in action.ActionValues.Keys) // Recorre las claves de los valores de acción
                {
                    if (repeated.Contains(item) || tokens.Contains(item)) // Verifica si el token ya está en la lista de tokens repetidos o en la lista de tokens
                    {
                        throw new Exception($"Error: El token {item} aparece más de una vez"); // Lanza una excepción si el token está repetido
                    }
                    else
                    {
                        repeated.Add(item); // Agrega el token a la lista de tokens repetidos
                    }
                }
            }
            // Si termina, es correcto
        }

        //SET reader
        /// <summary>
        /// Agrega un nuevo conjunto (SET) al diccionario de conjuntos.
        /// </summary>
        /// <param name="sets">Diccionario que contiene los conjuntos.</param>
        /// <param name="text">Texto que contiene la definición del conjunto.</param>
        private static void AddNewSET(ref Dictionary<string, string[]> sets, string text)
        {
            List<string> asciiValues = new List<string>(); // Lista para almacenar los valores ASCII del conjunto
            string setName = ""; // Nombre del conjunto

            string[] line = text.Split('='); // Divide el texto en dos partes usando el signo '='

            setName = line[0].Trim(); // Obtiene el nombre del conjunto (parte izquierda del signo '=')
            line[1] = line[1].Replace(" ", ""); // Elimina los espacios en blanco de la parte derecha del signo '='

            string[] values = line[1].Split('+'); // Divide los valores del conjunto usando el signo '+'

            foreach (var item in values) // Recorre los valores del conjunto
            {
                string[] tmpLimits = item.Split('.'); // Divide los límites del valor usando el punto '.'

                List<string> Limits = new List<string>(); // Lista para almacenar los límites

                // Formatea y elimina los límites vacíos
                foreach (var i in tmpLimits)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        Limits.Add(i);
                    }
                }

                if (Limits.Count == 2) // Si hay dos límites
                {
                    int lowerLimit = formatSET(Limits[0]); // Obtiene el límite inferior formateado
                    int upperLimit = formatSET(Limits[1]); // Obtiene el límite superior formateado

                    // Agrega el rango de valores
                    asciiValues.Add($"{lowerLimit},{upperLimit}");
                }
                else if (Limits.Count == 1) // Si hay un límite
                {
                    int character = formatSET(Limits[0]); // Obtiene el carácter formateado

                    asciiValues.Add(character.ToString()); // Agrega el carácter a la lista de valores ASCII
                }
            }

            if (setName.Length > 1) // Si el nombre del conjunto tiene más de un carácter
            {
                sets.Add(setName, asciiValues.ToArray()); // Agrega el conjunto al diccionario de conjuntos
            }
            else
            {
                throw new Exception($"El nombre del SET {setName} debe ser más largo."); // Lanza una excepción si el nombre del conjunto es demasiado corto
            }

        }
        /// <summary>
        /// Formatea un token para obtener su valor ASCII.
        /// </summary>
        /// <param name="token">El token que se va a formatear.</param>
        /// <returns>El valor ASCII del token.</returns>
        private static int formatSET(string token)
        {
            int result;

            if (token.Contains("CHR")) // Si el token contiene "CHR" (por ejemplo, CHR(90))
            {
                string value = token.Replace("CHR", ""); // Elimina "CHR" del token
                value = value.Replace("(", ""); // Elimina "(" del token
                value = value.Replace(")", ""); // Elimina ")" del token
                value = value.Replace(" ", ""); // Elimina espacios en blanco del token

                result = Convert.ToInt16(value); // Convierte el valor a entero
            }
            else // Si el token no contiene "CHR" (por ejemplo, 'A')
            {
                result = Encoding.ASCII.GetBytes(token)[1]; // Obtiene el valor ASCII del token
            }

            return result; // Retorna el valor ASCII del token
        }


        /// <summary>
        /// Agrega un nuevo token a la lista de números de token y al texto de tokens.
        /// </summary>
        /// <param name="tokenNumbers">Lista de números de token.</param>
        /// <param name="tokens">Texto que contiene la definición de los tokens.</param>
        /// <param name="text">Texto que contiene la definición del token.</param>
        private static void AddNewTOKEN(ref List<int> tokenNumbers, ref string tokens, string text)
        {
            text = text.Replace("TOKEN", ""); // Elimina la palabra "TOKEN" del texto
            text = text.Trim(); // Elimina los espacios en blanco al principio y al final del texto
            int tokenNumber = 0; // Número de token

            string[] line = SplitToken(text); // Divide el texto en dos partes usando el método SplitToken

            // Valida el número de token
            if (int.TryParse(line[0].Trim(), out tokenNumber)) // Esto valida que el número de token, un identificador para cada token leído, no esté repetido
            {
                if (!tokenNumbers.Contains(tokenNumber)) // Si el número de token no está en la lista de números de token
                {
                    tokenNumbers.Add(tokenNumber); // Agrega el número de token a la lista de números de token
                }
                else
                {
                    throw new Exception($"El TOKEN {tokenNumber} aparece más de una vez."); // Lanza una excepción si el número de token está repetido
                }
            }
            else
            {
                throw new Exception($"El nombre del TOKEN {line[0]} no es válido."); // Lanza una excepción si el nombre del token no es válido
            }

            string newToken = line[1].Trim(); // Obtiene la definición del nuevo token

            if (string.IsNullOrEmpty(tokens) || string.IsNullOrWhiteSpace(tokens)) // Si el texto de tokens está vacío o es nulo
            {
                tokens = $"({newToken})"; // Establece el nuevo token como el primer token
            }
            else
            {
                tokens += $"|({newToken})"; // Agrega el nuevo token al texto de tokens
            }
        }


        /// <summary>
        /// Divide una expresión en dos partes: el número de token y la definición del token.
        /// </summary>
        /// <param name="expression">La expresión que se va a dividir.</param>
        /// <returns>Un arreglo de dos elementos: el número de token y la definición del token.</returns>
        private static string[] SplitToken(string expression)
        {
            string functionName = ""; // Cuando el Token Contiene { función() }
            string[] token = { "", "" }; // Arreglo para almacenar el número de token y la definición del token

            for (int i = 0; i < expression.Length; i++) // Recorre la expresión
            {
                if (expression[i] != '=') // Si el carácter actual no es '='
                {
                    token[0] += expression[i]; // Concatena el carácter actual al número de token
                }
                else // Si el carácter actual es '='
                {
                    string tmp = "";

                    for (int j = i + 1; j < expression.Length; j++) // Recorre los caracteres después de '='
                    {
                        tmp += expression[j]; // Concatena los caracteres a una cadena temporal
                    }

                    token[1] = removeActionsFromExpression(tmp, ref functionName); // Elimina las acciones de la expresión y obtiene la definición del token
                    token[1] = token[1].Trim(); // Elimina los espacios en blanco al principio y al final de la definición del token
                    break; // Sale del bucle
                }
            }

            // Valida el número de token
            if (!string.IsNullOrEmpty(functionName)) // Si la función no está vacía (cuando el Token Contiene { función() })
            {
                if (int.TryParse(token[0].Trim(), out int tokenNumber)) // Intenta convertir el número de token a entero
                {
                    actionReference.Add(tokenNumber, functionName.Trim()); // Agrega el número de token y el nombre de la función al diccionario de referencias de acción
                }
                else
                {
                    throw new Exception($"El nombre del TOKEN {token[0]} no es válido."); // Lanza una excepción si el nombre del token no es válido
                }

            }

            return token; // Retorna el arreglo que contiene el número de token y la definición del token
        }


        /// <summary>
        /// Elimina las acciones de una expresión y obtiene el nombre de la función (si existe).
        /// </summary>
        /// <param name="text">La expresión de la que se van a eliminar las acciones.</param>
        /// <param name="functionName">El nombre de la función (si existe) que se obtendrá.</param>
        /// <returns>La expresión sin acciones.</returns>
        private static string removeActionsFromExpression(string text, ref string functionName)
        {
            // Elimina todo lo contenido dentro de {}
            string result = "";

            if (text.Contains('{') && text.Contains('}')) // Si la expresión contiene '{' y '}'
            {
                for (int i = 0; i < text.Length; i++) // Recorre la expresión
                {
                    if (text[i] == '\'') // Si el carácter actual es '
                    {
                        result += $"'{text[i + 1]}'"; // Añade el carácter entre '' a la expresión resultante
                        i += 2; // Avanza dos posiciones en la cadena original
                    }
                    else if (text[i] == '{') // Si el carácter actual es {
                    {
                        int counter = 0; // Contador para determinar la longitud de la función
                        char? actualChar = text[i]; // Carácter actual

                        while (actualChar != '}') // Mientras el carácter actual no sea }
                        {
                            counter++; // Incrementa el contador
                            actualChar = text[i + counter]; // Obtiene el siguiente carácter
                            functionName += actualChar; // Añade el carácter a la cadena del nombre de la función
                        }

                        functionName = functionName.Replace("}", ""); // Elimina el carácter '}' del nombre de la función
                        functionName = functionName.Replace(" ", ""); // Elimina los espacios en blanco del nombre de la función

                        i += counter; // Avanza el índice por la longitud de la función
                    }
                    else
                    {
                        result += text[i]; // Añade el carácter a la expresión resultante
                    }
                }
                return result; // Retorna la expresión resultante sin acciones
            }

            return text; // Retorna la expresión original si no hay acciones
        }



    }
}
