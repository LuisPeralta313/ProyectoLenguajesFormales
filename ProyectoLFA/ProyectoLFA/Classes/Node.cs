using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Classes
{
    public class Node
    {
        public string expresion { set; get; }
        public Node Left { set; get; }
        public Node Right { set; get; }
        public int number { set; get; }
        public List<int> firstPos { set; get; }
        public List<int> lastPos { set; get; }
        public bool nullable { set; get; }
        public bool isSet { set; get; }

        public Node() { }
        public Node(string exp)
        {
            this.expresion = exp;

            firstPos = new List<int>();
            lastPos = new List<int>();

        }

        //To modify the value of the boolean isSet
        public Node(string element, bool isSet)
        {
            this.expresion = element;
            this.isSet = isSet;

            firstPos = new List<int>();
            lastPos = new List<int>();

        }

        //Equalize nodes left and right with all data
        public Node(string exp, Node left, Node right)
        {
            this.expresion = exp;
            this.Left = left;
            this.Right = right;
        }

        //initialize the value of the variables
        public Node(string exp, string left, string right)
        {
            this.expresion = exp;
            this.Left = new Node(left);
            this.Right = new Node(right);
        }




        //Returns true when the actual node has null values in left and right nodes.
        public bool isLeaf()
        {
            return Left == null && Right == null;
        }

        //ROUTE METHOD: INORDER
        public string Inorder()
        {
            return Inorder(this);
        }
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
