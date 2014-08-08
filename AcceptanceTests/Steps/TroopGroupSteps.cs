using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTests.Steps
{
    [Binding]
    class TroopGroupSteps
    {
        [Given(@"that ""(.*)"" is selected into a troop group")]
        public void GivenThatIsSelectedIntoATroopGroup(string name)
        {
            Context.AddTroop(Repository.CreateTroop(name));
            Context.GetApplication().Refresh();
            Context.SelectTroop(name);
        }

        [When(@"I view the troop group")]
        public void WhenIViewTheTroopGroup()
        {
            Context.ViewTroopGroup();
        }

        [When(@"troop named ""(.*)"" is selected for a group")]
        public void WhenTroopNamedIsSelectedForAGroup(string name)
        {
            Context.AddTroop(Repository.CreateTroop(name));
            Context.SelectTroop(name);
        }

        [When(@"troop named ""(.*)"" is removed from a group")]
        public void WhenTroopNamedIsRemovedFromAGroup(string name)
        {
            Context.RemoveTroop(name);
        }

        [Then(@"the troop group is empty")]
        public void ThenTheTroopGroupIsEmpty()
        {
            Assert.AreEqual(0, Context.GetTroopGroup().Count());
        }

        [Then(@"the group list contains ""(.*)""")]
        public void ThenTheGroupListContains(string name)
        {
            Context.ViewTroopGroup();
            Assert.IsTrue(IsTroopSelected(name));
        }

        [Then(@"the group list does not contain ""(.*)""")]
        public void ThenTheGroupListDoesNotContain(string name)
        {
            Context.ViewTroopGroup();
            Assert.IsFalse(IsTroopSelected(name));
        }

        private static bool IsTroopSelected(string name)
        {
            return Context.GetTroopGroup().Any(t => t["Name"] == name);
        }
    }
}
