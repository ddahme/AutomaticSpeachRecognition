using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
  class SimpleLernAddStrategy : AddStrategyInterface
  {
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
        return false;
      }
    }

    public void Add(CompositeInterface parent, char elementIdent)
    {
      CompositeInterface node;

      //init List of added Elements
      _addedElements = new List<CompositeInterface>();
      //check if element allready exists
      node = parent.Elements.Find(e => e.Ident == elementIdent);
      if (node == null)
      {
        //create new element
        node = new Element(elementIdent, parent);
        //add it to parent
        parent.Elements.Add(node);
      }
      //increase weight of path
      IncreseWeightRecursiv(node);
      _addedElements.Add(node);
    }

    private void IncreseWeightRecursiv(CompositeInterface composite)
    {
      composite.IncreaseWeightByOne();
      if (!composite.IsRoot)
      {
        IncreseWeightRecursiv(composite.Parent);
      }
    }
  }
}
