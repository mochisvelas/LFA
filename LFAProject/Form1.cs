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
        Malgorithm malgorithmgg = new Malgorithm();
        CheckGrammar checkGramar = new CheckGrammar();
        string error = string.Empty;
        BTreeNode node = new BTreeNode();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BTreeNode SetsTree = malgorithmgg.FillRETree("S");
            BTreeNode TokensTree = malgorithmgg.FillRETree("T");
            BTreeNode ErrorsTree = malgorithmgg.FillRETree("E");            
            BTreeNode ActionsTree = malgorithmgg.FillRETree("A");                        
            

            openFileDialog1.Filter = "Text|*.txt|All|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {                
                fileClass.IsFileTypeCorrect(openFileDialog1.FileName, ".txt", ref error);
                if (error == "Bad filetype")
                {
                    MessageBox.Show("Seleccione un archivo .txt", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (error == "Null file")
                {
                    MessageBox.Show("El archivo está vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    checkGramar.CompareGrammar(openFileDialog1.FileName, ref error);
                    //fileClass.ReadGrammar(openFileDialog1.FileName, ref error, malgorithmgg.FillRETree(""));
                    if (error == "success")
                    {
                        MessageBox.Show("Gramática correcta", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }            
        }
    }
}
