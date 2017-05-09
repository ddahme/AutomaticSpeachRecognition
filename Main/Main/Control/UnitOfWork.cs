using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Main.Control
{
  class UnitOfWork
  {
    private enum _states { Init, ReadNextStateFromKeyboard, ReadKeyFromKeyboard, ReadKeyFromFile, ConvertLetterToKey, AddKeyToParseTree, ChangeParseTree, LoadLernTree, BuildLernTree, SaveLernTree, TestLernTree, PrintTree, Error };
    private List<_states> _lastStates;
    private KeyController _keyController;
    private TreeController _treeController;


    public static void DoWork(string[] args)
    {
      var worker = new UnitOfWork();
    }

    private UnitOfWork()
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
            ParseKeyFormKeyboard();
            break;
          case "F":
            ParseKeyFromFile();
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

    private void LoadLernTree()
    {
      _lastStates.Add(_states.LoadLernTree);
      Console.WriteLine("Please enter path to tree to restore.");
      var path = Console.ReadLine();
      try
      {
        _treeController.RestoreTree(path);
      }
      catch (Exception exception)
      {
        Error(exception);
      }
      ReadNextStateFromKeyboard();
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
      catch (Exception exception)
      {
        Error(exception);
      }
      ReadNextStateFromKeyboard();
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
      catch (Exception exception)
      {
        Error(exception);
      }
      ReadNextStateFromKeyboard();
    }

    private void TestLernTree()
    {
      try
      {
        _treeController.TestLernTree();
      }
      catch (Exception exception)
      {
        Error(exception);
      }
      ReadNextStateFromKeyboard();
    }

    private void PrintTree()
    {
      try
      {
        Console.Write(_treeController.LernTreeToString());
      }
      catch (Exception exception)
      {
        Error(exception);
      }
      ReadNextStateFromKeyboard();
    }

    private void ParseKeyFormKeyboard()
    {
      ConsoleKey input;
      bool isRunning = true;
      _lastStates.Add(_states.ReadKeyFromKeyboard);
      Console.WriteLine("Please enter key you want to parse. Or press Esc to exit.");
      while (isRunning)
      {
        input = Console.ReadKey().Key;
        isRunning = (input != ConsoleKey.Escape);
        _treeController.ParseKey((char)input);
      }
      ReadNextStateFromKeyboard();
    }

    private void ParseKeyFromFile()
    {
      char ident;
      _lastStates.Add(_states.ReadKeyFromFile);
      Console.WriteLine("Please enter path to file.");
      var input = Console.ReadLine();
      try
      {
        var fs = new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.Read);
        using (var reader = new StreamReader(fs))
        {
          while (!reader.EndOfStream)
          {
            //read key form text
            ident = (char)reader.Read();
            _treeController.ParseKey(ident);
          }
        }
      }
      catch (Exception exception)
      {
        Error(exception);
      }
      ReadNextStateFromKeyboard();
    }

    private void ConvertLetterToKey()
    {
      var result = new StringBuilder();
      var input = string.Empty;
      _lastStates.Add(_states.ConvertLetterToKey);
      Console.WriteLine("Please enter letter to convert to key.");
      input = Console.ReadLine();
      foreach (var letter in input)
      {
        result.Append(_keyController.GetKeyToLetter(letter).Name);
      }
      Console.WriteLine("the result to your inpot is:{result}", result);
      ReadNextStateFromKeyboard();
    }

    private void Error(Exception exception)
    {
      _lastStates.Add(_states.Error);
      Console.WriteLine("An {errorName} apperes.", exception.GetType());
      Console.WriteLine("{errorDiscription}", exception.Message);
      Console.WriteLine("List of last states:");
      foreach (var state in _lastStates)
      {
        Console.WriteLine("-{state}", state.ToString());
      }
      ReadNextStateFromKeyboard();
    }
  }
}
