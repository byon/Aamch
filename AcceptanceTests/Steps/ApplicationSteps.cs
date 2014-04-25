using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    [Binding]
    public class ApplicationSteps
    {
        [Given(@"the application is running")]
        public void GivenTheApplicationIsRunning()
        {
            Context.EnsureTroopFileExists();
            Context.GetApplication();
        }
        
        [When(@"I close the application")]
        public void WhenICloseTheApplication()
        {
            Context.GetApplication().Exit();
        }
        
        [Then(@"application is no longer running")]
        public void ThenApplicationIsNoLongerRunning()
        {
            Assert.IsFalse(Context.IsApplicationRunning());
        }

        [Given(@"there is no troop file")]
        public void GivenThereIsNoTroopFile()
        {
            Context.EnsureThereIsNoTroopFile();
        }

        [When(@"I start the application")]
        public void WhenIStartTheApplication()
        {
            Context.GetApplication();
        }

        [Then(@"application is running")]
        public void ThenApplicationIsRunning()
        {
            Assert.IsTrue(Context.IsApplicationRunning());
        }

        [Then(@"status message tells that troop file could not be read")]
        public void ThenStatusMessageTellsThatTroopFileCouldNotBeRead()
        {
            var expected = new Regex("Failed to read troop file '.+'");
            StringAssert.Matches(Context.GetStatusMessage(), expected);
        }
    }
}
