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

        public Tree CreateTreeOutOfTextFile(string trainingsFilePath)
        {
            char letter;
            CompositeInterface root = new Tree(_depth);
            List<CompositeInterface> addedElements = new List<CompositeInterface>();
            List<CompositeInterface> addedElementsOfParents = new List<CompositeInterface>();
            addedElementsOfParents.Add(root);
            int level = 0;

            //read text
            var fs = new FileStream(trainingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    //clear list of added elements
                    addedElements.Clear();
                    //read letter form text
                    letter = (char)reader.Read();
                    //for each added element in the last iteration
                    foreach (var parent in addedElementsOfParents)
                    {
                        //call the add-strategy
                        _addStrategy.Add(parent, letter);
                        //add add the result to the list of added elementrs for the next level
                        addedElements.AddRange(_addStrategy.AddedElements);
                    }
                    //if the trees depth is restricted by _depth, check if this level is to deep
                    if (_depth > 0 && ((++level) % _depth) == 0)
                    {
                        //if it is use the root as last level
                        addedElementsOfParents.Clear();
                        addedElementsOfParents.Add(root);
                    }
                    else
                    {
                        //if it is not just use the added elements of this iteratrion for the next one and go ahead
                        addedElementsOfParents = addedElements;
                    }
                }
            }
            _tree = (Tree)root;
            return _tree;
        }
    }
}
