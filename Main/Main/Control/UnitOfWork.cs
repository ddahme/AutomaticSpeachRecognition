using Main.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Main.Control
{
    class UnitOfWork
    {
        private enum _states { Init, IsInMainMenu, IsInlearnMenu, FinishedLoadlearnTree, StartSavelearnTree, FinishedSavelearnTree, ReadKeyFromKeyboard, ReadKeyFromFile, ConvertLetterToKey, AddKeyToParseTree, ChangeParseTree, StartLoadlearnTree, BuildlearnTree, SavelearnTree, TestlearnTree, PrintTree, Error, IsInUtilMenu, IsInConfigMenu, IsInParseMenu, StartBuildlearnTree, FinischedBuildlearnTree, StartTestlearnTree, FinishedTestlearnTree, StartParseKey, FinishedParseKey, StartParseFile, FinishedParseFile, StartProbabilityOfLetter, FinishedProbabilityOfLetter, StartProbabilityOfText, FinishedProbabilityOfText, StartGetBestResults, FinishedGetBestResults, StartSetStrategy, FinishedSetStrategy, StartSetDepth, FinishedSetDepth, StartConvertMenu, FinishedConvertMenu, FinishedConvertLetterFileToKeyFile, StartConvertLetterFileToKeyFile, IsInDrawMenu };
        private List<_states> _lastStates;
        private _states _state
        {
            get
            {
                return _lastStates[_lastStates.Count];
            }
            set
            {
                _lastStates.Add(value);
            }
        }
        private TreeController _treeController;


        public static void DoWork(string[] args)
        {
            var worker = new UnitOfWork();
        }

        private UnitOfWork()
        {
            _lastStates = new List<_states>();
            _state = _states.Init;
            _treeController = new TreeController();
            //SetDefaultValues();
            MainMenu();
        }

        private void MainMenu()
        {
            bool isBreaking = false;
            _state = _states.IsInMainMenu;
            while (!isBreaking)
            {
                Console.WriteLine("<<<<Main-menu>>>>");
                Console.WriteLine("Please select:");
                Console.WriteLine("[L]earn");
                Console.WriteLine("[P]arse");
                Console.WriteLine("[C]onfig");
                Console.WriteLine("[U]til");
                Console.WriteLine("Esc to get back");

                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        learnMenu();
                        break;
                    case (ConsoleKey.P):
                        ParseMenu();
                        break;
                    case (ConsoleKey.C):
                        ConfigMenu();
                        break;
                    case (ConsoleKey.U):
                        UtilMenu();
                        break;
                    case (ConsoleKey.Escape):
                        isBreaking = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void SetDefaultValues()
        {
            Console.WriteLine("set default values");
            try
            {
                //_treeController.learnStrategy = typeof(AddStrategy.SimplelearnStrategy);
                //_treeController.learnTreeDepth = 7;
                _treeController.learnFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "adventures_of_huckleberry_finn.txt"));
                _treeController.learnFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "alices_adventures_in_wonderland.txt"));
                _treeController.learnFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "pride_and_prejudice.txt"));
                _treeController.learnFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "the_jungle_book.txt"));
                //_treeController.BuildlearnTree();
                _treeController.RestorelearnTree("D:\\Studium\\10SoSe2017\\03AutomatischeSprachverarbeitung\\03Praktikum\\Praktikum4\\learnTree.xml");
                //_treeController.ParseStrategy = typeof(AddStrategy.P5);
                //_treeController.ParseTreeDepth = 7;
            }
            catch (Exception exception)
            {
                Error(exception);
            }
        }

        private void learnMenu()
        {
            bool isBreaking = false;
            _state = _states.IsInlearnMenu;
            while (!isBreaking)
            {
                Console.WriteLine("<<<<learn-menu>>>>");
                Console.WriteLine("Please select:");
                Console.WriteLine("[L]oad tree");
                Console.WriteLine("[S]ave tree");
                Console.WriteLine("[B]uild tree");
                Console.WriteLine("[T]est tree");
                Console.WriteLine("Esc to get back");

                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        LoadlearnTreeMenu();
                        break;
                    case (ConsoleKey.S):
                        SavelearnTreeMenu();
                        break;
                    case (ConsoleKey.B):
                        BuildlearnTreeMenu();
                        break;
                    case (ConsoleKey.T):
                        TestlearnTreeMenu();
                        break;
                    case (ConsoleKey.Escape):
                        isBreaking = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void LoadlearnTreeMenu()
        {
            _state = _states.StartLoadlearnTree;
            Console.WriteLine("Please enter path to tree to restore.");
            var path = Console.ReadLine();
            try
            {
                _treeController.RestorelearnTree(path);
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedLoadlearnTree;
            learnMenu();
        }

        private void SavelearnTreeMenu()
        {
            _state = _states.StartSavelearnTree;
            Console.WriteLine("Please enter path to save tree");
            var path = Console.ReadLine();
            try
            {
                _treeController.SavelearnTree(path);
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedSavelearnTree;
            learnMenu();
        }

        private void BuildlearnTreeMenu()
        {
            _state = _states.StartBuildlearnTree;
            Console.WriteLine("Please select files to build learn-tree");
            Console.WriteLine("You can select more by seperate them with a , like A,H");
            Console.WriteLine("Adventures of [H]uckleberry Finn (606,623 letters)");
            Console.WriteLine("[A]lice adventures in Wonderland (167,515 letters)");
            Console.WriteLine("[p]ride and prejudice (717,571 letters)");
            Console.WriteLine("the adventures of [S]herlock Holmes (594,915 letters)");
            Console.WriteLine("the [j]ungle book (298,775 letters)");

            var input = Console.ReadLine().Split(',');
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
                    _treeController.learnFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName));
                }
            }
            try
            {
                _treeController.BuildlearnTree();
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinischedBuildlearnTree;
        }

        private void TestlearnTreeMenu()
        {
            _state = _states.StartTestlearnTree;
            Console.WriteLine("Please select files to build learn-tree");
            Console.WriteLine("You can select more by seperate them with a , like A,H");
            Console.WriteLine("Adventures of [H]uckleberry Finn (606,623 letters)");
            Console.WriteLine("[A]lice adventures in Wonderland (167,515 letters)");
            Console.WriteLine("[p]ride and prejudice (717,571 letters)");
            Console.WriteLine("the adventures of [S]herlock Holmes (594,915 letters)");
            Console.WriteLine("the [j]ungle book (298,775 letters)");

            var input = Console.ReadLine().Split(',');
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
                    _treeController.TestFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName));
                }
            }
            try
            {
                _treeController.TestlearnTree();
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedTestlearnTree;
        }

        private void ParseMenu()
        {
            bool isBreaking = false;
            _state = _states.IsInParseMenu;
            while (!isBreaking)
            {
                Console.WriteLine("<<<<Parse-menu>>>>");
                Console.WriteLine("Please select:");
                Console.WriteLine("Parse [K]ey");
                Console.WriteLine("Parse [F]ile");
                Console.WriteLine("Get probability of [L]etter");
                Console.WriteLine("Get probability of [T]ext");
                Console.WriteLine("Get [B]est results");
                Console.WriteLine("Esc to get back");

                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.K):
                        ParseKeyMenu();
                        break;
                    case (ConsoleKey.F):
                        ParseFileMenu();
                        break;
                    case (ConsoleKey.L):
                        GetProbabilityOfLetterMenu();
                        break;
                    case (ConsoleKey.T):
                        GetProbabilityOfTextMenu();
                        break;
                    case (ConsoleKey.B):
                        GetBestResultsMenu();
                        break;
                    case (ConsoleKey.Escape):
                        isBreaking = true;
                        return;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void ParseKeyMenu()
        {
            _state = _states.StartParseKey;
            bool isBreaking = false;
            Console.WriteLine("Please enter key you want to parse. or Esc to exit. Valid keys are {0}", Keys.All.Select(k => k.Name));
            while (!isBreaking)
            {
                var input = Console.ReadKey();
                if (KeyController.IsValideKey(input.KeyChar))
                {
                    try
                    {
                        _treeController.ParseKey(input.KeyChar);
                    }
                    catch (Exception exception)
                    {
                        Error(exception);
                    }
                }
                else if (input.Key == ConsoleKey.Escape)
                {
                    isBreaking = true;
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            _state = _states.FinishedParseKey;
        }

        private void ParseFileMenu()
        {
            _state = _states.StartParseFile;
            Console.WriteLine("Please enter path to file you want to parse");
            var input = Console.ReadLine();
            try
            {
                _treeController.ParseFile(input);
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedParseFile;
        }

        private void GetProbabilityOfLetterMenu()
        {
            _state = _states.StartProbabilityOfLetter;
            double result = 0.0;
            Console.WriteLine("Please enter a letter to get its probability.");
            var input = Console.ReadKey().KeyChar;
            try
            {
                var key = KeyController.GetKeyToLetter(input).Name;
                _treeController.ResetParseTree();
                var resultSet = _treeController.ParseKey(key);
                result = resultSet.Where(e => e.Ident == input).FirstOrDefault().Weight;
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            Console.WriteLine("Your text has an probability of: {0}", result);
            _state = _states.FinishedProbabilityOfLetter;
        }

        private void GetProbabilityOfTextMenu()
        {
            _state = _states.StartProbabilityOfText;
            double result = 1.0;
            Console.WriteLine("Please enter text to get its probability.");
            var input = Console.ReadLine();
            foreach (var letter in input)
            {
                try
                {
                    var key = KeyController.GetKeyToLetter(letter);
                    var addedElements = _treeController.ParseKey(key.Name);
                    result = addedElements.Where(e => e.Ident == letter).FirstOrDefault().Weight;
                }
                catch (Exception exception)
                {
                    Error(exception);
                }
            }
            Console.WriteLine("Your text has an probability of: {0}", result);
            _state = _states.FinishedProbabilityOfText;
        }

        private void GetBestResultsMenu()
        {
            _state = _states.StartGetBestResults;
            Console.WriteLine("Please enter number of best results you want to get.");
            var input = Console.ReadLine();
            try
            {
                var n = 0;
                if (int.TryParse(input, out n))
                {
                    var results = _treeController.GetBestResults((uint)n);
                    foreach (var result in results)
                    {
                        Console.WriteLine("{0}. result with a probability of {0}: {1}", results.IndexOf(result), result.Value, result.Key);
                    }
                }
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedGetBestResults;
        }

        private void ConfigMenu()
        {
            bool isBreaking = false;
            _state = _states.IsInConfigMenu;
            while (!isBreaking)
            {
                Console.WriteLine("<<<<Config-menu>>>>");
                Console.WriteLine("Please select:");
                Console.WriteLine("set [S]trategy");
                Console.WriteLine("set [D]epth");
                Console.WriteLine("Esc to get back");

                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.S):
                        SetStrategyMenu();
                        break;
                    case (ConsoleKey.D):
                        SetDepthMenu();
                        break;
                    case (ConsoleKey.Escape):
                        isBreaking = true;
                        return;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void SetStrategyMenu()
        {
            bool isFinished = false;
            bool islearn = false;
            bool isParse = false;
            _state = _states.StartSetStrategy;
            Console.WriteLine("Please select which strategy you want to set.");
            Console.WriteLine("[L]ern");
            Console.WriteLine("[P]arse");
            Console.WriteLine("Esc to get back");
            Console.WriteLine("Then select the strategy you want to use.");
            foreach (var strategy in _treeController.AddStrategies)
            {
                Console.WriteLine("[{0}]: {1}", _treeController.AddStrategies.IndexOf(strategy), strategy.ToString());
            }
            while (!isFinished)
            {
                var input = Console.ReadKey();
                int index;
                if (int.TryParse(input.KeyChar.ToString(), out index))
                {
                    try
                    {
                        if (islearn)
                        {
                            //_treeController.learnStrategy = _treeController.AddStrategies[index];
                            isFinished = true;
                        }
                        if (isParse)
                        {
                            //_treeController.ParseStrategy = _treeController.AddStrategies[index];
                            isFinished = true;
                        }
                    }
                    catch (Exception exception)
                    {
                        Error(exception);
                    }
                }
                else
                {
                    switch (input.Key)
                    {
                        case (ConsoleKey.L):
                            Console.WriteLine("learnstrategy:");
                            islearn = true;
                            isParse = false;
                            break;
                        case (ConsoleKey.P):
                            Console.WriteLine("Parsestrategy:");
                            islearn = false;
                            isParse = true;
                            break;
                        case (ConsoleKey.Escape):
                            isFinished = true;
                            return;
                            break;
                        default:
                            Console.WriteLine("Invalid input.");
                            break;
                    }
                }
            }
            _state = _states.FinishedSetStrategy;
        }

        private void SetDepthMenu()
        {
            bool isFinished = false;
            bool islearn = false;
            bool isParse = false;
            _state = _states.StartSetDepth;
            Console.WriteLine("Please select which depth you want to set.");
            Console.WriteLine("[L]ern");
            Console.WriteLine("[P]arse");
            Console.WriteLine("Esc to get back");
            Console.WriteLine("Then enter the depth.[1-9]");
            while (!isFinished)
            {
                var input = Console.ReadKey();
                int depth;
                if (int.TryParse(input.KeyChar.ToString(), out depth))
                {
                    try
                    {
                        if (islearn)
                        {
                            _treeController.learnTreeDepth = depth;
                            isFinished = true;
                        }
                        if (isParse)
                        {
                            _treeController.ParseTreeDepth = depth;
                            isFinished = true;
                        }
                    }
                    catch (Exception exception)
                    {
                        Error(exception);
                    }
                }
                else
                    switch (input.Key)
                    {
                        case (ConsoleKey.L):
                            islearn = true;
                            isParse = false;
                            Console.WriteLine("learndepth:");
                            break;
                        case (ConsoleKey.P):
                            islearn = false;
                            isParse = true;
                            Console.WriteLine("Parsedepth:");
                            break;
                        case (ConsoleKey.Escape):
                            isFinished = true;
                            return;
                            break;
                        default:
                            Console.WriteLine("Invalid input.");
                            break;
                    }
            }
            _state = _states.FinishedSetDepth;
        }

        private void UtilMenu()
        {
            bool isBreaking = false;
            _state = _states.IsInUtilMenu;
            while (!isBreaking)
            {
                Console.WriteLine("<<<<Util-menu>>>>");
                Console.WriteLine("Please select:");
                Console.WriteLine("[D]raw");
                Console.WriteLine("[C]onvert");
                Console.WriteLine("Esc to get back");

                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.D):
                        DrawMenu();
                        break;
                    case (ConsoleKey.C):
                        ConvertMenu();
                        break;
                    case (ConsoleKey.Escape):
                        isBreaking = true;
                        return;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void ConvertMenu()
        {
            bool isBreaking = false;
            _state = _states.StartConvertMenu;
            while (!isBreaking)
            {
                Console.WriteLine("<<<<Convert-menu>>>>");
                Console.WriteLine("Please select:");
                Console.WriteLine("convert [F]ile of letters to file of keys");
                Console.WriteLine("Esc to get back");

                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.F):
                        ConvertLetterFileToKeyFileMenu();
                        break;
                    case (ConsoleKey.C):
                        ConvertMenu();
                        break;
                    case (ConsoleKey.Escape):
                        isBreaking = true;
                        return;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
            _state = _states.FinishedConvertMenu;
        }

        private void ConvertLetterFileToKeyFileMenu()
        {
            char letter;
            char keyIdent;
            _state = _states.StartConvertLetterFileToKeyFile;
            Console.WriteLine("Please enter path to file you want to convert.");
            var inputFilePath = Console.ReadLine();
            Console.WriteLine("Please enter path of ouitputfile.");
            var outputFilePath = Console.ReadLine();
            try
            {
                var fileStreamIn = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileStreamOut = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                using (var reader = new StreamReader(fileStreamIn))
                {
                    using (var writer = new StreamWriter(fileStreamOut))
                    {

                        while (!reader.EndOfStream)
                        {
                            letter = (char)reader.Read();
                            keyIdent = KeyController.GetKeyToLetter(letter).Name;
                            writer.Write(keyIdent);
                        }
                        writer.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedConvertLetterFileToKeyFile;
        }

        private void DrawMenu()
        {
            bool isBreaking = false;
            _state = _states.IsInDrawMenu;
            while (!isBreaking)
            {
                Console.WriteLine("<<<<Draw-menu>>>>");
                Console.WriteLine("Please select:");
                Console.WriteLine("Draw [L]ernTree");
                Console.WriteLine("Esc to get back");

                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        var result = _treeController.ConvertlearnTreeToString();
                        Console.WriteLine(result);
                        break;
                    case (ConsoleKey.Escape):
                        isBreaking = true;
                        return;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void Error(Exception exception)
        {
            _lastStates.Add(_states.Error);
            Console.WriteLine("An {0} apperes.", exception.GetType().ToString());
            Console.WriteLine("{0}", exception.Message);
            Console.WriteLine("List of last states:");
            foreach (var state in _lastStates)
            {
                Console.WriteLine("-{0}", state.ToString());
            }
            MainMenu();
        }
    }
}
