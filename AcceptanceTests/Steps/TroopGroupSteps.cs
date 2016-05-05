using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTests.Support;

namespace AcceptanceTests.Steps
{
    [Binding]
    class TroopGroupSteps
    {
        private TroopFile troopFile;
        private Repository repository;
        private TestedApplication application;

        public TroopGroupSteps(Repository repository, TroopFile troopFile,
            TestedApplication application)
        {
            this.repository = repository;
            this.troopFile = troopFile;
            this.application = application;
        }

        [Given(@"that ""(.*)"" is in a troop group")]
        public void GivenThatIsInATroopGroup(string name)
        {
            troopFile.AddTroop(repository, Repository.CreateTroop(name));
            application.Refresh();
            application.AddTroop(name);
        }

        [When(@"I view the troop group")]
        public void WhenIViewTheTroopGroup()
        {
            application.ViewTroopGroup();
        }

        [When(@"troop named ""(.*)"" is added to a group")]
        public void WhenTroopNamedIsSelectedForAGroup(string name)
        {
            troopFile.AddTroop(repository, Repository.CreateTroop(name));
            application.AddTroop(name);
        }

        [When(@"troop named ""(.*)"" is removed from a group")]
        public void WhenTroopNamedIsRemovedFromAGroup(string name)
        {
            application.RemoveTroop(name);
        }

        [Then(@"the troop group is empty")]
        public void ThenTheTroopGroupIsEmpty()
        {
            Assert.AreEqual(0, application.GetTroopGroup().Count());
        }

        [Then(@"the group list contains ""(.*)""")]
        public void ThenTheGroupListContainsTroop(string name)
        {
            application.ViewTroopGroup();
            Assert.IsTrue(IsTroopInAGroup(name),
                          "Troop " + name + " is not in a group");
        }

        [Then(@"the group list contains ""(.*)"" (\d+) times")]
        public void ThenTheGroupListContainsTroopNTimes(string name, int count)
        {
            var troops = application.GetTroopGroup();
            var matching = troops.Select(t => t["Name"] == name);
            Assert.AreEqual(count, matching.Count());
        }

        [Then(@"the group list does not contain ""(.*)""")]
        public void ThenTheGroupListDoesNotContain(string name)
        {
            application.ViewTroopGroup();
            Assert.IsFalse(IsTroopInAGroup(name));
        }

        private bool IsTroopInAGroup(string name)
        {
            return application.GetTroopGroup().Any(t => t["Name"] == name);
        }
    }
}
