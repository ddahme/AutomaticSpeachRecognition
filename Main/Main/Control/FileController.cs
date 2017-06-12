using Main.Model;
using Main.Model.DTO;
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
    class FileController
    {
        public static void SaveTree(Tree tree, string path)
        {
            if (tree == null)
            {
                throw new NullReferenceException("Unable to save tree because it is null.");
            }
            var dto = CovertTreeToDTO(tree);
            var document = new XmlDocument();
            var serializer = new XmlSerializer(dto.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, dto);
                stream.Position = 0;
                document.Load(stream);
                document.Save(path);
                stream.Close();
            }
        }

        public static Tree RestoreTree(string path)
        {
            var document = new XmlDocument();
            TreeDTO result;
            document.Load(path);
            var content = document.OuterXml;
            using (var reader = new StringReader(content))
            {
                var serializer = new XmlSerializer(typeof(TreeDTO));
                using (var innerReader = new XmlTextReader(reader))
                {
                    result = (TreeDTO)serializer.Deserialize(innerReader);
                    innerReader.Close();
                }
                reader.Close();
            }
            return ConvertDTOToTree(result);
        }

        private static TreeDTO CovertTreeToDTO(Tree tree)
        {
            var result = new TreeDTO
            {
                Depth = tree.Depth,
                Ident = tree.Ident,
                Name = tree.Name,
                Weigth = tree.Weight,
                Elements = new List<ElementDTO>()
            };
            foreach (var subElements in tree.Elements)
            {
                var elementDTO = ConvertElementToDTO(subElements);
                result.Elements.Add(elementDTO);
            }
            return result;
        }

        private static Tree ConvertDTOToTree(TreeDTO dto)
        {
            var result = new Tree
            {
                Name = dto.Name,
                Ident = dto.Ident,
                IsRoot = true,
                Weight = dto.Weigth,
                Depth = dto.Depth,
                Elements = new List<Element>()
            };
            foreach (var subElement in dto.Elements)
            {
                var element = ConvertDTOToElement(subElement, result);
                result.Elements.Add(element);
            }
            return result;
        }

        private static ElementDTO ConvertElementToDTO(Element element)
        {
            var result = new ElementDTO
            {
                Ident = element.Ident,
                Weigth = element.Weight,
                Elements = new List<ElementDTO>()
            };
            foreach (var subElements in element.Elements)
            {
                var elementDTO = ConvertElementToDTO(subElements);
                result.Elements.Add(elementDTO);
            }
            return result;
        }

        private static Element ConvertDTOToElement(ElementDTO dto, Element parent)
        {
            var result = new Element
            {
                Ident = dto.Ident,
                IsRoot = false,
                Weight = dto.Weigth,
                Parent = parent,
                Elements = new List<Element>()
            };
            foreach (var subElement in dto.Elements)
            {
                result.Elements.Add(ConvertDTOToElement(subElement, result));
            }
            return result;
        }

    }
}
