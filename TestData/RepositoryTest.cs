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

        protected delegate void TroopsChecker(JEnumerable<JToken> troops);
        protected delegate void SingleTroopChecker(JToken troop);

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

        protected void TestAllTroops(TroopsChecker checker)
        {
            checker(GetWrittenTroops());
        }

        protected void TestSingleTroop(SingleTroopChecker checker)
        {
            foreach (var child in GetWrittenTroops())
            {
                checker(child);
            }
        }

        protected static Repository.Troop TroopToWrite()
        {
            var result = new Repository.Troop("troop");
            result.Cost = 1234;
            result.Type = "Type";
            result.Subtype = "Subtype";
            result.Defense.Front = 4321;
            result.Defense.Rear = 5678;
            return result;
        }

        private JEnumerable<JToken> GetWrittenTroops()
        {
            return JArray.Parse(File.ReadAllText(TROOP_FILE_PATH)).Children();
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
            TestAllTroops(t => Assert.AreEqual(1, t.Count()));
        }

        [TestMethod]
        public void SeveralTroopsAreWritten()
        {
            for (var i = 0; i < 20; ++i)
            {
                repository.AddTroop(new Repository.Troop("troop" + i));
            }
            repository.Write(TROOP_FILE_PATH);
            TestAllTroops(t => Assert.AreEqual(20, t.Count()));
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
        [TestInitialize]
        public void WriteTroop()
        {
            repository.AddTroop(TroopToWrite());
            repository.Write(TROOP_FILE_PATH);
        }

        [TestMethod]
        public void NameIsWritten()
        {
            TestValue("Name", "troop");
        }

        [TestMethod]
        public void CostIsWritten()
        {
            TestValue("Cost", 1234);
        }

        [TestMethod]
        public void TypeIsWritten()
        {
            TestValue("Type", "Type");
        }

        [TestMethod]
        public void SubtypeIsWritten()
        {
            TestValue("Subtype", "Subtype");
        }

        [TestMethod]
        public void FrontDefenseIsWritten()
        {
            TestValue("Fdef", 4321);
        }

        [TestMethod]
        public void RearDefenseIsWritten()
        {
            TestValue("Rdef", 5678);
        }

        private void TestValue<T>(string name, T expected)
        {
            TestSingleTroop(t => Assert.AreEqual(expected, t.Value<T>(name)));
        }
    }

    [TestClass]
    public class WritingNullValuesToRepositoryTest : RepositoryTestBase
    {
        [TestInitialize]
        public void WriteTroop()
        {
            var troop = TroopToWrite();
            troop.Defense = null;
            repository.AddTroop(troop);
            repository.Write(TROOP_FILE_PATH);
        }

        [TestMethod]
        public void FrontDefenseIsNull()
        {
            TestSingleTroop(t => Assert.IsNull(t.Value<int?>("Fdef")));
        }

        [TestMethod]
        public void RearDefenseIsNull()
        {
            TestSingleTroop(t => Assert.IsNull(t.Value<int?>("Fdef")));
        }
    }

    [TestClass]
    public class ReadingFromRepositoryTest : RepositoryTestBase
    {
        private TroopReader reader = new TroopReader(TROOP_FILE_PATH);

        [TestMethod]
        public void TroopsAreEmptyBeforeFirstRead()
        {
            Assert.AreEqual(0, new Repository().GetTroops().Length);
        }

        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void ReadingFailureIsNoticed()
        {
            new Repository().Read(@"Troops\DoesNotExist.json");
        }

        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void EmptyFileResultsInNoTroops()
        {
            reader.WithFileContent("").ReadTroops();
        }

        [TestMethod]
        public void NoTroopsToRead()
        {
            Assert.AreEqual(0, reader.WithTroopCount(0).ReadTroops().Length);
        }

        [TestMethod]
        public void ReadingOneTroop()
        {
            Assert.AreEqual(1, reader.WithTroopCount(1).ReadTroops().Length);
        }

        [TestMethod]
        public void ReadingMultipleTroops()
        {
            Assert.AreEqual(101,
                            reader.WithTroopCount(101).ReadTroops().Length);
        }
    }

    [TestClass]
    public class ReadingValuesFromRepositoryTest : RepositoryTestBase
    {
        private TroopReader reader = new TroopReader(TROOP_FILE_PATH);

        [TestMethod]
        public void ReadingName()
        {
            Assert.AreEqual("troop1", reader.FirstReadTroop().Name);
        }

        [TestMethod]
        public void ReadingCost()
        {
            Assert.AreEqual(4321, reader.FirstReadTroop().Cost);
        }

        [TestMethod]
        public void ReadingType()
        {
            Assert.AreEqual("Type", reader.FirstReadTroop().Type);
        }

        [TestMethod]
        public void ReadingSubtype()
        {
            Assert.AreEqual("Subtype", reader.FirstReadTroop().Subtype);
        }

        [TestMethod]
        public void ReadingFrontDefense()
        {
            Assert.AreEqual(1234, reader.FirstReadTroop().Defense.Front);
        }

        [TestMethod]
        public void ReadingRearDefense()
        {
            Assert.AreEqual(5678, reader.FirstReadTroop().Defense.Rear);
        }
    }

    public class TroopReader
    {
        private int troopCount = 1;
        private string path;
        private TroopJsonBuilder jsonBuilder = new TroopJsonBuilder();
        private Repository repository = new Repository();
        private string fileContent = null;

        public TroopReader(string path)
        {
            this.path = path;
        }

        public Repository.Troop[] ReadTroops()
        {
            WriteTroopFile(path, SelectContent());
            repository.Read(path);
            return repository.GetTroops();
        }

        public Repository.Troop FirstReadTroop()
        {
            return ReadTroops()[0];
        }

        public TroopReader WithFileContent(string content)
        {
            fileContent = content;
            return this;
        }

        public TroopReader WithTroopCount(int count)
        {
            troopCount = count;
            return this;
        }

        public TroopReader WithCost(int cost)
        {
            jsonBuilder.cost = cost;
            return this;
        }

        public TroopReader WithType(string type)
        {
            jsonBuilder.type = type;
            return this;
        }

        public TroopReader WithSubtype(string subtype)
        {
            jsonBuilder.subtype = subtype;
            return this;
        }

        public TroopReader WithFrontDefense(int defense)
        {
            jsonBuilder.frontDefense = defense;
            return this;
        }

        public TroopReader WithRearDefense(int defense)
        {
            jsonBuilder.rearDefense = defense;
            return this;
        }

        private string SelectContent()
        {
            if (fileContent != null)
            {
                return fileContent;
            }
            return jsonBuilder.CreateTroops(troopCount);
        }

        private void WriteTroopFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        private class TroopJsonBuilder
        {
            public int cost = 4321;
            public string type = "Type";
            public string subtype = "Subtype";
            public int frontDefense = 1234;
            public int rearDefense = 5678;

            public string CreateTroops(int count)
            {
                return CreateTroopJson(count);
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
                                   new JProperty("Cost", cost),
                                   new JProperty("Type", type),
                                   new JProperty("Subtype", subtype),
                                   new JProperty("Fdef", frontDefense),
                                   new JProperty("Rdef", rearDefense));
            }
        }
    }
}
