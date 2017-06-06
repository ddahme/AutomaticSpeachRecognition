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
    class ParseTreeFactory
    {
        private double _threshold = -10.0;//ToDo think about value
        private List<Element> _addedElementInCurrentCall;
        private List<Element> _addedElementInLastCall;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElementInLastCall;
            }
        }
        //private AddStrategyInterface _addStrategy;
        private int _level;

        private Tree _learnTree;
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

        public ParseTreeFactory(Tree learnTree, int? depth = null)
        {
            _learnTree = learnTree;
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
                Weight = 0 //ToDo: think about this: 1 or 0
            };
            _addedElementInLastCall = new List<Element>();
            _addedElementInLastCall.Add(_tree);
            _level = 0;
        }

        public Tree AddFile(string filePath)
        {
            string text;
            int TextSize;
            //read text
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
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
            //remove aded elements of last call from list
            _addedElementInCurrentCall = new List<Element>();

            //for each added element in the last iteration
            foreach (var parent in _addedElementInLastCall)
            {
                if (parent.Weight < _threshold)
                {
                    RemovePath(parent);
                }
                else
                {
                    //call the add-strategy
                    Add(parent, ident);
                }
            }
            _level++;
            _addedElementInLastCall = _addedElementInCurrentCall;
        }

        private void Add(Element parent, char elementIdent)
        {
            var parentInLearnTree = GetParentInLearnTree(parent);
            var key = KeyController.GetKeyByName(elementIdent);
            foreach (var letter in key.Letters)
            {
                var elementInLearnTree = parentInLearnTree.Elements.Where(e => e.Ident == letter).FirstOrDefault();
                if (parentInLearnTree != null && elementInLearnTree != null)
                {
                    var weight = -(Math.Log(elementInLearnTree.Weight / parentInLearnTree.Weight) + parent.Weight);
                    var element = new Element()
                    {
                        Elements = new List<Element>(),
                        IsRoot = false,
                        Parent = parent,
                        Ident = letter,
                        Weight = weight
                    };
                    parent.Elements.Add(element);
                    _addedElementInCurrentCall.Add(element);
                }
            }
        }

        private Element GetParentInLearnTree(Element parent)
        {
            var parseElement = parent;
            if (parent.IsRoot)
            {
                return _learnTree;
            }
            else
            {
                Element learnElement = _learnTree;
                var path = new List<Element>();
                while (!parseElement.IsRoot && path.Count <= _depth)
                {
                    path.Add(parseElement);
                    parseElement = parseElement.Parent;
                }
                path.Reverse();
                foreach (var element in path)
                {
                    learnElement = learnElement.Elements.Where(e => e.Ident == element.Ident).FirstOrDefault();
                }
                return learnElement;
            }
        }

        private void RemovePath(Element element)
        {
            Element parent;
            while (element.Elements.Count == 0)
            {
                parent = element.Parent;
                parent.Elements.Remove(element);
                element = parent;
            }
        }
    }
}
