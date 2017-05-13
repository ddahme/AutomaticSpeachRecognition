using System;
using System.Collections.Generic;

namespace Main.Model
{
    public interface CompositeInterface
    {
        bool IsRoot { get;}
        double Weight { get; set;}

        //ToDo: add 'wahrscheinlichkeit' as double 
        char Ident { get;}
        List<CompositeInterface> Elements { get;}
        CompositeInterface Parent { get;}
        void Add(CompositeInterface element);
        void IncreaseWeightByOne();
    }
}
