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

        private bool _isParameterPathToLernFileSet;
        private bool _isParameterPathToTreeFileSet;
        private bool _isParameterDepthSet;

        public static void DoWork(string[] args)
        {
            var worker = new UnitOfWork();
        }

        private UnitOfWork()
        {

        }

        private void parseArgs(string[] args)
        {

        }
    }
}
