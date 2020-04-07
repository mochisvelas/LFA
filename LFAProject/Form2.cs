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
        DFA dfa = new DFA();
        string fileName;
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
            Queue<string> Tokens = dfa.getRegex(fileName, addedTSigns);
        }
    }
}
