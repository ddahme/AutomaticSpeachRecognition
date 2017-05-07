﻿using Main.Model;
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

        public double TestTree()
        {
            throw new NotImplementedException();
        }
    }
}