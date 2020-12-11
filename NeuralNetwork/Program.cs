using System;
using System.Collections;
using System.Collections.Generic;

namespace NeuralNetwork
{

    class NeuralNetwork
    {
        private Random rand = new Random();
        public int NumOfLayer { get; set; }
        public int[] SizeOfLayers { get; set; }
        public double[] InputLayer { get; set; }
        public double[] OutputLayer { get; set; }
        public double[] ExpectedResult { get; set; }


        public List<double[,]> Weights { get; set; } = new List<double[,]>();

        public void GenerateMatrix()
        {
            for (int i = 0; i < SizeOfLayers.Length - 1; i++)
            {
                Weights.Add(Matrix.Generate(SizeOfLayers[i + 1], SizeOfLayers[i]));
            }
        }

        public void PrintWeights()
        {
            foreach (var item in Weights)
            {
                Matrix.Print(item);
                Console.WriteLine();
            }
        }

        // Activation functions
        public double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public double ReLU(double x)
        {
            return Math.Max(0, x);
        }


        public double SigmoidDerivative(double x)
        {
            return Sigmoid(x) * (1 - Sigmoid(x));
        }

        public double[] GetDelta()
        {
            if (OutputLayer == null || ExpectedResult == null)
                throw new NullReferenceException();

            if (OutputLayer.Length == ExpectedResult.Length)
            {
                var res = new double[OutputLayer.Length];
                for (int i = 0; i < OutputLayer.Length; i++)
                {
                    res[i] = ExpectedResult[i] - OutputLayer[i];
                }
            }

            throw new Exception("Different size of output array and expected array");
        }

        public void Normalize(double[,] args, Func<double, double> func)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i, 0] = func(args[i, 0]);
            }
        }

        private double[,] _input;
        private double[,] _output;

        public double[,] GetResult()
        {
            var inputMatrix = new double[InputLayer.Length, 1];

            for (int i = 0; i < InputLayer.Length; i++)
            {
                inputMatrix[i, 0] = InputLayer[i];
            }

            _input = inputMatrix;

            Console.WriteLine("Results: ");

            var temp = inputMatrix;
            for (int i = 0; i < Weights.Count; i++)
            {
                temp = Matrix.Multiply(Weights[i], temp);

                //Test
                Matrix.Print(temp);
                Console.WriteLine();

                Normalize(temp, ReLU);
            }

            _output = temp;

            return temp;
        }

        public void Backpropagation()
        {
            var expectedResult = new double[ExpectedResult.Length, 1];

            for (int i = 0; i < ExpectedResult.Length; i++)
            {
                expectedResult[i, 0] = ExpectedResult[i];
            }

            var delta = new double[ExpectedResult.Length, 1];

            for (int i = 0; i < delta.GetLength(0); i++)
            {
                delta[i, 0] = expectedResult[i, 0] - _output[i, 0];
            }

            var temp = delta;

            Console.WriteLine("Backpropagation");
            for (int i = Weights.Count - 1; i >= 0; i--)
            {
                temp = Matrix.Multiply(temp, Weights[i]);

                //Test
                Matrix.Print(temp);
                Console.WriteLine();

                //Normalize(temp, ReLU);
            }

            _output = temp;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var nn = new NeuralNetwork();

            nn.SizeOfLayers = new int[] { 2, 3, 1 };

            nn.InputLayer = new double[] { 1, 2 };

            nn.OutputLayer = new double[] { 0.7 };

            nn.ExpectedResult = new double[] { 0.5 };



            //nn.GenerateMatrix();

            nn.Weights.Add(new double[,] {
                {  1,  3 },
                { -1,  2 },
                { -3, -2 }
            });

            nn.Weights.Add(new double[,] {
                { 0.3, 0.4, 0.3 }
            });

            nn.PrintWeights();

            var result = nn.GetResult();
            Console.WriteLine("Generation - 1");
            Matrix.Print(result);

            nn.Backpropagation();

            Console.WriteLine("Generation - 2");
            result = nn.GetResult();
            Matrix.Print(result);

            //var matrix = new double[,]
            //{
            //    { 1,2,3 },
            //    { 4,5,6 },
            //    { 7,8,9 },
            //};
            //Console.WriteLine(matrix);
        }
    }
}
