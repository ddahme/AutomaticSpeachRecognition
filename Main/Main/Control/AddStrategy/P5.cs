using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
    class P5 : AddStrategyInterface
    {
        private Tree _learnTree;
        private int _depth;
        private List<Element> _addedElements;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElements;
            }
        }

        public P5(Tree learnTree, int depth)
        {
            _learnTree = learnTree;
            _depth = depth;
        }

        public bool IsUsinglearnTree
        {
            get
            {
                return true;
            }
        }

        public void Add(Element parent, char elementIdent)
        {
            _addedElements = new List<Element>();
            if (parent.Weight == 0)
            {
                RemovePath(parent);
            }
            else
            {
                var parentInLearnTree = GetParentInLearnTree(parent);
                var key = KeyController.GetKeyByName(elementIdent);
                foreach (var letter in key.Letters)
                {
                    var elementInLearnTree = parentInLearnTree.Elements.Where(e => e.Ident == letter).FirstOrDefault();
                    if (parentInLearnTree != null && elementInLearnTree != null)
                    {
                        var weight = -(Math.Log(elementInLearnTree.Weight / parentInLearnTree.Weight) + parent.Weight);
                        var element = new Element()
                        {
                            Elements = new List<Element>(),
                            IsRoot = false,
                            Parent = parent,
                            Ident = letter,
                            Weight = weight
                        };
                        _addedElements.Add(element);
                        parent.Elements.Add(element);
                    }
                }
            }
        }

        private Element GetParentInLearnTree(Element parent)
        {
            var parseElement = parent;
            if (parent.IsRoot)
            {
                return _learnTree;
            }
            else
            {
                Element learnElement = _learnTree;
                var path = new List<Element>();
                while (!parseElement.IsRoot && path.Count <= _depth)
                {
                    path.Add(parseElement);
                    parseElement = parseElement.Parent;
                }
                path.Reverse();
                foreach (var element in path)
                {
                    learnElement = learnElement.Elements.Where(e => e.Ident == element.Ident).FirstOrDefault();
                }
                return learnElement;
            }
        }

        private void RemovePath(Element element)
        {
            Element parent;
            while (element.Elements.Count == 0)
            {
                parent = element.Parent;
                parent.Elements.Remove(element);
                element = parent;
            }
        }
    }
}
