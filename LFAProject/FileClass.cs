using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public bool CheckGrammar(string fileName, ref string error) 
        {
            StreamReader grammarFile = new StreamReader(fileName);
            string grammar = grammarFile.ReadToEnd();
            int index = 0;
            while (index != -1)
            {
                index = grammar.IndexOf(" ");
                if (index != -1)
                    grammar = grammar.Remove(index, 1);
                index = grammar.IndexOf("\r");
                if (index != -1)
                    grammar = grammar.Remove(index, 1);
                index = grammar.IndexOf("\t");
                if (index != -1)
                    grammar = grammar.Remove(index, 1);                
            }

            return true;
        }


    }
}
