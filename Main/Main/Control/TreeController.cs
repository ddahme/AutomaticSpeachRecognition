using Main.Control.AddStrategy;
using Main.Model;
using Main.Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Main.Control
{
    public class TreeController
    {
        private int? _learnTreeDepth;
        public int? learnTreeDepth
        {
            get
            {
                return _learnTreeDepth;
            }
            set
            {
                if (value.HasValue && value < 1)
                {
                    throw new ArgumentException("Depth of tree must be at least 1 or null");
                }
                _learnTreeDepth = value;
            }
        }
        private int? _parseTreeDepth = 3;
        public int? ParseTreeDepth
        {
            get
            {
                return _parseTreeDepth;
            }
            set
            {
                if (_learnTreeDepth.HasValue || value > _learnTreeDepth)
                {
                    throw new ArgumentException("Depth of parse-tree must be smaller then depth of learn-tree");
                }
                    _parseTreeDepth = value;
            }
        }
        private int? _parseTreeWidth;
        public int? ParseTreeWidth
        {
            get
            {
                return _parseTreeWidth;
            }
            set
            {
                _parseTreeWidth = value;
            }
        }
        private LearnTreeFactory _learnTreeFactory;
        private Parser _parser;
        private Tree _learnTree;
        public Tree LearnTree
        {
            get
            {
                return _learnTree;
            }
        }
        private List<string> _testFilePaths;
        public List<string> TestFilePaths
        {
            get
            {
                return _testFilePaths;
            }
            set
            {
                foreach(var path in value)
                {
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException("path to test-file is invalid");
                    }
                }
                _testFilePaths = value;
            }
        }
        private List<string> _learnFilePaths;
        public List<string> learnFilePaths
        {
            get
            {
                return _learnFilePaths;
            }
            set
            {
                foreach(var path in value)
                {
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException("path to learn-file is invalid");
                    }
                        
                }
                _learnFilePaths = value;
            }
        }

        public TreeController()
        {
            _learnFilePaths = new List<string>();
            _testFilePaths = new List<string>();
        }

        public void BuildLearnTree()
        {
            _learnTreeFactory = new LearnTreeFactory(_learnTreeDepth);

            foreach (var filePath in _learnFilePaths)
            {
                _learnTreeFactory.AddFile(filePath);
            }
            _learnTree = _learnTreeFactory.Tree;
        }

        public void SaveLearnTree(string path)
        {
            _learnTree.Name = Path.GetFileName(path);
            FileController.SaveTree(_learnTree, path);
        }

        public void RestoreLearnTree(string path)
        {
            _learnTree = FileController.RestoreTree(path);
            _learnTreeDepth = _learnTree.Depth;
        }

        public double TestLearnTree()
        {
            if (_learnTree == null)
            {
                throw new NullReferenceException("Unable to test learn-tree because it is null.");
            }
            if (_testFilePaths == null || _testFilePaths.Count == 0)
            {
                throw new NullReferenceException("Unable to test learn-tree because list of test-files is null or empty.");
            }

            var nWrongGuess = 0;
            var nLetter = 0;

            foreach (var filePath in _testFilePaths)
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        nLetter++;
                        var letter = (char)reader.Read();
                        var key = KeyController.GetKeyToLetter(letter);
                        var addedNotes = Parse(key).Cast<Element>();
                        if(letter!=addedNotes.OrderBy(e => e.Weight).LastOrDefault().Ident)
                        {
                            nWrongGuess++;
                        }
                    }
                }
            }
            return (double)nWrongGuess/(double)nLetter;
        }

        public List<Element> Parse(Key key)
        {
            if (_parser == null)
            {
                CreateResetParser();
            }
            _parser.Parse(key);
            return _parser.AddedElements;
        }

        public void CreateResetParser()
        {
            _parser = new Parser(_learnTree, _parseTreeDepth, _parseTreeWidth);
        }

        public string ConvertlearnTreeToString()
        {
            var result = TreeToString(_learnTreeFactory.Tree);
            return result;
        }

        public string ConvertParseTreeToString()
        {
            var result = TreeToString(_parser.Tree);
            return result;
        }

        public static string TreeToString(Tree tree)
        {
            var hasNextLevel = true;
            var result = new StringBuilder();
            var elementsOnLevel = new List<Element>();
            var elementsOnNextLevel = new List<Element>();

            elementsOnLevel.Add(tree);
            while (hasNextLevel)
            {
                elementsOnNextLevel = new List<Element>();
                foreach (var element in elementsOnLevel)
                {
                    result.Append(element.Ident);
                    elementsOnNextLevel.AddRange(element.Elements);
                }
                result.Append("\r\n");
                hasNextLevel = (elementsOnNextLevel.Count > 0);
                elementsOnLevel = elementsOnNextLevel;
            }
            return result.ToString();
        }

        public static double CalculateProbabilityOfText(string text, int depth, Tree learnTree)
        {
            var result = 0.0;

            if (learnTree == null)
            {
                throw new ArgumentNullException("learn-tree is null.");
            }
            if (depth > learnTree.Depth)
            {
                throw new ArgumentException("depth is bigger than lern-tree depth");
            }
            //get path in tree
            for (int n = 0; n < text.Length; n++)
            {
                var parent = (Element)learnTree;
                for (int k = n == 0 ? 0 : n < depth ? n : depth; k > 0; k--)
                {
                    var letter = text[n - k];
                    if (parent != null)
                    {
                        parent = parent.Elements.Where(e => e.Ident == letter).FirstOrDefault();
                    }
                }

                if (parent != null)
                {
                    var letter = text[n];
                    var element = parent.Elements.Where(e => e.Ident == letter).FirstOrDefault();
                    if (element != null)
                    {
                        result += -(Math.Log(element.Weight / parent.Weight));
                    }
                    else
                    {
                        result = 0.0;
                    }
                }
            }
            return result;
        }

        public static string CalculateText(Element parent, int? depth = null)
        {
            int i = 0;
            var text = string.Empty;
            while (((depth.HasValue && i < depth)||!depth.HasValue) && !parent.IsRoot)
            {
                text = parent.Ident + text;
                parent = parent.Parent;
                i++;
            }
            return text;
        }
    }
}
