using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Control
{
    class TreeFactory
    {

        public Tree CreateTree(string ident, int depth, string trainingsFilePath)
        {

            var tree = new Tree(ident, depth);

            return tree;
        }

        public Tree RestoreTree(string path)
        {
            throw new NotImplementedException();
        }

        public void SaveTree(Tree tree)
        {
            throw new NotImplementedException();
        }
    }
}
