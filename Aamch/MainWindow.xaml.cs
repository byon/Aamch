using System.Collections.ObjectModel;
using System.Windows;
using Data;

namespace Aamch
{
    public partial class MainWindow : Window
    {
        Repository repository = new Repository();
        ObservableCollection<Repository.Troop> troopCollection;

        public MainWindow()
        {
            ShowTroops();
            InitializeComponent();
        }

        private void ShowTroops()
        {
            repository.Read(@"Troops\troops.json");
            var troops = repository.GetTroops();
            troopCollection =
                new ObservableCollection<Repository.Troop>(troops);
        }

        public ObservableCollection<Repository.Troop> TroopCollection
        {
            get { return troopCollection; }
        }
    }
}
