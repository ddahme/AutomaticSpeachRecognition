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
        private Tree _lernTree;
        private List<Element> _addedElements;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElements;
            }
        }

        public P5(Tree lernTree)
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
                var elementInLearnTree = _lernTree.Elements.FirstOrDefault(e => e.Ident == letter);
                if(parent == null || parent.Weight==null || _lernTree == null || _lernTree.Weight == null || elementInLearnTree == null)
                {
                    throw new NullReferenceException("parent or lern-tree is null");
                }

                var weight = -(Math.Log(elementInLearnTree.Weight / _lernTree.Weight) + parent.Weight);                
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
