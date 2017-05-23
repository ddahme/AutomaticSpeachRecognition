using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
    class P52 : AddStrategyInterface
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

        public P52(Tree lernTree)
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
            var elenemtsWithPerentIdent = getElementsByIdent(parent.Ident);
            var weight = 1.0;
            foreach (var parentElement in elenemtsWithPerentIdent)
            {
                var childElement = parentElement.Elements.Where(e => e.Ident == elementIdent).FirstOrDefault();
                if (childElement != null)
                {
                    weight *= childElement.Weight/parentElement.Weight;
                }
            }
            var element = new Element()
            {
                Elements = new List<Element>(),
                IsRoot = false,
                Parent = parent,
                Ident = elementIdent,
                Weight = weight
            };
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
            if (element.Elements.Count > 0)
            {
                foreach (var child in element.Elements)
                {
                    utilGetElementsByIdent(child, ident, ref elements);
                }
            }
        }
    }
}
