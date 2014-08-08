using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTests.Steps
{
    [Binding]
    class TroopGroupSteps
    {
        [Given(@"that ""(.*)"" is in a troop group")]
        public void GivenThatIsInATroopGroup(string name)
        {
            Context.AddTroop(Repository.CreateTroop(name));
            Context.GetApplication().Refresh();
            Context.AddTroopToGroup(name);
        }

        [When(@"I view the troop group")]
        public void WhenIViewTheTroopGroup()
        {
            Context.ViewTroopGroup();
        }

        [When(@"troop named ""(.*)"" is added to a group")]
        public void WhenTroopNamedIsSelectedForAGroup(string name)
        {
            Context.AddTroop(Repository.CreateTroop(name));
            Context.AddTroopToGroup(name);
        }

        [When(@"troop named ""(.*)"" is removed from a group")]
        public void WhenTroopNamedIsRemovedFromAGroup(string name)
        {
            Context.RemoveTroopFromGroup(name);
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
            Assert.IsTrue(IsTroopInAGroup(name),
                          "Troop " + name + " is not in a group");
        }

        [Then(@"the group list does not contain ""(.*)""")]
        public void ThenTheGroupListDoesNotContain(string name)
        {
            Context.ViewTroopGroup();
            Assert.IsFalse(IsTroopInAGroup(name));
        }

        private static bool IsTroopInAGroup(string name)
        {
            return Context.GetTroopGroup().Any(t => t["Name"] == name);
        }
    }
}
