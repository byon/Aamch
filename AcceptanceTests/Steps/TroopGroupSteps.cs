using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTests.Steps
{
    [Binding]
    class TroopGroupSteps
    {
        [When(@"I view the troop group")]
        public void WhenIViewTheTroopGroup()
        {
            Context.ViewTroopGroup();
        }

        [Then(@"the troop group is empty")]
        public void ThenTheTroopGroupIsEmpty()
        {
            Assert.AreEqual(0, Context.GetTroopGroup().Count());
        }
    }
}
