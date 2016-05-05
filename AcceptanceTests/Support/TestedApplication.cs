using System.Linq;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        List<Dictionary<string, string>> troops;
        List<Dictionary<string, string>> troopGroup;

        public void Start()
        {
            if (IsApplicationRunning()) return;
            application = Application.Launch(APPLICATION);
        }

        public void Exit()
        {
            if (!IsApplicationRunning()) return;
            GetMainWindow().Close();
        }

        public bool IsApplicationRunning()
        {
            if (application == null) return false;
            return !application.HasExited;
        }

        public void ViewTroops()
        {
            troops = GetListViewItems("troopList");
        }

        public List<Dictionary<string, string>> GetTroops()
        {
            if (troops == null) ViewTroops();
            return troops;
        }

        public void ViewTroopGroup()
        {
            troopGroup = GetListViewItems("troopGroupList");
        }

        public List<Dictionary<string, string>> GetTroopGroup()
        {
            if (troopGroup == null) ViewTroopGroup();
            return troopGroup;
        }

        public void AddTroop(string name)
        {
            DoubleClickListViewItem(name, "troopList");
        }

        public void RemoveTroop(string name)
        {
            DoubleClickListViewItem(name, "troopGroupList");
        }

        public string GetStatusMessage()
        {
            return GetMainWindow().Get<Label>("statusMessage").Text;
        }

        public void Refresh()
        {
            var window = GetMainWindow();
            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F5);
            troops = null;
            troopGroup = null;
        }

        public void Reset()
        {
            var window = GetMainWindow();

            window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.SHIFT);

            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F5);

            window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
            window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.SHIFT);
            troops = null;
            troopGroup = null;
        }

        private List<Dictionary<string, string>>
            GetListViewItems(string listName)
        {
            var item = GetListView(listName);
            var headers = item.Header.Columns.Select(c => c.Text).ToList();
            return item.Rows.Select(r => RowToDictionary(r, headers)).ToList();
        }

        private void DoubleClickListViewItem(string name, string listViewName)
        {
            var listView = GetListView(listViewName);
            Assert.IsTrue(GetTroops().Any(t => t["Name"] == name),
                          "Troop list does not contain troop " + name);
            listView.Select("Name", name);
            listView.SelectedRows.First().DoubleClick();
        }

        private ListView GetListView(string listName)
        {
            return GetMainWindow().Get<ListView>(listName);
        }

        private Dictionary<string, string> RowToDictionary(ListViewRow row,
                                                           List<string> names)
        {
            var values = row.Cells.Select(c => c.Text).ToList();
            var combined = names.Zip(values, (k, v) => new { k, v });
            return combined.ToDictionary(x => x.k, x => x.v);
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
            return GetWindow("Axis and Allies Miniatures Troop Selection " +
                             "Utility");
        }

        private Window GetWindow(string name)
        {
            return application.GetWindow(name, InitializeOption.NoCache);
        }
    }
}
