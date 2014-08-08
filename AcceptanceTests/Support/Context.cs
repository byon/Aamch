using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    class Context
    {
        private static string TROOP_FILE = "troops/troops.json";
        private delegate object Creator();

        public static TestedApplication GetApplication()
        {
            return ApplicationLifetime.GetApplication();
        }

        public static Repository GetRepository()
        {
            Creator creator = () => new Repository();
            return CachedObject<Repository>("TroopRepository", creator);
        }

        public static void EnsureThereIsNoTroopFile()
        {
            if (System.IO.File.Exists(TROOP_FILE))
            {
                System.IO.File.Delete(TROOP_FILE);
            }
        }

        public static void CreateInvalidTroopFile()
        {
            CreateTroopFile("[");
        }

        public static void EnsureTroopFileExists()
        {
            CreateTroopFile("");
        }

        public static void ResetTroops()
        {
            CreateTroopFile("[]");
        }

        public static void AddTroop(Dictionary<string, string> troop)
        {
            var repository = GetRepository();
            repository.AddTroop(troop);
            repository.Write(TROOP_FILE);
        }

        public static bool IsApplicationRunning()
        {
            return ApplicationLifetime.IsRunning( );
        }

        public static void ViewTroops()
        {
            CacheObject("viewedTroops", GetApplication().GetTroops());
        }

        public static void ViewTroopGroup()
        {
            CacheObject("viewedTroopGroup", GetApplication().GetTroopGroup());
        }

        public static List<Dictionary<string, string>> GetTroops()
        {
            var name = "viewedTroops";
            return CachedObject<List<Dictionary<string, string>>>(name);
        }

        public static List<Dictionary<string, string>> GetTroopGroup()
        {
            var name = "viewedTroopGroup";
            return CachedObject<List<Dictionary<string, string>>>(name);
        }

        public static void SelectTroop(string selected)
        {
            GetApplication().SelectTroop(selected);
        }

        public static string GetStatusMessage()
        {
            return GetApplication().GetStatusMessage();
        }

        private static void CreateTroopFile(string contents)
        {
            EnsureTroopDirectoryExists();
            System.IO.File.WriteAllText(TROOP_FILE, contents);
        }

        private static void EnsureTroopDirectoryExists()
        {
            if (!Directory.Exists("troops"))
            {
                Directory.CreateDirectory("troops");
            }
        }

        private static T CachedObject<T>(string id)
        {
            Assert.IsTrue(ScenarioContext.Current.ContainsKey(id),
                         "'" + id + "' not stored in scenario context");
            return (T)ScenarioContext.Current[id];
        }

        private static T CachedObject<T>(string id, Creator creator)
        {
            var current = ScenarioContext.Current;
            if (!current.ContainsKey(id))
            {
                CacheObject(id, creator());
            }
            return (T)current[id];
        }

        private static void CacheObject(string id, object toStore)
        {
            ScenarioContext.Current.Add(id, toStore);
        }
    }
}
