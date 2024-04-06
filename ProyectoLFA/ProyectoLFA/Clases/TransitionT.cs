using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    /// <summary>
    /// Representa la tabla de transiciones para el análisis léxico.
    /// </summary>
    public class TransitionT
    {
        public List<string> symbolsList = new List<string>(); // Lista de símbolos

        /// <summary>
        /// Conjunto de estados y transiciones. Estas son las filas de la tabla.
        /// Key = índice del estado
        /// Value = Listado de transiciones (incluye el símbolo y el listado de follows)
        /// </summary>
        public Dictionary<int, List<Transition>> transitions = new Dictionary<int, List<Transition>>();

        /// <summary>
        /// Diccionario que hace referencia a la posición que representa un conjunto.
        /// </summary>
        public List<List<int>> states = new List<List<int>>(); // Estados

        public readonly FollowTable _followTable; // Tabla de follows

        // Constructor que inicializa la tabla de transiciones
        public TransitionT(FollowTable _followTable)
        {
            this._followTable = _followTable;
            getSymbolsList(); // Obtiene la lista de símbolos
            generateTransitions(); // Genera las transiciones
        }

        // Método para obtener la lista de símbolos
        private void getSymbolsList()
        {
            foreach (var element in _followTable.nodes)
            {
                if (!symbolsList.Contains(element.character) && element.character != CharSET.EndCharacter)
                {
                    symbolsList.Add(element.character);
                }
            }

            symbolsList.Remove(CharSET.Epsilon);
        }

        // Método para generar las transiciones
        private void generateTransitions()
        {
            // El primer estado siempre es la raíz del árbol
            states.Add(_followTable.nodes[0].follows);

            generateTransitionOfSingleState(0);
        }

        // Método para generar la transición de un único estado
        private void generateTransitionOfSingleState(int position)
        {
            transitions.Add(position, new List<Transition>());
            List<int> newStates = new List<int>();

            // Obtiene las transiciones actuales de los símbolos
            foreach (var item in symbolsList)
            {
                Transition newTransition = getTransition(states[position], item);
                transitions[position].Add(newTransition);

                // Agrega un nuevo estado si no existe y tiene nodos
                if (!StateContainsNodes(newTransition.nodes) && newTransition.nodes.Count > 0)
                {
                    states.Add(newTransition.nodes);
                    newStates.Add(states.Count - 1); // Puntero del nuevo elemento agregado
                }
            }

            // Genera nuevas transiciones a partir de nuevos estados
            if (newStates.Count > 0)
            {
                foreach (var item in newStates)
                {
                    // Obtiene la transición del nuevo estado
                    generateTransitionOfSingleState(item);
                }
            }
        }

        /// <summary>
        /// Obtiene la transición para un símbolo de un estado.
        /// Recorre el listado de nodos, evalúa el símbolo y agrega sus follows.
        /// </summary>
        /// <param name="state">Conjunto de nodos que conforma el estado</param>
        /// <param name="symbol">Símbolo</param>
        private Transition getTransition(List<int> state, string symbol)
        {
            List<int> follows = new List<int>();

            foreach (var item in state)
            {
                if (_followTable.nodes[item].character == symbol)
                {
                    follows = follows.Count > 0 ?
                        follows.Union(_followTable.nodes[item].follows).ToList() :
                                                                _followTable.nodes[item].follows;
                }
            }
            follows.Sort();

            return new Transition(symbol, follows);
        }


        // Evalúa si el nuevo estado ya existe
        private bool StateContainsNodes(List<int> nodes)
        {
            foreach (var state in states)
            {
                // Si cada elemento del estado es igual a los nodos y la cantidad de elementos es la misma, el nodo no es un nuevo estado
                if (state.All(nodes.Contains) && state.Count == nodes.Count)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
