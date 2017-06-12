using Main.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.View
{
    class SimpleMenu
    {
        private TreeController _treeController;
        private string _treePath = "Data\\learnTree.xml";
        private int _depth = 3;

        public static void DoWork()
        {
            var text = string.Empty;
            var stringBuilder = new StringBuilder();
            Console.WriteLine("Enter Letter");
            var menu = new SimpleMenu();
            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (KeyController.IsValideLetter(key))
                {
                    text  += key;
                    var probablity = menu._treeController.CalculateProbabilityOfText(text, menu._depth);
                    Console.WriteLine("{0}: {1}", text, probablity);
                }
                else
                {
                    Console.WriteLine("Invalid key");
                }
            }
        }

        private SimpleMenu()
        {
            _treeController = new TreeController();
            //var path = Path.Combine(Path.GetFullPath(@"..\..\"), _treePath);
            _treeController.RestorelearnTree(_treePath);

        }
    }
}
