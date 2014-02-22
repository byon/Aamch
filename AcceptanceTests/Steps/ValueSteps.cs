using System;
using TechTalk.SpecFlow;
using Data;

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
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"""(.*)"" should be included in list of troops")]
        public void ThenShouldBeIncludedInListOfTroops(string name)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
