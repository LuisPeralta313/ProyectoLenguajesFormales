using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    /// <summary>
    /// Representa un token definido en el archivo.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Número definido en el archivo.
        /// </summary>
        public int TokenNumber;

        /// <summary>
        /// Lista de códigos ASCII para el primer carácter en un token individual.
        /// </summary>
        public List<char> FirstPositions = new List<char>();

        /// <summary>
        /// Lista de códigos ASCII para el último carácter en un token individual.
        /// </summary>
        public List<char> LastPositions = new List<char>();

        /// <summary>
        /// Constructor para inicializar un nuevo token con su número, y las listas de posiciones iniciales y finales.
        /// </summary>
        /// <param name="tokenNumber">Número del token.</param>
        /// <param name="firstPosition">Lista de códigos ASCII para el primer carácter en el token.</param>
        /// <param name="lastPosition">Lista de códigos ASCII para el último carácter en el token.</param>
        public Token(int tokenNumber, List<char> firstPosition, List<char> lastPosition)
        {
            this.TokenNumber = tokenNumber;
            this.FirstPositions = firstPosition;
            this.LastPositions = lastPosition;
        }
    }
}
