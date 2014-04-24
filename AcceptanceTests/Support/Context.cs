﻿using Data;
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
            System.IO.File.Delete(TROOP_FILE);
        }

        public static void EnsureTroopFileExists()
        {
            EnsureTroopDirectoryExists();
            System.IO.File.WriteAllText(TROOP_FILE, "");
        }

        public static void ResetTroops()
        {
            EnsureTroopDirectoryExists();
            System.IO.File.WriteAllText(TROOP_FILE, "[]");
        }

        public static void EnsureTroopExists(string name)
        {
            var repository = GetRepository();
            if (!repository.HasTroop(name))
            {
                repository.AddTroop(CreateTroop(name));
                repository.Write(TROOP_FILE);
            }
        }

        public static bool IsApplicationRunning()
        {
            return ApplicationLifetime.IsRunning( );
        }

        public static void ViewTroops()
        {
            CacheObject("viewedTroops", GetApplication().GetTroops());
        }

        public static string[] GetTroops()
        {
            return CachedObject<string[]>("viewedTroops");
        }

        private static void EnsureTroopDirectoryExists()
        {
            if (!Directory.Exists("troops"))
            {
                Directory.CreateDirectory("troops");
            }
        }

        private static Repository.Troop CreateTroop(string name)
        {
            return new Repository.Troop(name);
        }

        private static T CachedObject<T>(string id)
        {
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
