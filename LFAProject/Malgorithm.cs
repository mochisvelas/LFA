using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Malgorithm
    {
        Queue<char> SetsRegex = new Queue<char>(@"((S·E·T·S·/n+·identifier·=·('·AZ·'·.·.·'·AZ·'|'·az·'·.·.·'·az·'|'·09·'·.·.·'·09·'|C·H·R·\(·09+·\)·.·.·C·H·R·\(·09+·\)|'·sym·')·((\+·('·AZ·'·.·.·'·AZ·'|'·az·'·.·.·'·az·'|'·09·'·.·.·'·09·'|C·H·R·\(·09+·\)·.·.·C·H·R·\(·09+·\)|'·sym·'))*)·/n+)?)·#".ToCharArray());
        Queue<char> TokensRegex = new Queue<char>(@"(T·O·K·E·N·S·/n·(T·O·K·E·N·blnkspc+·09+·=·('·sym·'·((·'·sym·')+)|('·AZ·')+|'·(\?|\+|\(|\))·'|'·(\?|\+|\(|\))·'·'·(\?|\+|\(|\))·'|(AZ·(·(\?|\+)?))+|('·(quote|')·'·AZ·'·(quote|')·')·((\|·'·(quote|')·'·AZ·'·(quote|')·')*)|AZ·\(·AZ·\|·AZ·\)·(·(\*|\?)?)))·((/n·T·O·K·E·N·blnkspc+·09+·=·('·sym·'·((·'·sym·')+)|('·AZ·')+|'·(\?|\+|\(|\))·'|'·(\?|\+|\(|\))·'·'·(\?|\+|\(|\))·'|(AZ·(·(\?|\+)?))+|('·(quote|')·'·AZ+·'·(quote|')·')·((\|·'·(quote|')·'·AZ·'·(quote|')·')*)|AZ+·\(·AZ·\|·AZ·\)·(·(\*|\?)?)))*)·{·R·E·S·E·R·V·A·D·A·S·\(·\)·})·#".ToCharArray());
        Queue<char> ActionsRegex = new Queue<char>(@"(A·C·T·I·O·N·S·(R·E·S·E·R·V·A·D·A·S·\(·\)·{·(09+·=·'·AZ+·')·(/n·09+·=·'·AZ·')·})·((/n·AZ·\(·\)·{·(09+·=·'·AZ+·')·(/n·09+·=·'·AZ·')·})*))·#".ToCharArray());
        Queue<char> ErrorrsRegex = new Queue<char>(@"AZ·E·R·R·O·R·=·09+·((/n·AZ·E·R·R·O·R·=·09+)*)".ToCharArray());
        Queue<char> eg = new Queue<char>("(a·b·c·d|a·c·d|c·d)·#".ToCharArray());        
        List<string> TerminalSigns = new List<string> { "S", "E", "T", "S", "i", "d", "A", "Z", ".", "a", "z", "0", "9", "n", "/", " ", "t", "=", "#", "'", "a", "b", "c", "s", "y", "m", "b", "o", "l", "C", "H", "R", "e","   ", "f", "r", @"\","O","K","N","V","q","o","u","t","b","l","s","k","n","p","D" };
        List<string> OperatorSigns = new List<string> { "+", "(", ")", "[", "]", "?", "*", "|", "·", @"\"};
        Stack<string> TokenStack = new Stack<string>();
        Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        Dictionary<string, int> dicPrecedence = new Dictionary<string, int> { /*{@"\", 7},*/ {"[", 6 },{"]", 6 }, {"(", 5}, {")", 5}, {"+", 4}, {"?", 4}, {"*", 4},
        {"·", 3}, {"^", 2}, {"$", 2}, {"|", 1}};

                
        public BTreeNode FillRETree(string section) 
        {
            //CreateRETree(eg);
            //BTreeNode node = new BTreeNode();
            //string iOrder = node.InOrderTraversal(CreateRETree(SetTokens));
            //return CreateRETree(SetTokens);
            if (section == "S")
            {
                return CreateRETree(SetsRegex);
            }
            else if (section =="T")
            {
                return CreateRETree(TokensRegex);
            }
            else if (section == "A")
            {
                return CreateRETree(ActionsRegex);
            }
            else if (section == "E")
            {
                return CreateRETree(ErrorrsRegex);
            }
            else
            {
                return null;
            }            
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
                        temp.right.parentNode = temp;
                        temp.left = BTreeStack.Pop();
                        temp.left.parentNode = temp;
                        BTreeStack.Push(temp);
                        if (TokenStack.Count() != 0)
                        {
                            /*while (TokenStack.Count() != 0 && TokenStack.Peek() == "(")
                            {
                                TokenStack.Pop();
                            }*/
                            if (TokenStack.Peek() != "|" && TokenStack.Peek() != "·")
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
                        opNode.left.parentNode = opNode;
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
                        temp.right.parentNode = temp;
                        temp.left = BTreeStack.Pop();
                        temp.left.parentNode = temp;
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
                temp.right.parentNode = temp;
                temp.left = BTreeStack.Pop();
                temp.left.parentNode = temp;
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
