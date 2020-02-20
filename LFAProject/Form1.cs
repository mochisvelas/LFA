using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LFAProject
{
    public partial class Form1 : Form
    {
        private readonly FileClass fileClass = new FileClass();
        string error = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {                
                fileClass.IsFileTypeCorrect(openFileDialog1.FileName, ".txt", ref error);
            }
        }
    }
}
