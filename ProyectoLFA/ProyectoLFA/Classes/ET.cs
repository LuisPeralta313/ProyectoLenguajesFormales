using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Drawing;

namespace ProyectoLFA.Classes
{
    public class ET : CharSET
    {
        public Node root;
        public string expression;


        public Dictionary<string, string[]> sets = new Dictionary<string, string[]>();
        public List<Token> tokens = new List<Token>();
        private readonly Dictionary<int, string> leafNodeValues = new Dictionary<int, string>();
        public List<Action> actions = new List<Action>();
        public Dictionary<int, string> actionReference = new Dictionary<int, string>();

        public ET()
        {
            root = null;
        }

        public ET(string expression, Dictionary<string, string[]> sets, List<int> tokenNumbers)
        {
            this.sets = sets;
            this.expression = expression;

            checkForEndCharacter(ref expression);
            Queue<string> Tokens = getTokensFromGrammarExpression(expression, sets);
            ShuntingYard(Tokens);

            setNumberInNodes();
            setNullableNodes();
            setFirstPos();
            setLastPos();

            setTokensListOfValues(tokenNumbers);

        }

        private void checkForEndCharacter(ref string expression)
        {
            if (expression[expression.Length - 1].ToString() != EndCharacter)
            {
                expression = $"({expression}){EndCharacter}";
            }
        }

        private Queue<string> getTokensFromExpression(string expression)
        {
            Queue<string> tokens = new Queue<string>();
            int lenght = expression.Length;

            for (int i = 0; i < lenght; i++)
            {
                if (expression[i].ToString() == EndCharacter)
                {
                    tokens.Enqueue(expression[i].ToString());
                    break;
                }
                if (expression[i].ToString() == Escape)
                {
                    tokens.Enqueue(expression[i].ToString());
                    tokens.Enqueue(expression[i + 1].ToString());
                    //Prevent a concatenation with an operation
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
                    tokens.Enqueue(expression[i].ToString());
                }
                else //it must be a valid char tha can be concatenated ( + * ? a..z etc
                {
                    tokens.Enqueue(expression[i].ToString());
                    tokens.Enqueue(Concatenation);
                }
            }

            return tokens;
        }

        /// <summary>
        /// Adds each character and custom values defined in sets from the string to a Queque 
        /// </summary>
        private Queue<string> getTokensFromGrammarExpression(string expression, Dictionary<string, string[]> sets)
        {
            expression = removeExtraSpacesFromString(expression);

            Queue<string> tokens = new Queue<string>();
            int lenght = expression.Length;

            for (int i = 0; i < lenght; i++)
            {
                string item = expression[i].ToString();

                if (item == EndCharacter)
                {
                    tokens.Enqueue(expression[i].ToString());
                    break;
                }

                if (item == Char_Separator) //if it begins with '
                {
                    string itemAhead = expression[i + 2].ToString();

                    if (itemAhead == Char_Separator) //if it ends with '
                    {

                        tokens.Enqueue(Escape);
                        tokens.Enqueue(expression[i + 1].ToString());

                        //Prevent a concatenation with an operation
                        if (expression[i + 3].ToString() != Grouping_Close &&
                            !isAnOperationChar(expression[i + 3].ToString()))
                        {
                            tokens.Enqueue(Concatenation);
                        }
                        i += 2;
                    }
                    else if (itemAhead != " ") //ignore blank spaces
                    {
                        throw new Exception("Formato invalido, se esperaba '");
                    }
                }
                else if ((isABinaryOperationChar(item) || item == Grouping_Open ||
                         item == EndCharacter || isAnOperationChar(expression[i + 1].ToString()) ||
                         expression[i + 1].ToString() == Grouping_Close))
                {
                    tokens.Enqueue(expression[i].ToString());
                }
                else //it must be a valid char that can be concatenated ) + * ? a..z etc
                {
                    if (item == Grouping_Close | item == Plus |
                             item == Star | item == QuestionMark)
                    {
                        tokens.Enqueue(item);

                        //Prevent a concatenation with an operation
                        if (expression[i + 1].ToString() != Grouping_Close && !isAnOperationChar(expression[i + 1].ToString()))
                        {
                            tokens.Enqueue(Concatenation);
                        }
                    }
                    else if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item))
                    //it is a special token (defined in set)
                    {
                        string value = "";
                        string token = expression[i].ToString();
                        int counter = 0;

                        while (token != " " && token != Char_Separator && token != Grouping_Close && token != Grouping_Open &&
                               lenght > i + counter && !isAnOperationChar(expression[i + counter].ToString()))
                        {
                            value += token;
                            counter++;
                            token = expression[i + counter].ToString();
                        }

                        if (sets.ContainsKey(value))
                        {
                            tokens.Enqueue(value);

                            //Prevent a concatenation with an operation
                            if (expression[i + counter].ToString() != Grouping_Close && !isAnOperationChar(expression[i + counter].ToString()))
                            {
                                tokens.Enqueue(Concatenation);
                            }

                            i += counter - 1;
                        }
                        else
                        {
                            throw new Exception($"No existe una definicion del SET {value}");
                        }
                    }
                }
            }

