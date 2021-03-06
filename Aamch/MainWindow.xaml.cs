﻿using System.Collections.ObjectModel;
using System.Windows;
using Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;

namespace Aamch
{
    public partial class MainWindow : Window
    {
        private Repository repository = new Repository();
        private ObservableCollection<Repository.Troop> troopCollection =
            new ObservableCollection<Repository.Troop>();
        private ObservableCollection<Repository.Troop> troopGroup =
            new ObservableCollection<Repository.Troop>();

        public MainWindow()
        {
            InitializeComponent();
            UpdateTroopsAndShow();
        }

        public ObservableCollection<Repository.Troop> TroopCollection
        {
            get { return troopCollection; }
        }

        public ObservableCollection<Repository.Troop> TroopGroup
        {
            get { return troopGroup; }
        }

        private void ResetApplication()
        {
            troopGroup.Clear();
            UpdateTroopsAndShow();
            statusMessage.Text = "Application was reset";
        }

        private void UpdateTroopsAndShow()
        {
            ReadTroopList();
            var troops = repository.GetTroops();
            UpdateTroopCollection(troops);
        }

        private void ReadTroopList()
        {
            try
            {
                repository.Read(@"Troops\troops.json");
                statusMessage.Text = "Refreshed troop list";
            }
            catch (Repository.IoFailure e)
            {
                statusMessage.Text = e.Message;
            }
        }

        private void UpdateTroopCollection(Repository.Troop[] troops)
        {
            troopCollection.Clear();
            foreach (var troop in troops)
            {
                troopCollection.Add(troop);
            }
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                HandleExplicitRefresh();
            }
        }

        private void HandleExplicitRefresh()
        {
            if (IsFullResetRequested())
            {
                ResetApplication();
                return;
            }

            if (IsTroopUpdateRequested())
            {
                UpdateTroopsAndShow();
            }
        }

        private bool IsFullResetRequested()
        {
            return Keyboard.Modifiers.HasFlag(ModifierKeys.Control) &&
                   Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
        }

        private bool IsTroopUpdateRequested()
        {
            return !Keyboard.Modifiers.HasFlag(ModifierKeys.Control) &&
                   !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
        }

        private void TroopListMouseDoubleClick(object sender,
                                               MouseButtonEventArgs e)
        {
            var selected = troopList.SelectedItem as Repository.Troop;
            troopGroup.Add(selected.Clone());
        }

        private void TroopGroupMouseDoubleClick(object sender,
                                                MouseButtonEventArgs e)
        {
            troopGroup.Remove(troopGroupList.SelectedItem as Repository.Troop);
        }
    }
}
