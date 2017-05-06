using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class Element : CompositeInterface
    {
        private Lazy<List<CompositeInterface>> _elements;
        public Lazy<List<CompositeInterface>> Elements
        {
            get
            {
                return _elements;
            }
        }
        private char _ident;
        public char Ident
        {
            get
            {
                return _ident;
            }
        }

        public bool IsRoot
        {
            get
            {
                return false;
            }
        }
        private CompositeInterface _parent;
        public CompositeInterface Parent
        {
            get
            {
                return _parent;
            }
        }
        private int _weight;
        public int Weight
        {
            get
            {
                return _weight;
            }
        }

        public Element(char ident, CompositeInterface parent)
        {
            _parent = parent;
            _ident = ident;
            _weight = 0;
        }

        public void Add(CompositeInterface element)
        {
            _elements.Value.Add(element);
        }

        public void IncreaseWeightByOne()
        {
            _weight++;
        }
    }
}
