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
        private const string APPLICATION_BASE = @"..\..\..\Aamch\bin\";
#if DEBUG
        private const string CONFIGURATION = @"Debug\";
#else
        private const string CONFIGURATION = @"Release\";
#endif
        private const string APPLICATION_DIRECTORY = APPLICATION_BASE +
                                                     CONFIGURATION;
        private const string APPLICATION_NAME = "Aamch.exe";
        private const string APPLICATION = APPLICATION_DIRECTORY +
                                           APPLICATION_NAME;

        private Application application;
        private Window  window;

        [TestInitialize]
        public void StartApplication()
        {
            application = Application.Launch(APPLICATION);
            window = application.GetWindow("MainWindow",
                                           InitializeOption.NoCache);
        }

        [TestCleanup]
        public void CloseApplication()
        {
            window.Close();
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
