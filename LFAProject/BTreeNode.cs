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
        //string inOrder = "";
        //Queue<char> copyGrammar;
        //int cont = 0;
        //bool isGrammarCorrect = true;
        //bool isParentOr = false;
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

        /*public BTreeNode hasOrParent(BTreeNode node) 
        {
            if (node.parentNode != null)
            {
                if (node.parentNode.Token == "|")
                {
                    isParentOr = true;
                    return node;
                }                    
                hasOrParent(node.parentNode);
            }
            return null;
        }*/

        /*public void InOrderAndCompare(BTreeNode node, Queue<char> grammar, ref string error) 
        {
            if (node == null)
                return;

            
            InOrderAndCompare(node.left, grammar, ref error);

            
            if (isGrammarCorrect)
            {

            }
            if (node.Token != "·")
            {
                hasOrParent(node);

                if (grammar.Peek().ToString() == node.Token && isParentOr && isGrammarCorrect)
                {
                    if (cont == 0)
                        copyGrammar = new Queue<char>(grammar);

                    grammar.Dequeue();
                    cont++;
                }
                else if (grammar.Peek().ToString() != node.Token && isParentOr)
                {
                    if (copyGrammar != null)
                        grammar = copyGrammar;
                    isGrammarCorrect = false;
                }
                else if (grammar.Peek().ToString() != node.Token && !isParentOr)
                {
                    error = "frick off, your grammar is bad!";
                    return; 
                }
                else if (grammar.Peek().ToString() == "|")
                {
                    grammar.Dequeue();
                    isGrammarCorrect = true;
                    grammar = copyGrammar;
                }
                else
                {
                    grammar.Dequeue();
                }
            }            

            if (grammar.Dequeue().ToString() != node.Token)
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

            
            {
                if (grammar.Dequeue().ToString() != node.Token)
                {
                    error = "frick off, your grammar is bad!";
                }
            }
            else if (node.Token != "·")
            {

            }

            
            InOrderAndCompare(node.right, grammar, ref error);
            
            return;
        }*/
    }
}
