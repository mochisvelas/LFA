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
        Stack<BTree> BTreeStack = new Stack<BTree>();

        public void FillRETree() 
        {
            foreach (string StTkn in SetsTokens)
            {
                CreateRETree(StTkn);
            }
        }

        public bool CreateRETree(string Token) 
        {
            BTree makeTree = new BTree();

            if (TerminalSigns.Contains(Token))
            {
                makeTree.root = new BTreeNode(Token);
                BTreeStack.Push(makeTree);
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


                } while (TokenStack.Count != 0 && TokenStack.Peek() != "(");                                
            }            
            
            return true;
        }
    }
}
