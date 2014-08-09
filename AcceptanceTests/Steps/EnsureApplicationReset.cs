using TechTalk.SpecFlow;

namespace AcceptanceTests.Support
{
    [Binding]
    class EnsureApplicationReset
    {
        [AfterScenario]
        public static void EnsureApplicationIsReset()
        {
            var application = ApplicationLifetime.GetApplication();
            if (application != null &&
                application.IsApplicationRunning())
            {
                application.Reset();
            }
        }
    }
}
