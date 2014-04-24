using System;
using TechTalk.SpecFlow;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTests.Steps;

namespace AcceptanceTests
{
    [Binding]
    public class StepDefinitions
    {
        [Given(@"that troops include ""(.*)""")]
        public void GivenThatTroopsInclude(string name)
        {
            Context.EnsureTroopExists(name);
        }
        
        [When(@"troops are viewed")]
        public void WhenTroopsAreViewed()
        {
            Context.GetApplication().Refresh();
            Context.ViewTroops();
        }
        
        [Then(@"""(.*)"" should be included in list of troops")]
        public void ThenShouldBeIncludedInListOfTroops(string name)
        {
            CollectionAssert.Contains(Context.GetTroops(), name);
        }

        [Given(@"that there are no troops")]
        public void GivenThatThereAreNoTroops()
        {
            Context.ResetTroops();
        }

        [Then(@"no troops are included in list of troops")]
        public void ThenNoTroopsAreIncludedInListOfTroops()
        {
            Assert.AreEqual(0, Context.GetTroops().Length);
        }
    }
}
