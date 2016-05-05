using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AcceptanceTests.Support;

namespace AcceptanceTests
{
    [Binding]
    class StepDefinitions
    {
        private TroopFile troopFile;
        private Repository repository;
        private TestedApplication application;

        public StepDefinitions(Repository repository, TroopFile troopFile,
            TestedApplication application)
        {
            this.repository = repository;
            this.troopFile = troopFile;
            this.application = application;
        }

        private delegate void ModifyTroop(Dictionary<string, string> troop);

        [Given(@"that troops include ""(.*)""")]
        public void GivenThatTroopsInclude(string name)
        {
            troopFile.AddTroop(repository, Repository.CreateTroop(name));
        }

        [Given(@"that viewed troops include ""(.*)""")]
        public void GivenThatViewedTroopsInclude(string name)
        {
            troopFile.AddTroop(repository, Repository.CreateTroop(name));
            application.Refresh();
            application.ViewTroops();
        }
        
        [When(@"troops are viewed")]
        public void WhenTroopsAreViewed()
        {
            application.Refresh();
            application.ViewTroops();
        }
        
        [Then(@"""(.*)"" should be included in list of troops")]
        public void ThenShouldBeIncludedInListOfTroops(string name)
        {
            var troops = application.GetTroops();
            Assert.IsTrue(troops.Any(t => t["Name"] == name));
        }

        [Given(@"that there are no troops")]
        public void GivenThatThereAreNoTroops()
        {
            TroopFile.ResetTroops();
        }

        [Then(@"no troops are included in list of troops")]
        public void ThenNoTroopsAreIncludedInListOfTroops()
        {
            Assert.AreEqual(0, application.GetTroops().Count());
        }

        [Given(@"a single troop with (.*) set to (.*)")]
        public void GivenASingleTroopWith(string name, string value)
        {
            AddSingleTroop(t => t[MapToFieldId(name)] = value);
        }

        [Given(@"a single troop without defense")]
        public void GivenASingleTroopWitoutDefense()
        {
            AddSingleTroop(t => t["Fdef"] = t["Rdef"] = null);
        }

        [Then(@"the single troop listed has (.*) of (.*)")]
        public void ThenTheSingleTroopListedHasFieldOf(string id, string value)
        {
            Assert.AreEqual(value, GetTroopValue(id, GetSingleTroop()));
        }

        private void AddSingleTroop(ModifyTroop modifier)
        {
            TroopFile.ResetTroops();
            var troop = Repository.CreateTroop("Troop name");
            modifier(troop);
            troopFile.AddTroop(repository, troop);
        }

        private static string GetTroopValue(string id,
                                            Dictionary<string, string> troop)
        {
            var headerName = MapToHeader(id);
            var error = "Column '" + headerName + "' does not exist";
            CollectionAssert.Contains(troop.Keys, headerName, error);
            return troop[headerName];
        }

        private Dictionary<string, string> GetSingleTroop()
        {
            var troops = application.GetTroops();
            Assert.AreEqual(1, troops.Count());
            return troops.First();
        }

        private static string MapToFieldId(string name)
        {
            var mapping = new Dictionary<string, string>{ {"cost", "Cost"},
                                                          {"type", "Type"},
                                                          {"subtype", "Subtype"},
                                                          {"front defense", "Fdef"},
                                                          {"rear defense", "Rdef"},
                                                          {"attack (soldier/short)", "SS"},
                                                          {"attack (soldier/medium)", "MS"},
                                                          {"attack (soldier/long)", "LS"},
                                                          {"attack (vehicle/short)", "SV"},
                                                          {"attack (vehicle/medium)", "MV"},
                                                          {"attack (vehicle/long)", "LV"},
                                                          {"special abilities", "Special"},
                                                          {"commander ability", "Com Effect"} };
            return mapping[name];
        }

        private static string MapToHeader(string id)
        {
            return char.ToUpper(id[0]) + id.Substring(1);
        }
    }
}
