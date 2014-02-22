using Data;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    class Context
    {
        private static string TROOP_FILE = "troops/troops.json";
        private delegate object Creator();

        public static TestedApplication GetApplication()
        {
            Creator creator = () => new TestedApplication();
            return CachedObject<TestedApplication>("application", creator);
        }

        public static Repository GetRepository()
        {
            Creator creator = () => new Repository();
            return CachedObject<Repository>("TroopRepository", creator);
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
            var application = CachedObject<TestedApplication>("application");
            return application.IsApplicationRunning();
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
                current.Add(id, creator());
            }
            return (T)current[id];
        }
    }
}
