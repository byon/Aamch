using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps
{
    [Binding]
    public class EnsureApplicationLifeTime
    {
        [AfterTestRun]
        public static void StopApplication()
        {
            ApplicationLifetime.Stop();
        }

        [BeforeScenario("ApplicationIsNotRunningBeforeTest")]
        public static void EnsureApplicationIsStopped()
        {
            ApplicationLifetime.Stop();
        }
    }
}
