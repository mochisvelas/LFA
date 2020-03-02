using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class BTreeNode
    {
        public string Token;
        public BTreeNode parentNode;
        public BTreeNode right, left;
        string inOrder = "";
        Queue<char> copyGrammar;
        int cont = 0;
        public BTreeNode(string value) 
        {
            Token = value;
            right = left = null;
        }

        public BTreeNode() 
        {
            Token = null;
            right = left = null;
        }

        public BTreeNode(string value, BTreeNode leftChild, BTreeNode rightChild)
        {
            Token = value;
            left = leftChild;
            right = rightChild;
        }

        public string GetValue()
        {
            return Token;
        }

        public BTreeNode hasOrParent(BTreeNode node) 
        {
            if (node.parentNode != null)
            {
                if (node.parentNode.Token == "|")
                    return node;
                hasOrParent(node.parentNode);
            }            
                return null;              
        }

        public void InOrderAndCompare(BTreeNode node, Queue<char> grammar, ref string error) 
        {
            if (node == null)
                return;

            /* first recur on left child */
            InOrderAndCompare(node.left, grammar, ref error);

            /* then print the data of node */

            if (grammar.Peek().ToString() == node.Token && hasOrParent(node) != null)
            {
                if (cont != 0)
                    copyGrammar = grammar;
                                
                grammar.Dequeue();
                cont++;
            }
            else if (grammar.Peek().ToString() != node.Token && hasOrParent(node) != null)
            {
                grammar = copyGrammar;
                return;               
            }
            else if (grammar.Peek().ToString() != node.Token && hasOrParent(node) == null)
            {
                error = "frick off, your grammar is bad!";
                return; //End proccess and return error
            }            

            /*if (grammar.Dequeue().ToString() != node.Token)
            {
                while (node.Token != "|")
                {
                    return inOrder;
                }

                grammar = copyGrammar;
                if (hasOrParent(node) != null)
                {

                }
                else
                {
                    error = "frick off, your grammar is bad!";
                }
            }

            //if (node.Token == "|")
            {
                if (grammar.Dequeue().ToString() != node.Token)
                {
                    error = "frick off, your grammar is bad!";
                }
            }
            else if (node.Token != "·")
            {

            }*/
            
            /* now recur on right child */
            InOrderAndCompare(node.right, grammar, ref error);
            
            return;
        }
    }
}
