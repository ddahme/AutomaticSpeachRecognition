using Main.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Control
{
    class UnitOfWork
    {
        private enum _states { Init, ReadNextStateFromKeyboard, ReadKeyFromKeyboard, ReadKeyFromFile, ConvertLetterToKey, AddKeyToParseTree, ChangeParseTree, LoadLernTree, BuildLernTree, SaveLernTree, TestLernTree, PrintTree, Error};
        private List<_states> _lastStates;
        private KeyController _keyController;
        private TreeController _treeController;
        private TreeFactory _lernTreeFactory;
        private Tree _lernTree;


        public static void DoWork(string[] args)
        {
            var worker = new UnitOfWork();
        }

        private UnitOfWork()
        {
            Init();
        }

        private void Init()
        {
            _lastStates = new List<_states>();
            _lastStates.Add(_states.Init);
            _keyController = new KeyController();
            _treeController = new TreeController();
        }

        private void ReadNextStateFromKeyboard()
        {
            _lastStates.Add(_states.ReadNextStateFromKeyboard);
            var isValidInput = false;
            string input; 

            Console.WriteLine("Please select next state.");
            Console.WriteLine("[L]oad lern-tree");
            Console.WriteLine("[B]uild lern-tree");
            Console.WriteLine("[S]ave lern-tree");
            Console.WriteLine("[T]est lern-tree");
            Console.WriteLine("[P]rint tree");
            Console.WriteLine("parse key for [K]eyboard");
            Console.WriteLine("parse key from [F]ile");
            Console.WriteLine("[C]onvert letter to key and parse it");
            while (!isValidInput)
            {
                input = Console.ReadLine().ToUpper();
                switch (input)
                {
                    case "L":
                        LoadLernTree();
                        break;
                    case "B":
                        BuildLernTree();
                        break;
                    case "S":
                        SaveLernTree();
                        break;
                    case "T":
                        TestLernTree();
                        break;
                    case "P":
                        PrintTree();
                        break;
                    case "K":
                        ReadKeyFormKeyboard();
                        break;
                    case "F":
                        ReadKeyFromFile();
                        break;
                    case "C":
                        ConvertLetterToKey();
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        private void AddKeyToParseTree()
        {
            _lastStates.Add(_states.AddKeyToParseTree);

        }

        private void LoadLernTree()
        {
            _lastStates.Add(_states.LoadLernTree);
            Console.WriteLine("Please enter path to tree to restore.");
            var path = Console.ReadLine();
            try
            {
                _treeController.RestoreTree(path);
            }
            catch(Exception exception)
            {
                Error(exception);
            }
            
        }

        private void BuildLernTree()
        {
            _lastStates.Add(_states.BuildLernTree);
            string[] input;
            var filePaths = new List<String>();
            Console.WriteLine("Please select files to build lern-tree");
            Console.WriteLine("You can select more by seperate them with a , like A,H");
            Console.WriteLine("Adventures of [H]uckleberry Finn (606,623 letters)");
            Console.WriteLine("[A]lice adventures in Wonderland (167,515 letters)");
            Console.WriteLine("[p]ride and prejudice (717,571 letters)");
            Console.WriteLine("the adventures of [S]herlock Holmes (594,915 letters)");
            Console.WriteLine("the [j]ungle book (298,775 letters)");

            input = Console.ReadLine().Split(',');

            Console.WriteLine("You selected:");
            foreach (var letter in input)
            {
                var fileName = string.Empty;
                switch (letter.ToUpper())
                {
                    case "H":
                        fileName = "adventures_of_huckleberry_finn.txt";
                        break;
                    case "A":
                        fileName = "alices_adventures_in_wonderland.txt";
                        break;
                    case "P":
                        fileName = "pride_and_prejudice.txt";
                        break;
                    case "S":
                        fileName = "the_adevntures_of_sherlock_holmes.txt";
                        break;
                    case "J":
                        fileName = "the_jungle_book.txt";
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine(fileName);
                    filePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName));
                }
            }
            _treeController.LernFilePaths = filePaths;
            try
            {
                _treeController.BuildLernTree();
            }
            catch(Exception exception)
            {
                Error(exception);
            }
        }

        private void SaveLernTree()
        {
            _lastStates.Add(_states.SaveLernTree);
            Console.WriteLine("Please enter path to save tree");
            var path = Console.ReadLine();
            try
            {
                _treeController.SaveLernTree(path);
            }
            catch(Exception exception)
            {
                Error(exception);
            }
        }

        private void TestLernTree()
        {
            try
            {
                _treeController.TestLernTree();
            }catch(Exception exception)
            {
                Error(exception);
            }
        }

        private void PrintTree()
        {

        }

        private void ReadKeyFormKeyboard()
        {
            _lastStates.Add(_states.ReadKeyFromKeyboard);
        }

        private void ReadKeyFromFile()
        {
            _lastStates.Add(_states.ReadKeyFromFile);
        }

        private void ConvertLetterToKey()
        {
            
        }

        private void Error(Exception exception)
        {
            _lastStates.Add(_states.Error);
            Console.WriteLine("An {errorName} apperes.", exception.GetType());
            Console.WriteLine("{errorDiscription}",exception.Message);
            Console.WriteLine("List of last states:");
            foreach(var state in _lastStates)
            {
                Console.WriteLine("-{state}", state.ToString());
            }
        }
    }
}
