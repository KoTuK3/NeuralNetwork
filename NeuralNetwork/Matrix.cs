using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    static class Matrix
    {
        static private Random rand = new Random();

        static public double[,] Generate(int height, int width)
        {
            var matrix = new double[height, width];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = rand.NextDouble();
                }
            }

            return matrix;
        }

        static public double[,] Multiply(double[,] matrix, double number)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            var res = new double[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    res[i, j] = number * matrix[i, j];

            return res;
        }
                
        static public double[,] Multiply(double[,] matrix1, double[,] matrix2)
        {
            if (matrix1 == null || matrix2 == null)
                throw new ArgumentNullException();

            // Res height1 x width2
            if (matrix1.GetLength(1) == matrix2.GetLength(0))
            {
                int n = matrix1.GetLength(1);

                var res = new double[matrix1.GetLength(0), matrix2.GetLength(1)];
                for (int i = 0; i < res.GetLength(0); i++)
                {
                    for (int j = 0; j < res.GetLength(1); j++)
                    {
                        res[i, j] = 0;
                        for (int k = 0; k < n; k++)
                            res[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
                return res;
            }

            throw new NullReferenceException();
        }

        static public double[,] Sum(double[,] matrix1, double[,] matrix2)
        {
            if (matrix1 == null || matrix2 == null)
                throw new ArgumentNullException();

            if (matrix1.GetLength(0) == matrix2.GetLength(0) &&
                matrix1.GetLength(1) == matrix2.GetLength(1))
            {
                var res = new double[matrix1.GetLength(0), matrix1.GetLength(1)];
                for (int i = 0; i < matrix1.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix1.GetLength(1); j++)
                    {
                        res[i, j] += matrix1[i, j] + matrix2[i, j];
                    }
                }
                return res;
            }

            throw new NullReferenceException();
        }

        static public double[,] Sub(double[,] matrix1, double[,] matrix2)
        {
            return Sum(matrix1, Multiply(matrix2, -1));
        }

        static public double[,] Transpose(double[,] matrix)
        {
            var res = new double[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res[j, i] = matrix[i, j];
                }
            }

            return res;
        }

        static public void Copy(double[,] matrix1, double[,] matrix2)
        {
            if (matrix1 == null || matrix2 == null)
                throw new ArgumentNullException();

            if (matrix1.GetLength(0) == matrix2.GetLength(0) &&
                matrix1.GetLength(1) == matrix2.GetLength(1))
            {                
                for (int i = 0; i < matrix1.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix1.GetLength(1); j++)
                    {
                        matrix1[i, j] = matrix2[i, j];
                    }
                }
            }
        }

        static public void Print(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0, 16}", matrix[i, j].ToString("0.00000"));
                }
                Console.WriteLine();
            }
        }
    }
}
