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
    }
}
