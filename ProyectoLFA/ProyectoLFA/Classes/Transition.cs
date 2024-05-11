using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Classes
{
    public class Transition
    {
        
        //Symbol to access at the status: terminal symbol
        public string symbol { get; }
        
        
        //Set of "follows" of the actual status
        public List<int> nodes { get; }

        //When the status has the EndCharcter, the boolean variable is true
        public bool isAcceptanceStatus { get; }

        // To create a new transition when follow values of the status is a new set
        public Transition(string simbolo, List<int> nodos)
        {
            symbol = simbolo;
            nodes = nodos;
            isAcceptanceStatus = nodos.Contains(CharSET.EndCharacter.ToCharArray()[0]); 
        }
    }
}
