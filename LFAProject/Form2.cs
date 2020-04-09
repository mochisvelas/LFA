using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LFAProject
{
    public partial class Form2 : Form
    {
        Malgorithm malgorithm = new Malgorithm();        
        readonly BTreeNode tree = new BTreeNode();
        readonly DFA dfa = new DFA();        
        string fileName;
        string regex;
        private readonly DataTable firstLastTable = new DataTable();
        private readonly DataTable followTable = new DataTable();
        readonly DataTable transitionTable = new DataTable();
        Dictionary<int, List<int>> Follows = new Dictionary<int, List<int>>();
        Dictionary<string, List<int>> SymStates = new Dictionary<string, List<int>>();
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
            List<string> Tokens = dfa.getRegex(fileName, addedTSigns);
            addedTSigns.Clear();
            foreach (var token in Tokens)
            {
                if (!addedTSigns.Contains(token) && token != "*" && token != "|" && token != "(" && token != ")" && token != "·" && token != "+" && token != "?")
                {
                    addedTSigns.Add(token);
                }
            }
            Queue<string> TokenQ = new Queue<string>(Tokens);
            BTreeNode DFATree = malgorithm.CreateDFA(TokenQ, addedTSigns);
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
            followTable.Columns.Add("Símbolo");
            followTable.Columns.Add("Follow");
            for (int i = 0; i <= SymbolQt; i++)
            {
                Follows.Add(i, new List<int>());
            }
            FollowTable(DFATree);
            foreach (var entry in Follows)
            {
                followTable.Rows.Add(entry.Key, string.Join(",", entry.Value));
            }
            dataGridView2.DataSource = followTable;

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
            //var keys = new List<List<int>>(stateList.Keys);
            //foreach (var key in keys)
            //{                
            //    if (stateList[key] != true)
            //    {
            //        transitionTable.Rows.Add();
            //        int numRow = transitionTable.Rows.Count - 1;
            //        TransitionTable(DFATree, stateList, numRow, key);
            //    }
            //}

            //foreach (var entry in stateList)
            //{
            //    if (entry.Value != true)
            //    {
            //        transitionTable.Rows.Add();
            //        int numRow = transitionTable.Rows.Count - 1;
            //        TransitionTable(DFATree, stateList, numRow, entry.Key);
            //    }
            //}
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
            //var keys = new List<List<int>>(stateList.Keys);
            //int numRow = transitionTable.Rows.Count-1;
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
            /*int numRow = transitionTable.Rows.Count - 1;
            foreach (var number in stateList[numRow])
            {
                BTreeNode node = SearchNode(number, root);
                int numColumn = transitionTable.Columns[node.Token].Ordinal;
                Follows.TryGetValue(number, out List<int> follows);
                transitionTable.Rows[numRow][numColumn] += string.Join(",", follows);
                SymStates.FirstOrDefault(x => x.Key == node.Token).Value.AddRange(follows);
            }
            transitionTable.Rows[numRow][numColumn] += string.Join(",", follows);
            transitionTable.Rows.Add(string.Join(",", stateList[numRow]));
            foreach (var entry in SymStates)
            {
                entry.Value.Sort();
                int numColumn = transitionTable.Columns[entry.Key].Ordinal;
                if (entry.Value.Count != 0)
                {
                    transitionTable.Rows[numRow][numColumn] = string.Join(",", entry.Value);
                    transitionTable.Rows.Add(string.Join(",", entry.Value));
                    if (!stateList.Any(c => c.SequenceEqual(entry.Value)))
                        stateList.Add(entry.Value);
                }
                else
                    transitionTable.Rows[numRow][numColumn] = "---";
            }*/
        }
    }
}
