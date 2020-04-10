using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Malgorithm
    {
        readonly Tools tools = new Tools();
        static string tk1 = @"([A-Z]·(\?|\+|\*)?)";
        static string tk2 = @"'·'·'·[A-Z]+·'·'·'|'·[quote]·'·[A-Z]+·'·[quote]·'";
        static readonly string tk3 = @"(\?|\+|\*)?";
        static string tk4 = @"T·O·K·E·N·[blnkspc]+·[0-9]+·=·(('·[sym]·')+|('·[A-Z]·')+|gg1+|(gg5|gg6)·(\|·(gg5|gg6))*|[A-Z]+·\(·[A-Z]+·\|·[A-Z]+·\)·"+tk3+@")";
        static string tk5 = @"'·'·'·[A-Z]+·'·'·'";
        static string tk6 = @"'·[quote]·'·[A-Z]+·'·[quote]·'";
        static string tk7 = @"'·[A-Z]·'·.·.·'·[A-Z]·'";
        static string tk8 = @"'·[a-z]·'·.·.·'·[a-z]·'";
        static string tk9 = @"[A-Z]+·=·((gg7|gg8|'·[sym]·')·(\+·(gg7|gg8|'·[sym]·'))*|'·[0-9]·'·.·.·'·[0-9]·'|C·H·R·\(·[0-9]+·\)·.·.·C·H·R·\(·[0-9]+·\))";
        static string tk10 = @"gg7|gg8|'·[sym]·'";
        readonly string SetsRegex = @"(S·E·T·S·\n+·([A-Z]+·=·((gg14)·(\+·(gg14))*|'·[0-9]·'·.·.·'·[0-9]·'|C·H·R·\(·[0-9]+·\)·.·.·C·H·R·\(·[0-9]+·\)))·(\n·gg9)*)?";
        readonly string TokensRegex = @"T·O·K·E·N·S·\n+·(T·O·K·E·N·[blnkspc]+·[0-9]+·=·(('·[sym]·')+|('·[A-Z]·')+|gg1+|(gg5|gg6)·(\|·(gg5|gg6))*|[A-Z]+·\(·[A-Z]+·\|·[A-Z]+·\)·gg3))·(\n·gg4)*
            ·{·R·E·S·E·R·V·A·D·A·S·\(·\)·}";
        readonly string ActionsRegex = @"A·C·T·I·O·N·S·\n+·(R·E·S·E·R·V·A·D·A·S·\(·\)·{·([0-9]+·=·'·[A-Z]+·')·(\n·[0-9]+·=·'·[A-Z]+·')*·\n·})·(\n·[A-Z]+·\(·\)·{·([0-9]+·=·'·[A-Z]+·')
            ·(\n·[0-9]+·=·'·[A-Z]+·')·\n·})*";
        readonly string ErrorrsRegex = @"([A-Z]+·E·R·R·O·R·=·[0-9]+)·(\n·[A-Z]+·E·R·R·O·R·=·[0-9]+)*";
        readonly List<string> TerminalSigns = new List<string> { "S", "E", "T", "S","A", "C","I","O","N","R","K","H","V","D","[A-Z]", "[a-z]", "[sym]", ".","[0-9]", @"\n", "[quote]",
            "[blnkspc]", @"\t", @"\(", @"\)", "=", "#", "'", @"\+", @"\?", @"\*", @"\|"};
        private readonly List<string> OperatorSigns = new List<string> { "+", "(", ")", "[", "]", "?", "*", "|", "·"};
        readonly Stack<string> TokenStack = new Stack<string>();
        readonly Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        readonly Dictionary<string, int> dicPrecedence = new Dictionary<string, int> {{ "(", 5 }, { ")", 5 }, { "+", 4 }, { "?", 4 }, { "*", 4 }, { "·", 3 }, { "^", 2 }, { "$", 2 },
            { "|", 1 } };        
        
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
