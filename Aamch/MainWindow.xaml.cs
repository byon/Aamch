using System.Collections.ObjectModel;
using System.Windows;
using Data;
using System.Windows.Input;

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
            ShowTroops();
        }

        public ObservableCollection<Repository.Troop> TroopCollection
        {
            get { return troopCollection; }
        }

        public ObservableCollection<Repository.Troop> TroopGroup
        {
            get { return troopGroup; }
        }

        private void ShowTroops()
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
            if (e.Key != Key.F5)
            {
                return;
            }

            ShowTroops();
        }

        private void ListViewMouseDoubleClick(object sender,
                                              MouseButtonEventArgs e)
        {
            troopGroup.Add(troopList.SelectedItem as Repository.Troop);
        }
    }
}
