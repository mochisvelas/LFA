using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    class BTree
    {
        public BTreeNode root;
        
        public BTree(string value)
        {
            root = new BTreeNode(value);
        }

        public BTree()
        {
            root = null;
        }
    }
}
