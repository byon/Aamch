using System;
using System.IO;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestData
{
    [TestClass]
    public class RepositoryTest
    {
        private static string TROOP_DIRECTORY = @"Troops\";
        private static string TROOP_FILE_PATH = TROOP_DIRECTORY + "troop.json";
        private Repository repository = new Repository();

        [TestInitialize]
        public void Initialize()
        {
            CleanupTroopData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupTroopData();
        }

        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void WritingFailureIsNoticed()
        {
            repository.Write(@"unexistingFolder\file.json");
        }

        private void CleanupTroopData()
        {
            if (Directory.Exists(TROOP_DIRECTORY))
            {
                Directory.Delete(TROOP_DIRECTORY, true);
            }
        }
    }
}
