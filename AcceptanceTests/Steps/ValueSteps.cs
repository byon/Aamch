using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AcceptanceTests
{
    [Binding]
    public class StepDefinitions
    {
        private delegate void ModifyTroop(Dictionary<string, string> troop);

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

        private static void AddSingleTroop(ModifyTroop modifier)
        {
            Context.ResetTroops();
            var troop = CreateTroop("Troop name");
            modifier(troop);
            Context.AddTroop(troop);
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
            var troops = Context.GetTroops();
            Assert.AreEqual(1, troops.Count());
            return troops.First();
        }

        private static Dictionary<string, string> CreateTroop(string name)
        {
            return new Dictionary<string, string> { { "Name", name },
                                                    { "Cost", "0" },
                                                    { "Type", "" },
                                                    { "Subtype", "" },
                                                    { "Fdef", "0" },
                                                    { "Rdef", "0" },
                                                    { "SS", "0" },
                                                    { "MS", "0" },
                                                    { "LS", "0" },
                                                    { "SV", "0" },
                                                    { "MV", "0" },
                                                    { "LV", "0" },
                                                    { "Special", "0" } };
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
                                                          {"special abilities", "Special"} };
            return mapping[name];
        }

        private static string MapToHeader(string id)
        {
            return char.ToUpper(id[0]) + id.Substring(1);
        }
    }
}
