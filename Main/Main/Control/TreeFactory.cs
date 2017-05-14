using Main.Control.AddStrategy;
using Main.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Main.Control
{
    public class TreeFactory
    {
        private List<Element> _addedElementsInLastCall;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElementsInLastCall;
            }
        }
        private List<Element> _rootList;
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

        public TreeFactory(AddStrategyInterface addStrategy, int? depth = null)
        {
            _addStrategy = addStrategy;
            _depth = depth.GetValueOrDefault();
            _tree = new Tree()
            {
                Depth = _depth,
                Elements = new List<Element>(),
                Ident = ' ',
                IsRoot = true,
                Weight = 0
            };
            _rootList = new List<Element>();
            _rootList.Add(_tree);
            _addedElementsInLastCall = _rootList;
            _level = 0;
        }

        public Tree CreateTreeOutOfTextFile(string trainingsFilePath)
        {
            char letter;

            //read text
            var fs = new FileStream(trainingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    //read letter form text
                    letter = (char)reader.Read();
                    //Add it to tree
                    Add(letter);
                }
            }
            return _tree;
        }

        public void Add(char ident)
        {
            var addedElementsInCurrentCall = new List<Element>();

            //for each added element in the last iteration
            foreach (var parent in _addedElementsInLastCall)
            {
                //call the add-strategy
                _addStrategy.Add(parent, ident);
                //add add the result to the list of added elementrs for the next level
                addedElementsInCurrentCall.AddRange(_addStrategy.AddedElements);
            }

            //increment level
            _level++;
            //if the trees depth is restricted by _depth, check if this level is to deep
            if (_depth > 0 && ((_level) % _depth) == 0)
            {
                //if it is use the root as last level
                _addedElementsInLastCall = _rootList;
            }
            else
            {
                //if it is not just use the added elements of this iteratrion for the next one and go ahead
                _addedElementsInLastCall = addedElementsInCurrentCall;
            }
        }
    }
}
