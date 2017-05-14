using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.Control.AddStrategy
{
  public class SimpleLernStrategy : AddStrategyInterface
  {
    private List<Element> _addedElements;
    public List<Element> AddedElements
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

    public void Add(Element parent, char elementIdent)
    {
      Element node;

      //init List of added Elements
      _addedElements = new List<Element>();
      //check if element allready exists
      node = parent.Elements.Find(e => e.Ident == elementIdent);
      if (node == null)
      {
                //create new element
                node = new Element()
                {
                    Ident = elementIdent,
                    Parent = parent,
                    IsRoot = false,
                    Elements = new List<Element>(),
                    Weight = 0
                };
        //add it to parent
        parent.Elements.Add(node);
      }
      //increase weight of path
      IncreseWeightRecursiv(node);
      _addedElements.Add(node);
    }

    private void IncreseWeightRecursiv(Element composite)
    {
      composite.Weight++;
      if (!composite.IsRoot)
      {
        IncreseWeightRecursiv(composite.Parent);
      }
    }
        public override string ToString()
        {
            return "SimpleLernStrategy";
        }
    }
}
