﻿using System;
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
            if (new FileInfo(fileName) != null)
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
    }
}