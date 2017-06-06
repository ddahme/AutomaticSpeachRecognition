using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Control.AddStrategy;
using Main.Model;
using System.IO;

namespace Main.Control
{
    class LearnTreeFactory
    {
        private Element _addedElementInLastCall;
        private Element _addedElementInCurrentCall;
        public List<Element> AddedElements
        {
            get
            {
                return new List<Element>
                {
                    _addedElementInLastCall
                };
            }
        }
        //private AddStrategyInterface _addStrategy;
        private int _level;

        private Tree _tree;
        public Tree Tree
        {
            get
            {
                return _tree;
            }
        }

        private int _depth;
        public int Depth
        {
            get
            {
                return _depth;
            }
        }

        public LearnTreeFactory(int? depth = null)
        {
            //_addStrategy = addStrategy;
            _depth = 1;
            if (depth.HasValue)
            {
                _depth = depth.GetValueOrDefault();
            }
            _tree = new Tree()
            {
                Depth = _depth,
                Elements = new List<Element>(),
                Ident = ' ',
                IsRoot = true,
                Weight = 0
            };
            _addedElementInLastCall = _tree;
            _level = 0;
        }

        public Tree AddFile(string trainingsFilePath)
        {
            string text;
            int TextSize;
            //read text
            var fs = new FileStream(trainingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var reader = new StreamReader(fs))
            {
                text = reader.ReadToEnd();
                TextSize = text.Count();
            }
            for (int index = 0; (index + _depth) < TextSize; index++)
            {
                for (int offset = 0; offset < _depth; offset++)
                {
                    var letter = text[index + offset];
                    Add(letter);
                }

            }
            return _tree;
        }

        public void Add(char ident)
        {
            //if the trees depth is restricted by _depth, check if this level is to deep
            if (_depth > 0 && (_level % _depth) == 0)
            {
                _addedElementInLastCall = _tree;
            }
            else
            {
                _addedElementInLastCall = _addedElementInCurrentCall;
            }
            Add(_addedElementInLastCall, ident);
            _level++;
        }

        private void Add(Element parent, char elementIdent)
        {
            Element node;

            //check if element allready exists
            node = parent.Elements.Find(e => e.Ident == elementIdent);
            if (node == null)
            {
                //create new element
                node = new Element()
                {
                    Ident = elementIdent,
                    Parent = parent,
                    IsRoot = false,
                    Elements = new List<Element>(),
                    Weight = 0
                };
                //add it to parent
                parent.Elements.Add(node);
            }
            //increase weight of path
            IncreseWeightRecursiv(node);
            _addedElementInCurrentCall = node;
        }

        private void IncreseWeightRecursiv(Element composite)
        {
            composite.Weight++;
            if (!composite.IsRoot)
            {
                IncreseWeightRecursiv(composite.Parent);
            }
        }
    }
}
