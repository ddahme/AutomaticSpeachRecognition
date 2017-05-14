using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    [Serializable]
    public class Element
    {
        private List<Element> _elements;
        public List<Element> Elements
        {
            get
            {
                return _elements;
            }
            set
            {
                _elements = value;
            }
        }
        private char _ident;
        public char Ident
        {
            get
            {
                return _ident;
            }
            set
            {
                _ident = value;
            }
        }

        private bool _isRoot;
        public bool IsRoot
        {
            get
            {
                return _isRoot;
            }
            set
            {
                _isRoot = value;
            }
        }
        private Element _parent;
        public Element Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
        private double _weight;
        public double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }
    }
}
