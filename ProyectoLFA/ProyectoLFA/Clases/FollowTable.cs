using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLFA.Clases
{

    public class FollowTable : CharSET
    {
        // Diccionario con las posibles siguientes posiciones
        public List<Follow> nodes = new List<Follow>();

        public FollowTable(ET tree)
        {
            nodes.Add(new Follow(Epsilon)); // Se aparta la primera posición, para usarla luego como estado inicial
            evaluateTree(tree.root); //Conseguir todos los follow

            //Inicializando
            nodes[0].follows = tree.root.firstPos;
        }

        private void evaluateTree(Node tree)
        {
            getEnumeration(tree);
            getFollowPos(tree);
        }

        private void getEnumeration(Node root)
        {
            if (root.isLeaf())
            {
                nodes.Add(new Follow(root.expresion));
            }
            else
            {
                getEnumeration(root.Left);

                if (root.Right != null)
                {
                    getEnumeration(root.Right);
                }
            }
        }

        private void getFollowPos(Node node)
        {
            if (node != null)
            {
                getFollowPos(node.Left);
                getFollowPos(node.Right);

                if (!node.isLeaf())
                {

                    if (node.expresion == Concatenation && node.Left != null && node.Right != null)
                    {
                        // Si "i" es una posición en lastPos(c1) 
                        // Entonces todas las posiciones en firstPos(c2) están en followPos(i)


                        foreach (var item in node.Left.lastPos)
                        {
                            nodes[item].follows = nodes[item].follows.Concat(node.Right.firstPos).ToList();
                        }

                    }
                    else if (node.expresion == Plus || node.expresion == Star)
                    {
                        // Si "n" es este nodo y "i" es una posición en lastPos(n) 
                        // Entonces todas las posiciones en firstPos(n) están en followPos(i)


                        foreach (var item in node.lastPos)
                        {
                            nodes[item].follows = nodes[item].follows.Concat(node.firstPos).ToList();
                        }
                    }
                }
            }
        }
    }

}
