using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
    public class MarcowParseStrategy : AddStrategyInterface
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

        public bool IsUsingLernTree
        {
            get
            {
                return true;
            }
        }

        public MarcowParseStrategy(Tree lernTree)
        {
            _lernTree = lernTree;
        }

        public void Add(Element parent, char elementIdent)
        {
            _addedElements = new List<Element>();
            var lernElements = _lernTree.Elements.SelectMany(e => e.Elements);
            var possibleLetters = KeyController.GetKeyByName(elementIdent).Letters;
            var parentInLernTree = _lernTree.Elements.Where(e => e.Ident == parent.Ident).FirstOrDefault();
            foreach (var letter in possibleLetters)
            {
                var elementInLernTree = parentInLernTree.Elements.Where(e => e.Ident == letter).FirstOrDefault();
                var probability = elementInLernTree.Weight / parentInLernTree.Weight;
                var element = new Element()
                {
                    Ident = letter,
                    IsRoot = false,
                    Parent = parent,
                    Weight = probability,
                    Elements = new List<Element>()
                };
                parent.Elements.Add(element);
                _addedElements.Add(element);
            }
        }
        //ToD think about Root-Element. Its Weight has to be set to 1 in parse-tree by marcow-chains n order.

        public override string ToString()
        {
            return "MarcowParseStrategy";
        }
    }
}
