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
        //ToDo think about lerntree and strategy and stuff
        //build lern-Tree with treeFactory
        //Create AdvancedParseAddStrategy and give it lern-Tree
        //Create TreeFactory and give it AdvancedParseAddStrategy as parseTree
        //use parse input with parseTree
        //-> treeController is useless
        //maybe use treeController as "wrapper" for Unit Of Work
        //TreeController handels TreeFactories and stuff
        private List<Type> _addStrategies;
        //public enum AddStrategies {Undefined, SimpleParseStrategy, SimpleLernStrategy, MarcowParseStrategy};
        public List<Type> AddStrategies
        {
            get
            {
                return _addStrategies;
            }
        }
        private Type _lernStrategy;
        public Type LernStrategy
        {
            get
            {
                return _lernStrategy;
            }
            set//ToDo: throw exception
            {
                if (_addStrategies.Contains(value))
                {
                    _lernStrategy = value;
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
            set//ToDo: throw exception
            {
                if (_addStrategies.Contains(value))
                {
                    _parseStrategy = value;
                }
            }
        }
        private int? _lernTreeDepth;
        public int? LernTreeDepth
        {
            get
            {
                return _lernTreeDepth;
            }
            set//ToDo: throw exception
            {
                _lernTreeDepth = value;
            }
        }
        private int? _parseTreeDepth;
        public int? ParseTreeDepth
        {
            get
            {
                return _parseTreeDepth;
            }
            set//ToDo: throw exception
            {
                //parse-depth must be smaller than lern-depth
                if (_lernTreeDepth.HasValue && value < _lernTreeDepth)
                {
                    _parseTreeDepth = value;
                }
            }
        }
        private TreeFactory _lernTreeFactory;
        private TreeFactory _parseTreeFactory;
        private Tree _lernTree;
        private List<string> _testFilePaths;
        public List<string> TestFilePaths
        {
            get
            {
                return _testFilePaths;
            }
            set//ToDo: throw exception
            {
                _testFilePaths = value;
            }
        }
        private List<string> _lernFilePaths;
        public List<string> LernFilePaths
        {
            get
            {
                return _lernFilePaths;
            }
            set//ToDo: throw exception
            {
                _lernFilePaths = value;
            }
        }

        public TreeController()
        {
            _addStrategies = new List<Type>()
            {
                typeof(SimpleLernStrategy),
                typeof(SimpleParseStrategy),
                typeof(MarcowParseStrategy)
            };
        }

        public void BuildLernTree()
        {
            _lernTreeFactory = new TreeFactory(_lernStrategy, _lernTreeDepth);

            foreach (var filePath in _lernFilePaths)
            {
                _lernTreeFactory.CreateTreeOutOfTextFile(filePath);
            }
            _lernTree = _lernTreeFactory.Tree;
        }

        public void SaveLernTree(string path)
        {
            _lernTree.Name = Path.GetFileName(path);
            SaveTree(_lernTree, path);
        }

        public void RestoreLernTree(string path)
        {
            _lernTree = RestoreTree(path);
        }

        public double TestLernTree()
        {
            if (_lernTree == null)
            {
                throw new NullReferenceException("Unable to test lern-tree because it is null.");
            }
            if (_testFilePaths == null || _testFilePaths.Count == 0)
            {
                throw new NullReferenceException("Unable to test lern-tree because list of test-files is null or empty.");
            }
            if (!_parseStrategy.IsUsingLernTree)
            {
                throw new Exception("Unable to test lern-tree because strategy do not use it");
            }

            var result = 1.0;
            char letter;
            char keyIdent;
            foreach (var filePath in _testFilePaths)
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        letter = (char)reader.Read();
                        keyIdent = KeyController.GetKeyToLetter(letter).Name;
                        var addedNotes = ParseKey(keyIdent).Cast<Element>();
                        var probalitity = addedNotes.Where(e => e.Ident == letter).FirstOrDefault().Weight;
                        result *= probalitity;
                    }
                }
            }
            return result;
        }

        public List<Element> ParseKey(char ident)
        {
            if (_parseTreeFactory == null)
            {
                _parseTreeFactory = new TreeFactory(_parseStrategy);
            }
            _parseTreeFactory.Add(ident);
            return _parseTreeFactory.AddedElements;
        }

        public List<Element> ParseFile(string path)
        {
            if (_parseTreeFactory == null)
            {
                _parseTreeFactory = new TreeFactory(_parseStrategy);
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

        public List<KeyValuePair<string,double>> GetBestResults(uint numberOfResults)
        {
            throw new NotImplementedException();
            var result = new List<KeyValuePair<string, double>>();
            return result;
        } 

        public string ConvertLernTreeToString()
        {
            var result = TreeToString(_lernTreeFactory.Tree);
            return result;
        }

        public string ConvertParseTreeToString()
        {
            var result = TreeToString(_parseTreeFactory.Tree);
            return result;
        }

        public List<string> PossibleResults()
        {
            var result = new List<string>();
            foreach (var leaf in _parseTreeFactory.AddedElements)
            {
                var path = new List<char>();
                var element = leaf;
                while (!element.IsRoot)
                {
                    path.Add(element.Ident);
                    element = element.Parent;
                }
                path.Reverse();
                result.Add(new String(path.ToArray()));
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
                ConvertDTOToElement(subElement, result);
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
                ConvertDTOToElement(subElement, result);
            }
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
