using System.Linq;
using TechTalk.SpecFlow;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AcceptanceTests
{
    [Binding]
    public class StepDefinitions
    {
        private delegate void ModifyTroop(Repository.Troop troop);

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
            var troops = Context.GetTroops();
            Assert.IsTrue(troops.Any(t => t["Name"] == name));
        }

        [Given(@"that there are no troops")]
        public void GivenThatThereAreNoTroops()
        {
            Context.ResetTroops();
        }

        [Then(@"no troops are included in list of troops")]
        public void ThenNoTroopsAreIncludedInListOfTroops()
        {
            Assert.AreEqual(0, Context.GetTroops().Count());
        }

        [Given(@"a single troop with cost (.*)")]
        public void GivenASingleTroopWithCost(int cost)
        {
            AddSingleTroop(t => t.Cost = cost);
        }

        [Given(@"a single troop with type (.*)")]
        public void GivenASingleTroopWithType(string type)
        {
            AddSingleTroop(t => t.Type = type);
        }

        [Given(@"a single troop with subtype (.*)")]
        public void GivenASingleTroopWithSubtype(string type)
        {
            AddSingleTroop(t => t.Subtype = type);
        }

        [Given(@"a single troop with front defense (.*)")]
        public void GivenASingleTroopWithFrontDefense(int defense)
        {
            AddSingleTroop(t => t.Defense.Front = defense);
        }

        [Given(@"a single troop with rear defense (.*)")]
        public void GivenASingleTroopWithRearDefense(int defense)
        {
            AddSingleTroop(t => t.Defense.Rear = defense);
        }

        [Then(@"the single troop listed has cost of (.*)")]
        public void ThenTheSingleTroopListedHasCostOf(string cost)
        {
            Assert.AreEqual(cost, GetSingleTroop()["Cost"]);
        }

        [Then(@"the single troop listed has type of (.*)")]
        public void ThenTheSingleTroopListedHasTypeOf(string type)
        {
            Assert.AreEqual(type, GetSingleTroop()["Type"]);
        }

        [Then(@"the single troop listed has subtype of (.*)")]
        public void ThenTheSingleTroopListedHasSubtypeOf(string type)
        {
            Assert.AreEqual(type, GetSingleTroop()["Subtype"]);
        }

        [Then(@"the single troop listed has front defense of (.*)")]
        public void ThenTheSingleTroopListedHasFrontDefenseOf(string type)
        {
            Assert.AreEqual(type, GetSingleTroop()["Front defense"]);
        }

        [Then(@"the single troop listed has rear defense of (.*)")]
        public void ThenTheSingleTroopListedHasRearDefenseOf(string type)
        {
            Assert.AreEqual(type, GetSingleTroop()["Rear defense"]);
        }

        private static void AddSingleTroop(ModifyTroop modifier)
        {
            Context.ResetTroops();
            var troop = new Repository.Troop("Troop name");
            modifier(troop);
            Context.AddTroop(troop);
        }

        private Dictionary<string, string> GetSingleTroop()
        {
            var troops = Context.GetTroops();
            Assert.AreEqual(1, troops.Count());
            return troops.First();
        }

        private static Repository.Troop CreateTroop(string name)
        {
            return new Repository.Troop(name);
        }
    }
}
