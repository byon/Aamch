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
