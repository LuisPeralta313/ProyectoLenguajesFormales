using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    public class Follow
    {
        public string character { get; set; }  // Carácter
        public bool isAcceptanceStatus { get; set; }  // Estado de aceptación
        public List<int> follows { get; set; }  // Lista de posiciones siguientes

        public Follow(string character)
        {
            this.character = character;
            isAcceptanceStatus = character == CharSET.EndCharacter;  // Se establece el estado de aceptación basado en si el carácter es el caracter final
            follows = new List<int>();  // Se inicializa la lista de posiciones siguientes
        }
    }
}
