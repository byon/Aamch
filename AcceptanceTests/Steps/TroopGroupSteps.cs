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

        [When(@"troop named ""(.*)"" is selected for a group")]
        public void WhenTroopNamedIsSelectedForAGroup(string name)
        {
            Context.SelectTroop(name);
        }

        [Then(@"the group list contains ""(.*)""")]
        public void ThenTheGroupListContains(string name)
        {
            Context.ViewTroopGroup();
            Assert.IsTrue(Context.GetTroopGroup().Any(t => t["Name"] == name));
        }
    }
}
