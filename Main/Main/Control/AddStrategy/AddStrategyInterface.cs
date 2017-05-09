using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Control.AddStrategy
{
  public interface AddStrategyInterface
  {
    void Add(CompositeInterface parent, char elementIdent);

    List<CompositeInterface> AddedElements { get; }

    bool IsUsingLernTree { get; }
  }
}
