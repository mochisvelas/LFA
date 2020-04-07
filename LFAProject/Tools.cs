using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class Tools
    {
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


    }
}
