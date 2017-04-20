using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    interface Composite
    {
        int Weight { get; }
        char Ident { get; }
        Lazy<List<Composite>> Elements { get; }
        void Add(Composite element);
        void IncreaseWeightByOne();
    }
}
