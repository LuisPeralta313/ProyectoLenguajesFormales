using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    /// <summary>
    /// Representa una transición en la tabla de transiciones para el análisis léxico.
    /// </summary>
    public class Transition
    {
        // Símbolo para acceder al estado: símbolo terminal
        public string symbol { get; }

        // Conjunto de "follows" del estado actual
        public List<int> nodes { get; }

        // Cuando el estado tiene el EndCharacter, la variable booleana es verdadera
        public bool isAcceptanceStatus { get; }

        // Constructor para crear una nueva transición cuando los valores de follow del estado son un nuevo conjunto
        public Transition(string simbolo, List<int> nodos)
        {
            symbol = simbolo;
            nodes = nodos;
            isAcceptanceStatus = nodos.Contains(CharSET.EndCharacter.ToCharArray()[0]);
        }
    }
}
