using BoDi;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{

    [Binding]
    class ApplicationLifetime
    {
        protected static TestedApplication application;
        private static IObjectContainer objectContainer;
        private static bool shouldStart;

        static ApplicationLifetime()
        {
            application = new TestedApplication();
        }

        public ApplicationLifetime(IObjectContainer container)
        {
            objectContainer = container;
            shouldStart = true;
        }

        [BeforeScenario(Order = 1)]
        public void RegisterApplication()
        {
            // By registering the shared application instance here, we ensure
            // that the same instance is used throughout all of the tests.
            // Otherwise a new instance would be initialized for each scenario.
            objectContainer.RegisterInstanceAs<TestedApplication>(
                application);
        }

        [BeforeScenario(Order = 2)]
        [Scope(Tag = "ApplicationIsNotRunningBeforeTest")]
        public void PreventAutomaticStart()
        {
            shouldStart = false;
            application.Exit();
        }

        [BeforeScenario(Order = 3)]
        public void StartIfNeeded()
        {
            if (!shouldStart) return;
            application.Start();
        }

        [AfterTestRun]
        public static void StopApplication()
        {
            application.Exit();
        }

        [AfterScenario(Order = 1)]
        public static void EnsureApplicationIsReset()
        {
            if (application.IsApplicationRunning())
            {
                application.Reset();
            }
        }
    }
}
