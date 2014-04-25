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
            foreach (var child in data.Children())
            {
                var troop = new Repository.Troop(child.Value<string>("Name"));
                troop.Cost = child.Value<int>("Cost");
                result.Add(troop);
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
    public class WritingValuesToRepositoryTest : RepositoryTestBase
    {
        private Repository.Troop troop;

        [TestInitialize]
        public void WriteTroop()
        {
            troop = new Repository.Troop("troop");
            troop.Cost = 1234;
            repository.AddTroop(troop);
            repository.Write(TROOP_FILE_PATH);
        }

        [TestMethod]
        public void NameIsWritten()
        {
            Assert.AreEqual("troop", WrittenTroops()[0].Name);
        }

        [TestMethod]
        public void CostIsWritten()
        {
            Assert.AreEqual(1234, WrittenTroops()[0].Cost);
        }
    }

    public class ReadingTestBase : RepositoryTestBase
    {
        protected void AddTroops(int count)
        {
            WriteTroopFile(CreateTroopJson(count));
        }

        protected void WriteTroopFile(string content)
        {
            File.WriteAllText(TROOP_FILE_PATH, content);
        }

        private string CreateTroopJson(int count)
        {
            var troops = CreateTroopNames(count);
            var jsonTroops = from t in troops select TroopToJson(t);
            return new JArray(jsonTroops).ToString();
        }

        private string[] CreateTroopNames(int count)
        {
            var result = new string[count];
            for (int i = 0; i < count; ++i)
            {
                result[i] = "troop" + (i + 1).ToString();
            }
            return result;
        }

        private JObject TroopToJson(string name)
        {
            return new JObject(new JProperty("Name", name),
                               new JProperty("Cost", 4321));
        }
    }

    [TestClass]
    public class ReadingFromRepositoryTest : ReadingTestBase
    {
        [TestMethod]
        public void TroopsAreEmptyBeforeFirstRead()
        {
            Assert.AreEqual(0, repository.GetTroops().Length);
        }

        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void ReadingFailureIsNoticed()
        {
            repository.Read(@"Troops\DoesNotExist.json");
        }

        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void EmptyFileResultsInNoTroops()
        {
            WriteTroopFile("");
            repository.Read(TROOP_FILE_PATH);
        }

        [TestMethod]
        public void NoTroopsToRead()
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
        public void ReadingMultipleTroops()
        {
            AddTroops(101);
            repository.Read(TROOP_FILE_PATH);
            Assert.AreEqual(101, repository.GetTroops().Length);
        }
    }

    [TestClass]
    public class ReadingValuesFromRepositoryTest : ReadingTestBase
    {
        private Repository.Troop readTroop;

        [TestInitialize]
        public void ReadOneTroop()
        {
            AddTroops(1);
            repository.Read(TROOP_FILE_PATH);
            readTroop = repository.GetTroops()[0];
        }

        [TestMethod]
        public void ReadingName()
        {
            Assert.AreEqual("troop1", readTroop.Name);
        }

        [TestMethod]
        public void ReadingCost()
        {
            Assert.AreEqual(4321, readTroop.Cost);
        }
    }
}
