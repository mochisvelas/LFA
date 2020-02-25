using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class BTreeNode
    {
        string Token;
        BTreeNode right, left;

        BTreeNode(string value) 
        {
            Token = value;
            right = left = null;
        }
    }
}
