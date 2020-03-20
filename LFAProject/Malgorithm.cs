using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Malgorithm
    {        
        string SetsRegex = @"((S·E·T·S·\n+·[A-Z]+·=)·('·[A-Z]·'·.·.·'·[A-Z]·'|'·[a-z]·'·.·.·'·[a-z]·'|'·[0-9]·'·.·.·'·[0-9]·'|C·H·R·\(·[0-9]+·\)·.·.·C·H·R·\(·[0-9]+·\)|'·[sym]·')·((\+·('·[A-Z]·'·.·.·'·[A-Z]·'|'·[a-z]·'·.·.·'·[a-z]·'|'·[0-9]·'·.·.·'·[0-9]·'|C·H·R·\(·[0-9]+·\)·.·.·C·H·R·\(·[0-9]+·\)|'·[sym]·'))*)·\n+)?";
        string TokensRegex = @"(T·O·K·E·N·S·/n·(T·O·K·E·N·blnkspc+·09+·=·('·sym·'(·('·sym·')+)|('·AZ·')+|'·(\?|\+|\(|\))·'|'·(\?|\+|\(|\))·'·'·(\?|\+|\(|\))·'|(AZ·(·(\?|\+)?))+|('·(quote|')·'·AZ·'·(quote|')·')·((\|·'·(quote|')·'·AZ·'·(quote|')·')*)|AZ·\(·AZ·\|·AZ·\)·(·(\*|\?)?)))·((/n·T·O·K·E·N·blnkspc+·09+·=·('·sym·'(·('·sym·')+)|('·AZ·')+|'·(\?|\+|\(|\))·'|'·(\?|\+|\(|\))·'·'·(\?|\+|\(|\))·'|(AZ·(·(\?|\+)?))+|('·(quote|')·'·AZ+·'·(quote|')·')·((\|·'·(quote|')·'·AZ·'·(quote|')·')*)|AZ+·\(·AZ·\|·AZ·\)·(·(\*|\?)?)))*)·{·R·E·S·E·R·V·A·D·A·S·\(·\)·})·#";
        string ActionsRegex = @"(A·C·T·I·O·N·S·(R·E·S·E·R·V·A·D·A·S·\(·\)·{·(09+·=·'·AZ+·')·(/n·09+·=·'·AZ·')·})·((/n·AZ·\(·\)·{·(09+·=·'·AZ+·')·(/n·09+·=·'·AZ·')·})*))·#";        
        string ErrorrsRegex = @"([A-Z]+·E·R·R·O·R·=·[0-9]+)·((\n·[A-Z]+·E·R·R·O·R·=·[0-9]+)*)";
        List<string> TerminalSigns = new List<string> { "S", "E", "T", "S","A", "C","I","O","N","R","K","H","V","D","[A-Z]", "[a-z]", "[sym]", ".","[0-9]", @"\n", "[quote]", "[blnkspc]", @"\t", @"\(", @"\)", "=", "#", "'", @"\+"};
        List<string> OperatorSigns = new List<string> { "+", "(", ")", "[", "]", "?", "*", "|", "·"};
        Stack<string> TokenStack = new Stack<string>();
        Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        Dictionary<string, int> dicPrecedence = new Dictionary<string, int> {{ "(", 5 }, { ")", 5 }, { "+", 4 }, { "?", 4 }, { "*", 4 }, { "·", 3 }, { "^", 2 }, { "$", 2 }, { "|", 1 } };
                
        public BTreeNode FillRETree(string section) 
        {
            //CreateRETree(eg);
            //BTreeNode node = new BTreeNode();
            //string iOrder = node.InOrderTraversal(CreateRETree(SetTokens));
            //return CreateRETree(SetTokens);
            if (section == "S")
            {
                return CreateRETree(TokenizeRegex(SetsRegex));
            }
            else if (section =="T")
            {
                return CreateRETree(TokenizeRegex(TokensRegex));
            }
            else if (section == "A")
            {
                return CreateRETree(TokenizeRegex(ActionsRegex));
            }
            else if (section == "E")
            {
                return CreateRETree(TokenizeRegex(ErrorrsRegex));
            }
            else
            {
                return null;
            }            
        }

        public Queue<string> TokenizeRegex(string regex) 
        {
            Queue<string> TokenizedRegexQ = new Queue<string>();
            Queue<char> regexQ = new Queue<char>(regex.ToCharArray());
            while (regexQ.Count() != 0)            
            {
                string token = regexQ.Dequeue().ToString();
                if (token == "[")
                {
                    do
                    {
                        token += regexQ.Dequeue().ToString();
                    } while (!token.Contains("]"));
                    TokenizedRegexQ.Enqueue(token);
                }
                else if (token == @"\")
                {
                    token += regexQ.Dequeue().ToString();
                    TokenizedRegexQ.Enqueue(token);
                }
                else
                {
                    TokenizedRegexQ.Enqueue(token);
                }
            }
            return TokenizedRegexQ;
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
                    BTreeNode treeNode;
                    /*if (Token != "#"  && TerminalSigns.Contains(Tokens.Peek().ToString()))
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
                    }*/
                    //else
                    //{
                        treeNode = new BTreeNode(Token);
                        BTreeStack.Push(treeNode);
                    //}                                                            
                }                                
                else if (Token == "(")
                {
                    TokenStack.Push(Token);
                }
                else if (Token == ")")
                {                       
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
                        if (TokenStack.Count() != 0 && TokenStack.Peek() != "|" && TokenStack.Peek() != "·")
                        {
                            TokenStack.Pop();
                        }                                            
                    }                             
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
                        opNode.left.parentNode = opNode;
                        BTreeStack.Push(opNode);
                        if (TokenStack.Count() != 0 && TokenStack.Peek() == "(")  
                        {
                            TokenStack.Pop();
                        }
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
                        TokenStack.Push(Token);
                    }
                    else if (Token != "*" && Token != "?" && Token != "+")
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
