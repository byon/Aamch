using TechTalk.SpecFlow;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace AcceptanceTests
{
    class Context
    {
        private const string APPLICATION_BASE = @"..\..\..\Aamch\bin\";
#if DEBUG
        private const string CONFIGURATION = @"Debug\";
#else
        private const string CONFIGURATION = @"Release\";
#endif
        private const string APPLICATION_DIRECTORY = APPLICATION_BASE +
                                                     CONFIGURATION;
        private const string APPLICATION_NAME = "Aamch.exe";
        private const string APPLICATION = APPLICATION_DIRECTORY +
                                           APPLICATION_NAME;

        private delegate object Creator();

        /// @todo Should not return White specific objects, but wrappers

        public static Application GetApplication()
        {
            Creator launch = () => Application.Launch(APPLICATION);
            return CachedObject<Application>("application", launch);
        }

        public static Window GetMainWindow()
        {
            return GetWindow("MainWindow");
        }

        public static void CloseMainWindow()
        {
            GetMainWindow().Close();
            ScenarioContext.Current.Remove("application");
        }

        private static Window GetWindow(string name)
        {
            return GetApplication().GetWindow(name, InitializeOption.NoCache);
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
