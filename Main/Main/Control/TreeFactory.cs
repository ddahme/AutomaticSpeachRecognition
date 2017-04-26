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
    class TreeFactory
    {

        public Composite CreateTree(string ident, int depth, string trainingsFilePath)
        {
            var tree = Parse(ident, depth, trainingsFilePath);
            return tree;
        }

        

        public Tree RestoreTree(string path)
        {
            Tree tree;
            var document = new XmlDocument();
            document.Load(path);
            var content = document.OuterXml;
            using (var reader = new StringReader(content))
            {
                var serializer = new XmlSerializer(typeof(Tree));
                using (var innerReader = new XmlTextReader(reader))
                {
                    tree = (Tree)serializer.Deserialize(innerReader);
                    innerReader.Close();
                }
                reader.Close();
            }
            return tree;
        }

        public void SaveTree(Tree tree, string path)
        {
            var document = new XmlDocument();
            var serializer = new XmlSerializer(tree.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, tree);
                stream.Position = 0;
                document.Load(stream);
                document.Save(path);
                stream.Close();
            }
        }

        private Composite Parse(string ident, int depth, string trainingsFilePath)
        {
            char letter;
            Composite root = new Tree(ident, depth, ' ');
            Composite parent = root;
            Composite me;
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
                        //check if char allready exist on level
                        me = parent.Elements.Value.Find(e => e.Ident == letter);
                        if (me == null)
                        {
                            //create new node
                            me = new Element(letter, parent);
                        }
                        //Increse Path
                        IncreseWeightRecursiv(me);
                        //traverse to levels of tree
                        if (((++level) % depth) != 0)
                        {
                            parent = me;
                        }
                        else
                        {
                            parent = root;
                        }
                    }
                }
            }
            return parent;
        }

        private void IncreseWeightRecursiv(Composite composite)
        {
            composite.IncreaseWeightByOne();
            if (!composite.IsRoot)
            {
                IncreseWeightRecursiv(composite.Parent);
            }
        }
    }
}
