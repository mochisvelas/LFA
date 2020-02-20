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
        /// <param name="file">The file uploaded by the user</param>
        /// <param name="filetype">The extension of the file to check</param>
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

        public bool SplitTokens(string fileName, ref string error) 
        {
            var lines = File.ReadLines(fileName);
            foreach (var line in lines) 
            {
                byte[] bArray = Encoding.ASCII.GetBytes(line);
                //btree method that will compare each ascii
                if (error != null && error != "Bad filetype" && error != "Null file")                
                    return false;  
            }
            return true;
        }
    }
}
