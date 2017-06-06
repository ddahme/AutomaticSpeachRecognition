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
    class learnTreeFactory
    {
        private Element _addedElementInLastCall;
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
        private AddStrategyInterface _addStrategy;
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

        public learnTreeFactory(AddStrategyInterface addStrategy, int? depth = null)
        {
            _addStrategy = addStrategy;
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
            var addedElementInCurrentCall = new Element();

            //call the add-strategy
            _addStrategy.Add(_addedElementInLastCall, ident);
            //add add the result to the list of added elementrs for the next level
            addedElementInCurrentCall = _addStrategy.AddedElements.FirstOrDefault();

            //increment level
            _level++;
            //if the trees depth is restricted by _depth, check if this level is to deep
            if (_depth > 0 && ((_level) % _depth) == 0)
            {
                //if it is use the root as last level
                _addedElementInLastCall = _tree;
            }
            else
            {
                //if it is not just use the added elements of this iteratrion for the next one and go ahead
                _addedElementInLastCall = addedElementInCurrentCall;
            }
        }
    }
}
