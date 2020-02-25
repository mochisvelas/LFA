using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Malgorithm
    {
        string TokensER = "";
        string ActionsER = "";
        List<string> SetsTokens = "(.SETS./n.+.(.id.+.' '.*.=.' '.*.(.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).(./+.'.TAZo.'.interval.'.TAZf.'.|.'.tazo.'.interval.'.tazf.'.|.'.t09o.'.interval.'.t09f.'.).*)./n.).+".Split('.').ToList<string>();
        List<string> TerminalSigns = new List<string> { "SETS", "id","TAZo", "TAZf", "interval", "tazo", "tazf", "T09o", "T09f", "/+", "/n", " ", "="};
        List<string> OperatorSigns = new List<string> { "+", "(", ")", "?", "*", "|"};
    }
}
