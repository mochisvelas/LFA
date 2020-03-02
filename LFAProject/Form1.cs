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
        string error = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //malgorithmgg.FillRETree();
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
                    fileClass.CheckGrammar(openFileDialog1.FileName, ref error, malgorithmgg.FillRETree());
                    if (error != "success")
                    {
                        //Grammar is correct
                    }
                    else
                    {
                        //Grammar has error in line {0} and col {0}
                    }
                }
            }            
        }
    }
}
