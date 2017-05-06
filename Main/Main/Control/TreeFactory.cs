using Main.Control.AddStrategy;
using Main.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Main.Control
{
    public class TreeFactory
    {
        private AddStrategyInterface _addStrategy;

        private Tree _tree;
        public Tree Tree
        {
            get
            {
                return _tree;
            }
        }

        private int _depth;
        public int Depth
        {
            get
            {
                return Depth;
            }
            set
            {
                //check if tree is under construction
                if (_tree == null)
                {
                    _depth = value;
                }
            }
        }

        public TreeFactory(AddStrategyInterface addStrategy)
        {
            _addStrategy = addStrategy;
        }



        public CompositeInterface CreateTreeOutOfTextFile(string trainingsFilePath)
        {
            char letter;
            CompositeInterface root = new Tree(_depth);
            CompositeInterface parent = root;
            List<CompositeInterface> addedElements;
            int level = 0;
            //read text
            var fs = new FileStream(trainingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    letter = (char)reader.Read();
                    //check if char is valide
                    if (Keyboard.IsValideLetter(letter))
                    {
                        _addStrategy.Add(parent, letter);
                        addedElements = _addStrategy.AddedElements;
                        
                        if (_depth > 0)
                        {
                            if (((++level) % _depth) == 0)
                            {
                                parent = root;
                            }
                        }
                    }
                }
            }
            return parent;
        }
    }
}
