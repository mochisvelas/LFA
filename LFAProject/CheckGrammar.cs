using System.Text.RegularExpressions;
using System.IO;

namespace LFAProject
{
    class CheckGrammar
    {
        public bool CompareGrammar(string fileName)
        {
            string sets = GetSets(fileName);
            string tokens = GetTokens(fileName);
            string text = File.ReadAllText(fileName);
            Regex SetsRegex = new Regex(@"(((SETS)((\t|\s)*(\n))*((\t|\s)*[A-Z]+(\t|\s)*=(\t|\s)*((('.')|(CHR\([0-9]+\)))((\.\.)(('.')|(CHR\([0-9]+\))))?(\+(('.')|(CHR\([0-9]+\)))((\.\.)(('.')|(CHR\([0-9]+\))))?)*)((\t|\s)*(\n))+)+))");
            Match setsMatch = SetsRegex.Match(sets);
            Regex TokensRegex = new Regex(@"^((TOKEN\s+[0-9]+=('[\-_\*\+@\$%&]{1}'(('[\-_\*\+@\$%&]{1}')+)|'[A-Z]+'|'(\?|\+|\(|\))'|'(\?|\+|\(|\))''(\?|\+|\(|\))'|(AZ+((\?|\+)?))+|('(''|')'[A-Z]+'(''|')')((\|'(''|')'[A-Z]+'(''|')')*)| [A-Z]+\([A-Z]+\|[A-Z]+\)((\*|\?)?)))((\nTOKEN\s+[0-9]+=('[\-_\*\+@\$%&]{1}'(('[\-_\*\+@\$%&]{1}')+)|'[A-Z]+'|'(\?|\+|\(|\))'|'(\?|\+|\(|\))''(\?|\+|\(|\))'|(AZ+((\?|\+)?))+|('(''|')'[A-Z]+'(''|')')((\|'(''|')'[A-Z]+'(''|')')*)|[A-Z]+\([A-Z]+\|[A-Z]+\)((\*|\?)?)))*){RESERVADAS\(\)})#$");
            Regex ActionsRegex = new Regex(@"^(ACTIONS([0-9]+='[A-Z]+')(\n[0-9]+='[A-Z]+')((\n[A-Z]+\(\){([0-9]+='[A-Z]+')(\n[0-9]+='[A-Z]+')})*))#$");
            Regex ErrorsRegex = new Regex(@"^([A-Z]+ERROR=[0-9]+((\n[A-Z]+ERROR=[0-9]+)*))#$");
            Regex Regex = new Regex(@"^(((SETS)((\t|\s)*(\n))+((\t|\s)*[A-Z]+(\t|\s)*=(\t|\s)*((('.')|(CHR\([0-9]+\)))((\.\.)(('.')|(CHR\([0-9]+\))))?(\+(('.')|(CHR\([0-9]+\)))((\.\.)(('.')|(CHR\([0-9]+\))))?)*)((\t|\s)*(\n))+)+))?((\n)*(\t|\s)*(TOKENS)((\t|\s)*(\n))+((\t|\s)*(TOKEN)(\t|\s)+[0-9]+(\t|\s)*=(\t|\s)*(\s|\*|\+|\?|\(|\)|\||'.'|[A-Z]+|{|})+((\t|\s)*(\n))+)+)((\n)*(ACTIONS)((\t|\s)*(\n))+((\t|\s)*(RESERVADAS\(\))((\t|\s)*(\n))+{((\t|\s)*(\n))+((\t|\s)*[0-9]+(\t|\s)*=(\t|\s)*'[A-Z]+'((\t|\s)*(\n))+)+}((\t|\s)*(\n))+)((\t|\s)*[A-Z]+\(\)((\t|\s)*(\n))+{((\t|\s)*(\n))+((\t|\s)*[0-9]+(\t|\s)*=(\t|\s)*'[A-Z]+'((\t|\s)*(\n))+)+}((\t|\s)*(\n))+)*)((([A-Z]*ERROR(\t|\s)*=(\t|\s)*[0-9]+)((\n[A-Z]*ERROR(\t|\s)*=(\t|\s)*[0-9]+)*)))$");
            Match match = Regex.Match(text);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        public string GetSets(string fileName) 
        {
            StreamReader line = new StreamReader(fileName);
            string readLine = line.ReadLine();
            while (!readLine.Contains("TOKENS"))
            {
                readLine += line.ReadLine();
            }
            if (!readLine.Contains("SETS"))
            {
                return "";
            }
            else
            {
                readLine = readLine.Replace("TOKENS", "");
                return readLine;
            }            
        }

        public string GetTokens(string fileName)
        {
            StreamReader line = new StreamReader(fileName);
            string readLine = line.ReadLine();
            while (!string.IsNullOrEmpty(readLine) && !readLine.Contains("TOKENS") && !readLine.Contains("TOKEN")) 
            {
                readLine = line.ReadLine();
            }                
            while (!readLine.Contains("ACTIONS"))
            {
                readLine += line.ReadLine();
            }
            readLine.Replace("ACTIONS", "");
            return readLine;
        }
    }
}
