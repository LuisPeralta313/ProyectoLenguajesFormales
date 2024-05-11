using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoLFA.Classes;

namespace ProyectoLFA.Classes
{
    public class FollowTable:CharSET
    {
        //Dictionary with posible next positions
        public List<Follow> nodes = new List<Follow>(); 

        public FollowTable(ET tree)
        {
            nodes.Add(new Follow(Epsilon)); //Set apart first position, to use it later as an initial state
            evaluateTree(tree.root); //Get all the follows

            //Initial state
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
                        //Being "i" a position in lastPos(c1) 
                        //Then all positions in firstPos(c2) are in followPos(i)

                        foreach (var item in node.Left.lastPos)
                        {
                            nodes[item].follows = nodes[item].follows.Concat(node.Right.firstPos).ToList(); 
                        }

                    }
                    else if (node.expresion == Plus || node.expresion == Star)
                    {
                        //Being "n" this node and "i" a position in lastPos(n) 
                        //Then all positions in firstPos(n) are in followPos(i)

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
