using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LFAProject
{
    public partial class Form2 : Form
    {
        readonly Malgorithm malgorithm = new Malgorithm();
        readonly BTreeNode tree = new BTreeNode();
        readonly DFA dfa = new DFA();
        string fileName;
        private readonly DataTable firstLastTable = new DataTable();
        private readonly DataTable transitionTable = new DataTable();
        readonly Dictionary<int, List<int>> Follows = new Dictionary<int, List<int>>();
        readonly Dictionary<string, List<int>> SymStates = new Dictionary<string, List<int>>();
        readonly Dictionary<List<int>, bool> stateList = new Dictionary<List<int>, bool>();
        
        Dictionary<int, bool> finalStates = new Dictionary<int, bool>();
        List<string> addedTSigns = new List<string>();
        string error = string.Empty;
        BTreeNode DFATree = new BTreeNode();
        public DataTable FollowTable1 { get; } = new DataTable();

        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void GetFileName(string FileName)
        {
            fileName = FileName;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (DFATree.Token != null)
            {
                return;
            }
            addedTSigns = dfa.TSigns(fileName);            
            List<string> Tokens = dfa.GetRegex(fileName, addedTSigns, ref error);
            if (!string.IsNullOrEmpty(error)) 
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return;
            }            
            addedTSigns.Clear();
            foreach (var token in Tokens)
            {
                if (!addedTSigns.Contains(token) && token != "*" && token != "|" && token != "(" && token != ")" && token != "·" && token != "+" && token != "?")                
                    addedTSigns.Add(token);                
            }
            Queue<string> TokenQ = new Queue<string>(Tokens);
            string Regex = string.Join("", TokenQ);
            label2.Text = Regex;
            DFATree = malgorithm.CreateDFA(TokenQ, addedTSigns, ref error);
            tree.Nullable(DFATree);
            int SymbolQt = tree.cont;
            tree.FirstLast(DFATree);

            //FirstLast table
            firstLastTable.Columns.Add("Símbolo");
            firstLastTable.Columns.Add("First");
            firstLastTable.Columns.Add("Last");
            firstLastTable.Columns.Add("Nullable");
            FirstLastTable(DFATree);
            dataGridView1.DataSource = firstLastTable;

            //Follow table
            FollowTable1.Columns.Add("Símbolo");
            FollowTable1.Columns.Add("Follow");
            for (int i = 0; i <= SymbolQt; i++)
            {
                Follows.Add(i, new List<int>());
            }
            FollowTable(DFATree);
            foreach (var entry in Follows)
            {
                FollowTable1.Rows.Add(entry.Key, string.Join(",", entry.Value));
            }
            dataGridView2.DataSource = FollowTable1;

            //Transition table
            transitionTable.Columns.Add("Estado");
            foreach (var Tsign in addedTSigns)
            {
                transitionTable.Columns.Add(Tsign);
                SymStates.Add(Tsign, new List<int>());
            }            
            stateList.Add(DFATree.First, false);            
            transitionTable.Rows.Add();
            transitionTable.Rows[0][0] = string.Join(",", stateList.FirstOrDefault(x => x.Key == DFATree.First).Key);
            TransitionTable(DFATree, stateList, 0, DFATree.First);            
            List<int> key = stateList.FinishTransitions<List<int>, bool>();
            while (key.Count != 0)
            {
                transitionTable.Rows.Add();
                int numRow = transitionTable.Rows.Count - 1;
                transitionTable.Rows[numRow][0] = string.Join(",", key);
                TransitionTable(DFATree, stateList, numRow, key);
                key = stateList.FinishTransitions<List<int>, bool>();
            }
            finalStates = dfa.GetFinalStates(stateList, Follows.Last().Key);
            dataGridView3.DataSource = transitionTable;
        }

        private BTreeNode SearchNode(int leafNum, BTreeNode root)
        {
            if (root != null)
            {
                if (root.leafNumber == leafNum)
                {
                    return root;
                }
                else
                {
                    BTreeNode foundNode = SearchNode(leafNum, root.left);
                    if (foundNode == null)
                    {
                        foundNode = SearchNode(leafNum, root.right);
                    }
                    return foundNode;
                }
            }
            else
            {
                return null;
            }
        }

        private void FirstLastTable(BTreeNode root) 
        {            
            if (root != null)
            {
                FirstLastTable(root.left);
                FirstLastTable(root.right);
                firstLastTable.Rows.Add(root.Token, string.Join(",", root.First), string.Join(",", root.Last), root.isNullable.ToString());
            }            
        }

        private void FollowTable(BTreeNode root)
        {
            if (root != null)
            {
                FollowTable(root.left);
                FollowTable(root.right);
                if (root.Token != "|" && root.left != null && root.right != null)
                {
                    foreach (var last in root.left.Last)
                    {
                        Follows.FirstOrDefault(x => x.Key == last).Value.AddRange(root.right.First);
                    }                    
                }
                else if (root.Token != "?" && root.left != null && root.right == null)
                {
                    foreach (var last in root.left.Last)
                    {
                        Follows.FirstOrDefault(x => x.Key == last).Value.AddRange(root.left.Last);
                    }
                }
            }
        }

        private void TransitionTable(BTreeNode root, Dictionary<List<int>, bool> stateList, int numRow, List<int> key) 
        {            
            List<int> states = new List<int>();
            states = stateList.FirstOrDefault(x => x.Key == key).Key;
            foreach (var state in states)
            {
                BTreeNode node = SearchNode(state, root);
                Follows.TryGetValue(state, out List<int> follows);
                if (node.Token != "#") 
                {
                    foreach (var follow in follows)
                    {
                        List<int> temp = SymStates.FirstOrDefault(y => y.Key == node.Token).Value;
                        if (!temp.Any(y => y == follow))
                        {
                            SymStates.FirstOrDefault(y => y.Key == node.Token).Value.Add(follow);
                        }
                    }                    
                }                   
            }
            stateList[key] = true;
            foreach (var pair in SymStates)
            {
                pair.Value.Sort();
                int numColumn = transitionTable.Columns[pair.Key].Ordinal;
                if (pair.Value.Count != 0)
                {
                    transitionTable.Rows[numRow][numColumn] = string.Join(",", pair.Value);                            
                    if (!stateList.Keys.Any(c => c.SequenceEqual(pair.Value)))
                        stateList.Add(pair.Value, false);
                }
                else
                    transitionTable.Rows[numRow][numColumn] = "---";
            }
            SymStates.ResetValues<string, List<int>>();            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DFATree.Token != null)
            {
                using (TreeDraw treeDraw = new TreeDraw(DFATree))
                {
                    treeDraw.ShowDialog();
                }                                
            }
            else
            {
                MessageBox.Show("No se ha cargado el archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private readonly FileClass fileClass = new FileClass();
        private void button3_Click(object sender, EventArgs e)
        {            
            string programFile = "C:\\VSprojects\\LFAProject\\LFAProject\\bin\\Debug\\Scanner\\Scanner\\Program.cs";            
            fileClass.IsFileTypeCorrect(programFile, ".cs", ref error);
            if (error == "Bad filetype")
            {
                MessageBox.Show("Select Program.cs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                File.WriteAllText(programFile, string.Empty);
                Dictionary<string, List<string>> setsRanges = dfa.GetSetsRanges(fileName, addedTSigns);
                Dictionary<int, Dictionary<List<string>, int>> transitionsDic = new Dictionary<int, Dictionary<List<string>, int>>();
                //Dictionary<int, string> reservedWords = dfa.GetReservedWords(fileName);
                List<string> stateListKeys = new List<string>();
                foreach (var key in stateList)
                {
                    stateListKeys.Add(string.Join(",", key.Key));
                }
                foreach (DataRow row in transitionTable.Rows)
                {
                    int numCase = 0;
                    Dictionary<List<string>, int> subDictionary = new Dictionary<List<string>, int>();
                    foreach (DataColumn dc in transitionTable.Columns)
                    {
                        if (dc.ColumnName.Equals("Estado"))
                        {
                            int rowIndex = transitionTable.Rows.IndexOf(row);
                            List<string> dcList = row[dc.ColumnName].ToString().Split(',').ToList();
                            numCase = stateListKeys.IndexOf(string.Join(",", dcList));
                        }
                        else
                        {
                            List<string> symRanges = new List<string>();
                            setsRanges.TryGetValue(dc.ColumnName, out symRanges);
                            int rowIndex = transitionTable.Rows.IndexOf(row);
                            List<string> dcList = row[dc.ColumnName].ToString().Split(',').ToList();
                            int nextState = stateListKeys.IndexOf(string.Join(",", dcList));
                            if (nextState > 0)
                            {
                                subDictionary.Add(symRanges, nextState);
                            }
                        }
                    }
                    transitionsDic.Add(numCase, subDictionary);
                }

                string cases = string.Empty;
                foreach (var states in transitionsDic)
                {
                    string ifs = string.Empty;
                    if (states.Value.Count() > 0)
                    {
                        var lastTransition = states.Value.Last();
                        var firstTransition = states.Value.First();

                        foreach (var transition in states.Value)
                        {
                            string ifcondition = string.Empty;
                            foreach (var state in transition.Key)
                            {
                                if (transition.Key.Count() > 1)
                                {
                                    var lastState = transition.Key.Last();
                                    if (state.Equals(lastState))
                                    {
                                        if (state.Contains("-"))
                                        {
                                            var rangesArr = state.Split('-');
                                            ifcondition += $" inputQ.Peek() >= {rangesArr[0]} && inputQ.Peek() <= {rangesArr[1]}";
                                        }
                                        else
                                        {
                                            ifcondition += $" inputQ.Peek() == {state}";
                                        }
                                    }
                                    else
                                    {
                                        if (state.Contains("-"))
                                        {
                                            var rangesArr = state.Split('-');
                                            ifcondition += $" inputQ.Peek() >= {rangesArr[0]} && inputQ.Peek() <= {rangesArr[1]} ||";
                                        }
                                        else
                                        {
                                            ifcondition += $" inputQ.Peek() == {state} ||";
                                        }
                                    }
                                }
                                else
                                {
                                    if (state.Contains("-"))
                                    {
                                        var rangesArr = state.Split('-');
                                        ifcondition += $" inputQ.Peek() >= {rangesArr[0]} && inputQ.Peek() <= {rangesArr[1]}";
                                    }
                                    else
                                    {
                                        ifcondition += $" inputQ.Peek() == {state}";
                                    }
                                }

                            }
                            if (transition.Equals(firstTransition) && transition.Equals(lastTransition))//JFKL;ASJFLSA;FJS
                            {
                                finalStates.TryGetValue(states.Key, out bool finalState);//finalStates.TryGetValue(transition.Value, out bool finalState);
                                if (finalState)
                                {
                                    ifs += Environment.NewLine + @"if(" + ifcondition + @"){" + Environment.NewLine + @"state=" + transition.Value + @";" + Environment.NewLine
                                    + @"inputQ.Dequeue();" + Environment.NewLine + @"}" + Environment.NewLine + @"else{" + Environment.NewLine + @"state=0;" + Environment.NewLine + @"}";
                                }
                                else
                                {
                                    ifs += Environment.NewLine + @"if(" + ifcondition + @"){" + Environment.NewLine + @"state=" + transition.Value + @";" + Environment.NewLine
                                    + @"inputQ.Dequeue();" + Environment.NewLine + @"}" + Environment.NewLine + @"else{" + Environment.NewLine + @"error=true;" + Environment.NewLine + @"}";
                                }
                            }
                            else
                            {
                                if (transition.Equals(lastTransition))
                                {
                                    finalStates.TryGetValue(states.Key, out bool finalState);//finalStates.TryGetValue(transition.Value, out bool finalState);
                                    if (finalState)
                                    {
                                        ifs += Environment.NewLine + @"else if(" + ifcondition + @"){" + Environment.NewLine + @"state=" + transition.Value + @";" + Environment.NewLine
                                        + @"inputQ.Dequeue();" + Environment.NewLine + @"}" + Environment.NewLine + @"else{" + Environment.NewLine + @"state=0;" + Environment.NewLine + @"}";
                                    }
                                    else
                                    {
                                        ifs += Environment.NewLine + @"else if(" + ifcondition + @"){" + Environment.NewLine + @"state=" + transition.Value + @";" + Environment.NewLine
                                    + @"inputQ.Dequeue();" + Environment.NewLine + @"}" + Environment.NewLine + @"else{" + Environment.NewLine + @"error=true;" + Environment.NewLine + @"}";
                                    }
                                }
                                else if (transition.Equals(firstTransition))
                                {
                                    ifs += Environment.NewLine + @"if(" + ifcondition + @"){" + Environment.NewLine + @"state=" + transition.Value + @";" + Environment.NewLine + @"inputQ.Dequeue();"
                                    + Environment.NewLine + @"}";
                                }
                                else
                                {
                                    ifs += Environment.NewLine + @"else if(" + ifcondition + @"){" + Environment.NewLine + @"state=" + transition.Value + @";" + Environment.NewLine + @"inputQ.Dequeue();"
                                    + Environment.NewLine + @"}";
                                }
                            }
                        }
                        cases += Environment.NewLine + @"case " + states.Key + @":" + Environment.NewLine + ifs + Environment.NewLine + @"break;";
                    }
                    else
                    {
                        string ifcondition = string.Empty;
                        finalStates.TryGetValue(states.Key, out bool finalState);
                        if (finalState)
                        {
                            cases += Environment.NewLine + @"case " + states.Key + @":" + Environment.NewLine + @"state=0;" + Environment.NewLine + @"break;";
                        }
                        else
                        {
                            cases += Environment.NewLine + @"case " + states.Key + @":" + Environment.NewLine + @"error=true;" + Environment.NewLine + @"break;";
                        }
                    }
                }
                string tokenWordStr = string.Empty;
                Dictionary<int, string> tokenWordDic = dfa.GetTokenWords(fileName);
                foreach (var tokenWord in tokenWordDic)
                {
                    tokenWordStr += $"{Environment.NewLine}existingTokensDic.Add({tokenWord.Key},\"{tokenWord.Value}\");{Environment.NewLine}";
                }
                string setsRangesStr = string.Empty;
                string rangeList = string.Empty;
                Dictionary<string, List<string>> setsRangesDic = setsRanges;
                Tools tools = new Tools();
                foreach (var setRange in setsRangesDic)
                {
                    var lastRange = setRange.Value.Last();                    
                    foreach (var range in setRange.Value)
                    {
                        if (setRange.Value.Count() > 1)
                        {
                            if (range == lastRange)
                            {
                                rangeList += $"\"{range}\"";
                            }                            
                            else
                            {
                                rangeList += $"\"{range}\",";
                            }
                        }
                        else
                        {
                            rangeList = $"\"{range}\"";
                        }
                    }
                    setsRangesStr += $"{Environment.NewLine}setsRangesDic.Add(\"{tools.RemoveUnwantedChars(fileClass.RemoveSingleQuotes(setRange.Key))}\", new List<string>(new string[] {{{rangeList}}}));{Environment.NewLine}";
                }
                File.WriteAllText(programFile, @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputString = string.Empty;
            while (!inputString.Equals(""\n""))
            {
                    Console.WriteLine(""Ingrese la cadena a analizar: "");
                    inputString = Console.ReadLine();
                    byte[] bytes = Encoding.ASCII.GetBytes(inputString.Replace("" "", """"));
                    Queue<byte> inputQ = new Queue<byte>(bytes);
                    bool error = false;
                    int state = 0;
                    while (inputQ.Count() != 0 && error != true)
                    {
                        switch(state){"
                            +cases+
                        @"}
                    }

                    if (error == false)
                    {
                        Console.WriteLine(""La cadena fue aceptada."");
                        Tools tools = new Tools();
                        Dictionary<int, string> existingTokensDic = new Dictionary<int, string>();"
                        +tokenWordStr+
                        @"Queue<string> tokensQ = tools.TokenizeText(inputString, existingTokensDic);
                        Dictionary<string, List<string>> setsRangesDic = new Dictionary<string, List<string>>();"
                        +setsRangesStr+Environment.NewLine+
                        @"List<string> finalTokenList = tools.TokenListToPrint(tokensQ, existingTokensDic, setsRangesDic);

                    foreach (var finalToken in finalTokenList)
                    {
                        Console.WriteLine(finalToken);
                    }
                
            }
                    else
                    {
                        Console.WriteLine(""La cadena no fue aceptada."");
                    }

                    Console.ReadKey();
                    Console.Clear();
                }

            }
        }
    }");              
            }

            dlg.Description = "Select a folder";
            string destDir=string.Empty;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                destDir = dlg.SelectedPath;
            }
            string srcDir = "C:\\VSprojects\\LFAProject\\LFAProject\\bin\\Debug\\Scanner";
            if (!string.IsNullOrEmpty(destDir))
            {
                fileClass.CopyFolder(srcDir, destDir);
            }
            else
            {
                MessageBox.Show("Seleccione una dirección de destino.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
