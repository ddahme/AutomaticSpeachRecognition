using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    class Tree : Composite
    {
        private TreeInfo _info;

        Composite Composite.Parent
        {
            get
            {
                return null;
            }
        }

        bool Composite.IsRoot
        {
            get
            {
                return true;
            }
        }

        private int _weight;
        int Composite.Weight
        {
            get
            {
                return _weight;
            }
        }
        private char _ident;
        char Composite.Ident
        {
            get
            {
                return _ident;
            }
        }
        private Lazy<List<Composite>> _elements;
        Lazy<List<Composite>> Composite.Elements
        {
            get
            {
                return _elements;
            }
        }

        public Tree(string treeIdent, int depth, char ident)
        {
            _info = new TreeInfo(treeIdent, depth);
            _ident = ident;
            _weight = 0;
            
        }

        void Composite.Add(Composite element)
        {
            _elements.Value.Add(element);
        }

        public void IncreaseWeightByOne()
        {
            _weight++;
        }
    }
}