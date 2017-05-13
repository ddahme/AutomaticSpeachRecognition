using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
  public class MarcowParseStrategy : AddStrategyInterface
  {
    private double _threshold = 0.3;
    private CompositeInterface _lernTree;
    private List<CompositeInterface> _addedElements;
    public List<CompositeInterface> AddedElements
    {
      get
      {
        return _addedElements;
      }
    }

    public bool IsUsingLernTree
    {
      get
      {
        return true;
      }
    }

    public MarcowParseStrategy(CompositeInterface lernTree)
    {
      _lernTree = lernTree;
    }

    public void Add(CompositeInterface parent, char elementIdent)
    {
            var lernElements = _lernTree.Elements.SelectMany(e => e.Elements);
            var possibleLetters = KeyController.GetKeyByName(elementIdent).Letters;
            var parentInLernTree = _lernTree.Elements.Where(e => e.Ident == parent.Ident).FirstOrDefault();
            foreach(var letter in possibleLetters)
            {
                var partentWeight = parentInLernTree.Weight;
                var letterWeight = parentInLernTree.Elements.Where(e => e.Ident == letter).Select(s => s.Weight).FirstOrDefault();
                var probability = letterWeight / partentWeight;
                if (probability >= _threshold)
                {
                    var element = new Element(elementIdent, parent);
                    element.Weight = probability;
                    parent.Add(element);
                }
            }
        }
  }
}
