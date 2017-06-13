using Main.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.View
{
    class SimpleParseMenu
    {
        private TreeController _treeController;
        private string _treePath = "Data\\learnTree.xml";
        private int _depth = 5;
        private int _width = 10;

        public static void DoWork()
        {
            Console.WriteLine("Enter Key[0-9,*,#]");
            var menu = new SimpleParseMenu();
            while (true)
            {
                var keyIdent = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (KeyController.IsValideKey(keyIdent))
                {
                    var key = KeyController.GetKeyByName(keyIdent);
                    var results = menu._treeController.Parse(key);
                    foreach(var result in results)
                    {
                        Console.WriteLine("{0}: {1}", TreeController.CalculateText(result), result.Weight);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid key");
                }
            }
        }

        private SimpleParseMenu()
        {
            _treeController = new TreeController();
            _treeController.ParseTreeDepth = _depth;
            _treeController.ParseTreeWidth = _width;
            _treeController.RestoreLearnTree(_treePath);

        }
    }
}
