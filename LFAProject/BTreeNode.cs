using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class BTreeNode
    {
        public string Token;
        public BTreeNode right, left;

        public BTreeNode(string value) 
        {
            Token = value;
            right = left = null;
        }

        public BTreeNode() 
        {
            Token = null;
            right = left = null;
        }
    }
}
