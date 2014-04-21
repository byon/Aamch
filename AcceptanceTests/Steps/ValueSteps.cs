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
            Context.ViewTroops();
        }
        
        [Then(@"""(.*)"" should be included in list of troops")]
        public void ThenShouldBeIncludedInListOfTroops(string name)
        {
            CollectionAssert.Contains(Context.GetTroops(), name);
        }
    }
}
