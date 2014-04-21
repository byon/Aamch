using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data;

namespace Aamch
{
    public partial class MainWindow : Window
    {
        Repository repository = new Repository();

        public MainWindow()
        {
            InitializeComponent();
            ShowTroops();
        }

        private void ShowTroops()
        {
            repository.Read(@"Troops\troops.json");
            var troops = repository.GetTroops();
            if (troops.Length > 0)
            {
                this.troopList.Content = troops[0].Name;
            }
        }
    }
}
