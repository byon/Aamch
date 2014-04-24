using System.Collections.ObjectModel;
using System.Windows;
using Data;
using System.Windows.Input;

namespace Aamch
{
    public partial class MainWindow : Window
    {
        Repository repository = new Repository();
        ObservableCollection<Repository.Troop> troopCollection =
            new ObservableCollection<Repository.Troop>();

        public MainWindow()
        {
            InitializeComponent();
            ShowTroops();
        }

        public ObservableCollection<Repository.Troop> TroopCollection
        {
            get { return troopCollection; }
        }

        private void ShowTroops()
        {
            repository.Read(@"Troops\troops.json");
            var troops = repository.GetTroops();
            troopCollection.Clear();
            foreach (var troop in troops)
            {
                troopCollection.Add(troop);
            }
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F5)
            {
                return;
            }

            ShowTroops();
        }
    }
}
