using ProyectoLFA.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace ProyectoLFA.Clases
{
    public class ET : CharSET
    {
        /// <summary>
        /// Raíz del árbol de expresión.
        /// </summary>
        public Node root;

        /// <summary>
        /// Expresión asociada al árbol de expresión.
        /// </summary>
        public string expression;

        /// <summary>
        /// Conjunto de sets definidos en la expresión regular.
        /// </summary>
        public Dictionary<string, string[]> sets = new Dictionary<string, string[]>();


        public List<Action> actions = new List<Action>();
        public Dictionary<int, string> actionReference = new Dictionary<int, string>();
        /// <summary>
        /// Lista de tokens utilizados en la expresión regular.
        /// </summary>
        public List<Token> tokens = new List<Token>();

        /// <summary>
        /// Diccionario que almacena los valores de los nodos hoja del árbol de expresión.
        /// </summary>
        private readonly Dictionary<int, string> leafNodeValues = new Dictionary<int, string>();

        /// <summary>
        /// Constructor de la clase ET.
        /// </summary>
        public ET()
        {
            root = null;
        }

        /// <summary>
        /// Constructor de la clase ET que inicializa un objeto ET con una expresión regular, conjuntos definidos y números de token.
        /// </summary>
        /// <param name="expression">Expresión regular.</param>
        /// <param name="sets">Conjuntos definidos en la expresión regular.</param>
        /// <param name="tokenNumbers">Números de token.</param>
        public ET(string expression, Dictionary<string, string[]> sets, List<int> tokenNumbers)
        {
            this.sets = sets;
            this.expression = expression;

            // Verifica si la expresión termina con el caracter de fin, si no, lo agrega.
            checkForEndCharacter(ref expression);

            // Obtiene los tokens de la expresión regular y convierte la expresión en notación posfija.
            Queue<string> Tokens = getTokensFromGrammarExpression(expression, sets);
            ShuntingYard(Tokens);

            // Establece números en los nodos.
            setNumberInNodes();

            // Establece si los nodos son nulos o no.
            setNullableNodes();

            // Establece las posiciones de primeros.
            setFirstPos();

            // Establece las posiciones de últimos.
            setLastPos();

            // Establece los valores de los tokens.
            setTokensListOfValues(tokenNumbers);
        }

        /// <summary>
        /// Verifica si la expresión termina con el caracter de fin '#', si no lo agrega.
        /// </summary>
        /// <param name="expression">Expresión regular.</param>
        private void checkForEndCharacter(ref string expression)
        {
            if (expression[expression.Length - 1].ToString() != EndCharacter)
            {
                expression = $"({expression}){EndCharacter}";
            }
        }


        /// <summary>
        /// Obtiene los tokens de una expresión regular.
        /// </summary>
        /// <param name="expression">Expresión regular.</param>
        /// <returns>Cola de tokens.</returns>
        private Queue<string> getTokensFromExpression(string expression)
        {
            Queue<string> tokens = new Queue<string>();
            int length = expression.Length;

            for (int i = 0; i < length; i++)
            {
                // Si el caracter actual es el caracter de fin, lo encola y termina el bucle.
                if (expression[i].ToString() == EndCharacter)
                {
                    tokens.Enqueue(expression[i].ToString());
                    break;
                }

                // Si el caracter actual es el caracter de escape, encola el caracter de escape y el siguiente caracter.
                // Además, verifica si se debe agregar una concatenación después del caracter de escape.
                if (expression[i].ToString() == Escape)
                {
                    tokens.Enqueue(expression[i].ToString());
                    tokens.Enqueue(expression[i + 1].ToString());
                    // Evita una concatenación con una operación.
                    if (expression[i + 2].ToString() != Grouping_Close && !isAnOperationChar(expression[i + 2].ToString()))
                    {
                        tokens.Enqueue(Concatenation);
                    }
                    i++;
                }
                else if (isABinaryOperationChar(expression[i].ToString()) || expression[i].ToString() == Grouping_Open ||
                            expression[i].ToString() == EndCharacter || isAnOperationChar(expression[i + 1].ToString()) ||
                            expression[i + 1].ToString() == Grouping_Close)
                {
                    // Si el caracter actual es un operador binario, un paréntesis de apertura, el caracter de fin,
                    // un operador o el siguiente caracter es un paréntesis de cierre, lo encola.
                    tokens.Enqueue(expression[i].ToString());
                }
                else // Debe ser un caracter válido que se puede concatenar ( + * ? a..z etc).
                {
                    tokens.Enqueue(expression[i].ToString());
                    tokens.Enqueue(Concatenation);
                }
            }

            return tokens;
        }


        /// <summary>
        /// Agrega cada carácter y valores personalizados definidos en conjuntos desde la cadena a una cola.
        /// </summary>
        /// <param name="expression">Expresión a analizar.</param>
        /// <param name="sets">Conjuntos definidos.</param>
        /// <returns>Cola de tokens.</returns>
        private Queue<string> getTokensFromGrammarExpression(string expression, Dictionary<string, string[]> sets)
        {
            expression = removeExtraSpacesFromString(expression);

            Queue<string> tokens = new Queue<string>();
            int length = expression.Length;

            for (int i = 0; i < length; i++)
            {
                string item = expression[i].ToString();

                // Si el carácter actual es el carácter de fin, lo encola y termina el bucle.
                if (item == EndCharacter)
                {
                    tokens.Enqueue(expression[i].ToString());
                    break;
                }

                // Si el carácter actual es el separador de caracteres, se analiza el próximo carácter.
                if (item == Char_Separator)
                {
                    string itemAhead = expression[i + 2].ToString();

                    // Si el próximo carácter es también el separador de caracteres, se añade el valor de escape y el siguiente carácter a la cola.
                    // Además, verifica si se debe agregar una concatenación después del valor de escape.
                    if (itemAhead == Char_Separator)
                    {
                        tokens.Enqueue(Escape);
                        tokens.Enqueue(expression[i + 1].ToString());

                        // Evita una concatenación con una operación.
                        if (expression[i + 3].ToString() != Grouping_Close &&
                            !isAnOperationChar(expression[i + 3].ToString()))
                        {
                            tokens.Enqueue(Concatenation);
                        }
                        i += 2;
                    }
                    // Si el próximo carácter no es un espacio en blanco, se lanza una excepción de formato inválido.
                    else if (itemAhead != " ")
                    {
                        throw new Exception("Formato inválido, se esperaba '");
                    }
                }
                // Si el carácter actual es un operador binario, un paréntesis de apertura, el carácter de fin,
                // un operador o el siguiente carácter es un paréntesis de cierre, se añade a la cola.
                else if ((isABinaryOperationChar(item) || item == Grouping_Open ||
                         item == EndCharacter || isAnOperationChar(expression[i + 1].ToString()) ||
                         expression[i + 1].ToString() == Grouping_Close))
                {
                    tokens.Enqueue(expression[i].ToString());
                }
                // Debe ser un carácter válido que se puede concatenar ) + * ? a..z, etc.
                else
                {
                    if (item == Grouping_Close | item == Plus |
                             item == Star | item == QuestionMark)
                    {
                        tokens.Enqueue(item);

                        // Evita una concatenación con una operación.
                        if (expression[i + 1].ToString() != Grouping_Close && !isAnOperationChar(expression[i + 1].ToString()))
                        {
                            tokens.Enqueue(Concatenation);
                        }
                    }
                    // Si no está vacío ni es nulo, y no es un espacio en blanco, es un token especial (definido en el conjunto).
                    else if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item))
                    {
                        string value = "";
                        string token = expression[i].ToString();
                        int counter = 0;

                        // Se recorre la cadena hasta encontrar un espacio en blanco, un separador de caracteres, un paréntesis de cierre, 
                        // un paréntesis de apertura o un operador.
                        while (token != " " && token != Char_Separator && token != Grouping_Close && token != Grouping_Open &&
                               length > i + counter && !isAnOperationChar(expression[i + counter].ToString()))
                        {
                            value += token;
                            counter++;
                            token = expression[i + counter].ToString();
                        }

                        // Si el valor está presente en los conjuntos definidos, se añade a la cola.
                        // Además, verifica si se debe agregar una concatenación después del valor del conjunto.
                        if (sets.ContainsKey(value))
                        {
                            tokens.Enqueue(value);

                            // Evita una concatenación con una operación.
                            if (expression[i + counter].ToString() != Grouping_Close && !isAnOperationChar(expression[i + counter].ToString()))
                            {
                                tokens.Enqueue(Concatenation);
                            }

                            i += counter - 1;
                        }
                        // Si no existe una definición para el conjunto, se lanza una excepción.
                        else
                        {
                            throw new Exception($"No existe una definición para el conjunto '{value}'");
                        }
                    }
                }
            }

            return tokens;
        }


        /// <summary>
        /// Elimina los espacios adicionales de una cadena, manteniendo aquellos necesarios para separar tokens.
        /// </summary>
        /// <param name="input">Cadena de entrada.</param>
        /// <returns>Cadena sin espacios adicionales.</returns>
        private string removeExtraSpacesFromString(string input)
        {
            string result = "";

            for (int i = 0; i < input.Length; i++)
            {
                char item = input[i];

                // Si el carácter actual no es un espacio en blanco.
                if (item != ' ')
                {
                    // Si el carácter actual es el comienzo de un carácter delimitado por comillas, se procesa como un solo carácter.
                    if (item == '\'')
                    {
                        result += $"'{input[i + 1]}'";
                        i += 2;
                    }
                    else
                    {
                        result += item;
                    }
                }
                // Si el último carácter agregado no fue un espacio en blanco, ni el próximo carácter es un operador,
                // ni el último carácter agregado fue el inicio de un carácter delimitado por comillas, 
                // ni el próximo carácter es un paréntesis de cierre, ni el próximo carácter es un espacio en blanco.
                else if ((result[result.Length - 1] != ' ' &&
                         !isAnOperationChar(input[i + 1].ToString()) &&
                         result[result.Length - 1] != '\'') &&
                         (input[i + 1].ToString() != Grouping_Close) &&
                         (input[i + 1] != ' '))
                {
                    result += item;
                }
            }

            return result;
        }



        /// <summary>
        /// Implementa el algoritmo de Shunting Yard para convertir una expresión regular en una estructura de árbol.
        /// </summary>
        /// <param name="regularExpression">Expresión regular en forma de cola de tokens.</param>
        private void ShuntingYard(Queue<string> regularExpression)
        {
            // Pila para los tokens
            Stack<string> T = new Stack<string>();
            // Pila para los nodos del árbol
            Stack<Node> S = new Stack<Node>();

            // Paso 1
            while (regularExpression.Count > 0)
            {
                // Paso 2
                string token = regularExpression.Dequeue();

                // Paso 3
                if (token == Escape)
                {
                    if (regularExpression.Count > 0) // Se evalúa si la cola de tokens no está vacía
                    {
                        token = regularExpression.Dequeue();
                        S.Push(new Node(token, false));
                    }
                    else
                    {
                        throw new Exception("Se esperaban más tokens");
                    }
                }
                // Paso 4
                else if (isATerminalCharacter(token))
                {
                    S.Push(new Node(token, true));
                }
                // Paso 5
                else if (token == Grouping_Open)
                {
                    T.Push(token);
                }
                // Paso 6
                else if (token == Grouping_Close)
                {
                    while (T.Count > 0 && T.Peek() != Grouping_Open)
                    {
                        if (T.Count == 0 || S.Count < 2)
                        {
                            throw new Exception("Faltan operandos");
                        }

                        Node temp = new Node(T.Pop());
                        temp.Right = S.Pop();
                        temp.Left = S.Pop();
                        S.Push(temp);
                    }
                    string tmp = T.Pop();
                }
                // Paso 7
                else if (isAnOperationChar(token))
                {
                    if (isASingleOperationChar(token))
                    {
                        Node op = new Node(token);

                        if (S.Count >= 0)
                        {
                            op.Left = S.Pop();
                            S.Push(op);
                        }
                        else
                        {
                            throw new Exception("Faltan operandos");
                        }
                    }
                    else if (T.Count > 0 && T.Peek() != Grouping_Open &&
                                ((T.Peek() == Concatenation && token == Alternation) ||
                                    (T.Peek() == token)))
                    {
                        // Orden de operación:
                        // La alternación (|) tiene menos precedencia que la concatenación (.)

                        Node temp = new Node(T.Pop());

                        if (S.Count >= 2)
                        {
                            temp.Right = S.Pop();
                            temp.Left = S.Pop();

                            S.Push(temp);
                            T.Push(token);
                        }
                        else
                        {
                            throw new Exception("Faltan operandos");
                        }
                    }
                    else
                    {
                        T.Push(token);
                    }
                }
                // Paso 8
                else
                {
                    throw new Exception("No es un token reconocido");
                }
            }
            // Paso 9 -> Regresa al Paso 2 si aún hay tokens en la expresión
            // Paso 10
            while (T.Count > 0)
            {
                Node temp = new Node(T.Pop());
                if (temp.expresion != Grouping_Open && S.Count >= 2)
                {
                    temp.Right = S.Pop();
                    temp.Left = S.Pop();
                    S.Push(temp);
                }
                else
                {
                    throw new Exception("Faltan operandos");
                }
            }
            // Paso 11 -> Regresa al Paso 10 si aún hay tokens en T
            // Paso 12
            if (S.Count == 1)
            {
                // Paso 13
                root = S.Pop();
            }
            else
            {
                throw new Exception("Faltan operandos");
            }
        }


        /// <summary>
        /// Determina si el elemento es +, * o ?.
        /// </summary>
        /// <param name="item">El elemento a evaluar.</param>
        /// <returns>Verdadero si el elemento es +, * o ?, de lo contrario, falso.</returns>
        private bool isASingleOperationChar(string item)
        {
            string[] SpecialCharacters = { Star, Plus, QuestionMark };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Determina si el elemento es | o ..
        /// </summary>
        /// <param name="item">El elemento a evaluar.</param>
        /// <returns>Verdadero si el elemento es | o .., de lo contrario, falso.</returns>
        private bool isABinaryOperationChar(string item)
        {
            string[] SpecialCharacters = { Alternation, Concatenation };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Determina si el elemento es una operación (+, *, ?, ., |).
        /// </summary>
        /// <param name="item">El elemento a evaluar.</param>
        /// <returns>Verdadero si el elemento es una operación, de lo contrario, falso.</returns>
        private bool isAnOperationChar(string item)
        {
            if (isABinaryOperationChar(item) || isASingleOperationChar(item))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Verifica si el elemento es un carácter terminal que se utiliza para realizar una operación, como salto, paréntesis, concatenación, etc.
        /// </summary>
        /// <param name="item">El elemento a evaluar.</param>
        /// <returns>Verdadero si el elemento es un carácter terminal, de lo contrario, falso.</returns>
        private bool isATerminalCharacter(string item)
        {
            string[] SpecialCharacters = { Escape, Grouping_Open, Grouping_Close };

            if (SpecialCharacters.Contains(item) || isAnOperationChar(item))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Asigna números a los nodos hoja del árbol de expresión y los almacena junto con sus valores en un diccionario.
        /// </summary>
        private void setNumberInNodes()
        {
            int number = 1;
            setNumberInNodes(ref root, ref number);
        }

        /// <summary>
        /// Asigna números a los nodos hoja del árbol de expresión y los almacena junto con sus valores en un diccionario.
        /// </summary>
        /// <param name="i">El nodo actual que se está procesando.</param>
        /// <param name="number">El número a asignar al nodo actual.</param>
        private void setNumberInNodes(ref Node i, ref int number)
        {
            if (i.isLeaf())
            {
                if (i.expresion != Epsilon)
                {
                    leafNodeValues.Add(number, i.expresion);
                    i.number = number;
                    number++;
                }
            }
            else
            {
                if (i.Left != null)
                {
                    Node k = i.Left;
                    setNumberInNodes(ref k, ref number);
                    i.Left = k;
                }
                if (i.Right != null)
                {
                    Node k = i.Right;
                    setNumberInNodes(ref k, ref number);
                    i.Right = k;
                }
            }
        }


        /// <summary>
        /// Establece el valor de "nullable" para cada nodo en el árbol de expresión.
        /// </summary>
        private void setNullableNodes()
        {
            setNullableNodes(ref root);
        }

        /// <summary>
        /// Establece el valor de "nullable" para cada nodo en el árbol de expresión.
        /// </summary>
        /// <param name="i">El nodo actual que se está procesando.</param>
        private void setNullableNodes(ref Node i)
        {
            if (i.isLeaf())
            {
                i.nullable = i.expresion == Epsilon;
            }
            else
            {
                // Primero, obtén los estados "nullable" de los hijos. (Recorrido posterior)
                if (i.Left != null)
                {
                    Node k = i.Left;
                    setNullableNodes(ref k);
                    i.Left = k;
                }
                if (i.Right != null)
                {
                    Node k = i.Right;
                    setNullableNodes(ref k);
                    i.Right = k;
                }

                // Reglas
                switch (i.expresion)
                {
                    case Alternation: // nullable(c1) o nullable(c2)
                        i.nullable = i.Right.nullable || i.Left.nullable;
                        break;
                    case Concatenation: // nullable(c1) y nullable(c2)
                        i.nullable = i.Right.nullable && i.Left.nullable;
                        break;
                    case Star:
                        i.nullable = true;
                        break;
                    case Plus:
                        i.nullable = i.Left.nullable;
                        break;
                    case QuestionMark:
                        i.nullable = true;
                        break;
                    default:
                        throw new Exception("Operación no reconocida.");
                }
            }
        }


        /// <summary>
        /// Establece los conjuntos de primeras posiciones para cada nodo en el árbol de expresión.
        /// </summary>
        private void setFirstPos()
        {
            setFirstPos(ref root);
            root.firstPos.Sort(); // Ordena las primeras posiciones del nodo raíz.
        }

        /// <summary>
        /// Establece los conjuntos de primeras posiciones para cada nodo en el árbol de expresión.
        /// </summary>
        /// <param name="i">El nodo actual que se está procesando.</param>
        private void setFirstPos(ref Node i)
        {
            if (i.isLeaf())
            {
                if (i.expresion != Epsilon)
                {
                    i.firstPos.Add(i.number);
                }
            }
            else
            {
                // Recorrido posterior
                if (i.Left != null)
                {
                    Node k = i.Left;
                    setFirstPos(ref k);
                    i.Left = k;
                }
                if (i.Right != null)
                {
                    Node k = i.Right;
                    setFirstPos(ref k);
                    i.Right = k;
                }

                // Reglas para calcular las primeras posiciones
                switch (i.expresion)
                {
                    case Alternation: // fistPos(c1) ∪ fistPos(c2)
                        i.firstPos = i.Left.firstPos.Concat(i.Right.firstPos).ToList();
                        break;

                    case Concatenation:
                        if (i.Left.nullable) // fistPos(c1) ∪ fistPos(c2)
                        {
                            i.firstPos = i.Left.firstPos.Concat(i.Right.firstPos).ToList();
                        }
                        else // fistPos(c1)
                        {
                            i.firstPos = i.Left.firstPos;
                        }
                        break;

                    case Star: // fistPos(c1)
                        i.firstPos = i.Left.firstPos;
                        break;

                    case Plus: // fistPos(c1)
                        i.firstPos = i.Left.firstPos;
                        break;

                    case QuestionMark: // fistPos(c1)
                        i.firstPos = i.Left.firstPos;
                        break;

                    default:
                        throw new Exception("Operación no reconocida.");
                }
            }
        }


        /// <summary>
        /// Establece los conjuntos de últimas posiciones para cada nodo en el árbol de expresión.
        /// </summary>
        private void setLastPos()
        {
            setLastPos(ref root);
        }

        /// <summary>
        /// Establece los conjuntos de últimas posiciones para cada nodo en el árbol de expresión.
        /// </summary>
        /// <param name="i">El nodo actual que se está procesando.</param>
        private void setLastPos(ref Node i)
        {
            if (i.isLeaf())
            {
                if (i.expresion != Epsilon)
                {
                    i.lastPos.Add(i.number);
                }
            }
            else
            {
                // Recorrido posterior
                if (i.Left != null)
                {
                    Node k = i.Left;
                    setLastPos(ref k);
                    i.Left = k;
                }
                if (i.Right != null)
                {
                    Node k = i.Right;
                    setLastPos(ref k);
                    i.Right = k;
                }

                // Casos para calcular las últimas posiciones
                switch (i.expresion)
                {
                    case Alternation: // LastPos(c1) ∪ LastPos(c2)
                        i.lastPos = i.Left.lastPos.Concat(i.Right.lastPos).ToList();
                        break;

                    case Concatenation:
                        if (i.Right.nullable) // LastPos(c1) ∪ LastPos(c2)
                        {
                            i.lastPos = i.Left.lastPos.Concat(i.Right.lastPos).ToList();
                        }
                        else // LastPos(c2)
                        {
                            i.lastPos = i.Right.lastPos;
                        }
                        break;

                    case Star: // LastPos(c1)
                        i.lastPos = i.Left.lastPos;
                        break;

                    case Plus: // LastPos(c1)
                        i.lastPos = i.Left.lastPos;
                        break;

                    case QuestionMark: // LastPos(c1)
                        i.lastPos = i.Left.lastPos;
                        break;

                    default:
                        throw new Exception("Operación no reconocida.");
                }
            }
        }


        /// <summary>
        /// Obtiene los valores de los nodos del árbol de expresión, incluyendo el símbolo, el conjunto de primeras posiciones,
        /// el conjunto de últimas posiciones y si el nodo es nullable o no.
        /// </summary>
        /// <returns>Una lista de arreglos de cadenas que contiene los valores de los nodos.</returns>
        public List<string[]> getValuesOfNodes()
        {
            // Simbolo, First, Last, Nullable
            List<string[]> cells = new List<string[]>();
            int j = 0;

            // Llama al método recursivo para obtener los valores de los nodos
            getValuesOfNodes(root, ref cells, ref j);

            return cells;
        }

        /// <summary>
        /// Recorre el árbol de expresión en postorden para obtener los valores de los nodos, incluyendo el símbolo,
        /// el conjunto de primeras posiciones, el conjunto de últimas posiciones y si el nodo es nullable o no.
        /// </summary>
        /// <param name="i">El nodo actual que se está evaluando.</param>
        /// <param name="cells">La lista donde se almacenarán los valores de los nodos.</param>
        /// <param name="j">Un contador utilizado para rastrear la posición actual en la lista de nodos.</param>
        private void getValuesOfNodes(Node i, ref List<string[]> cells, ref int j)
        {
            if (i.Left != null)
            {
                j++;
                getValuesOfNodes(i.Left, ref cells, ref j);
            }
            if (i.Right != null)
            {
                j++;
                getValuesOfNodes(i.Right, ref cells, ref j);
            }

            // Agrega los valores del nodo a la lista de celdas
            // en el formato [Símbolo, Primeras posiciones, Últimas posiciones, Nullable]
            cells.Add(new[]
            {
                i.expresion,
                string.Join(",", i.firstPos),
                string.Join(",", i.lastPos),
                i.nullable.ToString()
            });
        }

        /// <summary>
        /// Asigna los valores de los tokens a partir de los números de token especificados.
        /// </summary>
        /// <param name="TokenNumbers">La lista de números de token.</param>
        private void setTokensListOfValues(List<int> TokenNumbers)
        {
            int tmp = 0;
            setTokensListOfValues(this.root.Left, TokenNumbers, ref tmp);
        }

        /// <summary>
        /// Recorre el árbol de expresión para asignar los valores de los tokens a los nodos del árbol.
        /// </summary>
        /// <param name="i">El nodo actual que se está evaluando.</param>
        /// <param name="TokenNumbers">La lista de números de token.</param>
        /// <param name="actualToken">El índice del token actual en la lista de números de token.</param>
        private void setTokensListOfValues(Node i, List<int> TokenNumbers, ref int actualToken)
        {
            if (actualToken <= TokenNumbers.Count - 1) // Si no es el último token
            {
                if (i.expresion == Alternation)
                {
                    // Asigna el valor del token para el nodo derecho
                    tokens.Add(new Token(TokenNumbers[TokenNumbers.Count - 1 - actualToken],
                        getCharValuesOfNode(i.Right.firstPos),
                        getCharValuesOfNode(i.Right.lastPos)));

                    actualToken++;

                    // Asigna el valor del token para el nodo izquierdo (último elemento)
                    if (TokenNumbers.Count - actualToken == 1)
                    {
                        tokens.Add(new Token(TokenNumbers[0],
                            getCharValuesOfNode(i.Left.firstPos),
                            getCharValuesOfNode(i.Left.lastPos)));

                        actualToken++;
                    }

                    // Recursivamente procesa el nodo izquierdo
                    setTokensListOfValues(i.Left, TokenNumbers, ref actualToken);
                }
            }
        }


        /// <summary>
        /// Obtiene los valores de caracteres correspondientes a los nodos especificados.
        /// </summary>
        /// <param name="nodes">La lista de nodos para los cuales se obtendrán los valores de caracteres.</param>
        /// <returns>Una lista de caracteres correspondientes a los nodos especificados.</returns>
        private List<char> getCharValuesOfNode(List<int> nodes)
        {
            List<char> chars = new List<char>();

            // Itera a través de los nodos
            foreach (var item in nodes)
            {
                string NodeValue = leafNodeValues[item];

                // Si es un solo carácter
                if (NodeValue.Length == 1)
                {
                    chars.Add(NodeValue[0]);
                }
                // Si es un conjunto
                else if (NodeValue.Length > 1)
                {
                    string[] setNumbers = sets[NodeValue];

                    // Itera a través de los números de conjunto
                    foreach (var VAR in setNumbers)
                    {
                        if (VAR.Contains(","))
                        {
                            // Si es un rango de caracteres
                            string[] limits = VAR.Split(',');
                            int lowerlimit = int.Parse(limits[0]);
                            int upperlimit = int.Parse(limits[1]);

                            // Agrega todos los caracteres en el rango al conjunto de caracteres
                            for (int actualChar = lowerlimit; actualChar <= upperlimit; actualChar++)
                            {
                                chars.Add((char)actualChar);
                            }
                        }
                        else
                        {
                            // Si es un carácter individual
                            int character = int.Parse(VAR);
                            chars.Add((char)character);
                        }
                    }
                }
            }

            // Elimina duplicados y devuelve la lista de caracteres
            return chars.Distinct().ToList();
        }


        /// <summary>
        /// Cuenta el número total de nodos en el árbol, comenzando desde el nodo especificado.
        /// </summary>
        /// <param name="i">El nodo desde el cual se iniciará el recuento.</param>
        /// <returns>El número total de nodos en el árbol.</returns>
        private int countNodes(Node i)
        {
            // Si el nodo no es nulo
            if (i != null)
            {
                // Retorna 1 (para el nodo actual) más el recuento de nodos en los subárboles izquierdo y derecho
                return 1 + countNodes(i.Left) + countNodes(i.Right);
            }
            else
            {
                // Si el nodo es nulo, retorna 0
                return 0;
            }
        }



    }
}
