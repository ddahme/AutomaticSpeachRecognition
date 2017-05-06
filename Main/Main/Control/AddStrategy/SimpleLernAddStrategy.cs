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
        public CompositeInterface Add(CompositeInterface parent, char elementIdent)
        {
            CompositeInterface node;
            //check if element allready exists
            node = parent.Elements.Value.Find(e => e.Ident == elementIdent);
            if (node == null)
            {
                //create new element
                node = new Element(elementIdent, parent);
                //add it to parent
                parent.Elements.Value.Add(node);
            }
            //increase weight of path
            IncreseWeightRecursiv(node);
            return node;
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
