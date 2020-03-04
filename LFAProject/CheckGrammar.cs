using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace LFAProject
{
    class CheckGrammar
    {
        public void CompareGrammar(string fileName, ref string error) 
        {
            int cont = 0;
            StreamReader grammarFile = new StreamReader(fileName);
            string grammarfirst = RemoveUnwantedChars(grammarFile.ReadLine());
            //Regex SetsReg = new Regex(@"^([A-Za-z]+=(('[A-Z]'\.\.'[A-Z]')|('[a-z]'\.\.'[a-z]')|('[0-9]'\.\.'[0-9]')|(CHR\([0-9]+\)\.\.CHR\([0-9]+\))|('[\-_\*\+@\$%&]{1}'))(((\+('[A-Z]'\.\.'[A-Z]')|('[a-z]'\.\.'[a-z]')|('[0-9]'\.\.'[0-9]')|(CHR\([0-9]+\)\.\.CHR\([0-9]+\))|('[\-_\*\+@\$%&]{1}')))*))?$");
            Regex SetsRegex = new Regex(@"^(([A-Z]+=(('[A-Za-z0-9]'\.\.'[A-Za-z0-9]')|(CHR\([0-9]+\)\.\.CHR\([0-9]+\))|('[A-Za-z0-9_]')))((((\+(('[A-Za-z0-9_]')|('[A-Za-z0-9]'\.\.'[A-Za-z0-9]')))*))))$");
            //Regex SetsRege = new Regex(@"^([a-zA-ZñÑ\s])+=((('([a-zA-Z0-9ñÑ<>=;:(){}\.\[\],'\+\-_\*\.\s]{1})'|'([a-zA-Z0-9Ññ<>=;:(){}\.\[\],'\+\-_\*\.\s]{1})'\.\.'([0-9a-zA-ZñÑ<>=;:(){}\.\[\],'\+\-_\*\.\s]{1})')(\+('([0-9a-zA-ZñÑ<>=;:(){}\.\[\],'\+\-_\*\.\s]{1})'|'([0-9a-zA-ZñÑ<>=;:(){}\.\[\],'\+\-_\*\.\s]{1})'\.\.'([0-9a-zA-ZñÑ<>=;:(){}\.\[\],'\+\-_\*\.\s]{1})'))*)|((CHR\([0-9]+\)\.\.CHR\([0-9]+\))(\+(CHR\([0-9]+\)\.\.CHR\([0-9]+\)))))$");
            Regex TokensRegex = new Regex(@"^((TOKEN\s+[0-9]+=('[\-_\*\+@\$%&]{1}'(('[\-_\*\+@\$%&]{1}')+)|'[A-Z]+'|'(\?|\+|\(|\))'|'(\?|\+|\(|\))''(\?|\+|\(|\))'|(AZ+((\?|\+)?))+|('(''|')'[A-Z]+'(''|')')((\|'(''|')'[A-Z]+'(''|')')*)| [A-Z]+\([A-Z]+\|[A-Z]+\)((\*|\?)?)))((\nTOKEN\s+[0-9]+=('[\-_\*\+@\$%&]{1}'(('[\-_\*\+@\$%&]{1}')+)|'[A-Z]+'|'(\?|\+|\(|\))'|'(\?|\+|\(|\))''(\?|\+|\(|\))'|(AZ+((\?|\+)?))+|('(''|')'[A-Z]+'(''|')')((\|'(''|')'[A-Z]+'(''|')')*)|[A-Z]+\([A-Z]+\|[A-Z]+\)((\*|\?)?)))*){RESERVADAS\(\)})#$");
            Regex ActionsRegex = new Regex(@"^(ACTIONS([0-9]+='[A-Z]+')(\n[0-9]+='[A-Z]+')((\n[A-Z]+\(\){([0-9]+='[A-Z]+')(\n[0-9]+='[A-Z]+')})*))#$");
            Regex ErrorsRegex = new Regex(@"^([A-Z]+ERROR=[0-9]+((\n[A-Z]+ERROR=[0-9]+)*))#$");

            if (grammarfirst == "SETS")
            {
                cont++;

                string grammarSecond = RemoveUnwantedChars(grammarFile.ReadLine());
                while (!grammarFile.EndOfStream && grammarSecond != "TOKENS")
                {
                    cont++;
                    if (!SetsRegex.IsMatch(grammarSecond))
                    {
                        error = "Error en set en la línea " + cont;
                    }
                    grammarSecond = RemoveUnwantedChars(grammarFile.ReadLine());
                }                                


                while (!grammarFile.EndOfStream && RemoveUnwantedChars(grammarFile.ReadLine()) != "ACTIONS")
                {
                    cont++;
                    if (!grammarFile.EndOfStream && !TokensRegex.IsMatch(RemoveUnwantedChars(grammarFile.ReadLine())))
                    {
                        error = "Error en token en la línea " + cont;
                    }
                }

                if (RemoveUnwantedChars(grammarFile.ReadLine()) != "RESERVADAS()")
                {
                    error = "Error en la línea " + cont + ", se esperaba RESERVADAS()";
                }
                if (RemoveUnwantedChars(grammarFile.ReadLine()) != "{")
                {
                    error = "Error en la línea " + cont + ", se esperaba una '{'.";
                }

                while (!grammarFile.EndOfStream && RemoveUnwantedChars(grammarFile.ReadLine()) != "}")
                {
                    cont++;
                    if (!ActionsRegex.IsMatch(RemoveUnwantedChars(grammarFile.ReadLine())))
                    {
                        error = "Error en token en la línea " + cont;
                    }
                    if (grammarFile.EndOfStream)
                    {
                        error = "Error en la línea " + cont + ", se esperaba al menos un ERROR";
                    }
                }

                while (grammarFile.EndOfStream != true)
                {
                    cont++;
                    if (!ErrorsRegex.IsMatch(RemoveUnwantedChars(grammarFile.ReadLine())))
                    {
                        error = "Error en error en la línea " + cont;
                    }
                }


            }
            else if (grammarfirst == "TOKENS")
            {
                while (!grammarFile.EndOfStream && RemoveUnwantedChars(grammarFile.ReadLine()) != "ACTIONS")
                {
                    cont++;
                    if (!TokensRegex.IsMatch(RemoveUnwantedChars(grammarFile.ReadLine())))
                    {
                        error = "Error en token en la línea " + cont;
                    }
                }

                if (RemoveUnwantedChars(grammarFile.ReadLine()) != "RESERVADAS()")
                {
                    error = "Error en la línea " + cont + ", se esperaba RESERVADAS()";
                }
                if (RemoveUnwantedChars(grammarFile.ReadLine()) != "{")
                {
                    error = "Error en la línea " + cont + ", se esperaba una '{'.";
                }

                while (!grammarFile.EndOfStream && RemoveUnwantedChars(grammarFile.ReadLine()) != "}")
                {
                    cont++;
                    if (!ActionsRegex.IsMatch(RemoveUnwantedChars(grammarFile.ReadLine())))
                    {
                        error = "Error en token en la línea " + cont;
                    }
                    if (grammarFile.EndOfStream)
                    {
                        error = "Error en la línea " + cont + ", se esperaba al menos un ERROR";
                    }
                    else
                    {
                        error = "success";
                    }
                }

                while (grammarFile.EndOfStream != true)
                {
                    cont++;
                    if (!ErrorsRegex.IsMatch(RemoveUnwantedChars(grammarFile.ReadLine())))
                    {
                        error = "Error en error en la línea " + cont;
                    }
                    else
                    {
                        error = "success";
                    }
                }
            }
            else
            {
                error = "Se esperaba SETS o TOKENS";
            }
                       
        }

        public string RemoveUnwantedChars(string grammar)
        {
            int index = 0;
            if (grammar != null)
            {
                while (index != -1)
                {
                    index = grammar.IndexOf(" ");
                    if (index != -1)
                        grammar = grammar.Remove(index, 1);
                }
                index = 0;
                while (index != -1)
                {
                    index = grammar.IndexOf("\r");
                    if (index != -1)
                        grammar = grammar.Remove(index, 1);
                }
                index = 0;
                while (index != -1)
                {
                    index = grammar.IndexOf("\t");
                    if (index != -1)
                        grammar = grammar.Remove(index, 1);
                }
                return grammar;
            }
            return grammar;
        }
    }
}
