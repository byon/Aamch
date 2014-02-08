using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    class Context
    {
        private delegate object Creator();

        public static TestedApplication GetApplication()
        {
            Creator launch = () => new TestedApplication();
            return CachedObject<TestedApplication>("application", launch);
        }

        public static bool IsApplicationRunning()
        {
            var application = CachedObject<TestedApplication>("application");
            return application.IsApplicationRunning();
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
