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
        private Tree _learnTree;
        private List<Element> _addedElements;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElements;
            }
        }

        public bool IsUsinglearnTree
        {
            get
            {
                return true;
            }
        }

        public MarcowParseStrategy(Tree learnTree)
        {
            _learnTree = learnTree;
        }

        public void Add(Element parent, char elementIdent)
        {
            _addedElements = new List<Element>();
            //var learnElements = _learnTree.Elements.SelectMany(e => e.Elements);
            var possibleLetters = KeyController.GetKeyByName(elementIdent).Letters;
            var parentInlearnTree = _learnTree.Elements.Where(e => e.Ident == parent.Ident).FirstOrDefault();
            foreach (var letter in possibleLetters)
            {
                var elementInlearnTree = parentInlearnTree.Elements.Where(e => e.Ident == letter).FirstOrDefault();
                //ToDo thinlk about elements which are not in the learntree
                var probability = 0.0;
                if (elementInlearnTree != null && parentInlearnTree != null)
                {
                    probability = elementInlearnTree.Weight / parentInlearnTree.Weight;
                }
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
