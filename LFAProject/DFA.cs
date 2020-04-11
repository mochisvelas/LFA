﻿using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace LFAProject
{
    class DFA
    {
        readonly Tools tools = new Tools();

        /// <summary>Adds the terminal signs found in the SETS section</summary>
        /// <param name="FileName">File to read</param>
        /// <returns>List of terminal signs</returns>
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

        public List<string> GetRegex(string FileName, List<string> TSigns, ref string error)
        {
            List<string> tokens = new List<string>();
            StreamReader line = new StreamReader(FileName);
            string readLine = line.ReadLine();
            while (!readLine.Contains("TOKENS"))            
                readLine = line.ReadLine();            
            while (!readLine.Contains("ACTIONS"))
            {
                if (!readLine.Equals("") && !readLine.Contains("TOKENS"))
                {
                    int index = readLine.IndexOf("=");
                    if (index > 0)
                        readLine = tools.RemoveUnwantedChars(readLine.Substring(index + 1));
                    int inic = readLine.IndexOf("{");
                    index = readLine.IndexOf("}");
                    int length = index - inic + 1;
                    if (inic>0 && index>0)
                    {
                        string function = readLine.Substring(inic, length);
                        if (!function.Contains("'") && function.Contains("(") && function.Contains(")"))
                            readLine = tools.RemoveUnwantedChars(readLine.Replace(function, ""));
                    }                                        
                    tokens.Add(readLine);
                }
                readLine = line.ReadLine();
            }
            List<string> TokenList = CreateRegex(tokens, TSigns, ref error);            
            return TokenList;
        }

        public List<string> CreateRegex(List<string> tokens, List<string> TSigns, ref string error) 
        {
            Queue<string> fixedTokens = new Queue<string>();
            fixedTokens.Enqueue("(");
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
                    else if (chr.Any(x => char.IsLetter(x)) && TSigns.Count == 0)
                    {
                        error="No hay nigún Set al cual hacer referencia.";
                    }
                    else                    
                        actualToken.Enqueue(chr);                    
                }
                if (actualToken.Count >= 2)
                {                    
                    while (actualToken.Count != 0)
                    {
                        string insert = actualToken.Dequeue();
                        fixedTokens.Enqueue(insert);
                        if (actualToken.Count !=0 && insert != "|" && insert != "(" && actualToken.Peek() != "*" && actualToken.Peek() != "|" && actualToken.Peek() != ")" && actualToken.Peek() != "?" && actualToken.Peek() != "+")                        
                            fixedTokens.Enqueue("·");                        
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
