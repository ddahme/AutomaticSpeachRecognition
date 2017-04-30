using System;
using System.Collections.Generic;

namespace Main.Model
{
    interface Composite
    {
        bool IsRoot { get; }
        int Weight { get; }
        char Ident { get; }
        Lazy<List<Composite>> Elements { get; }
        Composite Parent { get; }
        void Add(Composite element);
        void IncreaseWeightByOne();
    }
}
