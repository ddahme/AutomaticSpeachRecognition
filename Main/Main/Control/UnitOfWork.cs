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
        private enum _states { Init, IsInMainMenu, IsInLernMenu, FinishedLoadLernTree, StartSaveLernTree, FinishedSaveLernTree, ReadKeyFromKeyboard, ReadKeyFromFile, ConvertLetterToKey, AddKeyToParseTree, ChangeParseTree, StartLoadLernTree, BuildLernTree, SaveLernTree, TestLernTree, PrintTree, Error, IsInUtilMenu, IsInConfigMenu, IsInParseMenu, StartBuildLernTree, FinischedBuildLernTree, StartTestLernTree, FinishedTestLernTree, StartParseKey, FinishedParseKey, StartParseFile, FinishedParseFile, StartProbabilityOfLetter, FinishedProbabilityOfLetter, StartProbabilityOfText, FinishedProbabilityOfText, StartGetBestResults, FinishedGetBestResults, StartSetStrategy, FinishedSetStrategy, StartSetDepth, FinishedSetDepth, StartConvertMenu, FinishedConvertMenu, FinishedConvertLetterFileToKeyFile, StartConvertLetterFileToKeyFile, IsInDrawMenu };
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
            MainMenu();
        }

        private void MainMenu()
        {
            bool validSelect = false;
            _state = _states.IsInMainMenu;
            Console.WriteLine("<<<<Main-menu>>>>");
            Console.WriteLine("Please select:");
            Console.WriteLine("[L]earn");
            Console.WriteLine("[P]arse");
            Console.WriteLine("[C]onfig");
            Console.WriteLine("[U]til");
            Console.WriteLine("Esc to get back");
            while (!validSelect)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        validSelect = true;
                        LernMenu();
                        break;
                    case (ConsoleKey.P):
                        validSelect = true;
                        ParseMenu();
                        break;
                    case (ConsoleKey.C):
                        validSelect = true;
                        ConfigMenu();
                        break;
                    case (ConsoleKey.U):
                        validSelect = true;
                        UtilMenu();
                        break;
                    case (ConsoleKey.Escape):
                        validSelect = true;
                        break;
                        break;
                    default:
                        validSelect = false;
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void LernMenu()
        {
            bool validSelect = false;
            _state = _states.IsInLernMenu;
            Console.WriteLine("<<<<Lern-menu>>>>");
            Console.WriteLine("Please select:");
            Console.WriteLine("[L]oad tree");
            Console.WriteLine("[S]ave tree");
            Console.WriteLine("[B]uild tree");
            Console.WriteLine("[T]est tree");
            Console.WriteLine("Esc to get back");
            while (!validSelect)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        validSelect = true;
                        LoadLernTreeMenu();
                        break;
                    case (ConsoleKey.S):
                        validSelect = true;
                        SaveLernTreeMenu();
                        break;
                    case (ConsoleKey.B):
                        validSelect = true;
                        BuildLernTreeMenu();
                        break;
                    case (ConsoleKey.T):
                        validSelect = true;
                        TestLernTreeMenu();
                        break;
                    case (ConsoleKey.Escape):
                        validSelect = true;
                        return;
                        break;
                    default:
                        validSelect = false;
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void LoadLernTreeMenu()
        {
            _state = _states.StartLoadLernTree;
            Console.WriteLine("Please enter path to tree to restore.");
            var path = Console.ReadLine();
            try
            {
                _treeController.RestoreLernTree(path);
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedLoadLernTree;
            LernMenu();
        }

        private void SaveLernTreeMenu()
        {
            _state = _states.StartSaveLernTree;
            Console.WriteLine("Please enter path to save tree");
            var path = Console.ReadLine();
            try
            {
                _treeController.SaveLernTree(path);
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedSaveLernTree;
            LernMenu();
        }

        private void BuildLernTreeMenu()
        {
            _state = _states.StartBuildLernTree;
            Console.WriteLine("Please select files to build lern-tree");
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
                    _treeController.LernFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName));
                }
            }
            try
            {
                _treeController.BuildLernTree();
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinischedBuildLernTree;
        }

        private void TestLernTreeMenu()
        {
            _state = _states.StartTestLernTree;
            Console.WriteLine("Please select files to build lern-tree");
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
                _treeController.TestLernTree();
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedTestLernTree;
        }

        private void ParseMenu()
        {
            bool validSelect = false;
            _state = _states.IsInParseMenu;
            Console.WriteLine("<<<<Parse-menu>>>>");
            Console.WriteLine("Please select:");
            Console.WriteLine("Parse [K]ey");
            Console.WriteLine("Parse [F]ile");
            Console.WriteLine("Get probability of [L]etter");
            Console.WriteLine("Get probability of [T]ext");
            Console.WriteLine("Get [B]est results");
            Console.WriteLine("Esc to get back");
            while (!validSelect)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.K):
                        validSelect = true;
                        ParseKeyMenu();
                        break;
                    case (ConsoleKey.F):
                        validSelect = true;
                        ParseFileMenu();
                        break;
                    case (ConsoleKey.L):
                        validSelect = true;
                        GetProbabilityOfLetterMenu();
                        break;
                    case (ConsoleKey.T):
                        validSelect = true;
                        GetProbabilityOfTextMenu();
                        break;
                    case (ConsoleKey.B):
                        validSelect = true;
                        GetBestResultsMenu();
                        break;
                    case (ConsoleKey.Escape):
                        validSelect = true;
                        return;
                        break;
                    default:
                        validSelect = false;
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void ParseKeyMenu()
        {
            _state = _states.StartParseKey;
            bool isRunning = false;
            Console.WriteLine("Please enter key you want to parse. or Esc to exit. Valid keys are {0}", Keys.All.Select(k => k.Name));
            while (!isRunning)
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
                    isRunning = false;
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
                result = _treeController.ParseKey(key).Where(e => e.Ident == input).FirstOrDefault().Weight;
            }
            catch (Exception exception)
            {
                Error(exception);
            }
            Console.WriteLine("Your text has an probability of: {}", result);
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
                    result = _treeController.ParseKey(letter).Where(e => e.Ident == letter).FirstOrDefault().Weight;
                }
                catch (Exception exception)
                {
                    Error(exception);
                }
            }
            Console.WriteLine("Your text has an probability of: {}", result);
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
                if(int.TryParse(input, out n))
                {
                    var results = _treeController.GetBestResults((uint)n);
                    foreach(var result in results)
                    {
                        Console.WriteLine("{0}. result with a probability of {1}: {2}", results.IndexOf(result), result.Value, result.Key);
                    }
                }
            }
            catch(Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedGetBestResults;
        }

        private void ConfigMenu()
        {
            bool validSelect = false;
            _state = _states.IsInConfigMenu;
            Console.WriteLine("<<<<Config-menu>>>>");
            Console.WriteLine("Please select:");
            Console.WriteLine("set [S]trategy");
            Console.WriteLine("set [D]epth");
            Console.WriteLine("Esc to get back");
            while (!validSelect)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.S):
                        validSelect = true;
                        SetStrategyMenu();
                        break;
                    case (ConsoleKey.D):
                        validSelect = true;
                        SetDepthMenu();
                        break;
                    case (ConsoleKey.Escape):
                        validSelect = true;
                        return;
                        break;
                    default:
                        validSelect = false;
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void SetStrategyMenu()
        {
            bool isFinished = false;
            bool isLern = false;
            bool isParse = false;
            _state = _states.StartSetStrategy;
            Console.WriteLine("Please select which strategy you want to set.");
            Console.WriteLine("[L]ern");
            Console.WriteLine("[P]arse");
            Console.WriteLine("Esc to get back");
            Console.WriteLine("Then select the strategy you want to use.");
            foreach (var strategy in _treeController.AddStrategies)
            {
                Console.WriteLine("[{0}]: {1}",_treeController.AddStrategies.IndexOf(strategy) ,strategy.ToString());
                }
            while (!isFinished)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        isLern = true;
                        isParse = false;
                        break;
                    case (ConsoleKey.P):
                        isLern = false;
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
                input = Console.ReadKey();
                int index;
                if (int.TryParse(input.KeyChar.ToString(), out index))
                {
                    try
                    {
                        if (isLern)
                        {
                            _treeController.LernStrategy = _treeController.AddStrategies[index];
                        }
                        if (isParse)
                        {
                            _treeController.ParseStrategy = _treeController.AddStrategies[index];
                        }
                    }
                    catch (Exception exception)
                    {
                        Error(exception);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            _state = _states.FinishedSetStrategy;
        }

        private void SetDepthMenu()
        {
            bool isFinished = false;
            bool isLern = false;
            bool isParse = false;
            _state = _states.StartSetDepth;
            Console.WriteLine("Please select which depth you want to set.");
            Console.WriteLine("[L]ern");
            Console.WriteLine("[P]arse");
            Console.WriteLine("Esc to get back");
            Console.WriteLine("Then enter the depth.");
            while (!isFinished)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        isLern = true;
                        isParse = false;
                        break;
                    case (ConsoleKey.P):
                        isLern = false;
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
                var input2 = Console.ReadLine();
                int depth;
                if (int.TryParse(input2, out depth))
                {
                    try
                    {
                        if (isLern)
                        {
                            _treeController.LernTreeDepth = depth;
                        }
                        if (isParse)
                        {
                            _treeController.ParseTreeDepth = depth;
                        }
                    }
                    catch (Exception exception)
                    {
                        Error(exception);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            _state = _states.FinishedSetDepth;
        }

        private void UtilMenu()
        {
            bool validSelect = false;
            _state = _states.IsInUtilMenu;
            Console.WriteLine("<<<<Util-menu>>>>");
            Console.WriteLine("Please select:");
            Console.WriteLine("[D]raw");
            Console.WriteLine("[C]onvert");
            Console.WriteLine("Esc to get back");
            while (!validSelect)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.D):
                        validSelect = true;
                        DrawMenu();
                        break;
                    case (ConsoleKey.C):
                        validSelect = true;
                        ConvertMenu();
                        break;
                    case (ConsoleKey.Escape):
                        validSelect = true;
                        return;
                        break;
                    default:
                        validSelect = false;
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private void ConvertMenu()
        {
            bool validSelect = false;
            _state = _states.StartConvertMenu;
            Console.WriteLine("<<<<Convert-menu>>>>");
            Console.WriteLine("Please select:");
            Console.WriteLine("convert [F]ile of letters to file of keys");
            Console.WriteLine("Esc to get back");
            while (!validSelect)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.F):
                        validSelect = true;
                        ConvertLetterFileToKeyFileMenu();
                        break;
                    case (ConsoleKey.C):
                        validSelect = true;
                        ConvertMenu();
                        break;
                    case (ConsoleKey.Escape):
                        validSelect = true;
                        return;
                        break;
                    default:
                        validSelect = false;
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
            }catch(Exception exception)
            {
                Error(exception);
            }
            _state = _states.FinishedConvertLetterFileToKeyFile;
        }

        private void DrawMenu()
        {
            bool isFinished = false;
            _state = _states.IsInDrawMenu;
            Console.WriteLine("<<<<Draw-menu>>>>");
            Console.WriteLine("Please select:");
            Console.WriteLine("Draw [L]ernTree");
            Console.WriteLine("Esc to get back");
            while (!isFinished)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case (ConsoleKey.L):
                        var result = _treeController.ConvertLernTreeToString();
                        Console.WriteLine(result);
                        break;
                    case (ConsoleKey.Escape):
                        isFinished = true;
                        return;
                        break;
                    default:
                        isFinished = false;
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
