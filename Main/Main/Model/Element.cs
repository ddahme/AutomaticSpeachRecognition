using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    class Element : Composite
    {
        bool Composite.IsRoot
        {
            get
            {
                return false;
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
        private Composite _parent;
        Composite Composite.Parent
        {
            get
            {
                return _parent;
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
        private int _weight;
        int Composite.Weight
        {
            get
            {
                return _weight;
            }
        }

        public Element(char ident, Composite parent)
        {
            _parent = parent;
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
