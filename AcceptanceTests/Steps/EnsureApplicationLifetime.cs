using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps
{
    [Binding]
    public class EnsureApplicationLifetime
    {
        [BeforeScenario]
        public static void EnsureApplicationIsRunning()
        {
            /// @todo Explicit start method might be more clear
            Context.GetApplication();
        }

        [AfterFeature]
        public static void EnsureApplicationIsStopped()
        {
            Context.GetApplication().Exit();
        }
    }
}
