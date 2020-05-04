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

        public void EmptyFile(string fileName, ref string error) 
        {
            if (true)
            {

            }
        }

        public void CopyFolder(string sourceDir, string destDir) 
        {
            if (!string.IsNullOrEmpty(sourceDir) && !string.IsNullOrEmpty(destDir))
            {
                string destination_dir = Path.Combine(destDir, "Generated Scanner");
                string source_dir = /*@"E:\" + */sourceDir;
                if (!Directory.Exists(destination_dir))
                {
                    Directory.CreateDirectory(destination_dir);
                }
                else
                {
                    DeleteDirectory(destination_dir);
                }
                    
                foreach (string dir in Directory.GetDirectories(source_dir, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(Path.Combine(destination_dir, dir.Substring(source_dir.Length + 1)));                    
                }
                foreach (string file_name in Directory.GetFiles(source_dir, "*", SearchOption.AllDirectories))
                {
                    File.Copy(file_name, Path.Combine(destination_dir, file_name.Substring(source_dir.Length + 1)));
                }
            }  
        }

        private static void DeleteDirectory(string dirTodelete) 
        {
            var files = Directory.GetFiles(dirTodelete);
            var dir = Directory.GetDirectories(dirTodelete);

            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var d in dir)
            {
                DeleteDirectory(d);
            }
            Directory.Delete(dirTodelete, false);
        }
    }
}
