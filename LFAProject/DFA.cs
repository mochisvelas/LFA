﻿using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace LFAProject
{
    class DFA
    {
        readonly Tools tools = new Tools();
        readonly FileClass fileHelper = new FileClass();

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

        public Dictionary<int, string> GetTokenWords(string FileName) 
        {
            Dictionary<int, string> reservedWords = new Dictionary<int, string>();
            StreamReader line = new StreamReader(FileName);
            string readLine = line.ReadLine();            
            while (!readLine.Contains("TOKENS"))
                readLine = line.ReadLine();
            while (!readLine.Contains("ACTIONS"))
            {
                if (!string.IsNullOrEmpty(readLine) && !readLine.Contains("TOKENS"))
                {                    
                    var actualToken = readLine.Split(new[] { '=' }, 2);
                    string fixedToken = actualToken[1];
                    fixedToken = fileHelper.RemoveSingleQuotes(fixedToken);
                    if (fixedToken.Contains("RESERVADAS()"))
                    {
                        fixedToken = fixedToken.Replace("RESERVADAS()", "");
                        fixedToken = fixedToken.Replace("{", "");
                        fixedToken = fixedToken.Replace("}", "");
                    }
                    fixedToken = tools.RemoveUnwantedChars(fixedToken);                    
                    reservedWords.Add(int.Parse(tools.RemoveUnwantedChars(actualToken[0].Replace("TOKEN", ""))), fixedToken);
                }
                readLine = line.ReadLine();
            }
            while (!readLine.Contains("}"))
            {
                if (!string.IsNullOrEmpty(readLine) && !readLine.Contains("ACTIONS") && !readLine.Contains("{") && !readLine.Contains("RESERVADAS()"))
                {
                    readLine = tools.RemoveUnwantedChars(readLine);
                    readLine = readLine.Replace("'", "");
                    var actualToken = readLine.Split(new[] { '=' }, 2);                                        
                    reservedWords.Add(int.Parse(tools.RemoveUnwantedChars(actualToken[0])), actualToken[1]);
                }
                readLine = line.ReadLine();
            }
            bool read = true;
            while (read && !line.EndOfStream)
            {
                readLine = line.ReadLine();
                if (!string.IsNullOrEmpty(readLine))
                {                    
                    if (readLine.Contains("}"))
                    {
                        read = false;
                    }
                    else if (!readLine.Contains("{") && !readLine.Contains("(") && !readLine.Contains("ERROR"))
                    {
                        readLine = tools.RemoveUnwantedChars(readLine);
                        readLine = readLine.Replace("'", "");
                        var actualToken = readLine.Split(new[] { '=' }, 2);
                        reservedWords.Add(int.Parse(tools.RemoveUnwantedChars(actualToken[0])), actualToken[1]);
                    }
                }
            }
            return reservedWords;
        }

        public Dictionary<string, List<string>> GetSetsRanges(string FileName, List<string> addedTSigns)
        {            
            Dictionary<string, List<string>> sets = new Dictionary<string, List<string>>();
            Queue<string> setsQ = new Queue<string>(addedTSigns);
            StreamReader line = new StreamReader(FileName);
            string readLine = line.ReadLine();            
            while (!readLine.Contains("TOKENS"))
            {
                List<string> ranges = new List<string>();
                if (!readLine.Contains("SETS") && !string.IsNullOrEmpty(readLine))
                {
                    var setArr = tools.RemoveUnwantedChars(readLine).Split(new[] { '=' }, 2);
                    //readLine = tools.RemoveUnwantedChars(readLine.Substring(readLine.IndexOf("=") + 1));
                    if (setArr[1].Contains(".."))
                    {
                        readLine = setArr[1].Replace("..", "-");
                    }
                    if (readLine.Contains("'"))
                    {
                        readLine = readLine.Replace(@"'", "");
                    }                       
                    if (readLine.Contains("+"))
                    {
                        var rangeArray = readLine.Split('+');
                        foreach (var range in rangeArray)
                        {
                            if (range.Contains("-"))
                            {
                                var subRangeArr = range.Split('-');
                                string subRange = string.Empty;
                                foreach (var sub in subRangeArr)
                                {
                                    if (sub.Contains("CHR"))
                                    {
                                        Regex pattern = new Regex("[CHR()]");
                                        if (!string.IsNullOrEmpty(subRange))
                                        {
                                            subRange += pattern.Replace(sub, "");
                                        }
                                        else
                                        {
                                            subRange += pattern.Replace(sub, "") + "-";
                                        }                                       
                                    }
                                    else
                                    {
                                        int asciiValue = System.Convert.ToChar(sub);
                                        if (!string.IsNullOrEmpty(subRange))
                                        {
                                            subRange += asciiValue;
                                        }
                                        else
                                        {
                                            subRange = asciiValue + "-";
                                        }
                                    }                                    
                                }
                                ranges.Add(subRange);
                            }
                            else
                            {
                                ranges.Add(System.Convert.ToInt32(System.Convert.ToChar(range)).ToString());
                            }
                        }
                        sets.Add(setArr[0], ranges);                        
                    }
                    else
                    {
                        if (readLine.Contains("-"))
                        {
                            var subRangeArr = readLine.Split('-');
                            string subRange = string.Empty;
                            foreach (var sub in subRangeArr)
                            {
                                if (sub.Contains("CHR"))
                                {
                                    Regex pattern = new Regex("[CHR()]");
                                    if (!string.IsNullOrEmpty(subRange))
                                    {
                                        subRange += pattern.Replace(sub, "");
                                    }
                                    else
                                    {
                                        subRange += pattern.Replace(sub, "") + "-";
                                    }
                                }
                                else
                                {
                                    int asciiValue = System.Convert.ToChar(sub);
                                    if (!string.IsNullOrEmpty(subRange))
                                    {
                                        subRange += asciiValue;
                                    }
                                    else
                                    {
                                        subRange = asciiValue + "-";
                                    }
                                }                                
                            }
                            ranges.Add(subRange);
                        }
                        else
                        {
                            ranges.Add(System.Convert.ToInt32(System.Convert.ToChar(readLine)).ToString());
                        }                        
                        sets.Add(setArr[0], ranges);                        
                    }                    
                }
                readLine = line.ReadLine();
            }
            if (setsQ.Count() != 0)
            {               
                while (setsQ.Count() != 0)
                {
                    List<string> alreadyAdded = sets.Keys.ToList();
                    string find = alreadyAdded.Find(x => setsQ.Peek().Contains(x));
                    if (!string.IsNullOrEmpty(find))
                    {
                        setsQ.Dequeue();
                    }
                    else
                    {
                        List<string> singleRanges = new List<string>();
                        string set = string.Empty;
                        if (!string.IsNullOrEmpty(setsQ.Peek().Replace(@"'", "")))
                        {
                            set = setsQ.Peek().Replace("'", "");
                        }
                        else
                        {
                            set = setsQ.Peek().Replace("'''", "'");
                        }
                        singleRanges.Add(System.Convert.ToInt32(System.Convert.ToChar(set)).ToString());
                        sets.Add(setsQ.Dequeue(), singleRanges);
                    }                                        
                }                
            }
            return sets;
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
                if (!string.IsNullOrEmpty(readLine) && !readLine.Contains("TOKENS"))
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
                        } while (regexQ.Count != 0 && !TSigns.Contains(chr));
                        if (!TSigns.Contains(chr))
                        {
                            error = "Se está intentando hacer referencia a un Set inexistente.";
                        }
                        else
                        {
                            actualToken.Enqueue(chr);
                        }                        
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
                    if (actualToken.Count != 0)
                    {
                        fixedTokens.Enqueue(actualToken.Dequeue());
                    }                    
                    fixedTokens.Enqueue("|");
                }
            }            
            List<string> fixedTokensList = fixedTokens.ToList<string>();
            fixedTokensList.RemoveAt(fixedTokensList.Count - 1);
            return fixedTokensList;
        }

        public Dictionary<int, bool> GetFinalStates(Dictionary<List<int>, bool> statesList, int finalState)
        {
            Dictionary<int, bool> finalStatesDic = new Dictionary<int, bool>();
            int cont = 0;
            foreach (var state in statesList)
            {
                if (state.Key.Contains(finalState))
                {
                    finalStatesDic.Add(cont, true);
                }
                else
                {
                    finalStatesDic.Add(cont, false);
                }
                cont++;
            }
            return finalStatesDic;
        }
    }
}