            return tokens;
        }

        private string removeExtraSpacesFromString(string input)
        {
            string result = "";

            for (int i = 0; i < input.Length; i++)
            {
                char item = input[i];

                if (item != ' ')
                {
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
                //if last item added was not a blankspace or the bext is a parentesis
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


        private void ShuntingYard(Queue<string> regularExpression)
        {
            //Tokens
            Stack<string> T = new Stack<string>();
            //Trees
            Stack<Node> S = new Stack<Node>();

            //Step 1
            while (regularExpression.Count > 0)
            {
                //Step 2
                string token = regularExpression.Dequeue();

                //Step 3
                if (token == Escape)
                {
                    if (regularExpression.Count > 0) //Evaluates if the file is not empty after Tokens
                    {
                        token = regularExpression.Dequeue();
                        S.Push(new Node(token, false));
                    }
                    else
                    {
                        throw new Exception("Se esperaban mas tokens");
                    }
                }
                //Step 4
                else if (isATerminalCharacter(token))
                {
                    S.Push(new Node(token, true));
                }
                //Step 5
                else if (token == Grouping_Open)
                {
                    T.Push(token);
                }
                //Step 6
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
                //Step 7
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
                        // Operation order:
                        // aleternation(|) has less precedence than concat(.)

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
                //Step 8
                else
                {
                    throw new Exception("No es un token reconocido" +
                        "");
                }
            }
            //Step 9 -> return to step 2 if there are still tokens in expression
            //Step 10
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
            //Step 11 -> return to step 10 if there are still tokens in T
            //Step 12
            if (S.Count == 1)
            {
                //Step 13
                root = S.Pop();
            }
            else
            {
                throw new Exception("Faltan operandos");
            }
        }

        //Evaluates if item is +, * or ?
        private bool isASingleOperationChar(string item)
        {
            string[] SpecialCharacters = { Star, Plus, QuestionMark };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }

        //Evaluates if item is | or .
        private bool isABinaryOperationChar(string item)
        {
            string[] SpecialCharacters = { Alternation, Concatenation };

            if (SpecialCharacters.Contains(item))
            {
                return true;
            }

            return false;
        }

        //Evaluates if item is an operation(+,*,?,.,|)
        private bool isAnOperationChar(string item)
        {
            if (isABinaryOperationChar(item) || isASingleOperationChar(item))
            {
                return true;
            }

            return false;
        }

        //Verify if item is a character that is used to do an operation like jump, parenthesis, concatenation, etc.
        private bool isATerminalCharacter(string item)
        {
            string[] SpecialCharacters = { Escape, Grouping_Open, Grouping_Close };

            if (SpecialCharacters.Contains(item) || isAnOperationChar(item))
            {
                return false;
            }

            return true;
        }

        private void setNumberInNodes()
        {
            int number = 1;
            setNumberInNodes(ref root, ref number);
        }
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

        private void setNullableNodes()
        {
            setNullableNodes(ref root);
        }
        private void setNullableNodes(ref Node i)
        {
            if (i.isLeaf())
            {
                i.nullable = i.expresion == Epsilon;
            }
            else
            {
                //First, get nullable states from child. (Postorder)
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

                //Rules
                switch (i.expresion)
                {
                    case Alternation: //nullable(c1) or nullable(c2)
                        i.nullable = i.Right.nullable || i.Left.nullable;
                        break;
                    case Concatenation: //nullable(c1) and nullable(c2)
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
                        throw new Exception("Operacion no reconocida.");
                }
            }
        }

        private void setFirstPos()
        {
            setFirstPos(ref root);
            root.firstPos.Sort();
        }
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
                //(Postorder)
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

                //Rules
                switch (i.expresion)
                {
                    case Alternation: //fistpos(c1) U fistpos(c2)
                        i.firstPos = i.Left.firstPos.Concat(i.Right.firstPos).ToList();
                        break;

                    case Concatenation:
                        if (i.Left.nullable) //fistpos(c1) U fistpos(c2)
                        {
                            i.firstPos = i.Left.firstPos.Concat(i.Right.firstPos).ToList();
                        }
                        else //fistpos(c1)
                        {
                            i.firstPos = i.Left.firstPos;
                        }
                        break;

                    case Star: //fistpos(c1)
                        i.firstPos = i.Left.firstPos;
                        break;

                    case Plus: //fistpos(c1)
                        i.firstPos = i.Left.firstPos;
                        break;

                    case QuestionMark: //fistpos(c1)
                        i.firstPos = i.Left.firstPos;
                        break;

                    default:
                        throw new Exception("Operacion no reconocida.");
                }
            }
        }

        private void setLastPos()
        {
            setLastPos(ref root);
        }
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
                //(Postorder)
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

                //Cases
                switch (i.expresion)
                {
                    case Alternation: //Lastpos(c1) U Lastpos(c2)
                        i.lastPos = i.Left.lastPos.Concat(i.Right.lastPos).ToList();
                        break;

                    case Concatenation:
                        if (i.Right.nullable) //Lastpos(c1) U Lastpos(c2)
                        {
                            i.lastPos = i.Left.lastPos.Concat(i.Right.lastPos).ToList();
                        }
                        else //lastpos(c2)
                        {
                            i.lastPos = i.Right.lastPos;
                        }
                        break;
                    case Star: //lastpos(c1)
                        i.lastPos = i.Left.lastPos;
                        break;

                    case Plus: //lastpos(c1)
                        i.lastPos = i.Left.lastPos;
                        break;

                    case QuestionMark: //lastpos(c1)
                        i.lastPos = i.Left.lastPos;
                        break;

                    default:
                        throw new Exception("Operacion no reconocida.");
                }
            }
        }

        public List<string[]> getValuesOfNodes()
        {
            //Simbolo, First, Last, Nullable
            List<string[]> cells = new List<string[]>();
            int j = 0;

            getValuesOfNodes(root, ref cells, ref j);

            return cells;
        }
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

            //Symbol, First, Last, Nullable
            cells.Add(new[]
            {i.expresion, string.Join( ",", i.firstPos),
                string.Join( ",", i.lastPos), i.nullable.ToString()});

        }
        private void setTokensListOfValues(List<int> TokenNumbers)
        {
            int tmp = 0;
            setTokensListOfValues(this.root.Left, TokenNumbers, ref tmp);
        }
        private void setTokensListOfValues(Node i, List<int> TokenNumbers, ref int actualToken)
        {
            if (actualToken <= TokenNumbers.Count - 1) //If it's not the last token
            {
                if (i.expresion == Alternation)
                {
                    //Right
                    tokens.Add(new Token(TokenNumbers[TokenNumbers.Count - 1 - actualToken],
                    getCharValuesOfNode(i.Right.firstPos),
                    getCharValuesOfNode(i.Right.lastPos)));

                    actualToken++;

                    //Left (last item)
                    if (TokenNumbers.Count - actualToken == 1)
                    {
                        tokens.Add(new Token(TokenNumbers[0],
                            getCharValuesOfNode(i.Left.firstPos),
                            getCharValuesOfNode(i.Left.lastPos)));

                        actualToken++;
                    }

                    setTokensListOfValues(i.Left, TokenNumbers, ref actualToken);
                }
            }
        }

        private List<char> getCharValuesOfNode(List<int> nodes)
        {
            List<char> chars = new List<char>();

            foreach (var item in nodes)
            {
                string NodeValue = leafNodeValues[item];

                //If it is single char
                if (NodeValue.Length == 1)
                {
                    chars.Add(NodeValue.ToCharArray()[0]);
                }
                //If it is a set
                else if (NodeValue.Length > 1)
                {
                    string[] setNumbers = sets[NodeValue];

                    foreach (var VAR in setNumbers)
                    {
                        if (VAR.Contains(","))
                        {
                            string[] limits = VAR.Split(',');
                            int lowerlimit = int.Parse(limits[0]);
                            int upperlimit = int.Parse(limits[1]);

                            for (int actualChar = lowerlimit; actualChar <= upperlimit; actualChar++)
                            {
                                chars.Add((char)actualChar);
                            }
                        }
                        else
                        {
                            int character = int.Parse(VAR);
                            chars.Add((char)character);
                        }
                    }
                }
            }

            return chars.Distinct().ToList();
        }

        private int countNodes(Node i)
        {
            if (i != null)
            {
                return 1 + countNodes(i.Left) + countNodes(i.Right);
            }
            else
            {
                return 0;
            }
        }
    

    }    
}
