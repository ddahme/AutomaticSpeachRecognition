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
        private List<Element> _addedElements;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElements;
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
                Weight = 1.0
            };
            _rootList = new List<Element>();
            _rootList.Add(_tree);
            _addedElements = _rootList;
            _level = 0;
        }

        public Tree AddFile(string trainingsFilePath)
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
            List<Element> addedElementsInLastCall;
            //if the trees depth is restricted by _depth, check if this level is to deep
            if (_depth > 0 && (_level % _depth) == 0)
            {
                addedElementsInLastCall = _rootList;
            }
            else
            {
                addedElementsInLastCall = _addedElements;
            }
            //remove aded elements of last call from list
            _addedElements = new List<Element>();

            //for each added element in the last iteration
            foreach (var parent in addedElementsInLastCall)
            {
                //call the add-strategy
                _addStrategy.Add(parent, ident);
                //add add the result to the list of added elementrs for the next level
                _addedElements.AddRange(_addStrategy.AddedElements);
            }
            _level++;
        }
    }
}
