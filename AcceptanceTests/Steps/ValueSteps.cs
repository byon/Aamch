using System.Linq;
using TechTalk.SpecFlow;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTests
{
    [Binding]
    public class StepDefinitions
    {
        [Given(@"that troops include ""(.*)""")]
        public void GivenThatTroopsInclude(string name)
        {
            Context.AddTroop(CreateTroop(name));
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
            Assert.IsTrue(Context.GetTroops().Any(t => t.Name == name));
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

        [Given(@"a single troop with cost (.*)")]
        public void GivenASingleTroopWithCost(int cost)
        {
            Context.ResetTroops();
            var troop = new Repository.Troop("Troop name");
            troop.Cost = cost;
            Context.AddTroop(troop);
        }

        [Then(@"the single troop listed has cost of (.*)")]
        public void ThenTheSingleTroopListedHasCostOf(int cost)
        {
            var troops = Context.GetTroops();
            Assert.AreEqual(1, troops.Length);
            Assert.AreEqual(cost, troops[0].Cost);
        }

        private static Repository.Troop CreateTroop(string name)
        {
            return new Repository.Troop(name);
        }
    }
}
