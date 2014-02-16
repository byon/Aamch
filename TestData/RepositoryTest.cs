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
            Directory.CreateDirectory(TROOP_DIRECTORY);
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

        [TestMethod]
        public void WritingTroopsCreatesCorrectFile()
        {
            repository.Write(TROOP_FILE_PATH);
            Assert.IsTrue(Directory.Exists(TROOP_DIRECTORY));
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
