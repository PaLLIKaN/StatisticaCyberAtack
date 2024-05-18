using Aspose.Cells;

namespace StatisticaCyberAtack
{
    public class NormalityDistribution
    {
        public List<List<double>> List_table(double[,] A)
        {
            List<List<double>> list = new List<List<double>>();
            for (int i = 0; i < A.GetLength(1); i++)
            {
                List<double> list2 = new List<double>();
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    list2.Add(A[j, i]);
                }
                list.Add(list2);
            }
            return list;
        }

        public double[] Create_Intervals(List<double> Columns)
        {
            int k = Convert.ToInt32(1 + 3.322 * Math.Log10(Columns.Count()));
            double[] Intervals = new double[k + 1];
            Intervals[0] = 0;
            for (int i = 1; i < Intervals.Length; i++)
            {
                Intervals[i] = 1.0 / k + Intervals[i - 1];
            }
            return Intervals;
        }

        public List<double> Create_middle_Intervals(double[] Intervals)
        {
            List<double> Intervals_middle = new List<double>();
            for (int i = 0; i < Intervals.Length - 1; i++)
            {
                Intervals_middle.Add((Intervals[i] + Intervals[i + 1]) / 2);
            }
            return Intervals_middle;
        }


        public List<List<int>> Frequency_selection(List<List<double>> Table_list) //Отбор частот
        {
            List<List<int>> Table_Intervals_frequency = new List<List<int>>();
            foreach (var Table_table in Table_list)
            {
                var Intervals = Create_Intervals(Table_table);

                List<int> Intervals_frequency = new List<int>();
                for (int i = 0; i < Intervals.Length - 1; i++)
                {
                    int result;
                    if (i == 0)
                    {
                        result = Table_table.Count(r => r >= Intervals[i] && r < Intervals[i + 1]);
                    }
                    else
                    {
                        result = Table_table.Count(r => r > Intervals[i] && r <= Intervals[i + 1]);
                    }
                    Intervals_frequency.Add(result);
                }

                Table_Intervals_frequency.Add(Intervals_frequency);
            }
            return Table_Intervals_frequency;
        }

        public double Sample_average(List<double> Middle_Int, List<int> X, int V)
        {
            double Xa = 0;
            for (int i = 0; i < Middle_Int.Count; i++)
            {
                Xa += Middle_Int[i] * X[i];
            }
            return Xa / V;
        }

        public double Sample_average_quadro(List<double> Middle_Int, List<int> X, int V) //Выборочная средняя в квадрате
        {
            double Xa = 0;
            for (int i = 0; i < Middle_Int.Count; i++)
            {
                Xa += Math.Pow(Middle_Int[i], 2) * X[i];
            }
            return Xa / V;
        }

        public double Sample_variance(double Xa, double XaQ) //Выборочная дисперсия
        {
            double D = XaQ - Math.Pow(Xa, 2);
            return D;
        }

        public double Update_Sample_variance(double D, int V) //Исправленная выборочная дисперсия
        {
            double SQ = V * D / (V - 1);
            return SQ;
        }

        public double Corrected_quadratic_mean_score(List<double> Middle_Int, List<int> X, int V) //Исправленное среднее квадратичное
        {
            double D = Sample_variance(Sample_average(Middle_Int, X, V), Sample_average_quadro(Middle_Int, X, V));
            double SQ = Update_Sample_variance(D, V);
            double S = Math.Sqrt(SQ);
            return S;
        }

        public double[,] Sample_values(string[,] ReceptionT)
        {
            int row = ReceptionT.GetLength(0);
            int columns = ReceptionT.GetLength(1);
            double[,] SampleV = new double[row, columns];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        SampleV[i, j] = double.Parse(ReceptionT[i, j]);
                    }

                }
            }
            return SampleV;
        }

        public double Probabilities(double a1, double a2, double[,] Table_Laplace)
        {
            double f1 = The_Laplace_function(a2, Table_Laplace);
            double f2 = The_Laplace_function(a1, Table_Laplace);
            return f1 - f2;
        }

        public double[,] Getting_the_Laplace_table()
        {
            Workbook wb = new Workbook("Function.xlsx");
            // Получить все рабочие листы
            WorksheetCollection collection = wb.Worksheets;
            Worksheet worksheet = collection[0];
            int rows = worksheet.Cells.MaxDataRow + 1;
            int cols = worksheet.Cells.MaxDataColumn + 1;
            string[,] ReceptionT = new string[rows, cols];
            for (int i = 0; i < rows; i++)
            {

                // Перебрать каждый столбец в выбранной строке
                for (int j = 0; j < cols; j++)
                {
                    ReceptionT[i, j] = (string)worksheet.Cells[i, j].StringValue;
                }

            }
            return Sample_values(ReceptionT);

        }


        public double The_Laplace_function(double a, double[,] Table_Laplace)
        {
            int k = 0;
            if (a < 0)
            {
                a *= -1;
                k = 1;
            }


            int index_rows = 1;
            for (int i = 2; i < Table_Laplace.GetLength(0); i++)
            {
                if (Table_Laplace[i - 1, 0] <= a && Table_Laplace[i, 0] >= a)
                {
                    index_rows = i - 1;
                    break;
                }

            }

            int index_cols = 1;
            index_cols += (int)(a * 100 % 10);


            if (k == 1)
                return Table_Laplace[index_rows, index_cols] * (-1);
            return Table_Laplace[index_rows, index_cols];
        }



        public List<double> Score_N(double[,] A)
        {

            double[,] Table_Laplace = Getting_the_Laplace_table();

            var Table = List_table(A);
            var n = Frequency_selection(Table);
            var Interval = Create_Intervals(Table[0]);
            var Mid_Int = Create_middle_Intervals(Interval);
            List<double> Score_Norm = new List<double>();
            foreach (var X in n)
            {
                int V = A.GetLength(0);

                double Xa = Sample_average(Mid_Int, X, V);
                double Sx = Corrected_quadratic_mean_score(Mid_Int, X, V);
                List<double> p = new List<double>();
                for (int i = 1; i < Interval.Length; i++)
                {
                    double a2 = (Interval[i] - Xa) / Sx;
                    double a1 = (Interval[i - 1] - Xa) / Sx;
                    p.Add(Probabilities(a1, a2, Table_Laplace));
                }
                double Score = 0;
                for (int i = 0; i < p.Count; i++)
                {
                    Score += Math.Pow(X[i] - V * p[i], 2) / (V * p[i]);
                }
                Score_Norm.Add(Score);
            }
            return Score_Norm;

        }
    }
}
