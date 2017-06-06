using Main.Control.AddStrategy;
using Main.Model;
using Main.Model.DTO;
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
    public class TreeController
    {
        //ToDo think about learntree and strategy and stuff
        //build learn-Tree with treeFactory
        //Create AdvancedParseAddStrategy and give it learn-Tree
        //Create TreeFactory and give it AdvancedParseAddStrategy as parseTree
        //use parse input with parseTree
        //-> treeController is useless
        //maybe use treeController as "wrapper" for Unit Of Work
        //TreeController handels TreeFactories and stuff
        private List<Type> _addStrategies;
        public List<Type> AddStrategies
        {
            get
            {
                return _addStrategies;
            }
        }
        private Type _learnStrategy;
        public Type learnStrategy
        {
            get
            {
                return _learnStrategy;
            }
            set
            {
                if (_addStrategies.Contains(value))
                {
                    _learnStrategy = value;
                }
            }
        }
        private Type _parseStrategy;
        public Type ParseStrategy
        {
            get
            {
                return _parseStrategy;
            }
            set
            {
                if (_addStrategies.Contains(value))
                {
                    _parseStrategy = value;
                }
            }
        }
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
        private int? _parseTreeDepth;
        public int? ParseTreeDepth
        {
            get
            {
                return _parseTreeDepth;
            }
            set
            {
                if (!_learnTreeDepth.HasValue || value > _learnTreeDepth)
                {
                    throw new ArgumentException("Depth of parse-tree must be smaller then depth of learn-tree");
                }
                    _parseTreeDepth = value;
            }
        }
        private learnTreeFactory _learnTreeFactory;
        private TreeFactory _parseTreeFactory;
        private Tree _learnTree;
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
            _addStrategies = new List<Type>()
            {
                typeof(SimplelearnStrategy),
                typeof(SimpleParseStrategy),
                typeof(MarcowParseStrategy),
                typeof(P5)
            };
            _learnFilePaths = new List<string>();
            _testFilePaths = new List<string>();
        }

        public void BuildlearnTree()
        {
            var strategy = CreateInstaceOfStategy(_learnStrategy);
            _learnTreeFactory = new learnTreeFactory(strategy, _learnTreeDepth);

            foreach (var filePath in _learnFilePaths)
            {
                _learnTreeFactory.AddFile(filePath);
            }
            _learnTree = _learnTreeFactory.Tree;
        }

        public void SavelearnTree(string path)
        {
            _learnTree.Name = Path.GetFileName(path);
            SaveTree(_learnTree, path);
        }

        public void RestorelearnTree(string path)
        {
            _learnTree = RestoreTree(path);
        }

        public double TestlearnTree()
        {
            if (_learnTree == null)
            {
                throw new NullReferenceException("Unable to test learn-tree because it is null.");
            }
            if (_testFilePaths == null || _testFilePaths.Count == 0)
            {
                throw new NullReferenceException("Unable to test learn-tree because list of test-files is null or empty.");
            }
            var strategy = CreateInstaceOfStategy(_parseStrategy);
            if (!strategy.IsUsinglearnTree)
            {
                throw new Exception("Unable to test learn-tree because strategy do not use it");
            }

            var nWrongGuess = 0;
            var nLetter = 0;
            char letter;
            char keyIdent;
            foreach (var filePath in _testFilePaths)
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        nLetter++;
                        letter = (char)reader.Read();
                        keyIdent = KeyController.GetKeyToLetter(letter).Name;
                        var addedNotes = ParseKey(keyIdent).Cast<Element>();
                        if(letter!=addedNotes.OrderBy(e => e.Weight).LastOrDefault().Ident)
                        {
                            nWrongGuess++;
                        }
                    }
                }
            }
            return (double)nWrongGuess/(double)nLetter;
        }

        public List<Element> ParseKey(char ident)
        {
            if (_parseTreeFactory == null)
            {
                var strategy = CreateInstaceOfStategy(_parseStrategy);
                _parseTreeFactory = new TreeFactory(strategy);
            }
            _parseTreeFactory.Add(ident);
            return _parseTreeFactory.AddedElements;
        }

        public void ResetParseTree()
        {
            var strategy = CreateInstaceOfStategy(_parseStrategy);
            _parseTreeFactory = new TreeFactory(strategy);
        }

        private AddStrategyInterface CreateInstaceOfStategy(Type type)
        {
            if(type == typeof(SimplelearnStrategy) || type == typeof(SimpleParseStrategy))
            {
                var strategy = (AddStrategyInterface)Activator.CreateInstance(type);
                return strategy;
            }
            if(type == typeof(MarcowParseStrategy))
            {
                var strategy = (AddStrategyInterface)Activator.CreateInstance(type, _learnTree);
                return strategy;
            }
            if(type == typeof(P5))
            {
                var strategy = (AddStrategyInterface)Activator.CreateInstance(type, _learnTree, _parseTreeDepth);
                return strategy;
            }
            throw new ArgumentException("Unable to create an instance of type. Strategy is not supported.");

        }

        public List<Element> ParseFile(string path)
        {
            if (_parseTreeFactory == null)
            {
                var strategy = CreateInstaceOfStategy(_parseStrategy);
                _parseTreeFactory = new TreeFactory(strategy);
            }
            return _parseTreeFactory.AddedElements;
        }

        public double GetProbabilityToText(string text)
        {
            foreach(var letter in text)
            {
                var keyIdent = KeyController.GetKeyToLetter(letter).Name;
                _parseTreeFactory.Add(keyIdent);
            }
            var result = _parseTreeFactory.AddedElements.Where(w => w.Ident == text.Last()).FirstOrDefault().Weight;
            return result;
        }

        public List<KeyValuePair<string, double>> GetBestResults(uint? numberOfResults)
        {
            var result = new List<KeyValuePair<string, double>>();
            var nResults = (uint)_parseTreeFactory.AddedElements.Count;
            if (numberOfResults.HasValue && numberOfResults.Value < nResults)
            {
                nResults = numberOfResults.Value;
            }
            var sortedAddedElements = _parseTreeFactory.AddedElements.OrderBy(e => e.Weight);
            foreach(var leaf in sortedAddedElements)
            {
                if(nResults == 0) {
                    break;
                }else
                {
                    nResults--;
                }
                var path = new List<char>();
                var element = leaf;
                while (!element.IsRoot)
                {
                    path.Add(element.Ident);
                    element = element.Parent;
                }
                path.Reverse();
                var key = new String(path.ToArray());
                var value = leaf.Weight;
                var subResult = new KeyValuePair<string, double>(key, value);
                result.Add(subResult);
            }

            return result;
        }

        private Tree RestoreTree(string path)
        {
            var document = new XmlDocument();
            TreeDTO result;
            document.Load(path);
            var content = document.OuterXml;
            using (var reader = new StringReader(content))
            {
                var serializer = new XmlSerializer(typeof(TreeDTO));
                using (var innerReader = new XmlTextReader(reader))
                {
                    result = (TreeDTO)serializer.Deserialize(innerReader);
                    innerReader.Close();
                }
                reader.Close();
            }
            return ConvertDTOToTree(result);
        }

        private void SaveTree(Tree tree, string path)
        {
            if (tree == null)
            {
                throw new NullReferenceException("Unable to save tree because it is null.");
            }
            var dto = CovertTreeToDTO(tree);
            var document = new XmlDocument();
            var serializer = new XmlSerializer(dto.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, dto);
                stream.Position = 0;
                document.Load(stream);
                document.Save(path);
                stream.Close();
            }
        }

        private TreeDTO CovertTreeToDTO(Tree tree)
        {
            var result = new TreeDTO
            {
                Depth = tree.Depth,
                Ident = tree.Ident,
                Name = tree.Name,
                Weigth = tree.Weight,
                Elements = new List<ElementDTO>()
            };
            foreach (var subElements in tree.Elements)
            {
                var elementDTO = ConvertElementToDTO(subElements);
                result.Elements.Add(elementDTO);
            }
            return result;
        }

        private Tree ConvertDTOToTree(TreeDTO dto)
        {
            var result = new Tree
            {
                Name = dto.Name,
                Ident = dto.Ident,
                IsRoot = true,
                Weight = dto.Weigth,
                Depth = dto.Depth,
                Elements = new List<Element>()
            };
            foreach (var subElement in dto.Elements)
            {
                var element = ConvertDTOToElement(subElement, result);
                result.Elements.Add(element);
            }
            return result;
        }

        private ElementDTO ConvertElementToDTO(Element element)
        {
            var result = new ElementDTO
            {
                Ident = element.Ident,
                Weigth = element.Weight,
                Elements = new List<ElementDTO>()
            };
            foreach (var subElements in element.Elements)
            {
                var elementDTO = ConvertElementToDTO(subElements);
                result.Elements.Add(elementDTO);
            }
            return result;
        }

        private Element ConvertDTOToElement(ElementDTO dto, Element parent)
        {
            var result = new Element
            {
                Ident = dto.Ident,
                IsRoot = false,
                Weight = dto.Weigth,
                Parent = parent,
                Elements = new List<Element>()
            };
            foreach (var subElement in dto.Elements)
            {
                result.Elements.Add(ConvertDTOToElement(subElement, result));
            }
            return result;
        }

        public string ConvertlearnTreeToString()
        {
            var result = TreeToString(_learnTreeFactory.Tree);
            return result;
        }

        public string ConvertParseTreeToString()
        {
            var result = TreeToString(_parseTreeFactory.Tree);
            return result;
        }

        private string TreeToString(Tree tree)
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
    }
}
