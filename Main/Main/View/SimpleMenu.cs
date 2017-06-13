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
            Console.WriteLine("Enter Letter");
            var menu = new SimpleMenu();
            var tree = menu._treeController.LearnTree;
            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (KeyController.IsValideLetter(key))
                {
                    text  += key;
                    var probablity = TreeController.CalculateProbabilityOfText(text, menu._depth, tree);
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
            _treeController.RestoreLearnTree(_treePath);

        }
    }
}
