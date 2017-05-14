﻿using System;
using NUnit.Framework;
using System.Threading.Tasks;
using Main.Control;
using Main.Control.AddStrategy;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    public class NUnitTests
    {
        private TreeController treeController;

        [SetUp]
        public void SetUp()
        {
            treeController = new TreeController();
            //add lern-strategy
            treeController.LernStrategy = typeof(SimpleLernStrategy);
            //Add lern-files
            treeController.LernFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "adventures_of_huckleberry_finn.txt"));
            treeController.LernFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "pride_and_prejudice.txt"));
            treeController.LernFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "alices_adventures_in_wonderland.txt"));
            treeController.LernFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "the_jungle_book.txt"));
            //Add test-files
            treeController.TestFilePaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "the_adevntures_of_sherlock_holmes.txt"));
            //lern
            treeController.BuildLernTree();
            //add parse Strategy
            treeController.ParseStrategy = typeof(MarcowParseStrategy);
        }

        [Category("Parsing")]
        [Test]
        public void P1()
        {
            var expacted = 0.2;
            var delta = 0.3;
            var actual = treeController.GetProbabilityToText("Did it ever rain in Steinfurt?");
            Assert.AreEqual(expacted, actual, delta);
        }

        [TearDown]
        public void Teardown()
        {
            treeController = null;
        }
    }
}