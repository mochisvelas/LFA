using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
        readonly Dictionary<List<int>, bool> stateList = new Dictionary<List<int>, bool>();
        List<string> addedTSigns = new List<string>();
        string error = string.Empty;
        BTreeNode DFATree = new BTreeNode();
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
            if (DFATree.Token != null)
            {
                return;
            }
            addedTSigns = dfa.TSigns(fileName);
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
            DFATree = malgorithm.CreateDFA(TokenQ, addedTSigns, ref error);
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
            //Dictionary<List<int>, bool> stateList = new Dictionary<List<int>, bool>()
            //{
            //    {DFATree.First, false}
            //};
            stateList.Add(DFATree.First, false);
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
                {
                    foreach (var follow in follows)
                    {
                        List<int> temp = SymStates.FirstOrDefault(y => y.Key == node.Token).Value;
                        if (!temp.Any(y => y == follow))
                        {
                            SymStates.FirstOrDefault(y => y.Key == node.Token).Value.Add(follow);
                        }
                    }                    
                }                   
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (DFATree.Token != null)
            {
                using (TreeDraw treeDraw = new TreeDraw(DFATree))
                {
                    treeDraw.ShowDialog();
                }                                
            }
            else
            {
                MessageBox.Show("No se ha cargado el archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private readonly FileClass fileClass = new FileClass();
        private void button3_Click(object sender, EventArgs e)
        {
            string programFile = "C:\\VSprojects\\LFAProject\\LFAProject\\bin\\Debug\\Scanner\\Scanner\\Program.cs";
            fileClass.IsFileTypeCorrect(programFile, ".cs", ref error);
            if (error == "Bad filetype")
            {
                MessageBox.Show("Select Program.cs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (error == "Null file")
            {
                MessageBox.Show("Program.cs is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //File.WriteAllText(programFile, string.Empty);                                
                //byte[] bytes = Encoding.ASCII.GetBytes(cases);
                //int result = BitConverter.ToInt32(bytes, 0);
                string inputString = "Program x a:=b c=d const a";
                inputString = inputString.Replace(" ", "");
                byte[] bytes = Encoding.ASCII.GetBytes(inputString);
                Queue<byte> inputQ = new Queue<byte>(bytes);
                Dictionary<string, List<string>> setsRanges = dfa.GetSetsRange(fileName, addedTSigns);
                string cases = string.Empty;
                string ifs = string.Empty;
                foreach (DataRow row in transitionTable.Rows)
                {
                    foreach (string dc in row.ItemArray)
                    {
                        List<string> dcList = dc.Split(',').ToList();                        
                        List<string> stateListKeys = new List<string>();
                        foreach (var key in stateList)
                        {
                            stateListKeys.Add(string.Join(",", key.Key));
                        }
                        int numCase = stateListKeys.IndexOf(string.Join(",", dcList));

                        if (inputQ.Peek() >= 65 && inputQ.Peek() <=90 || inputQ.Peek() == 95 || inputQ.Peek()>=97 && inputQ.Peek()<=122)
                        {

                        }
                        cases += Environment.NewLine + @"case " + numCase + @":" + Environment.NewLine + @"" + Environment.NewLine + @"break;";
                    }
                }


                //File.WriteAllText(programFile, @"using System;
                //using System.Collections.Generic;
                //using System.Linq;
                //using System.Text;
                //using System.Threading.Tasks;

                //namespace Scanner
                //{
                //    class Program
                //    {
                //        static void Main(string[] args)
                //        {
                //          Console.WriteLine("Ingrese la cadena a analizar:");
                //          string inputString = Console.ReadLine();
                //          inputString.replace(" ", "");
                //          byte[] bytes = Encoding.ASCII.GetBytes(inputString);
                //          Queue<byte> inputQ = new Queue<byte>(bytes);
                //          bool error = false;
                //          int state = 0;
                //          while (inputQ.count() != 0 && error != true && end != true)
                //          {
                //              CASES
                //          }
                //          if (error == true)
                //          {
                //              Console.WriteLine("La cadena fue aceptada.");
                //          }
                //          else
                //          {
                //              Console.WriteLine("La cadena no fue aceptada.");
                //          }
                //          Console.ReadKey;
                //        }
                //    }
                //}");                
            }            
        }
    }
}
