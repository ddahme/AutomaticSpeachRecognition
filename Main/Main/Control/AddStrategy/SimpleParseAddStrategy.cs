﻿using System;
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
        private KeyController _keyController;
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
            _keyController = new KeyController();
        }

        public void Add(CompositeInterface parent, char elementIdent)
        {
            _addedElements = new List<CompositeInterface>();
            var key = _keyController.GetKeyByName(elementIdent);
            foreach(var letter in key.Letters)
            {
                var element = new Element(letter, parent);
                parent.Add(element);
                IncreseWeightRecursiv(element);
                _addedElements.Add(element);
            }
        }

        private void IncreseWeightRecursiv(CompositeInterface composite)
        {
            composite.IncreaseWeightByOne();
            if (!composite.IsRoot)
            {
                IncreseWeightRecursiv(composite.Parent);
            }
        }
    }
}
