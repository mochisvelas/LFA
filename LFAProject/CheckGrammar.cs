using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LFAProject
{
    class CheckGrammar
    {
        public void CompareGrammar(Queue<char> grammar, ref string error) 
        {
            while (grammar.Count() != 0)
            {
                if (grammar.Dequeue().ToString() == "S")
                {
                    if (grammar.Dequeue().ToString() == "E")
                    {
                        if (grammar.Dequeue().ToString() == "T")
                        {
                            if (grammar.Dequeue().ToString() == "S")
                            {
                                if (grammar.Dequeue().ToString() == "/n")
                                {
                                    while (grammar.Peek().ToString() == "/n")                                    
                                        grammar.Dequeue();

                                    if (grammar.Peek().ToString() == "=")
                                    {
                                        error = "Se esperaba un identificador para el set ";
                                    }
                                    else
                                    {
                                        bool IsinvalidId = false;
                                        bool letter;
                                        bool number;
                                        while (grammar.Peek().ToString() != "=")
                                        {
                                            letter = grammar.Dequeue().ToString().Any(x => !char.IsLetter(x));
                                            number = grammar.Dequeue().ToString().Any(x => !char.IsNumber(x));
                                            if (letter || number)
                                            {
                                                IsinvalidId = true;
                                            }
                                        }

                                        if (IsinvalidId)
                                        {
                                            error = "El identificador el set contiene un caracter que no es letra o número ";
                                        }
                                    }                                       
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (grammar.Dequeue().ToString() == "T") 
                    {
                        if (grammar.Dequeue().ToString() == "O")
                        {
                            if (grammar.Dequeue().ToString() == "K")
                            {
                                if (grammar.Dequeue().ToString() == "E")
                                {
                                    if (grammar.Dequeue().ToString() == "N")
                                    {
                                        if (grammar.Dequeue().ToString() == "S")
                                        {
                                            if (grammar.Dequeue().ToString() == "S")
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        error = "Se esperaba la palabra 'TOKENS' o 'SETS' ";
                    }
                } 
                
            }            
        }
    }
}
