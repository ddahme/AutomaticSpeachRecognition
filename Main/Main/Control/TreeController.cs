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
    public class TreeController
    {
        //ToDo think about lerntree and strategy and stuff
        //build lern-Tree with treeFactory
        //Create AdvancedParseAddStrategy and give it lern-Tree
        //Create TreeFactory and give it AdvancedParseAddStrategy as parseTree
        //use parse input with parseTree
        //-> treeController is useless
        //maybe use treeController as "wrapper" for Unit Of Work
        //TreeController handels TreeFactories and stuff

        private Tree _lernTree;
        private List<string> _lernFilePaths;
        public List<string> LernFilePaths
        {
            get
            {
                return _lernFilePaths;
            }
            set
            {
                _lernFilePaths = value;
            }
        }

        public void BuildLernTree()
        {
            throw new NotImplementedException();
        }

        public void RestoreTree(string path)
        {
            var document = new XmlDocument();
            document.Load(path);
            var content = document.OuterXml;
            using (var reader = new StringReader(content))
            {
                var serializer = new XmlSerializer(typeof(Tree));
                using (var innerReader = new XmlTextReader(reader))
                {
                    _lernTree = (Tree)serializer.Deserialize(innerReader);
                    innerReader.Close();
                }
                reader.Close();
            }
        }

        public void SaveLernTree(string path)
        {
            var document = new XmlDocument();
            var serializer = new XmlSerializer(_lernTree.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, _lernTree);
                stream.Position = 0;
                document.Load(stream);
                document.Save(path);
                stream.Close();
            }
        }

        public double TestLernTree()
        {
            throw new NotImplementedException();
        }

        public string LernTreeToString()
        {
            throw new NotImplementedException();
            var result = string.Empty;
            return result;
        }
    }
}
