using System.Collections.Generic;
using System.IO;

namespace AcceptanceTests.Support
{
    class TroopFile
    {
        private const string TROOP_FILE = "troops/troops.json";

        internal static void DeleteIfExists()
        {
            if (!File.Exists(TROOP_FILE)) return;
            File.Delete(TROOP_FILE);
        }

        public static void CreateInvalidTroopFile()
        {
            Create("[");
        }

        public static void EnsureTroopFileExists()
        {
            Create("");
        }

        public static void ResetTroops()
        {
            Create("[]");
        }

        internal static void Create(string contents)
        {
            EnsureTroopDirectoryExists();
            File.WriteAllText(TROOP_FILE, contents);
        }

        internal void AddTroop(Repository repository, Dictionary<string, string> troop)
        {
            repository.AddTroop(troop);
            repository.Write(TROOP_FILE);
        }

        private static void EnsureTroopDirectoryExists()
        {
            if (Directory.Exists("troops")) return;
            Directory.CreateDirectory("troops");
        }
    }
}
