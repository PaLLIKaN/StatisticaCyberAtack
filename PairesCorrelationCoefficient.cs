namespace StatisticaCyberAtack
{
    public class PairesCorrelationCoefficient
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

        public List<List<List<int>>> Frequency_selection_Index(List<List<double>> Table_list)
        {
            List<List<List<int>>> Table_Intervals_frequency = new List<List<List<int>>>();
            foreach (var Table_table in Table_list)
            {
                var Intervals = Create_Intervals(Table_table);

                List<List<int>> Intervals_frequency = new List<List<int>>();
                for (int i = 0; i < Intervals.Length - 1; i++)
                {
                    List<int> result = new List<int>();
                    for (int j = 0; j < Table_table.Count; j++)
                        if (i == 0)
                        {
                            if (Table_table[j] >= Intervals[i] && Table_table[j] < Intervals[i + 1])
                            {
                                result.Add(j);
                            }
                        }
                        else
                        {
                            if (Table_table[j] > Intervals[i] && Table_table[j] <= Intervals[i + 1])
                            {
                                result.Add(j);
                            }
                        }
                    Intervals_frequency.Add(result);
                }

                Table_Intervals_frequency.Add(Intervals_frequency);
            }
            return Table_Intervals_frequency;
        }
        public List<List<double>> Score_table_frequency(List<List<List<int>>> Table, double[] Intervals, int V)
        {
            List<List<double>> Matrix_Rv = new List<List<double>>();
            foreach (var Rows_A in Table)
            {
                List<double> combination = new List<double>();
                foreach (var Rows_B in Table)
                {
                    if (Rows_A == Rows_B)
                    {
                        combination.Add(1);
                    }
                    else
                    {
                        var One_combination = Frequency_A_B(Rows_A, Rows_B, Intervals, V);
                        combination.Add(One_combination);
                    }
                }
                Matrix_Rv.Add(combination);
            }
            return Matrix_Rv;
        }

        public double Frequency_A_B(List<List<int>> A, List<List<int>> B, double[] Intervals, int V)
        {
            List<double> Middle_Int = Create_middle_Intervals(Intervals);
            double Xa = Sample_average(Middle_Int, A, V);
            double Sx = Corrected_quadratic_mean_score(Middle_Int, A, V);
            double Ya = Sample_average(Middle_Int, B, V);
            double Sy = Corrected_quadratic_mean_score(Middle_Int, B, V);

            double Sum = 0;
            for (int i = 0; i < A.Count; i++)
            {
                for (int j = 0; j < B.Count; j++)
                {

                    List<int> X = A[i];

                    List<int> Y = B[j];
                    var result = X.Intersect(Y);

                    Sum += Middle_Int[i] * Middle_Int[j] * result.Count();

                }
            }
            double Kv = Sum / V - Xa * Ya;

            double Rv = Kv / (Sx * Sy);
            return Rv;
        }

        public double Sample_average(List<double> Middle_Int, List<List<int>> X, int V)
        {
            double Xa = 0;
            for (int i = 0; i < Middle_Int.Count; i++)
            {
                Xa += Middle_Int[i] * X[i].Count;
            }
            return Xa / V;
        }

        public double Sample_average_quadro(List<double> Middle_Int, List<List<int>> X, int V)
        {
            double Xa = 0;
            for (int i = 0; i < Middle_Int.Count; i++)
            {
                Xa += Math.Pow(Middle_Int[i], 2) * X[i].Count;
            }
            return Xa / V;
        }

        public double Sample_variance(double Xa, double XaQ)
        {
            double D = XaQ - Math.Pow(Xa, 2);
            return D;
        }

        public double Update_Sample_variance(double D, int V)
        {
            double SQ = V * D / (V - 1);
            return SQ;
        }

        public double Corrected_quadratic_mean_score(List<double> Middle_Int, List<List<int>> X, int V)
        {
            double D = Sample_variance(Sample_average(Middle_Int, X, V), Sample_average_quadro(Middle_Int, X, V));
            double SQ = Update_Sample_variance(D, V);
            double S = Math.Sqrt(SQ);
            return S;
        }


        public List<List<double>> Tables_of_correlation_coefficients(double[,] A)
        {
            var Start_List = List_table(A);
            var Score = Score_table_frequency(Frequency_selection_Index(Start_List), Create_Intervals(Start_List[0]), A.GetLength(0));
            Score = Rounding_up(Score);
            return Score;
        }

        public List<List<double>> Table_Student(List<List<double>> Score, int V)
        {
            List<List<double>> Table_student = new List<List<double>>();
            foreach (var T in Score)
            {
                List<double> T_column = new List<double>();
                foreach (var r in T)
                {
                    T_column.Add(r * Math.Sqrt(V - 2) / Math.Sqrt(1 - r * r));
                }
                Table_student.Add(T_column);
            }
            Table_student = Rounding_up(Table_student);
            return Table_student;
        }

        public List<List<double>> Rounding_up(List<List<double>> Score)
        {
            List<List<double>> Table_Score = new List<List<double>>();
            foreach (var T in Score)
            {
                List<double> cl = new List<double>();
                foreach (var r in T)
                {
                    cl.Add(Math.Round(r, 2));
                }
                Table_Score.Add(cl);

            }
            return Table_Score;
        }


        public double[,] GetMinor(double[,] matrix, int row, int column)
        {
            double[,] buf = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((i != row) || (j != column))
                    {
                        if (i > row && j < column) buf[i - 1, j] = matrix[i, j];
                        if (i < row && j > column) buf[i, j - 1] = matrix[i, j];
                        if (i > row && j > column) buf[i - 1, j - 1] = matrix[i, j];
                        if (i < row && j < column) buf[i, j] = matrix[i, j];
                    }
                }

            return buf;
        }

        public double Determ(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double det = 0;

            if (n == 1)
            {
                det = matrix[0, 0];
            }
            else if (n == 2)
            {
                det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            else
            {
                double[,] temp = new double[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        temp[i, j] = matrix[i, j];
                    }
                }

                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        double ratio = temp[j, i] / temp[i, i];
                        for (int k = 0; k < n; k++)
                        {
                            temp[j, k] -= ratio * temp[i, k];
                        }
                    }
                }

                det = 1;
                for (int i = 0; i < n; i++)
                {
                    det *= temp[i, i];
                }
            }

            return det;
        }

        public double Algebraic_addition(double[,] matrix, int row, int column)
        {
            double[,] buf = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((i != row) || (j != column))
                    {
                        if (i > row && j < column) buf[i - 1, j] = matrix[i, j];
                        if (i < row && j > column) buf[i, j - 1] = matrix[i, j];
                        if (i > row && j > column) buf[i - 1, j - 1] = matrix[i, j];
                        if (i < row && j < column) buf[i, j] = matrix[i, j];
                    }
                }
            double det = Determ(buf);
            if ((row + column) % 2 == 1)
            {
                return -det;
            }
            else
            {
                return det;
            }

        }
        public double[,] Matrix_Aij(double[,] matrix)
        {
            double[,] Score = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Score[i, j] = Algebraic_addition(matrix, i, j);
            return Score;
        }

        public double[,] Tables_of_correlation_private_coefficients(double[,] Correlation_coefficients)
        {
            int rows = Correlation_coefficients.GetLength(0);
            int cols = Correlation_coefficients.GetLength(1);
            double[,] Correlation_coefficients_private = new double[rows, cols];
            var A = Matrix_Aij(Correlation_coefficients);
            for (int i = 0; i < rows; i++)
            {

                for (int j = 0; j < cols; j++)
                {
                    Correlation_coefficients_private[i, j] = A[i, j] / Math.Sqrt(A[i, i] * A[j, j]);
                }

            }
            return Correlation_coefficients_private;
        }
    }
}
