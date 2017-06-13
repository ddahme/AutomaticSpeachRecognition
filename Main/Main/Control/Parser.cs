using System;
using System.Collections.Generic;
using System.Linq;
using Main.Model;
using System.IO;
using System.Text;

namespace Main.Control
{
    class Parser
    {
        private double _threshold = 10.0;//ToDo think about value
        private List<Element> _addedElementInCurrentCall;
        private List<Element> _addedElementInLastCall;
        public List<Element> AddedElements
        {
            get
            {
                return _addedElementInLastCall;
            }
        }
        private Tree _learnTree;
        private Tree _tree;
        public Tree Tree
        {
            get
            {
                return _tree;
            }
        }
        private int? _width;
        public int? Width
        {
            get
            {
                return _width;
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

        public Parser(Tree learnTree, int? depth = null, int? width = null)
        {
            _learnTree = learnTree;
            _depth = 1;
            if (depth.HasValue)
            {
                _depth = depth.GetValueOrDefault();
            }
            _width = width;
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
        }

        public void Parse(string filePath)
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
            foreach (var letter in text)
            {
                var key = KeyController.GetKeyByName(letter);
                Parse(key);
            }
        }

        public void Parse(Key key)
        {
            //remove aded elements of last call from list
            _addedElementInCurrentCall = new List<Element>();
            //for each added element in the last iteration
            foreach (var parent in _addedElementInLastCall)
            {
                var text = TreeController.CalculateText(parent, _depth);
                foreach (var letter in key.Letters)
                {
                    var weight = TreeController.CalculateProbabilityOfText(text+letter, _depth, _learnTree);

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
            _addedElementInLastCall = _addedElementInCurrentCall;

            TrimmTree();
        }

        

        public void TrimmTree()
        {
            if (_width.HasValue)
            {
                _addedElementInLastCall = _addedElementInLastCall.OrderBy(e => e.Weight).ToList();//.Take(_width.Value).ToList();
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
