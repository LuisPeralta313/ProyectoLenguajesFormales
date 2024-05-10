using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    class Scanner
    {

        public static string[] GetSourceCode(ET tree)
        {
            DFA automata = new DFA(tree);
            TransitionT transitions = new TransitionT(automata.states);

            string sourceCode = Classes.Resource1.Program;
            string LineasdeCodigo = Classes.Resource1.Final;


            //GetValues
            string TitleColor = getColor();
            string Character_Token_First = getFistPositionsWithCharacters(tree.tokens);
            string Character_Token_Last = getLastPositionsWithCharacters(tree.tokens);
            string Reservadas_Values = getReservedValues(tree.actions);
            string TokensConReferencia = getReferences(tree.actionReference);
            string Estados_Aceptacion = getAcceptedStates(transitions);
            string States = getTransitions(transitions, tree.sets);

            //Replace Values
            sourceCode = sourceCode.Replace("</TitleColor>", TitleColor);
            sourceCode = sourceCode.Replace("</FirstPos>", Character_Token_First);
            sourceCode = sourceCode.Replace("</LastPos>", Character_Token_Last);
            sourceCode = sourceCode.Replace("</Reservadas>", Reservadas_Values);
            sourceCode = sourceCode.Replace("</Referencias>", TokensConReferencia);
            sourceCode = sourceCode.Replace("</States>", States);
            sourceCode = sourceCode.Replace("</Aceptacion>", Estados_Aceptacion);

            LineasdeCodigo = LineasdeCodigo.Replace("</TitleColor>", TitleColor);
            LineasdeCodigo = LineasdeCodigo.Replace("</FirstPos>", Character_Token_First);
            LineasdeCodigo = LineasdeCodigo.Replace("</LastPos>", Character_Token_Last);
            LineasdeCodigo = LineasdeCodigo.Replace("</Reservadas>", Reservadas_Values);
            LineasdeCodigo = LineasdeCodigo.Replace("</Referencias>", TokensConReferencia);
            LineasdeCodigo = LineasdeCodigo.Replace("</States>", States);
            LineasdeCodigo = LineasdeCodigo.Replace("</Aceptacion>", Estados_Aceptacion);

            string[] miArray = new string[] { sourceCode, LineasdeCodigo };

            return miArray;
        }



    }
}
