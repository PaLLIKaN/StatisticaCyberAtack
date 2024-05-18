using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StatisticaCyberAtack
{
    public partial class StatistickPage : Page
    {
        public string Path;
        public StatistickPage()
        {
            InitializeComponent();
        }
        private void Function_selection_Combobox(object sender, SelectionChangedEventArgs e)
        {
            HideTabs();
            if (Function_selection.SelectedIndex == 0)
            {

                string[,] start_table_score = new DescriptiveStatistics().Score_table(Path);
                TableScore.Children.Clear();
                Start_table_Output(start_table_score);
            }
            else if (Function_selection.SelectedIndex == 1)
            {
                string[,] start_table = new DescriptiveStatistics().reception_tableNorm(Path);
                string[,] start_table_norm = new DescriptiveStatistics().Table_norm(start_table);
                TableScore.Children.Clear();
                Start_table_Output(start_table_norm);
            }
            else
            {
                string[,] start_table = new DescriptiveStatistics().reception_table(Path);
                Start_table_Output(start_table);
            }
        }

        public void Start_table_Output(string[,] start_table)
        {
            TableScore.Children.Clear();
            TableScore.RowDefinitions.Clear();
            TableScore.ColumnDefinitions.Clear();

            for (int i = 0; i < start_table.GetLength(1); i++)
            {
                TableScore.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int j = 0; j < start_table.GetLength(0); j++)
            {
                TableScore.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < start_table.GetLength(0); i++)
            {
                for (int j = 0; j < start_table.GetLength(1); j++)
                {
                    TextBlock celltable = new TextBlock();
                    celltable.Text = start_table[i, j];
                    celltable.HorizontalAlignment = HorizontalAlignment.Center;
                    celltable.TextWrapping = TextWrapping.Wrap;
                    celltable.FontSize = SystemFonts.MessageFontSize;
                    celltable.Foreground = Brushes.White;
                    Grid.SetRow(celltable, i);
                    Grid.SetColumn(celltable, j);

                    TableScore.Children.Add(celltable);
                }
            }
        }

        public List<List<double>> ConvertDoulbeToListList(double[,] input)
        {
            List<List<double>> list2D = new List<List<double>>();
            for (int i = 0; i < input.GetLength(0); i++)
            {
                List<double> row = new List<double>();
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    row.Add(input[i, j]);
                }
                list2D.Add(row);
            }
            return list2D;
        }


        public void StartTableOutputNorm(string[,] startTable)
        {
            TableScore.Children.Clear();
            TableScore.RowDefinitions.Clear();
            TableScore.ColumnDefinitions.Clear();

            for (int i = 0; i < startTable.GetLength(1); i++)
            {
                TableScore.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int j = 0; j < startTable.GetLength(0); j++)
            {
                TableScore.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < startTable.GetLength(0); i++)
            {
                for (int j = 0; j < startTable.GetLength(1); j++)
                {

                    TextBlock cell = new TextBlock();
                    cell.Foreground = Brushes.White;
                    cell.Text = startTable[i, j];
                    cell.TextAlignment = TextAlignment.Center;
                    cell.TextWrapping = TextWrapping.Wrap;
                    cell.FontSize = SystemFonts.MessageFontSize;
                    if (startTable[i, j] == "Да")
                        cell.Background = new SolidColorBrush(Colors.Green);
                    else if (startTable[i, j] == "Нет")
                        cell.Background = new SolidColorBrush(Colors.Red);
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);

                    TableScore.Children.Add(cell);
                }
            }
        }

        private void Button_Click_InputFile(object sender, RoutedEventArgs e) // Получение строки пути к файлу
        {

            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                string[,] start_table = new DescriptiveStatistics().reception_table(openFileDlg.FileName);
                Path = openFileDlg.FileName;
                Start_table_Output(start_table);
                Function_selection.IsEnabled = true;
                Function_Correlation.IsEnabled = true;
                Function_Norm.IsEnabled = true;
                Function_Regress.IsEnabled = true;
                //Korreletion.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Выберите файл с таблицей!", "Файл с таблицей не выбран", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        public void StartTableOutputCorel(string[,] startTable)
        {
            TableScore.Children.Clear();
            TableScore.RowDefinitions.Clear();
            TableScore.ColumnDefinitions.Clear();

            for (int i = 0; i < startTable.GetLength(1); i++)
            {
                TableScore.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int j = 0; j < startTable.GetLength(0); j++)
            {
                TableScore.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < startTable.GetLength(0); i++)
            {
                for (int j = 0; j < startTable.GetLength(1); j++)
                {

                    TextBlock cell = new TextBlock();
                    cell.Foreground = Brushes.White;
                    cell.Text = startTable[i, j];
                    cell.TextAlignment = TextAlignment.Center;
                    cell.TextWrapping = TextWrapping.Wrap;
                    cell.FontSize = SystemFonts.MessageFontSize;
                    if (IsIntegerOrDouble(startTable[i, j]))
                    {
                        double cellValue = double.Parse(startTable[i, j]);
                        if (cellValue >= 0.7)
                            cell.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        else if (cellValue < 0.7 && cellValue >= 0.5)
                            cell.Background = new SolidColorBrush(Color.FromRgb(200, 0, 0));
                        else if (cellValue < 0.5 && cellValue >= 0.3)
                            cell.Background = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                        else if (cellValue < 0.3 && cellValue >= 0.2)
                            cell.Background = new SolidColorBrush(Color.FromRgb(90, 0, 0));
                        else
                            cell.Background = new SolidColorBrush(Color.FromRgb(60, 0, 0));
                    }
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    TableScore.Children.Add(cell);
                }
            }
        }

        public bool IsIntegerOrDouble(string value)
        {
            return Regex.IsMatch(value, @"^-?\d+(\,\d+)?$");
        }

        private void Function_Cor_Combobox(object sender, SelectionChangedEventArgs e)
        {
            HideTabs();
            if (Function_Correlation.SelectedIndex == 0)
            {
                MainWindow.index = false;
                string[,] start_table = MainWindow.myTable;
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
                var K = new DescriptiveStatistics().rationing(Table);
                var Score_Table = new PairesCorrelationCoefficient().Tables_of_correlation_coefficients(K);
                TableScore.Children.Clear();
                string[,] startTable = ConvertListListToStringArray(Score_Table);
                StartTableOutputCorel(startTable);
                ShowTabs();
                Pleiad pleiad = new Pleiad();
                pleiad.Show();
            }
            else if (Function_Correlation.SelectedIndex == 1) 
            {
                MainWindow.index = true;
                string[,] start_table = MainWindow.myTable;
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
                var K = new DescriptiveStatistics().rationing(Table);
                var Score_Table = new PairesCorrelationCoefficient().Tables_of_correlation_coefficients(K);
                var T = new PairesCorrelationCoefficient().Tables_of_correlation_private_coefficients(new Pleiad().Matrix_table(Score_Table));
                TableScore.Children.Clear();
                ShowTabs();
                string[,] startTable = ConvertDoubleToStringArray(T);
                StartTableOutputCorel(startTable);
                Pleiad pleiad = new Pleiad();
                pleiad.Show();

            }
            else if (Function_Correlation.SelectedIndex == 2)
            {
                string[,] start_table = MainWindow.myTable;
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
                var K = new DescriptiveStatistics().rationing(Table);
                var Score_Table = new PairesCorrelationCoefficient().Tables_of_correlation_coefficients(K);
                var Student = new PairesCorrelationCoefficient().Table_Student(Score_Table, Table.GetLength(0));
                string[,] strStudent = ConvertListListToStringArray(Student);
                StartTableOutputStudent(strStudent);

            }
            else if (Function_Correlation.SelectedIndex == 3)
            {
                string[,] start_table = MainWindow.myTable;
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
                var K = new DescriptiveStatistics().rationing(Table);
                var Score_Table = new PairesCorrelationCoefficient().Tables_of_correlation_coefficients(K);
                var T = ConvertDoulbeToListList(new PairesCorrelationCoefficient().Tables_of_correlation_private_coefficients(new Pleiad().Matrix_table(Score_Table)));
                var Student = new PairesCorrelationCoefficient().Table_Student(T, Table.GetLength(0));
                string[,] strStudent = ConvertListListToStringArray(Student);
                StartTableOutputStudent(strStudent);

            }
        }

        public static string[,] ConvertListListToStringArray(List<List<double>> input)
        {
            int rowCount = input.Count + 1;
            int colCount = input[0].Count + 1;

            string[,] output = new string[rowCount, colCount];
            for (int j = 0; j < colCount; j++)
            {
                output[0, j] = MainWindow.myTable[0, j];
                output[j, 0] = MainWindow.myTable[0, j];
            }

            for (int i = 0; i < rowCount - 1; i++)
            {
                for (int j = 0; j < colCount - 1; j++)
                {
                    output[i + 1, j + 1] = input[i][j].ToString();
                }
            }
            return output;
        }

        private void ShowTabs()
        {
            firstTab.Visibility = Visibility.Visible;
            firstTab.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            secondTab.Visibility = Visibility.Visible;
            secondTab.Background = new SolidColorBrush(Color.FromRgb(200, 0, 0));

            thirdTab.Visibility = Visibility.Visible;
            thirdTab.Background = new SolidColorBrush(Color.FromRgb(140, 0, 0));

            fourTab.Visibility = Visibility.Visible;
            fourTab.Background = new SolidColorBrush(Color.FromRgb(90, 0, 0));

            fiveTab.Visibility = Visibility.Visible;
            fiveTab.Background = new SolidColorBrush(Color.FromRgb(60, 0, 0));
        }

        private void HideTabs()
        {
            firstTab.Visibility = Visibility.Hidden;
            secondTab.Visibility = Visibility.Hidden;
            thirdTab.Visibility = Visibility.Hidden;
            fourTab.Visibility = Visibility.Hidden;
            fiveTab.Visibility = Visibility.Hidden;
        }

        private void Function_Norm_Combobox(object sender, SelectionChangedEventArgs e)
        {
            if(Function_Norm.SelectedIndex == 0)
            {
                string[,] start_table = new DescriptiveStatistics().reception_tableNorm(Path);
                string[,] start_table_norm = new DescriptiveStatistics().Table_norm(start_table);
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table_norm);
                var Normality = new NormalityDistribution().Score_N(Table);
                var result = new DescriptiveStatistics().Score_table_Norm(Normality);
                StartTableOutputNorm(result);
            }
        }

        public string[,] ConvertDoubleToStringArray(double[,] input)
        {
            int rowCount = input.GetLength(0) + 1;
            int colCount = input.GetLength(1) + 1;

            string[,] output = new string[rowCount, colCount];
            for (int j = 0; j < colCount; j++)
            {
                output[0, j] = MainWindow.myTable[0, j];
                output[j, 0] = MainWindow.myTable[0, j];
            }

            for (int i = 0; i < rowCount - 1; i++)
            {
                for (int j = 0; j < colCount - 1; j++)
                {
                    output[i + 1, j + 1] = Math.Round(input[i, j], 4).ToString();
                }
            }
            return output;
        }

        public void StartTableOutputStudent(string[,] startTable)
        {
            TableScore.Children.Clear();
            TableScore.RowDefinitions.Clear();
            TableScore.ColumnDefinitions.Clear();

            for (int i = 0; i < startTable.GetLength(1); i++)
            {
                TableScore.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int j = 0; j < startTable.GetLength(0); j++)
            {
                TableScore.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < startTable.GetLength(0); i++)
            {
                for (int j = 0; j < startTable.GetLength(1); j++)
                {

                    TextBlock cell = new TextBlock();
                    cell.Foreground = Brushes.White;
                    cell.Text = startTable[i, j];
                    cell.TextAlignment = TextAlignment.Center;
                    cell.TextWrapping = TextWrapping.Wrap;
                    cell.FontSize = SystemFonts.MessageFontSize;
                    if (IsIntegerOrDouble(startTable[i, j]))
                    {
                        double cellValue = double.Parse(startTable[i, j]);
                        if (cellValue >= 2.13)
                            cell.Background = new SolidColorBrush(Colors.Green);
                        else
                            cell.Background = new SolidColorBrush(Colors.Red);
                    }
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);

                    TableScore.Children.Add(cell);
                }
            }
        }

        private void Function_Reg_Combobox(object sender, SelectionChangedEventArgs e)
        {
            if (Function_Regress.SelectedIndex == 0)
            {
                string[,] start_table = MainWindow.myTable;
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
                var regress = new Regressions().Regress(Table, 0);
                var result = new Regressions().OutputRegress(regress);
                StartTableOutput(result);
                var equat = new Regressions().RegressionEquation(regress);
                TextBlock cell = new TextBlock();
                cell.Foreground = Brushes.White;
                cell.Text = equat;
                cell.TextAlignment = TextAlignment.Center;
                cell.TextWrapping = TextWrapping.Wrap;
                cell.FontSize = 20;
                Grid.SetRow(cell, 4);
                Grid.SetColumn(cell, 0);
                Grid.SetColumnSpan(cell, 10);
                TableScore.Children.Add(cell);
            }
            else if (Function_Regress.SelectedIndex == 1)
            {
                string[,] start_table = MainWindow.myTable;
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
                var regress = new Regressions().Regress(Table, 0);
                var newY = new Regressions().New_Y(regress, Table, 0);
                var Y = new Regressions().Y(Table, 0);
                var E = new Regressions().E(newY, Table, 0);
                string[,] solution = new Regressions().TableSrav(newY, E, Y);
                StartTableOutput(solution);
            }
            else if (Function_Regress.SelectedIndex == 2)
            {
                string[,] start_table = MainWindow.myTable;
                double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
                var regress = new Regressions().Regress(Table, 0);
                var newY = new Regressions().New_Y(regress, Table, 0);
                var Y = new Regressions().Y(Table, 0);
                var E = new Regressions().E(newY, Table, 0);
                var Criteria = new Regressions().Criteria(newY, Table, 0);
                string[,] solution = new Regressions().TableCriteria(Criteria);
                StartTableOutput(solution);
            }
        }
        public void StartTableOutput(string[,] startTable)
        {
            TableScore.Children.Clear();
            TableScore.RowDefinitions.Clear();
            TableScore.ColumnDefinitions.Clear();

            for (int i = 0; i < startTable.GetLength(1); i++)
            {
                TableScore.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int j = 0; j < startTable.GetLength(0); j++)
            {
                TableScore.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < startTable.GetLength(0); i++)
            {
                for (int j = 0; j < startTable.GetLength(1); j++)
                {
                    TextBlock cell = new TextBlock
                    {
                        Foreground = Brushes.White,
                        Text = startTable[i, j],
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = SystemFonts.MessageFontSize
                    };
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    TableScore.Children.Add(cell);
                }
            }
        }
    }
}
