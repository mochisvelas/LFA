using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LFAProject
{
    class DFA
    {
        Tools tools = new Tools();

        public List<string> TSigns(string FileName)
        {
            List<string> addedTSigns = new List<string>();
            StreamReader line = new StreamReader(FileName);
            string readLine = line.ReadLine();
            while (!readLine.Contains("TOKENS"))
            {
                if (!readLine.Contains("SETS") && !readLine.Equals(""))
                {                    
                    int index = readLine.IndexOf("=");
                    if (index > 0)
                        readLine = tools.RemoveUnwantedChars(readLine.Substring(0, index));
                    addedTSigns.Add(readLine);
                }
                readLine = line.ReadLine();
            }
            
            return addedTSigns;
        }

        public string trimText(string FileName)
        {
            string trimmedText = string.Empty;
            StreamReader line = new StreamReader(FileName);
            while (!trimmedText.Contains("ACTIONS"))
            {
                trimmedText += line.ReadLine();
            }

            trimmedText = tools.RemoveUnwantedChars(trimmedText);
            return trimmedText;
        }
        
    }
}
