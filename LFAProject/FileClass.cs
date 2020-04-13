using System;
using System.IO;


namespace LFAProject
{
    class FileClass
    {
        /// <summary>Checks if the filetype is correct</summary>
        /// <param name="fileName">The file selected by the user</param>
        /// <param name="filetype">The correct extension</param>
        /// <param name="error">The error message to get</param>
        /// <returns>True if the file type is correct, otherwise false</returns>
        public bool IsFileTypeCorrect(string fileName, string filetype, ref string error)
        {
            if (new FileInfo(fileName).Length != 0)
            {
                if (filetype.Equals(Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase))
                    return true;
                error = "Bad filetype";
                return false;
            }
            else
            {
                error = "Null file";
                return false;
            }
        }

        /// <summary>Splits each line to tokens and compares them with the regular expression tree</summary>
        /// <param name="fileName">The file selected by the user</param>
        /// <param name="error">The error message to get</param>
        /// <returns>True if the tokens are compared successfully, otherwise false</returns>
        //public bool ReadGrammar(string fileName, ref string error, BTreeNode regexTree)
        //{
        //    BTreeNode btree = new BTreeNode();
        //    CheckGrammar checkgrammar = new CheckGrammar();
        //    StreamReader grammarFile = new StreamReader(fileName);
        //    string grammar = grammarFile.ReadToEnd();
        //    var lines = File.ReadAllLines(fileName);
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        string copyGrammar = new string(grammar.ToCharArray());
        //        int index = 0;
        //        while (index != -1)
        //        {
        //            index = grammar.IndexOf(" ");
        //            if (index != -1)
        //                grammar = grammar.Remove(index, 1);
        //            index = grammar.IndexOf("\r");
        //            if (index != -1)
        //                grammar = grammar.Remove(index, 1);
        //            index = grammar.IndexOf("\t");
        //            if (index != -1)
        //                grammar = grammar.Remove(index, 1);
        //        }
        //        checkgrammar.CompareGrammar(fileName, ref error);
        //        error += ("en la línea {0}", "i");
        //    }
        //    //btree.InOrderAndCompare(regexTree, grammarQ, ref error);            
        //    return true;
        //}


    }
}
