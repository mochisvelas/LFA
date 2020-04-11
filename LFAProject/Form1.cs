using System;
using System.Windows.Forms;


namespace LFAProject
{
    public partial class Form1 : Form
    {
        readonly Malgorithm malgorithm = new Malgorithm();
        private readonly FileClass fileClass = new FileClass();
        readonly CheckGrammar checkGramar = new CheckGrammar();
        string error = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BTreeNode RegexTree = malgorithm.FillRETree(ref error);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                    string selectedFileName = openFileDialog1.FileName;
                    Form2 DFAform = new Form2();
                    DFAform.GetFileName(selectedFileName);
                    DFAform.Show();
                    checkGramar.CompareGrammar(selectedFileName, ref error);                    
                    if (error == "success")
                    {
                        MessageBox.Show("Gramática correcta", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //Form2 DFAform = new Form2();
                        //DFAform.GetFileName(selectedFileName);
                        //DFAform.Show();
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
