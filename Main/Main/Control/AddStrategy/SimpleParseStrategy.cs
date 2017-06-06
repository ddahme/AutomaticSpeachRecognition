using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
  /// <summary>
  /// Add-function for live-parsing
  /// just add all possible letters for the key to the list of perents elements.
  /// </summary>
  public class SimpleParseStrategy : AddStrategyInterface
  {
    private List<Element> _addedElements;
    public List<Element> AddedElements
    {
      get
      {
        return _addedElements;
      }
    }

    public bool IsUsinglearnTree
    {
      get
      {
        return false;
      }
    }

    public void Add(Element parent, char elementIdent)
    {
      _addedElements = new List<Element>();
      var key = KeyController.GetKeyByName(elementIdent);
      foreach (var letter in key.Letters)
      {
                var element = new Element()
                {
                    Ident = letter,
                    Parent = parent,
                    IsRoot = false,
                    Elements = new List<Element>(),
                    Weight = 0
        };
        parent.Elements.Add(element);
        _addedElements.Add(element);
      }
    }

        public override string ToString()
        {
            return "SimpleParseStrategy";
        }
    }
}
