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

        public List<string> getRegex(string FileName, List<string> TSigns) //validate that if TSings is empty, dont remove spaces in certain tokens and remove all functions
        {
            List<string> tokens = new List<string>();
            StreamReader line = new StreamReader(FileName);
            string readLine = line.ReadLine();
            while (!readLine.Contains("TOKENS"))
            {
                readLine = line.ReadLine();
            }
            while (!readLine.Contains("ACTIONS"))
            {
                if (!readLine.Equals("") && !readLine.Contains("TOKENS"))
                {
                    int index = readLine.IndexOf("=");
                    if (index > 0)
                        readLine = tools.RemoveUnwantedChars(readLine.Substring(index + 1));
                    if (readLine.Contains("RESERVADAS"))
                    {
                        readLine = tools.RemoveUnwantedChars(readLine.Replace("{RESERVADAS()}", ""));
                    }
                    tokens.Add(readLine);
                }
                readLine = line.ReadLine();
            }
            List<string> TokenList = createRegex(tokens, TSigns);            
            return TokenList;
        }

        public List<string> createRegex(List<string> tokens, List<string> TSigns) 
        {
            Queue<string> fixedTokens = new Queue<string>();
            foreach (var token in tokens)
            {
                Queue<string> actualToken = new Queue<string>();
                Queue<char> regexQ = new Queue<char>(token.ToCharArray());
                while (regexQ.Count() != 0)
                {
                    string chr = regexQ.Dequeue().ToString();
                    if (chr == "'" && regexQ.Peek().ToString().Equals("'"))
                    {
                        do
                        {
                            chr += regexQ.Dequeue().ToString();
                        } while (!chr.Equals("\'\'\'"));
                        actualToken.Enqueue(chr);
                    }
                    else if (chr == "'")
                    {
                        do
                        {
                            chr += regexQ.Dequeue().ToString();
                        } while (chr.Count(c => c == '\'') != 2);
                        actualToken.Enqueue(chr);
                    }
                    else if (chr.Any(x => char.IsLetter(x)) && TSigns.Count != 0)
                    {
                        do
                        {
                            chr += regexQ.Dequeue().ToString();
                        } while (!TSigns.Contains(chr));
                        actualToken.Enqueue(chr);
                    }
                    else
                    {
                        actualToken.Enqueue(chr);
                    }
                }
                if (actualToken.Count >= 2)
                {                    
                    while (actualToken.Count != 0)
                    {
                        string insert = actualToken.Dequeue();
                        fixedTokens.Enqueue(insert);
                        if (actualToken.Count !=0 && insert != "|" && insert != "(" && actualToken.Peek() != "*" && actualToken.Peek() != "|" && actualToken.Peek() != ")" && actualToken.Peek() != "?" && actualToken.Peek() != "+")
                        {
                            fixedTokens.Enqueue("·");
                        }
                    }                    
                    fixedTokens.Enqueue("|");                    
                }
                else
                {
                    fixedTokens.Enqueue(actualToken.Dequeue());
                    fixedTokens.Enqueue("|");
                }
            }
            List<string> fixedTokensList = fixedTokens.ToList<string>();
            fixedTokensList.RemoveAt(fixedTokensList.Count - 1);
            return fixedTokensList;
        }
        
    }
}
