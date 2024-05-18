using System.Windows;

namespace StatisticaCyberAtack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[,] myTable;
        public static bool index = false;
        public MainWindow()
        {
            InitializeComponent();
            _NavigationFrame.Navigate(new StartPage());
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            if (result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}