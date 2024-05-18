using System.Windows;
using System.Windows.Controls;
namespace StatisticaCyberAtack
{
    public partial class StartPage : Page
    {
        public StartPage()
        {
             InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("StatistickPage.xaml", UriKind.Relative));
        }
    }
}
