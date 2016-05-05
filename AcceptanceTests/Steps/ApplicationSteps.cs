using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using AcceptanceTests.Support;

namespace AcceptanceTests
{
    [Binding]
    public class ApplicationSteps
    {
        private TestedApplication application;

        ApplicationSteps(TestedApplication application)
        {
            this.application = application;
        }

        [Given(@"the application is running")]
        public void GivenTheApplicationIsRunning()
        {
            application.Start();
        }

        [Given(@"the application is just started")]
        public void GivenTheApplicationJustStarted()
        {
            application.Exit();
            application.Start();
        }
        
        [When(@"I close the application")]
        public void WhenICloseTheApplication()
        {
            application.Exit();
        }
        
        [Then(@"application is no longer running")]
        public void ThenApplicationIsNoLongerRunning()
        {
            Assert.IsFalse(application.IsApplicationRunning());
        }

        [Given(@"there is no troop file")]
        public void GivenThereIsNoTroopFile()
        {
            TroopFile.DeleteIfExists();
        }

        [Given(@"the troop file is invalid")]
        public void GivenTheTroopFileIsInvalid()
        {
            TroopFile.CreateInvalidTroopFile();
        }

        [When(@"I start the application")]
        public void WhenIStartTheApplication()
        {
            application.Start();
        }

        [Then(@"application is running")]
        public void ThenApplicationIsRunning()
        {
            Assert.IsTrue(application.IsApplicationRunning());
        }

        [Then(@"status message tells that troop file could not be read")]
        public void ThenStatusMessageTellsThatTroopFileCouldNotBeRead()
        {
            var expected = new Regex("Failed to read troop file '.+'");
            StringAssert.Matches(application.GetStatusMessage(), expected);
        }
    }
}
