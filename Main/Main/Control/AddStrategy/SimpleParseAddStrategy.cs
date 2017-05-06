using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
    /// <summary>
    /// Add-function for live-parsing
    /// just add all possible letters for the key to the list of perents elements.
    /// </summary>
    public class SimpleParseAddStrategy : AddStrategyInterface
    {
        private List<CompositeInterface> _addedElements;
        public List<CompositeInterface> AddedElements
        {
            get
            {
                return _addedElements;
            }
        }

        public SimpleParseAddStrategy()
        {
            _addedElements = new List<CompositeInterface>();
        }

        public void Add(CompositeInterface parent, char elementIdent)
        {
            var key = Keyboard.GetKeyByName(elementIdent);
            foreach(var letter in key.Letters)
            {
                var element = new Element(letter, parent);
                parent.Add(element);
                _addedElements.Add(element);
                parent.IncreaseWeightByOne();
            }
        }
    }
}
