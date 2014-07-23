using System.Linq;
using Data;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace AcceptanceTests
{
    class TestedApplication
    {
        private const string APPLICATION_BASE = @"..\..\..\Aamch\bin\";
#if DEBUG
        private const string CONFIGURATION = @"Debug\";
#else
        private const string CONFIGURATION = @"Release\";
#endif
        private const string APPLICATION_DIRECTORY = APPLICATION_BASE +
                                                     CONFIGURATION;
        private const string APPLICATION_NAME = "Aamch.exe";
        private const string APPLICATION = APPLICATION_DIRECTORY +
                                           APPLICATION_NAME;

        private Application application;

        public TestedApplication()
        {
            application = Application.Launch(APPLICATION);
        }

        public void Exit()
        {
            if (IsApplicationRunning())
            {
                GetMainWindow().Close();
            }
        }

        public bool IsApplicationRunning()
        {
            return !application.HasExited;
        }

        public Repository.Troop[] GetTroops()
        {
            var item = GetMainWindow().Get<ListView>("troopList");
            return item.Rows.Select(r => RowToTroop(r)).ToArray();
        }

        public string GetStatusMessage()
        {
            return GetMainWindow().Get<Label>("statusMessage").Text;
        }

        public void Refresh()
        {
            var window = GetMainWindow();
            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F5);
        }

        private Repository.Troop RowToTroop(ListViewRow row)
        {
            var cells = row.Cells;
            var troop = new Repository.Troop(CellValue(cells["Name"]));
            troop.Cost = IntFromCell(cells["Cost"]);
            troop.Type = CellValue(cells["Type"]);
            troop.Subtype = CellValue(cells["Subtype"]);
            troop.Defense.Front = IntFromCell(cells["Front defense"]);
            troop.Defense.Rear = IntFromCell(cells["Rear defense"]);
            return troop;
        }

        private static int IntFromCell(ListViewCell cell)
        {
            return System.Convert.ToInt32(CellValue(cell));
        }

        private static string CellValue(ListViewCell value)
        {
            return value == null ? "" : value.Text;
        }

        private Window GetMainWindow()
        {
            return GetWindow("MainWindow");
        }

        private Window GetWindow(string name)
        {
            return application.GetWindow(name, InitializeOption.NoCache);
        }
    }
}
