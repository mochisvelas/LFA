using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Tools
    {
        /// <summary>Removes blank spaces</summary>
        /// <param name="text">Text trim</param>
        /// <returns>Text without blank spaces</returns>
        public string RemoveUnwantedChars(string text)
        {
            int index = 0;
            if (text != null)
            {
                while (index != -1)
                {
                    index = text.IndexOf(" ");
                    if (index != -1)
                        text = text.Remove(index, 1);
                }
                index = 0;
                while (index != -1)
                {
                    index = text.IndexOf("\r");
                    if (index != -1)
                        text = text.Remove(index, 1);
                }
                index = 0;
                while (index != -1)
                {
                    index = text.IndexOf("\t");
                    if (index != -1)
                        text = text.Remove(index, 1);
                }
                return text;
            }
            return text;
        }

        /// <summary>Tokenizes a regex</summary>
        /// <param name="regex">Regex to tokenize</param>
        /// <returns>Q of tokens</returns>
        public Queue<string> TokenizeRegex(string regex)
        {
            Queue<string> TokenizedRegexQ = new Queue<string>();
            Queue<char> regexQ = new Queue<char>(regex.ToCharArray());
            while (regexQ.Count() != 0)
            {
                string token = regexQ.Dequeue().ToString();
                if (token == "[")
                {
                    do
                    {
                        token += regexQ.Dequeue().ToString();
                    } while (!token.Contains("]"));
                    TokenizedRegexQ.Enqueue(token);
                }
                else if (token == @"\")
                {
                    token += regexQ.Dequeue().ToString();
                    TokenizedRegexQ.Enqueue(token);
                }
                else                    
                    TokenizedRegexQ.Enqueue(token);                
            }
            return TokenizedRegexQ;
        }
    }
}
