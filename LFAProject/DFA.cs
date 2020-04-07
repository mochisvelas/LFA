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
        string trimmedText = string.Empty;

        public void trimText(string FileName)
        {
            StreamReader line = new StreamReader(FileName);
            do
            {
                trimmedText += line.ReadLine();
            } while (!trimmedText.Contains("ACTIONS"));
        }


    }
}
