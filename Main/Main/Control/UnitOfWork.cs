using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Control
{
    class UnitOfWork
    {

        private const string _welcomeString = "Hello,\r\n 1)test";

        private enum _states { Init, LoadTree, LoadTextForTree, ReadKey, TestTree, PrintTree, Error};
        private _states _lastState;
        private bool _isParameterPathToLernFileSet;
        private bool _isParameterPathToTreeFileSet;
        private bool _isParameterDepthSet;

        public static void DoWork(string[] args)
        {
            var worker = new UnitOfWork();
        }

        private UnitOfWork()
        {
            //_state = _states.
        }

        private void Init()
        {
            _lastState = _states.Init;
        }

        private void LoadTree()
        {
            _lastState = _states.LoadTree;
        }

        private void parseArgs(string[] args)
        {

        }
    }
}
