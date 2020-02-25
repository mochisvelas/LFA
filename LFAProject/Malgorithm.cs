using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Malgorithm
    {
        string TokensER = "";
        string ActionsER = "";
        List<string> SetsTokens = "(.SETS./n.+.(.id.+.' '.*.=.' '.*.(.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).(./+.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).*)./n.).+.#".Split('.').ToList<string>();
        List<string> TerminalSigns = new List<string> { "SETS", "id","TAZo", "TAZf", "interval", "tazo", "tazf", "T09o", "T09f", "/+", "/n", " ", "=", "#"};
        List<string> OperatorSigns = new List<string> { "+", "(", ")", "?", "*", "|"};
        Stack<string> TokenStack = new Stack<string>();
        Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        BTree makeTree = new BTree();
        BTreeNode nodeTree = new BTreeNode();
        public void FillRETree() 
        {
            foreach (string StTkn in SetsTokens)
            {
                CreateRETree(StTkn);
            }
        }

        public bool CreateRETree(string Token) 
        {            

            if (TerminalSigns.Contains(Token))
            {
                BTreeNode treeNode = new BTreeNode(Token);
                BTreeStack.Push(treeNode);
                return true;

            } 
            else if(Token == "(")
            {
                TokenStack.Push(Token);
                return true;

            }
            else if (Token == ")")
            {                
                do
                {
                    if (TokenStack.Count() == 0)
                    {
                        return false;
                    }
                    if (BTreeStack.Count() < 2)
                    {
                        return false;
                    }
                    if (TokenStack.Peek() == "(")
                    {
                        return false;
                    }

                    BTreeNode temp = new BTreeNode(TokenStack.Pop());
                    temp.right = BTreeStack.Pop();
                    temp.left = BTreeStack.Pop();
                    BTreeStack.Push(temp);
                    TokenStack.Pop();

                } while (TokenStack.Count != 0 && TokenStack.Peek() != "(");                                
            }
            else if (OperatorSigns.Contains(Token))
            {
                if (Token == "*" || Token == "+" || Token == "?")
                {
                    BTreeNode opNode = new BTreeNode(Token);
                    if (BTreeStack.Count() == 0)
                    {
                        return false;
                    }
                    opNode.left = BTreeStack.Pop();
                    BTreeStack.Push(opNode);
                }
                else if (true)
                {

                }
            }
            return true;
        }
    }
}
