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
        [AfterFeature]
        public static void StopApplication()
        {
            Context.GetApplication().Exit();
        }
    }
}
