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
        public bool isNullable;
        public int leafNumber;
        public List<int> First = new List<int>();
        public List<int> Last = new List<int>();
        public List<int> Follow = new List<int>();
        public int cont = 0;
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

        public void Nullable(BTreeNode root) 
        {
            if (root != null)
            {
                Nullable(root.left);
                Nullable(root.right);
                if (root.left == null && root.right == null)
                {
                    root.isNullable = false;
                    root.leafNumber = ++cont;
                    root.First.Add(cont);
                    root.Last.Add(cont);
                }
                else if (root.left != null && root.right != null)
                {
                    if (root.Token == "|")
                    {
                        if (root.left.isNullable == true || root.right.isNullable == true)
                            root.isNullable = true;
                    }
                    else if (root.Token == "·")
                    {
                        if (root.left.isNullable == true && root.right.isNullable == true)
                            root.isNullable = true;
                    }
                }
                else if (root.left != null && root.right == null)
                {
                    if (root.Token == "*" || root.Token == "?")
                        root.isNullable = true;
                    else
                        root.isNullable = false;
                }
            }            
        }

        public void FirstLast(BTreeNode root)
        {
            if (root != null)
            {
                FirstLast(root.left);
                FirstLast(root.right);
                if (root.left != null && root.right != null)
                {
                    if (root.Token == "|")
                    {
                        root.First.AddRange(root.left.First);
                        root.First.AddRange(root.right.First);
                        root.Last.AddRange(root.left.Last);
                        root.Last.AddRange(root.right.Last);
                    }
                    else if (root.Token == "·")
                    {
                        if (root.left.isNullable == true)
                        {
                            root.First.AddRange(root.left.First);
                            root.First.AddRange(root.right.First);
                        }
                        else
                            root.First.AddRange(root.left.First);

                        if (root.right.isNullable == true)
                        {
                            root.Last.AddRange(root.left.Last);
                            root.Last.AddRange(root.right.Last);
                        }
                        else
                            root.Last.AddRange(root.right.Last);
                    }
                }
                else if (root.left != null && root.right == null)
                {
                    root.First.AddRange(root.left.First);
                    root.Last.AddRange(root.left.Last);
                }
            }            
        }
    }
}
