using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
  public class ProbabilityValueElement : CompositeInterface
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
    private double _probalitityValue;
    public double ProbalitityValue
    {
      get
      {
        return _probalitityValue;
      }
            set
            {
                _probalitityValue = value;
            }
    }

    public ProbabilityValueElement(char ident, CompositeInterface parent, double probalitityValue)
    {
            _ident = ident;
            _parent = parent;
            _probalitityValue = probalitityValue;
            _elements = new List<CompositeInterface>();
      
            
    }

    public void Add(CompositeInterface element)
    {
      _elements.Add(element);
    }

        public void IncreaseWeightByOne()
        {
            throw new NotImplementedException();
        }
    }
}
