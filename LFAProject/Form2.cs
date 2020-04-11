using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        string error = string.Empty;

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
            List<string> addedTSigns = dfa.TSigns(fileName);
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
            BTreeNode DFATree = malgorithm.CreateDFA(TokenQ, addedTSigns, ref error);
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
            Dictionary<List<int>, bool> stateList = new Dictionary<List<int>, bool>()
            {
                {DFATree.First, false}
            };
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
                    SymStates.FirstOrDefault(y => y.Key == node.Token).Value.AddRange(follows);                                                 
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
    }
}
