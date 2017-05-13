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
        private CompositeInterface _lernTree;
        private List<CompositeInterface> _addedElements;
        public List<CompositeInterface> AddedElements
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

        public MarcowParseStrategy(WeightElement lernTree)
        {
            _lernTree = lernTree;
        }

        public void Add(CompositeInterface parent, char elementIdent)
        {
            _addedElements = new List<CompositeInterface>();
            var lernElements = _lernTree.Elements.SelectMany(e => e.Elements);
            var possibleLetters = KeyController.GetKeyByName(elementIdent).Letters;
            var parentInLernTree = (WeightElement)_lernTree.Elements.Where(e => e.Ident == parent.Ident).FirstOrDefault();
            foreach (var letter in possibleLetters)
            {
                var elementInLernTree = (WeightElement)parentInLernTree.Elements.Where(e => e.Ident == letter).FirstOrDefault();
                var probability = (double)elementInLernTree.Weight / (double)parentInLernTree.Weight;
                var element = new ProbabilityValueElement(elementIdent, parent, probability);
                parent.Add(element);
                _addedElements.Add(element);
            }
        }
    }
}
