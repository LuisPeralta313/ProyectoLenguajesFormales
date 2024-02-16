using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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




                }
            }
        }
    }
}