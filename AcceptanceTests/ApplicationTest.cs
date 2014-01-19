using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace AcceptanceTests
{
    [TestClass]
    public class ApplicationTest
    {
        [TestInitialize]
        public void StartApplication()
        {
            Context.GetMainWindow();
        }

        [TestCleanup]
        public void CloseApplication()
        {
            Context.CloseMainWindow();
        }

        [TestMethod]
        public void AccessMainWindow()
        {
            // don't do anything
        }

        [TestMethod]
        public void AccessMainWindow1()
        {
            // don't do anything
        }

        [TestMethod]
        public void AccessMainWindow2()
        {
            // don't do anything
        }

        [TestMethod]
        public void AccessMainWindow3()
        {
            // don't do anything
        }

        [TestMethod]
        public void AccessMainWindow4()
        {
            // don't do anything
        }

        [TestMethod]
        public void AccessMainWindow5()
        {
            // don't do anything
        }

        [TestMethod]
        public void AccessMainWindow6()
        {
            // don't do anything
        }

        [TestMethod]
        public void AccessMainWindow7()
        {
            // don't do anything
        }
    }
}
