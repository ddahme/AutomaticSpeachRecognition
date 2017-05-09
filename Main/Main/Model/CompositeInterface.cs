using System;
using System.Collections.Generic;

namespace Main.Model
{
    public interface CompositeInterface
    {
        bool IsRoot { get;}
        int Weight { get;}
        char Ident { get;}
        List<CompositeInterface> Elements { get;}
        CompositeInterface Parent { get;}
        void Add(CompositeInterface element);
        void IncreaseWeightByOne();
    }
}
