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
        List<Element> AddedElements { get; }

        bool IsUsinglearnTree { get; }

        void Add(Element parent, char elementIdent);
    }
}
