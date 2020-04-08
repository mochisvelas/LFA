using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Malgorithm
    {
        Tools tools = new Tools();
        string SetsRegex = @"((S·E·T·S·\n+·[A-Z]+·=)·('·[A-Z]·'·.·.·'·[A-Z]·'|'·[a-z]·'·.·.·'·[a-z]·'|'·[0-9]·'·.·.·'·[0-9]·'|C·H·R·\(·[0-9]+·\)·.·.·C·H·R·\(·[0-9]+·\)|'·[sym]·')·((\+·('·[A-Z]·'·.·.·'·[A-Z]·'|'·[a-z]·'·.·.·'·[a-z]·'|'·[0-9]·'·.·.·'·[0-9]·'|C·H·R·\(·[0-9]+·\)·.·.·C·H·R·\(·[0-9]+·\)|'·[sym]·'))*)·\n+)?";
        string TokensRegex = @"T·O·K·E·N·S·\n·(T·O·K·E·N·[blnkspc]+·[0-9]+·=·('·[sym]·'·(('·[sym]·')+)|('·[A-Z]·')+|'·(\?|\+|\(|\))·'|'·(\?|\+|\(|\))·'·'·(\?|\+|\(|\))·'|([A-Z]·((\?|\+)?))+|('·([quote]|')·'·[A-Z]+·'·([quote]|')·')·((\|·'·([quote]|')·'·[A-Z]+·'·([quote]|')·')*)|[A-Z]+·\(·[A-Z]+·\|·[A-Z]+·\)·((\*|\?)?)))·((\n·T·O·K·E·N·[blnkspc]+·[0-9]+·=·('·[sym]·'·(('·[sym]·')+)|('·[A-Z]·')+|'·(\?|\+|\(|\))·'|'·(\?|\+|\(|\))·'·'·(\?|\+|\(|\))·'|([A-Z]·((\?|\+)?))+|('·([quote]|')·'·[A-Z]+·'·([quote]|')·')·((\|·'·([quote]|')·'·[A-Z]+·'·([quote]|')·')*)|[A-Z]+·\(·[A-Z]+·\|·[A-Z]+·\)·((\*|\?)?)))*)·{·R·E·S·E·R·V·A·D·A·S·\(·\)·}";
        string ActionsRegex = @"(A·C·T·I·O·N·S·(R·E·S·E·R·V·A·D·A·S·\(·\)·{·(09+·=·'·AZ+·')·(/n·09+·=·'·AZ·')·})·((/n·AZ·\(·\)·{·(09+·=·'·AZ+·')·(/n·09+·=·'·AZ·')·})*))·#";        
        string ErrorrsRegex = @"([A-Z]+·E·R·R·O·R·=·[0-9]+)·((\n·[A-Z]+·E·R·R·O·R·=·[0-9]+)*)";
        List<string> TerminalSigns = new List<string> { "S", "E", "T", "S","A", "C","I","O","N","R","K","H","V","D","[A-Z]", "[a-z]", "[sym]", ".","[0-9]", @"\n", "[quote]", "[blnkspc]", @"\t", @"\(", @"\)", "=", "#", "'", @"\+", @"\?", @"\*", @"\|", "a", "b"};
        List<string> OperatorSigns = new List<string> { "+", "(", ")", "[", "]", "?", "*", "|", "·"};
        Stack<string> TokenStack = new Stack<string>();
        Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        Dictionary<string, int> dicPrecedence = new Dictionary<string, int> {{ "(", 5 }, { ")", 5 }, { "+", 4 }, { "?", 4 }, { "*", 4 }, { "·", 3 }, { "^", 2 }, { "$", 2 }, { "|", 1 } };
        //string eg = @"a|(a·b)|a|b|a";
        
        public BTreeNode FillRETree(string section) 
        {            
            //BTreeNode node = new BTreeNode();
            //string iOrder = node.InOrderTraversal(CreateRETree(SetTokens));
            //return CreateRETree(SetTokens);
            if (section == "S")
            {
                //return CreateRETree(tools.TokenizeRegex(eg));
                return CreateRETree(tools.TokenizeRegex(SetsRegex));
            }
            else if (section =="T")
            {
                return CreateRETree(tools.TokenizeRegex(TokensRegex));
            }
            else if (section == "A")
            {
                return CreateRETree(tools.TokenizeRegex(ActionsRegex));
            }
            else if (section == "E")
            {
                return CreateRETree(tools.TokenizeRegex(ErrorrsRegex));
            }
            else
            {
                return null;
            }            
        }

        public BTreeNode CreateDFA(Queue<string> Tokens, List<string> TSigns) 
        {
            TerminalSigns.AddRange(TSigns);
            Tokens.Enqueue(")");
            Tokens.Enqueue("·");
            Tokens.Enqueue("#");
            return CreateRETree(Tokens);
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
                    BTreeStack.Push(new BTreeNode(Token));                                                                              
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
                    }
                    TokenStack.Pop();
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
                    }
                    else if (TokenStack.Count() != 0)
                    {
                        while (TokenStack.Count() != 0 && TokenStack.Peek() != "(" && HasMinorPrecedence(Token))
                        {
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
                    }
                    if (Token != "*" && Token != "?" && Token != "+")
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
