using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proyecto_Fase_Uno
{

    internal class ReglasExpresionRegular
    {

        public static string Expresion_Regular_ACCIONES_Y_ERRORES = @"^((\s*RESERVADAS\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*)+|}+\s*|(\s*([A-Z]|[a-z]|[0-9])\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*|}+\s)*(\s*ERROR\s*=\s*[0-9]+\s*))$";

        public static string Expresion_Regular_SETS = @"^(\s*([A-Z])+\s*=\s*((((\'([A-Z]|[a-z]|[0-9]|_)\'\.\.\'([A-Z]|[a-z]|[0-9]|_)\')\+)*(\'([A-z]|[a-z]|[0-9]|_)\'\.+\'([A-z]|[a-z]|[0-9]|_)\')*(\'([A-z]|[a-z]|[0-9]|_)\')+)|(CHR\(+([0-9])+\)+\.\.CHR\(+([0-9])+\)+)+)\s*)";

        public static string Expresion_Regular_TOKENS = @"^(\s*TOKEN\s*[0-9]+\s*=\s*(([A-Z]+)|((\'*)([a-z]|[A-Z]|[1-9]|(\<|\>|\=|\+|\-|\*|\(|\)|\{|\}|\[|\]|\.|\,|\:|\;))(\'))+|((\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9]|\')*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9])*\s*\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*\{*\s*([A-Z]|[a-z]|[0-9])*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*)+)+)";


        public string Archivo(string data, ref int linea)
        {

            //Contadores
            int cantidadAcciones = 0;
            int accionesError = 0;
            int cantidadTokens = 0;
            int cantidadConjuntos = 0;

            //Booleanos de Existencia Verdadera o Falsa
            bool primeraLinea = true;
            bool conjuntosExistentes = false;
            bool tokensExistentes = false;
            bool accionesExistentes = false;
            string MensajeResultado = "";

            string[] lineas = data.Split('\n');
            int contadorLineas = 0;  //Para los Errores

            foreach (var lineaActual in lineas)
            {
                contadorLineas++;
                if (!string.IsNullOrWhiteSpace(lineaActual) && !string.IsNullOrEmpty(lineaActual))
                {
                    if (primeraLinea)
                    {
                        primeraLinea = false;
                        if (lineaActual.Contains("SETS"))
                        {
                            conjuntosExistentes = true;
                            MensajeResultado = "Gramática Válida :D";
                        }
                        else if (lineaActual.Contains("TOKENS"))
                        {
                            tokensExistentes = true;
                            MensajeResultado = "Formato Correcto";
                        }
                        else
                        {
                            linea = 1;
                            return "Error en la línea 1: Se esperaba SET o TOKENS";
                        }
                    }
                    else if (conjuntosExistentes)  ///walter
                    {
                        Match conjuntoCoincide = Regex.Match(lineaActual, Expresion_Regular_SETS);
                        if (lineaActual.Contains("TOKENS"))
                        {
                            if (cantidadConjuntos < 1)
                            {
                                linea = contadorLineas;
                                return "Error: Se esperaba al menos un SET";
                            }
                            conjuntosExistentes = false;
                            tokensExistentes = true;
                        }
                        else
                        {
                            if (!conjuntoCoincide.Success)
                            {
                                return $"Error en la línea: {contadorLineas}";
                            }
                            cantidadTokens++;
                        }

                        cantidadConjuntos++;
                    }
                    else if (tokensExistentes) //rodrigol
                    {
                        Match coincideToken = Regex.Match(lineaActual, Expresion_Regular_TOKENS);
                        if (lineaActual.Contains("ACTIONS"))
                        {
                            if (cantidadTokens < 1)
                            {
                                linea = contadorLineas;
                                return "Error: Se esperaba al menos un TOKEN";
                            }
                            cantidadAcciones++;
                            tokensExistentes = false;
                            accionesExistentes = true;
                        }
                        else
                        {
                            if (!coincideToken.Success)
                            {
                                return $"Error en la línea: {contadorLineas}";
                            }
                            cantidadTokens++;
                        }
                    }
                    else if (accionesExistentes)
                    {
                        if (lineaActual.Contains("ERROR"))
                        {
                            accionesError++;
                        }
                        Match coincideAccion = Regex.Match(lineaActual, Expresion_Regular_ACCIONES_Y_ERRORES);
                        if (!coincideAccion.Success)
                        {
                            return $"Error en la línea: {contadorLineas}";
                        }
                    }
                }
            }
            if (cantidadAcciones < 1)
            {
                return $"Error: Ausencia de ACCIONES";
            }
            if (accionesError < 1)
            {
                return $"Error: Ausencia de ERROR";
            }
            linea = contadorLineas;
            return MensajeResultado;
        }

    }
}
