using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
  public class FancyParseStrategy : AddStrategyInterface
  {
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

    public FancyParseStrategy(CompositeInterface lernTree)
    {
      _lernTree = lernTree;
    }

    public void Add(CompositeInterface parent, char elementIdent)
    {
      throw new NotImplementedException();
    }
  }
}
