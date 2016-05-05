using System.IO;
using TechTalk.SpecFlow;

namespace AcceptanceTests.Steps
{
    [Binding]
    public class CleanTroopData
    {
        [BeforeScenario(Order = 0)]
        [AfterScenario(Order = 0)]
        public void RemoveTroopDirectory()
        {
            const string TROOP_DIRECTORY = "Troops";
            if (Directory.Exists(TROOP_DIRECTORY))
            {
                Directory.Delete(TROOP_DIRECTORY, true);
            }
        }
    }
}
