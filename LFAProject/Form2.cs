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

            firstLastTable.Columns.Add("Símbolo");
            firstLastTable.Columns.Add("First");
            firstLastTable.Columns.Add("Last");
            firstLastTable.Columns.Add("Nullable");
            FirstLastTable(DFATree);
            dataGridView1.DataSource = firstLastTable;

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

            transitionTable.Columns.Add("Estado");
            foreach (var Tsign in addedTSigns)
            {
                transitionTable.Columns.Add(Tsign);
            }
            List<List<int>> stateList = new List<List<int>>();
            stateList.Add(DFATree.First);
            transitionTable.Rows.Add();
            transitionTable.Rows[0][0] = string.Join(",", stateList[0]);
            TransitionTable(DFATree,stateList);
            
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

        private void TransitionTable(BTreeNode root, List<List<int>> stateList) 
        {
            int numRow = transitionTable.Rows.Count - 1;            
            foreach (var number in stateList[numRow])
            {
                BTreeNode node = SearchNode(number, root);
                int numColumn = transitionTable.Columns[node.Token].Ordinal;
                Follows.TryGetValue(number, out List<int> follows);
                transitionTable.Rows[numRow][numColumn] = string.Join(",", follows);                
            }
        }
    }
}
