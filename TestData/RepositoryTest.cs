using System;
using System.Collections.Generic;
using System.IO;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace TestData
{
    public class RepositoryTestBase
    {
        protected static string TROOP_DIRECTORY = @"Troops\";
        protected static string TROOP_FILE_PATH = TROOP_DIRECTORY +
                                                  "troop.json";
        protected Repository repository = new Repository();

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

        protected Repository.Troop[] WrittenTroops()
        {
            var result = new List<Repository.Troop>();
            var data = JArray.Parse(File.ReadAllText(TROOP_FILE_PATH));
            foreach (var troop in data.Children())
            {
                result.Add(new Repository.Troop(troop.Value<string>("Name")));
            }
            return result.ToArray();
        }

        private void CleanupTroopData()
        {
            if (Directory.Exists(TROOP_DIRECTORY))
            {
                Directory.Delete(TROOP_DIRECTORY, true);
            }
        }
    }

    [TestClass]
    public class WritingToRepositoryTest : RepositoryTestBase
    {
        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void WritingFailureIsNoticed()
        {
            File.WriteAllText(@"Troops\file", "irrelevant contents");
            repository.Write(@"Troops\file\file.json");
        }

        [TestMethod]
        public void WritingTroopsCreatesCorrectFile()
        {
            repository.Write(TROOP_FILE_PATH);
            Assert.IsTrue(File.Exists(TROOP_FILE_PATH));
        }

        [TestMethod]
        public void WritingTroopsCreatesDirectoryStructure()
        {
            var directory = TROOP_DIRECTORY + @"dir\";
            repository.Write(directory + "troop.json");
            Assert.IsTrue(Directory.Exists(directory));
        }

        [TestMethod]
        public void OneTroopIsWritten()
        {
            repository.AddTroop(new Repository.Troop("troop"));
            repository.Write(TROOP_FILE_PATH);
            Assert.AreEqual(1, WrittenTroops().Length);
        }

        [TestMethod]
        public void SeveralTroopsAreWritten()
        {
            for (var i = 0; i < 20; ++i)
            {
                repository.AddTroop(new Repository.Troop("troop" + i));
            }
            repository.Write(TROOP_FILE_PATH);
            Assert.AreEqual(20, WrittenTroops().Length);
        }

        [TestMethod]
        public void NameIsWritten()
        {
            repository.AddTroop(new Repository.Troop("troop"));
            repository.Write(TROOP_FILE_PATH);
            Assert.AreEqual("troop", WrittenTroops()[0].Name);
        }

        [TestMethod]
        public void CanRecognizeThatTroopDoesNotExist()
        {
            Assert.IsFalse(repository.HasTroop("doesNotExist"));
        }

        [TestMethod]
        public void CanRecognizeThatTroopDoesExists()
        {
            repository.AddTroop(new Repository.Troop("existing"));
            Assert.IsTrue(repository.HasTroop("existing"));
        }
    }

    [TestClass]
    public class ReadingFromRepositoryTest : RepositoryTestBase
    {
        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void ReadingFailureIsNoticed()
        {
            repository.Read(@"Troops\DoesNotExist.json");
        }

        [TestMethod]
        public void EmptyFileResultsInNoTroops()
        {
            AddTroops(0);
            repository.Read(TROOP_FILE_PATH);
            Assert.AreEqual(0, repository.GetTroops().Length);
        }

        [TestMethod]
        public void ReadingOneTroop()
        {
            AddTroops(1);
            repository.Read(TROOP_FILE_PATH);
            Assert.AreEqual(1, repository.GetTroops().Length);
        }

        [TestMethod]
        public void ReadingName()
        {
            AddTroops(1);
            repository.Read(TROOP_FILE_PATH);
            Assert.AreEqual("troop1", repository.GetTroops()[0].Name);
        }

        private void AddTroops(int count)
        {
            File.WriteAllText(TROOP_FILE_PATH, CreateTroopJson(count));
        }

        private string CreateTroopJson(int count)
        {
            var troops = CreateTroops(count);
            var jsonTroops = from t in troops select TroopToJson(t);
            return new JArray(jsonTroops).ToString();
        }

        private Repository.Troop[] CreateTroops(int count)
        {
            var result = new Repository.Troop[count];
            for (int i = 0; i < count; ++i)
            {
                var name = "troop" + (i + 1).ToString();
                result[0] = new Repository.Troop(name);
            }
            return result;
        }

        private JObject TroopToJson(Repository.Troop troop)
        {
            return new JObject(new JProperty("Name", troop.Name));
        }
    }
}
