using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
    class P51 : AddStrategyInterface
    {
        private Tree _lernTree;
        private List<Element> _addedElements;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElements;
            }
        }

        public P51(Tree lernTree)
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
            var weight = getElementsByIdent(elementIdent).Sum(e => e.Weight)/_lernTree.Weight;
            var element = new Element()
            {
                Elements = new List<Element>(),
                IsRoot = false,
                Parent = parent,
                Ident = elementIdent,
                Weight = weight
            };
            _addedElements.Add(element);
            parent.Elements.Add(element);
        }

        private List<Element> getElementsByIdent(char ident)
        {
            var result = new List<Element>();
            utilGetElementsByIdent(_lernTree, ident, ref result);
            return result;
        }

        private void utilGetElementsByIdent(Element element, char ident, ref List<Element> elements)
        {
            if (element.Ident == ident)
            {
                elements.Add(element);
            }
            if(element.Elements.Count > 0)
            {
                foreach(var child in element.Elements)
                {
                    utilGetElementsByIdent(child, ident, ref elements);
                }
            }
        }
    }
}
