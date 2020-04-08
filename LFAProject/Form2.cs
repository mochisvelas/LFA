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
        BTreeNode tree = new BTreeNode();
        DFA dfa = new DFA();        
        string fileName;
        string regex;
        DataTable firstLastTable = new DataTable();
        DataTable followTable = new DataTable();
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
            for (int i = 0; i < SymbolQt; i++) //it wont go up to 56
            {
                Follows.Add(i, new List<int>());
            }
            FollowTable(DFATree);
            foreach (var entry in Follows)
            {
                followTable.Rows.Add(entry.Key, string.Join(",", entry.Value));
            }
            dataGridView2.DataSource = followTable;
            PostOrder(DFATree);
        }

        private void PostOrder(BTreeNode root)
        {            
            if (root != null)
            {                
                PostOrder(root.left);                
                PostOrder(root.right);
                regex += root.Token;
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
    }
}
