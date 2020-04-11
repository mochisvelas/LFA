using System.Collections.Generic;
using System.Linq;

namespace LFAProject
{
    class Malgorithm
    {
        readonly Tools tools = new Tools();
        const string tk1 = @"([A-Z]·(\?|\+|\*)?)";        
        const string tk2 = @"(\?|\+|\*)?";
        const string tk3 = @"'·'·'·[A-Z]+·'·'·'";
        const string tk4 = @"'·[quote]·'·[A-Z]+·'·[quote]·'";
        const string tk5 = @"'·[A-Z]·'·.·.·'·[A-Z]·'";
        const string tk6 = @"'·[a-z]·'·.·.·'·[a-z]·'";
        const string tk7 = tk3 + @"|" + tk4;
        const string tk8 = @"T·O·K·E·N·[blnkspc]+·[0-9]+·=·(('·[sym]·')+|('·[A-Z]·')+|" + tk1 + @"+|(" + tk7 + @")·(\|·(" + tk7 + @"))*|[A-Z]+·\(·[A-Z]+·\|·[A-Z]+·\)·" + tk2 + @")";
        const string tk9 = tk5 + @"|" + tk6 + @"|'·[sym]·'";
        const string tk10 = @"[A-Z]+·=·((" + tk9 + @")·(\+·(" + tk9 + @"))*|'·[0-9]·'·.·.·'·[0-9]·'|C·H·R·\(·[0-9]+·\)·.·.·C·H·R·\(·[0-9]+·\))";
        const string SetsRegex = @"(S·E·T·S·\n+·(" + tk10 + @")·(\n·" + tk10 + @")*)?";
        const string TokensRegex = @"T·O·K·E·N·S·\n+·(" + tk8 + @")·(\n·" + tk8 + @")*·{·R·E·S·E·R·V·A·D·A·S·\(·\)·}";
        const string ActionsRegex = @"A·C·T·I·O·N·S·\n+·(R·E·S·E·R·V·A·D·A·S·\(·\)·{·([0-9]+·=·'·[A-Z]+·')·(\n·[0-9]+·=·'·[A-Z]+·')*·\n·})·(\n·[A-Z]+·\(·\)·{·([0-9]+·=·'·[A-Z]+·')·(\n·[0-9]+·=·'·[A-Z]+·')·\n·})*";
        const string ErrorrsRegex = @"([A-Z]+·E·R·R·O·R·=·[0-9]+)·(\n·[A-Z]+·E·R·R·O·R·=·[0-9]+)*";
        readonly string Regex = SetsRegex + @"·" + TokensRegex + @"·" + ActionsRegex + @"·" + ErrorrsRegex + @"·#";
        readonly List<string> TerminalSigns = new List<string> { "S", "E", "T", "S","A", "C","I","O","N","R","K","H","V","D","[A-Z]", "[a-z]", "[sym]", ".","[0-9]", @"\n", "[quote]",
            "[blnkspc]", @"\t", @"\(", @"\)", "=", "#", "'", @"\+", @"\?", @"\*", @"\|", "{", "}"};
        private readonly List<string> OperatorSigns = new List<string> { "+", "(", ")", "[", "]", "?", "*", "|", "·"};
        readonly Stack<string> TokenStack = new Stack<string>();
        readonly Stack<BTreeNode> BTreeStack = new Stack<BTreeNode>();
        readonly Dictionary<string, int> dicPrecedence = new Dictionary<string, int> {{ "(", 5 }, { ")", 5 }, { "+", 4 }, { "?", 4 }, { "*", 4 }, { "·", 3 }, { "^", 2 }, { "$", 2 },
            { "|", 1 } };        
        
        public BTreeNode FillRETree(ref string error) 
        {
            return CreateRETree(tools.TokenizeRegex(Regex), ref error);
        }

        public BTreeNode CreateDFA(Queue<string> Tokens, List<string> TSigns, ref string error) 
        {
            TerminalSigns.AddRange(TSigns);
            Tokens.Enqueue(")");
            Tokens.Enqueue("·");
            Tokens.Enqueue("#");
            return CreateRETree(Tokens, ref error);
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

        public BTreeNode CreateRETree(Queue<string> Tokens, ref string error) 
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
                            error = "Faltan operandos 1";
                            return null;
                        }
                        if (BTreeStack.Count() < 2)
                        {
                            error = "Faltan operandos 2";
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
                            error = "Faltan operandos 3";
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
                                error = "Faltan operandos 4";
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
                    error = "Faltan operandos 5";
                    return null;
                }
            }
            while (TokenStack.Count() > 0)
            {
                if (TokenStack.Peek() == "(")
                {
                    error = "Faltan operandos 6";
                    return null;
                }
                if (BTreeStack.Count() < 2)
                {
                    error = "Faltan operandos 7";
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
                error = "Faltan operandos 8";
                return null;
            }

            return BTreeStack.Pop();
        }
    }
}
