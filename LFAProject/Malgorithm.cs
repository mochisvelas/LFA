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
        Queue<string> gg = new Queue<string>("(.SETS./n.+.(.id.+.' '.*.=.' '.*.(.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).(./+.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).*)./n.).+.#".Split('.'));
        List<string> SetsTokens = "(.SETS./n.+.(.id.+.' '.*.=.' '.*.(.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).(./+.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).*)./n.).+.#".Split('.').ToList<string>();
        List<string> TerminalSigns = new List<string> { "SETS", "id", "TAZo", "TAZf", "interval", "tazo", "tazf", "T09o", "T09f", "/+", "/n", " ", "=", "#" };
        List<string> OperatorSigns = new List<string> { "+", "(", ")", "?", "*", "|" };
        Stack<string> TokenStack = new Stack<string>();
        Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        Dictionary<string, int> dicPrecedence= new Dictionary<string, int> { {"/", 1}, {"/", 2}, {"/", 2}, {"/", 3}, {"/", 3}, {"/", 3},
        {"/", 4}, {"/", 5}, {"/", 5}, {"/", 6}};
        public void FillRETree() 
        {
            CreateRETree(gg);                       
        }

        public bool HasMinorPrecedence(string Token) 
        {
            dicPrecedence.TryGetValue(Token, out int tokenValue);
            dicPrecedence.TryGetValue(TokenStack.Peek(), out int opValue);
            if (tokenValue <= opValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public BTreeNode CreateRETree(Queue<string> Tokens) 
        {
            while (Tokens.Count() != 0)
            {
                string Token = Tokens.Dequeue();
                if (TerminalSigns.Contains(Token))
                {
                    BTreeNode treeNode = new BTreeNode(Token);
                    BTreeStack.Push(treeNode);
                    return null;

                }
                else if (Token == "(")
                {
                    TokenStack.Push(Token);
                }
                else if (Token == ")")
                {
                    do
                    {
                        if (TokenStack.Count() == 0)
                        {
                            return null;
                        }
                        if (BTreeStack.Count() < 2)
                        {
                            return null;
                        }
                        if (TokenStack.Peek() == "(")
                        {
                            return null;
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
                            return null;
                        }
                        opNode.left = BTreeStack.Pop();
                        BTreeStack.Push(opNode);
                    }
                    else if (TokenStack.Count() != 0 && TokenStack.Peek() != "(" && HasMinorPrecedence(Token))
                    {
                        BTreeNode temp = new BTreeNode(TokenStack.Pop());
                        if (BTreeStack.Count() < 2)
                        {
                            return null;
                        }
                        temp.right = BTreeStack.Pop();
                        temp.left = BTreeStack.Pop();
                        BTreeStack.Push(temp);
                    }
                    else if (Token == "|" || Token == ".")
                    {
                        TokenStack.Push(Token);
                    }
                }
                else
                {
                    return null;
                }
            }

            while (TokenStack.Count() > 0)
            {
                if (TokenStack.Peek() == "(")
                {
                    return null;
                }
                if (BTreeStack.Count() < 2)
                {
                    return null;
                }
                BTreeNode temp = new BTreeNode(TokenStack.Pop());
                temp.right = BTreeStack.Pop();
                temp.left = BTreeStack.Pop();
                BTreeStack.Push(temp);
            }

            if (BTreeStack.Count() != 1)
            {
                return null;
            }

            return BTreeStack.Pop();
        }
    }
}
