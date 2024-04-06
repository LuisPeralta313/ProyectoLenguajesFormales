using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{
    // Clase que representa un nodo en un árbol de expresiones regulares
    public class Node
    {
        public string expresion { set; get; }  // Expresión que contiene el nodo
        public Node Left { set; get; }  // Nodo izquierdo
        public Node Right { set; get; }  // Nodo derecho
        public int number { set; get; }  // Número asignado al nodo
        public List<int> firstPos { set; get; }  // Lista de las primeras posiciones del nodo en el árbol
        public List<int> lastPos { set; get; }  // Lista de las últimas posiciones del nodo en el árbol
        public bool nullable { set; get; }  // Indica si el nodo es nulo o no
        public bool isSet { set; get; }  // Indica si el nodo está establecido o no

        // Constructores
        public Node() { }
        public Node(string exp)
        {
            this.expresion = exp;
            firstPos = new List<int>();
            lastPos = new List<int>();
        }

        // Método para modificar el valor del booleano isSet
        public Node(string element, bool isSet)
        {
            this.expresion = element;
            this.isSet = isSet;
            firstPos = new List<int>();
            lastPos = new List<int>();
        }

        // Iguala los nodos izquierdo y derecho con todos los datos
        public Node(string exp, Node left, Node right)
        {
            this.expresion = exp;
            this.Left = left;
            this.Right = right;
        }

        // Inicializa el valor de las variables
        public Node(string exp, string left, string right)
        {
            this.expresion = exp;
            this.Left = new Node(left);
            this.Right = new Node(right);
        }

        // Devuelve true cuando el nodo actual tiene valores nulos en los nodos izquierdo y derecho
        public bool isLeaf()
        {
            return Left == null && Right == null;
        }

        // Método de recorrido: INORDEN
        public string Inorder()
        {
            return Inorder(this);
        }

        // Método auxiliar para recorrido inorden
        private string Inorder(Node root)
        {
            string result = "";

            if (root.Left != null)
            {
                result += Inorder(root.Left);
            }

            result += root.expresion;

            if (root.Right != null)
            {
                result += Inorder(root.Right);
            }

            return result;
        }
    }
}
