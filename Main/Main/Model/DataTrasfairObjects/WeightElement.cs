using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model.DataTrasfairObjects
{
    class WeightElement : CompositeInterface
    {
        public List<CompositeInterface> Elements
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public char Ident
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public bool IsRoot
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public CompositeInterface Parent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public void Add(CompositeInterface element)
        {
            throw new NotImplementedException();
        }

        public void IncreaseWeightByOne()
        {
            throw new NotImplementedException();
        }
    }
}
