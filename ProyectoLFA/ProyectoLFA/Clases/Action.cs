using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    /// <summary>
    /// Representa una acción en el análisis léxico.
    /// </summary>
    public class Action
    {
        public string ActionName; // Nombre de la acción
        public Dictionary<int, string> ActionValues = new Dictionary<int, string>(); // Valores de la acción (número de token y su correspondiente valor)

        // Se pueden agregar métodos adicionales relacionados con las acciones si es necesario
    }

}
