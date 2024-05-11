import java.util.*;

namespace Scanner
{
    public class Scanner
    {
        static Scanner scanner = new Scanner(System.in);

        public static void Main(String[] args)
        {
            Stack<Character> Input = new Stack<Character>();

            while(true)
            {
                System.out.print("\nEscriba la cadena para analizar: ");
                Input = getStack(scanner.nextLine());

                System.out.print("\nAnalizando");
                Thread.Sleep(300);
                System.out.print(".");
                Thread.Sleep(300);
                System.out.print(".");
                Thread.Sleep(800);
                System.out.print(".\n");

                try
                {
                    AnalizarTexto(Input);
                }
                catch (Exception e)
                {
                    System.out.println("\nError: " + e.getMessage());
                }

                System.out.println("\nPresiona cualquier tecla.");
                scanner.nextLine();
                System.out.println();
                goto Inicio;
            }
            
        }

        static Stack<Character> getStack(String input)
        {
            Stack<Character> output = new Stack<Character>();

            for (int i = input.length() - 1; i >= 0; i--)
            {
                output.push(input.charAt(i));
            }

            return output;
        }

        static void Error()
        {
            System.out.println("\nError: La cadena no es valida");
        }

        static void Valido(StringBuilder token)
        {
            System.out.println("\n" + token.toString() + " » " + getTokenNumber(token.toString()));
            token = "";
        }

        static int getTokenNumber(string text)
        {
            char firstCharacterFromText = text.charAt(0);

            char lastCharacterFromText = text.charAt(0);

            if (text.Length() > 1)
            {
                lastCharacterFromText = text[text.length() - 1];
            }

            Map<Integer, List<Character>> Character_Token_First = new HashMap<Integer, List<Character>>
            {
                </FirstPos>
            };

            Dictionary<List<Character>, Integer> Character_Token_Last = new Dictionary<List<Character>, Integer>
            {
                </LastPos>
            };

            Dictionary<string, int> ReservadasValues = new Dictionary<string, int>
            {
                </Reservadas>
            };

            List<int> TokensConReferencia = new List<int>
            {
                </Referencias>
            };

            foreach (var Pair in Character_Token_Last)
            {
                if (Pair.Key.Contains(lastCharacterFromText) &&
                    Character_Token_First[Pair.Value].Contains(firstCharacterFromText))
                {
                    if (TokensConReferencia.Contains(Pair.Value))
                    {
                        if (ReservadasValues.Keys.Contains(text.ToUpper()))
                        {
                            return ReservadasValues[text.ToUpper()];
                        }
                    }
                    return Pair.Value;
                }
            }

            return 0;
        }

        static void AnalizarTexto(Stack<char> Input)
        {
            Inicio:

            int Estado = 0;
            StringBuilder actualText = StringBuilder();

            while (!Input.isEmpty())
            {
                actualText = new StringBuilder(actualText.toString().trim());
                char actualChar = Input.Pop();

                if (actualChar != ' ' && actualChar != 0)
                {
                    switch (Estado)
                    {
                        </ States >

                        case -1:
                            Valido:
                    if (actualText.Length > 0)
                    {
                        Valido(ref actualText);
                    }
                    Estado = 0;
                    Input.Push(actualChar);
                    actualChar = ' ';
                    break;
                    default:
                            Error:
                    Error();
                    return;
                }

                actualText += actualChar;

                }
                else
                {
                    goto CheckForAcceptedStates;
                }
            }
            
            if (actualText.Length > 0)
            {
                goto CheckForAcceptedStates;    
            }

            return;

            CheckForAcceptedStates:
            if (!string.IsNullOrEmpty(actualText) && !string.IsNullOrWhiteSpace(actualText))
            {
                if (</Aceptacion>) 
                {
                    Valido(ref actualText);
                }
                else
                {
                   Error();
                }   
            }
            goto Inicio;
        }
    }
}
