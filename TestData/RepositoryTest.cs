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
        private TroopWriter writer = new TroopWriter(TROOP_FILE_PATH);

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
            var troops = writer.WithTroopCount(1).GetWrittenTroops();
            Assert.AreEqual(1, troops.Count());
        }

        [TestMethod]
        public void SeveralTroopsAreWritten()
        {
            var troops = writer.WithTroopCount(20).GetWrittenTroops();
            Assert.AreEqual(20, troops.Count());
        }

        [TestMethod]
        public void CanRecognizeThatTroopDoesNotExist()
        {
            Assert.IsFalse(new Repository().HasTroop("doesNotExist"));
        }

        [TestMethod]
        public void CanRecognizeThatTroopDoesExists()
        {
            var repository = new Repository();
            repository.AddTroop(new Repository.Troop("existing"));
            Assert.IsTrue(repository.HasTroop("existing"));
        }
    }

    [TestClass]
    public class WritingValuesToRepositoryTest : RepositoryTestBase
    {
        [TestMethod]
        public void NameIsWritten()
        {
            Assert.AreEqual("id0", Test<string>(t => t.Name = "id", "Name"));
        }

        [TestMethod]
        public void CostIsWritten()
        {
            TestValue(t => t.Cost = 1234, "Cost", 1234);
        }

        [TestMethod]
        public void TypeIsWritten()
        {
            TestValue(t => t.Type = "a value", "Type", "a value");
        }

        [TestMethod]
        public void SubtypeIsWritten()
        {
            TestValue(t => t.Subtype = "value", "Subtype", "value");
        }

        [TestMethod]
        public void FrontDefenseIsWritten()
        {
            TestValue(t => t.Defense.Front = 4321, "Fdef", 4321);
        }

        [TestMethod]
        public void RearDefenseIsWritten()
        {
            TestValue(t => t.Defense.Rear = 5678, "Rdef", 5678);
        }

        [TestMethod]
        public void FrontDefenseIsNull()
        {
            Assert.IsNull(Test<int?>(t => t.Defense = null, "Fdef"));
        }

        [TestMethod]
        public void RearDefenseIsNull()
        {
            Assert.IsNull(Test<int?>(t => t.Defense = null, "Rdef"));
        }

        private void TestValue<T>(TroopWriter.TroopChanger changer,
                                  string name, T value)
        {
            Assert.AreEqual(value, Test<T>(changer, name));
        }

        private T Test<T>(TroopWriter.TroopChanger changer, string name)
        {
            TroopWriter writer = new TroopWriter(TROOP_FILE_PATH);
            return writer.WithTroop(changer).GetWrittenTroop().Value<T>(name);
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

        [TestMethod]
        public void ReadingNullFrontDefense()
        {
            var troop = reader.WithFrontDefense(null).FirstReadTroop();
            Assert.IsNull(troop.Defense);
        }

        [TestMethod]
        public void ReadingNullRearDefense()
        {
            var troop = reader.WithRearDefense(null).FirstReadTroop();
            Assert.IsNull(troop.Defense);
        }
    }

    public class TroopWriter
    {
        private int troopCount = 1;
        private string path;
        private Repository repository = new Repository();
        private Repository.Troop troopTemplate = new Repository.Troop("troop");

        public delegate void TroopChanger(Repository.Troop troop);

        public TroopWriter(string path)
        {
            this.path = path;
        }

        public JEnumerable<JToken> GetWrittenTroops()
        {
            for (int i = 0; i < troopCount; ++i)
            {
                repository.AddTroop(CreateTroop(i.ToString()));
            }
            repository.Write(path);
            return JArray.Parse(File.ReadAllText(path)).Children();
        }

        public JToken GetWrittenTroop()
        {
            return GetWrittenTroops().ElementAt(0);
        }

        public TroopWriter WithTroopCount(int count)
        {
            troopCount = count;
            return this;
        }

        public TroopWriter WithTroop(TroopChanger changer)
        {
            changer(troopTemplate);
            return this;
        }

        private Repository.Troop CreateTroop(string id)
        {
            var result = troopTemplate.Copy();
            result.Name += id;
            return result;
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

        public TroopReader WithFrontDefense(int? defense)
        {
            jsonBuilder.frontDefense = defense;
            return this;
        }

        public TroopReader WithRearDefense(int? defense)
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
            public int? frontDefense = 1234;
            public int? rearDefense = 5678;

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
