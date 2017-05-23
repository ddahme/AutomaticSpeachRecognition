using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
    class P54 : AddStrategyInterface
    {
        private Tree _lernTree;
        private int depth;
        private List<Element> _addedElements;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElements;
            }
        }

        public P54(Tree lernTree, int depth)
        {
            _lernTree = lernTree;
        }

        public bool IsUsingLernTree
        {
            get
            {
                return true;
            }
        }

        public void Add(Element parent, char elementIdent)
        {
            _addedElements = new List<Element>();
            var key = KeyController.GetKeyByName(elementIdent);
            foreach (var letter in key.Letters)
            {
                var element = new Element()
                {
                    Elements = new List<Element>(),
                    IsRoot = false,
                    Parent = parent,
                    Ident = letter,
                    Weight = CalculateWeight(parent)
                };
                _addedElements.Add(element);
                parent.Elements.Add(element);
            }
        }

        private double CalculateWeight(Element parent)
        {
            var result = 0.0;
            var parentInLerntree = _lernTree.
            return result;
        }

        private Element GetElementInLerntreeByIdentInDepth(char ident, int depth)
        {
            Element parent;
            parent.Elements.Where(e1 => e1.Elements)
            for(int i = 0; i < depth-1; i++)
            {
                parent = parent.
            }
        }
    }
}
