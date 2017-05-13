using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
  public class WeightElement : CompositeInterface
  {
    private List<CompositeInterface> _elements;
    public List<CompositeInterface> Elements
    {
      get
      {
        return _elements;
      }
    }
    private char _ident;
    public char Ident
    {
      get
      {
        return _ident;
      }
    }

    public bool IsRoot
    {
      get
      {
        return false;
      }
    }
    private CompositeInterface _parent;
    public CompositeInterface Parent
    {
      get
      {
        return _parent;
      }
    }
    private double _weight;
    public double Weight
    {
      get
      {
        return _weight;
      }
            set
            {
                _weight = value;
            }
    }

    public WeightElement(char ident, CompositeInterface parent)
    {
      _parent = parent;
      _elements = new List<CompositeInterface>();
      _ident = ident;
      _weight = 0;
    }

    public void Add(CompositeInterface element)
    {
      _elements.Add(element);
    }

    public void IncreaseWeightByOne()
    {
      _weight++;
    }
  }
}
