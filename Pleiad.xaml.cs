using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StatisticaCyberAtack
{
    public partial class Pleiad : Window
    {
        public Pleiad()
        {
            InitializeComponent();
            if (MainWindow.index == false)
                ShowPleiad();
            else if (MainWindow.index == true)
                ShowPleiadPrivate();
        }

        private void ShowPleiadPrivate()
        {
            StatistickPage mainPage = new StatistickPage();
            string[,] start_table = MainWindow.myTable;
            double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
            var K = new DescriptiveStatistics().rationing(Table);
            var Score_Table = new PairesCorrelationCoefficient().Tables_of_correlation_coefficients(K);
            var T = new PairesCorrelationCoefficient().Tables_of_correlation_private_coefficients(Matrix_table(Score_Table));
            ShowPleiadesChart(Matrix_table(mainPage.ConvertDoulbeToListList(T)), start_table);
        }

        private void ShowPleiad()
        {
            string[,] start_table = MainWindow.myTable;
            double[,] Table = new DescriptiveStatistics().Sample_values(start_table);
            var K = new DescriptiveStatistics().rationing(Table);
            var Score_Table = new PairesCorrelationCoefficient().Tables_of_correlation_coefficients(K);
            ShowPleiadesChart(Matrix_table(Score_Table), start_table);
        }
        public double[,] Matrix_table(List<List<double>> A)
        {
            int row = A[0].Count;
            int cols = A.Count;
            double[,] list = new double[row, cols];
            for (int i = 0; i < cols; i++)
            {
                List<double> list2 = A[i];
                for (int j = 0; j < row; j++)
                {
                    list[j, i] = list2[j];
                }
            }
            return list;
        }
        private void ShowPleiadesChart(double[,] correlationMatrix, string[,] A)
        {
            int numColumns = correlationMatrix.GetLength(0);
            double angleStep = 360.0 / numColumns;
            double centerX = CanvasContainer.ActualWidth / 2;
            double centerY = CanvasContainer.ActualHeight / 2;
            double radius = Math.Min(centerX, centerY) - 270;

            for (int i = 0; i < numColumns; i++)
            {
                double angle = angleStep * i;
                double labelX = centerX + radius * Math.Cos(angle * Math.PI / 180);
                double labelY = centerY + radius * Math.Sin(angle * Math.PI / 180);

                for (int j = i + 1; j < numColumns; j++)
                {
                    double correlation = correlationMatrix[i, j];

                    Brush lineColor = GetLineColor(correlation);

                    Line connectionLine = new Line
                    {
                        X1 = labelX,
                        Y1 = labelY,
                        X2 = centerX + radius * Math.Cos(angleStep * j * Math.PI / 180),
                        Y2 = centerY + radius * Math.Sin(angleStep * j * Math.PI / 180),
                        Stroke = lineColor,
                        StrokeThickness = 5
                    };

                    CanvasContainer.Children.Add(connectionLine);
                }


            }
            for (int i = 0; i < numColumns; i++)
            {
                double angle = angleStep * i;
                string columnName = A[0, i + 1];
                double labelX = centerX + radius * Math.Cos(angle * Math.PI / 180);
                double labelY = centerY + radius * Math.Sin(angle * Math.PI / 180);
                TextBlock columnLabel = new TextBlock
                {
                    Text = columnName,
                    Foreground = Brushes.White,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Width = 100
                };

                double labelWidth = columnLabel.ActualWidth;
                double labelHeight = columnLabel.ActualHeight;
                Canvas.SetLeft(columnLabel, labelX - labelWidth / 2);
                Canvas.SetTop(columnLabel, labelY - labelHeight / 2);
                CanvasContainer.Children.Add(columnLabel);
            }
        }
        private Brush GetLineColor(double correlation)
        {
            if (correlation >= 0.7)
                return new SolidColorBrush(Colors.Red);
            else if (correlation < 0.7 && correlation >= 0.5)
                return new SolidColorBrush(Colors.Orange);
            else if (correlation < 0.5 && correlation >= 0.3)
                return new SolidColorBrush(Colors.Blue);
            else if (correlation < 0.3 && correlation >= 0.2)
                return new SolidColorBrush(Colors.Purple);
            else
                return new SolidColorBrush(Colors.Green);
        }
    }
}
