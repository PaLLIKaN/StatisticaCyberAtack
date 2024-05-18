using Aspose.Cells;
using Worksheet = Aspose.Cells.Worksheet;
using Workbook = Aspose.Cells.Workbook;

namespace StatisticaCyberAtack
{   
        public class DescriptiveStatistics
        {
            public string[,] reception_tableNorm(string path)
            {
                Workbook wb = new Workbook(path);
                WorksheetCollection collection = wb.Worksheets;

                Worksheet worksheet = collection[0];
                int rows = worksheet.Cells.MaxDataRow + 1;
                int cols = worksheet.Cells.MaxDataColumn + 1;
                string[,] ReceptionT = new string[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        ReceptionT[i, j] = (string)worksheet.Cells[i, j].StringValue;
                    }

                }
                return ReceptionT;
            }

        public double[,] RoundDoubleArray(double[,] array)
            {
                for(int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++ )
                    {
                        array[i,j] = Math.Round(array[i,j], 4);
                    }
                }
                 return array;
            }

            public double[] RoundArray(double[] array)
            {
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    array[i] = Math.Round(array[i], 4);
                }
                return array;
            }

        public double[] Max(double[,] A)
            {
                double[] max = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    double max1 = -1;
                    for (int j = 0; j < A.GetLength(0); j++)
                    {
                        if (j == 0)
                            max1 = A[j, i];

                        else if (max1 < A[j, i])
                        {
                            max1 = A[j, i];
                        }
                    }

                    max[i] = max1;
                }
                return max;
            }

            public double[] Min(double[,] A)
            {
                double[] min = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    double min1 = -1;
                    for (int j = 0; j < A.GetLength(0); j++)
                    {

                        if (j == 0)
                            min1 = A[j, i];

                        else if (min1 > A[j, i])
                        {
                            min1 = A[j, i];
                        }
                    }

                    min[i] = min1;
                }
                return min;
            }

            public double[,] rationing(double[,] A)
            {
                double[,] RTable = new double[A.GetLength(0), A.GetLength(1)];
                double[] max = Max(A);
                double[] min = Min(A);
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    for (int j = 0; j < A.GetLength(0); j++)
                    {
                        RTable[j, i] = (A[j, i] - min[i]) / (max[i] - min[i]);
                    }
                }

                return RTable;

            }

            public string[,] Table_norm(string[,] ReceptionT)
            {
                double[,] tablenorm = Sample_values(ReceptionT);
                tablenorm = rationing(tablenorm);
                tablenorm = RoundDoubleArray(tablenorm);
                string[,] Table_norm_Score = distribution_Rows(ReceptionT, tablenorm);
                return Table_norm_Score;
            }

            public double Sum(double[,] A, int i)
            {

                double SumX = 0;
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    SumX += A[j, i];
                }

                return SumX;
            }

            public double SumQuadro(double[,] A, int i, double[] Mx)
            {

                double SumX = 0;
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    SumX += Math.Pow(A[j, i] - Mx[i], 2);
                }

                return SumX;
            }

            public int Sample_size(double[,] A)
            {
                return A.GetLength(0);
            }

            public double[] Arithmetic_mean(double[,] A)
            {
                double[] Mx = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    Mx[i] = Sum(A, i);
                    Mx[i] /= A.GetLength(0);
                }

                return Mx;
            }

            public double[] Median(double[,] A)
            {
                double[] Me = new double[A.GetLength(1)];
                if (Sample_size(A) % 2 == 0)
                {
                    for (int i = 0; i < A.GetLength(1); i++)
                    {
                        Me[i] = (A[Sample_size(A) / 2 - 1, i] + A[Sample_size(A) / 2, i]) / 2;
                    }
                }
                else
                {
                    for (int i = 0; i < A.GetLength(1); i++)
                    {
                        Me[i] = A[Sample_size(A) / 2 + 1, i];
                    }
                }

                return Me;
            }

            public double[] Moda(double[,] A)
            {
                double[] Mo = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    var count = 0;
                    var index = -1;
                    for (var row = 0; row < A.GetLength(0); row++)
                    {
                        var k = 1;
                        for (var j = row + 1; j < A.GetLength(0); ++j)
                            if (A[row, i] == A[j, i]) k++;
                        if (k <= count) continue;
                        count = k;
                        index = row;
                    }

                    if (count <= 1)
                    {
                        Mo[i] = -1;
                    }
                    else
                    {
                        Mo[i] = A[index, i];
                    }

                }

                return Mo;
            }

            public double[] Dispersia(double[,] A, double[] Mx)
            {
                double[] D = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    D[i] = SumQuadro(A, i, Mx) * 1 / (Sample_size(A) - 1);
                }

                return D;
            }

            public double[] Standard_deviation(double[] D)
            {
                double[] S = new double[D.GetLength(0)];
                for (int i = 0; i < D.GetLength(0); i++)
                {
                    S[i] = Math.Sqrt(D[i]);
                }

                return S;
            }

            public double[] Excess(double[] S, double[] Mx, double[,] A)
            {
                double[] E = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    E[i] = 0;
                    for (int j = 0; j < A.GetLength(0); j++)
                    {
                        E[i] += Math.Pow(A[j, i] - Mx[i], 4);
                    }

                    E[i] /= Math.Pow(S[i], 4) * (Sample_size(A));
                    E[i] -= 3;
                }

                return E;
            }

            public double[] Asymmetry(double[] S, double[] Mx, double[,] A)
            {
                double[] As = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    As[i] = 0;
                    for (int j = 0; j < A.GetLength(0); j++)
                    {
                        As[i] += Math.Pow(A[j, i] - Mx[i], 3);
                    }

                    As[i] /= Math.Pow(S[i], 3) * (Sample_size(A));

                }

                return As;
            }

            public double[] The_standard_error(double[] S, double[,] A)
            {
                double[] m = new double[A.GetLength(1)];
                for (int i = 0; i < A.GetLength(1); i++)
                {
                    m[i] = S[i] / Math.Sqrt(Sample_size(A) - 1);
                }

                return m;
            }

            public double[,] Sample_values(string[,] ReceptionT)
            {
                int row = ReceptionT.GetLength(0) - 1;
                int columns = ReceptionT.GetLength(1) - 1;
                double[,] SampleV = new double[row, columns];
                for (int i = 1; i <= row; i++)
                {
                    for (int j = 1; j <= columns; j++)
                    {
                        SampleV[i - 1, j - 1] = double.Parse(ReceptionT[i, j]);
                    }
                }
                return SampleV;
            }

            public string[,] Score_table_Norm(List<double> normality)
            {
                int colCount = normality.Count();

                string[,] output = new string[4, colCount + 1];

                output[1, 0] = "Расчетное значение";
                output[2, 0] = "Табличное значение";
                output[3, 0] = "Нормальность распределения";

                for (int i = 0; i <= colCount; i++)
                    output[0, i] = MainWindow.myTable[0, i];

                for (int j = 0; j < colCount; j++)
                {
                    output[1, j + 1] = Convert.ToString(Math.Round(normality[j], 4));
                    output[2, j + 1] = "7,8";
                    if (normality[j] > 7.8)
                    {
                        output[3, j + 1] = "Нет";
                    }
                    else
                    {
                        output[3, j + 1] = "Да";
                    }
                }
                return output;
            }

            public string[,] reception_table(string path)
                {
                    Workbook wb = new Workbook(path);
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
                    MainWindow.myTable = ReceptionT;
                    return ReceptionT;
                }

                public string[,] distribution_Rows(string[,] B, double[,] A)
                {
                    for (int i = 1; i < B.GetLength(0); i++)
                    {
                        for (int j = 1; j < B.GetLength(1); j++)
                            B[i, j] = Convert.ToString(A[i - 1, j - 1]);
                    }
                    return B;
                }

                public string[,] Score_table(string path)
                {
                    string[,] B = reception_table(path);
                    string[,] Score = new string[10, B.GetLength(1)];
                    double[,] A = rationing(Sample_values(B));
                    int n = Sample_size(A);
                    double[] N = new double[A.GetLength(1)];
                    for (int i = 0; i < A.GetLength(1); i++)
                    {
                        N[i] = n;
                    }
                    double[] mx = RoundArray(Arithmetic_mean(A));
                    double[] me = RoundArray(Median(A));
                    double[] mo = RoundArray(Moda(A));
                    double[] d = RoundArray(Dispersia(A, mx));
                    double[] s = RoundArray(Standard_deviation(d));
                    double[] e = RoundArray(Excess(s, mx, A));
                    double[] asymmetry = RoundArray(Asymmetry(s, mx, A));
                    double[] m = RoundArray(The_standard_error(s, A));

                    Score[1, 0] = "Мощность";
                    Score[2, 0] = "Мат.ожидание";
                    Score[3, 0] = "Медиана";
                    Score[4, 0] = "Мода";
                    Score[5, 0] = "Дисперсия";
                    Score[6, 0] = "Стандартное отклонение";
                    Score[7, 0] = "Эксцесс";
                    Score[8, 0] = "Асимметрия";
                    Score[9, 0] = "Стандартная ошибка";

                    for (int i = 1; i < B.GetLength(1); i++)
                    {
                        Score[0, i] = B[0, i];
                    }

                    for (int j = 1; j < B.GetLength(1); j++)
                    {
                        int i = 1;

                        Score[i, j] = Convert.ToString(N[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(mx[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(me[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(mo[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(d[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(s[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(e[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(asymmetry[j - 1]);
                        i++;

                        Score[i, j] = Convert.ToString(m[j - 1]);
                    }

                    return Score;
                }
            }
    
}
