using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Malgorithm
    {
        string TokensER = string.Empty;
        string ActionsER = string.Empty;
        Queue<char> eg = new Queue<char>("(a+·b*·c?)·#".ToCharArray());
        Queue<char> SetTokens = new Queue<char>(@"((S·E·T·S·/n+·identifier·=·('·AZ·'·.·.·'·AZ·'|'·az·'·.·.·'·az·'|'·09·'·.·.·'·09·'|C·H·R·\(·09+·\)·.·.·C·H·R·\(·09+·\)|'·s·y·m·')·(\+·('·AZ·'·.·.·'·AZ·'|'·az·'·.·.·'·az·'|'·09·'·.·.·'·09·'|C·H·R·\(·09+·\)·.·.·C·H·R·\(·09+·\)|'·s·y·m·'))*·/n+)?)·#".ToCharArray());
        List<string> TerminalSigns = new List<string> { "S", "E", "T", "S", "i", "d", "A", "Z", ".", "a", "z", "0", "9", "n", "/", " ", "t", "=", "#", "'", "a", "b", "c", "s", "y", "m", "b", "o", "l", "C", "H", "R", "e","   ", "f", "r", @"\" };
        List<string> OperatorSigns = new List<string> { "+", "(", ")", "[", "]", "?", "*", "|", "·", @"\" };
        Stack<string> TokenStack = new Stack<string>();
        Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        Dictionary<string, int> dicPrecedence = new Dictionary<string, int> { /*{@"\", 7},*/ {"[", 6 },{"]", 6 }, {"(", 5}, {")", 5}, {"+", 4}, {"?", 4}, {"*", 4},
        {"·", 3}, {"^", 2}, {"$", 2}, {"|", 1}};
        public void FillRETree() 
        {
            //CreateRETree(eg);
            CreateRETree(SetTokens);            
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

        public bool IsTerminalSign(string Token, Queue<char> Tokens)
        {
            if (Token == @"\" && OperatorSigns.Contains(Tokens.Peek().ToString()))            
                return true;
            else
                return false;
        }

        public BTreeNode CreateRETree(Queue<char> Tokens) 
        {
            while (Tokens.Count() != 0)
            {
                string Token = Tokens.Dequeue().ToString();
                if (TerminalSigns.Contains(Token) /*|| Token == "\r"*/)
                {
                    BTreeNode treeNode;
                    if (Token != "#"  && TerminalSigns.Contains(Tokens.Peek().ToString()))
                    {
                        while (TerminalSigns.Contains(Tokens.Peek().ToString()))
                        {
                            Token += Tokens.Dequeue().ToString();
                        }
                        treeNode = new BTreeNode(Token);
                        BTreeStack.Push(treeNode);
                    }
                    else if (Token == @"\")
                    {
                        Token += Tokens.Dequeue().ToString();
                        treeNode = new BTreeNode(Token);
                        BTreeStack.Push(treeNode);
                    }
                    else
                    {
                        treeNode = new BTreeNode(Token);
                        BTreeStack.Push(treeNode);
                    }                                                            
                }
                /*else if (IsTerminalSign(Token, Tokens))
                {                    
                    BTreeNode treeNode = new BTreeNode(Tokens.Dequeue().ToString());
                    BTreeStack.Push(treeNode);
                }*/                
                else if (Token == "(")
                {
                    TokenStack.Push(Token);
                }
                else if (Token == ")")
                {
                    /*if (TokenStack.Peek() == "(" && BTreeStack.Peek().GetValue() != "+" )
                    {
                        string temp = BTreeStack.Pop().GetValue();
                        BTreeNode treeNode = new BTreeNode(BTreeStack.Pop().GetValue() + temp);
                        BTreeStack.Push(treeNode);
                    }*/
                                        
                    while (TokenStack.Count != 0 && TokenStack.Peek() != "(")
                    {
                        if (TokenStack.Count() == 0)
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
                        if (TokenStack.Count() != 0)
                        {
                            if (TokenStack.Peek() != "|")
                            {
                                TokenStack.Pop();
                            }
                        }                        
                    }                             
                }
                else if (OperatorSigns.Contains(Token))
                {
                    if (Token == "*" || Token == "+" || Token == "?" /*|| Token == @"\"*/)
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
                        if (Token == "·")
                        {
                            TokenStack.Push(Token);
                        }
                        if (Token == "|")
                        {
                            TokenStack.Push(Token);
                        }
                    }
                    else if (Token == "|" || Token == "·")
                    {
                        TokenStack.Push(Token);                        
                    }
                    /*else if (Token == "·")
                    {
                        BTreeNode temp = new BTreeNode(BTreeStack.Pop().GetValue() + Tokens.Dequeue().ToString());
                        BTreeStack.Push(temp);
                    }*/
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
