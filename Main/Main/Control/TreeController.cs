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

    private List<AddStrategyInterface> _addStrategies;
    private AddStrategyInterface _lernStrategy;
    private AddStrategyInterface _parseStrategy;
    private int _lernTreeDepth = 7;
    private int _parseTreeDepth;
    private TreeFactory _lernTreeFactory;
    private TreeFactory _parseTreeFactory;
    private List<string> _testFilePaths;
    private List<string> _lernFilePaths;
    public List<string> LernFilePaths
    {
      get
      {
        return _lernFilePaths;
      }
      set
      {
        _lernFilePaths = value;
      }
    }

    public TreeController()
    {
      _lernStrategy = new SimpleLernAddStrategy();
      _parseStrategy = new SimpleParseAddStrategy();
    }

    public void BuildLernTree()
    {
      var lernAddStrategy = new SimpleLernAddStrategy();
      _lernTreeFactory = new TreeFactory(lernAddStrategy, _lernTreeDepth);

      foreach (var filePath in _lernFilePaths)
      {
        _lernTreeFactory.CreateTreeOutOfTextFile(filePath);
      }
      //_lernTree = _lernTreeFactory.Tree;
    }

    public void RestoreTree(string path)
    {
      var document = new XmlDocument();
      document.Load(path);
      var content = document.OuterXml;
      using (var reader = new StringReader(content))
      {
        var serializer = new XmlSerializer(typeof(Tree));
        using (var innerReader = new XmlTextReader(reader))
        {
          //_lernTree = (Tree)serializer.Deserialize(innerReader);
          innerReader.Close();
        }
        reader.Close();
      }
    }

    public void SaveLernTree(string path)
    {
      if (_lernTreeFactory.Tree == null)
      {
        throw new NullReferenceException("Unable to save lern-tree because it is null.");
      }
      //ToDo: fix by using DTOs
      var document = new XmlDocument();
      //var serializer = new XmlSerializer(_lernTree.GetType());
      using (var stream = new MemoryStream())
      {
        //serializer.Serialize(stream, _lernTree);
        stream.Position = 0;
        document.Load(stream);
        document.Save(path);
        stream.Close();
      }
    }

    public double TestLernTree()
    {
      if (_lernTreeFactory.Tree == null)
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
            var addedNotes = ParseKey(keyIdent);
            var correctNodeWeight = addedNotes.Find(n => n.Ident == letter).Weight;
            var otherNodeWeigt = addedNotes.FindAll(n => n.Ident != letter).Sum(n => n.Weight);
            if (otherNodeWeigt != 0)
            {
              result *= (correctNodeWeight / otherNodeWeigt);
            }
          }
        }
      }
      return result;
    }

    public List<CompositeInterface> ParseKey(char ident)
    {
      if (_parseTreeFactory == null)
      {
        _parseTreeFactory = new TreeFactory(_parseStrategy);
      }
      _parseTreeFactory.Add(ident);
      return _parseTreeFactory.AddedElements;
    }

    public string LernTreeToString()
    {
      var result = TreeToString(_lernTreeFactory.Tree);
      return result;
    }

    public string ParseTreeToString()
    {
      var result = TreeToString(_parseTreeFactory.Tree);
      return result;
    }

    private string TreeToString(Tree tree)
    {
      var hasNextLevel = true;
      var result = new StringBuilder();
      var elementsOnLevel = new List<CompositeInterface>();
      var elementsOnNextLevel = new List<CompositeInterface>();

      elementsOnLevel.Add(tree);
      while (hasNextLevel)
      {
        elementsOnNextLevel = new List<CompositeInterface>();
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
