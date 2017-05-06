using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class Tree : CompositeInterface
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public CompositeInterface Parent
        {
            get
            {
                return null;
            }
        }

        public bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private int _depth;
        public int Depth
        {
            get
            {
                return _depth;
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
        private char _ident;
        public char Ident
        {
            get
            {
                return ' ';
            }
        }
        private Lazy<List<CompositeInterface>> _elements;
        public Lazy<List<CompositeInterface>> Elements
        {
            get
            {
                return _elements;
            }
        }

        public Tree(int depth)
        {
            _depth = depth;
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