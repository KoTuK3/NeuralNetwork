using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    class Matrix
    {
        static private Random rand = new Random();

        private double[,] matrix;

        public double[,] Value
        {
            get => matrix;
            set => matrix = value;
        }

        public int Height
        {
            get => matrix.GetLength(0);
        }

        public int Width
        {
            get => matrix.GetLength(1);
        }

        public double this[int i, int j]
        {
            get => matrix[i, j];
            set => matrix[i, j] = value;
        }

        public Matrix(double[,] matrix)
        {
            Value = matrix;
        }

        public static Matrix operator *(Matrix leftMatrix, Matrix rightMatrix)
        {
            return new Matrix(Multiply(leftMatrix.Value, rightMatrix.Value));
        }

        public static Matrix operator *(Matrix leftMatrix, double number)
        {
            return new Matrix(Multiply(leftMatrix.Value, number));
        }

        public static Matrix operator +(Matrix leftMatrix, Matrix rightMatrix)
        {
            return new Matrix(Sum(leftMatrix.Value, rightMatrix.Value));
        }

        public static Matrix operator -(Matrix leftMatrix, Matrix rightMatrix)
        {
            return new Matrix(Sub(leftMatrix.Value, rightMatrix.Value));
        }

        //static private double[,] Generate(int height, int width)
        //{
        //    var matrix = new double[height, width];

        //    for (int i = 0; i < matrix.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < matrix.GetLength(1); j++)
        //        {
        //            matrix[i, j] = rand.NextDouble();
        //        }
        //    }

        //    return matrix;
        //}

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
                        res[i, j] = matrix1[i, j] + matrix2[i, j];
                    }
                }
                return res;
            }

            throw new NullReferenceException();
        }

        static public double[,] Sub(double[,] matrix1, double[,] matrix2)
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
                        res[i, j] = matrix1[i, j] - matrix2[i, j];
                    }
                }
                return res;
            }

            throw new NullReferenceException();
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

        static public Matrix Transpose(Matrix matrix)
        {
            return new Matrix(Transpose(matrix.Value));
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

        static public void Copy(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 == null || matrix2 == null)
                throw new ArgumentNullException();

            if (matrix1.Height == matrix2.Height &&
                matrix1.Width == matrix2.Width)
            {
                for (int i = 0; i < matrix1.Height; i++)
                {
                    for (int j = 0; j < matrix1.Width; j++)
                    {
                        matrix1[i, j] = matrix2[i, j];
                    }
                }
            }
        }



        public Matrix Change(Func<double, double> func)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            var res = new double[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    res[i, j] = func(matrix[i, j]);
                }
            }

            return new Matrix(res);
        }

        static public Matrix Generate(int height, int width, double from, double to)
        {
            if (height <= 0 || width <= 0)
                throw new Exception("Height or width less than zero.");

            var res = new double[height, width];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    res[i, j] = from + (to - from) * rand.NextDouble();
                }
            }
            return new Matrix(res);
        }

        static public Matrix Generate(int height, int width)
        {
            if (height <= 0 || width <= 0)
                throw new Exception("Height or width less than zero.");

            var res = new double[height, width];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    res[i, j] = rand.NextDouble();
                }
            }
            return new Matrix(res);
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

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res += $"{matrix[i, j].ToString("0.00000"), 16}";
                }
                res += "\n";
            }
            return res;
        }
    }
}
