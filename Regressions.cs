using System.Text;

namespace StatisticaCyberAtack
{
    public class Regressions
    {
        public double[,] Y(double[,] matrix, int col)
        {
            double[,] y = new double[matrix.GetLength(0), 1];
            for (int i = 0; i < y.Length; i++)
            {
                y[i, 0] = matrix[i, col];
            }
            return y;
        }

        public double[,] X(double[,] matrix, int col)
        {
            double[,] array = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (j == col)
                    {
                        array[i, j] = 1;
                    }
                    else
                    {
                        array[i, j] = matrix[i, j];
                    }
                }

            }
            return array;
        }

        public double[,] TransposeMatrix(double[,] matrix)
        {

            // Создание транспонированной матрицы
            double[,] transposedMatrix = new double[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(1); i++)
            {

                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    transposedMatrix[i, j] = matrix[j, i];
                }
            }

            return transposedMatrix;
        }

        public double[,] MultiplyMatrices(double[,] matrix1, double[,] matrix2)
        {

            // Создание результирующей матрицы
            int rows = matrix1.GetLength(0);
            int cols = matrix2.GetLength(1);
            double[,] resultMatrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {

                for (int j = 0; j < cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    resultMatrix[i, j] = sum;
                }
            }

            return resultMatrix;
        }


        public double[,] inverse_matrix(double[,] matrix)
        {
            int row = matrix.GetLength(0);
            int col = matrix.GetLength(1);


            double[,] A = new double[row, row * 2];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    A[i, j] = matrix[i, j];
                }
            }
            int k = 0;
            for (int j = row; j < row * 2; j++)
            {
                A[k, j] = 1;
                k++;
            }
            k = 0;
            for (int i = 0; i < row; i++)
            {
                A = reduction_main_diagonal(A, row * 2, i, k);
                A = converting_rows_main_diagonal(row, row * 2, A, i);
                k++;

            }

            double[,] X = new double[row, row];
            for (int j = 0; j < row; j++)
            {
                for (int i = row - 1; i >= 0; i--)
                {
                    X[i, j] = A[i, j + row];
                    for (int l = row - 1; l > i; l--)
                        X[i, j] -= X[l, j] * A[i, l];
                }
            }
            return X;
        }

        public double[,] reduction_main_diagonal(double[,] matrix, int m, int i, int k)
        {
            double koef = matrix[k, i];

            for (int j = i; j < m; ++j)
                matrix[k, j] /= koef;
            return matrix;
        }

        public double[,] converting_rows_main_diagonal(int n, int m, double[,] matrix, int i)
        {
            for (int j = i + 1; j < n; j++)
            {
                double koef;
                koef = matrix[j, i] / matrix[i, i];
                if (koef != 0)
                {

                    for (int k = i; k < m; k++)
                    {
                        matrix[j, k] -= matrix[i, k] * koef;
                    }
                }
            }
            return matrix;
        }

        public double[,] Regress(double[,] matrix, int col)
        {
            var Ya = Y(matrix, col);
            var Xa = X(matrix, col);

            var b = MultiplyMatrices(MultiplyMatrices(inverse_matrix(MultiplyMatrices(TransposeMatrix(Xa), Xa)), TransposeMatrix(Xa)), Ya);
            return b;
        }

        public string[,] OutputRegress(double[,] regress)
        {
            int colCount = regress.GetLength(0);

            string[,] output = new string[5, colCount];
            for (int i = 0; i < colCount; i++)
                output[0, i] = "B" + i;

            for (int j = 0; j < colCount; j++)
            {
                output[1, j] = Convert.ToString(Math.Round(regress[j, 0], 4));
            }
            return output;
        }

        public string RegressionEquation(double[,] coefficients)
        {
            StringBuilder equation = new StringBuilder("Y = ");

            int numCoefficients = coefficients.GetLength(0);
            for (int i = 0; i < numCoefficients; i++)
            {
                double coefficient = coefficients[i, 0];
                equation.Append(coefficient.ToString("+0.####;-0.####"));
                equation.Append(" * X");
                equation.Append(i);
                equation.Append(" ");
                if (i == 0)
                {
                    equation.Replace("+", "");
                    equation.Replace("* X0", "");
                }
            }

            return equation.ToString();
        }

        public double[] New_Y(double[,] b, double[,] matrix, int col)
        {
            var Matrix_X = X(matrix, col);
            double[] newY = new double[matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double sum = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sum += b[j, 0] * Matrix_X[i, j];
                }
                newY[i] = sum;
            }
            return newY;
        }

        public double[] E(double[] NewY, double[,] matrix, int col)
        {
            double[] Score_E = new double[NewY.GetLength(0)];
            var Ya = Y(matrix, col);
            for (int i = 0; i < Ya.Length; i++)
            {
                Score_E[i] = Math.Abs(Ya[i, 0] - NewY[i]);
            }
            return Score_E;
        }

        public double[] Criteria(double[] NewY, double[,] matrix, int col)
        {
            var Ya = Y(matrix, col);
            var Ymx = Arithmetic_mean(Ya);
            double RSS = 0;
            double SST = 0;
            double SSE = 0;
            double E = 0;
            for (int i = 0; i < Ya.GetLength(0); i++)
            {
                RSS += Math.Pow(NewY[i] - Ymx, 2);
                SST += Math.Pow(Ya[i, 0] - Ymx, 2);
                SSE += Math.Pow(Ya[i, 0] - NewY[i], 2);
                E += Math.Abs((Ya[i, 0] - NewY[i]) / Ya[i, 0]);
            }
            double[] criteria = new double[3];
            criteria[0] = 1 - (SSE / SST);
            criteria[1] = E / matrix.GetLength(0) * 100;
            criteria[2] = criteria[0] / (1 - criteria[0]) * (matrix.GetLength(0)-matrix.GetLength(1)-1);
            return criteria;
        }

        public double Arithmetic_mean(double[,] A)
        {
            double Mx = 0;
            for (int i = 0; i < A.GetLength(0); i++)
            {
                Mx += A[i, 0];
            }
            Mx /= A.GetLength(0);

            return Mx;
        }
        public string[,] TableSrav(double[] newY, double[] E, double[,] Y)
        {
            int rowCount = newY.Count() + 1;

            string[,] output = new string[rowCount, 3];

            output[0, 0] = "Статические данные";
            output[0, 1] = "Данные модели";
            output[0, 2] = "Погрешность";


            for (int j = 0; j < rowCount - 1; j++)
            {
                output[j + 1, 0] = Convert.ToString(Math.Round(Y[j, 0], 4));
                output[j + 1, 1] = Convert.ToString(Math.Round(newY[j], 4));
                output[j + 1, 2] = Convert.ToString(Math.Round(E[j], 4));
            }
            return output;
        }

        public string[,] TableCriteria(double[] Criteria)
        {

            string[,] output = new string[8, 2];

            output[0, 0] = "R-квадрат";
            output[1, 0] = "Средняя ошибка аппроксимации";
            output[2, 0] = "Критерий Фишера";


            for (int j = 0; j < 3; j++)
            {
                output[j, 1] = Convert.ToString(Math.Round(Criteria[j], 4));
            }
            return output;
        }
    }
}
