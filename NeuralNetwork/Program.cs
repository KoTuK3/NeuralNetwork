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

        public List<double[,]> Results { get; set; } = new List<double[,]>();

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
        public double Linear(double x)
        {
            return x;
        }

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
        public double ReLUDerivative(double x)
        {
            return x <= 0 ? 0 : 1;
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

            //Test
            Matrix.Print(temp);
            Console.WriteLine();

            Results.Add(inputMatrix);

            for (int i = 0; i < Weights.Count; i++)
            {
                temp = Matrix.Multiply(Weights[i], temp);

                //Test
                Matrix.Print(temp);
                Console.WriteLine();

                if (i < Weights.Count - 1)
                    Results.Add(temp);

                Normalize(temp, Sigmoid);
            }

            _output = temp;

            return temp;
        }
        // Test
        public double N { get; set; } = 0.1;
        public List<double[,]> Deltas { get; set; } = new List<double[,]>();
        //public void Backpropagation()
        //{
        //    var expectedResult = new double[ExpectedResult.Length, 1];

        //    for (int i = 0; i < ExpectedResult.Length; i++)
        //    {
        //        expectedResult[i, 0] = ExpectedResult[i];
        //    }

        //    var delta = new double[ExpectedResult.Length, 1];


        //    for (int i = 0; i < delta.GetLength(0); i++)
        //    {
        //        delta[i, 0] = _output[i, 0] - expectedResult[i, 0];
        //    }

        //    var temp = delta;
        //    Deltas.Add(temp);

        //    // TODO i > 0 or i >= 0
        //    for (int i = Weights.Count - 1; i > 0; i--)
        //    {
        //        temp = Matrix.Multiply(temp, Weights[i]);
        //        Deltas.Add(temp);

        //        //Normalize(temp, ReLU);
        //    }

        //    //Test
        //    Console.WriteLine("\n===Backpropagation===\n");
        //    //Print Deltas
        //    Console.WriteLine("Deltas");
        //    foreach (var item in Deltas)
        //    {
        //        Matrix.Print(item);
        //        Console.WriteLine();
        //    }

        //    _output = temp;
        //}
        public void Backpropagation()
        {
            // Test
            var delta = _output[0, 0] - 1;
            var alpha = N * delta;
            var newWeights = new List<double[,]>();

            for (int i = Weights.Count - 1; i >= 0; i--)
            {
                var trans = Matrix.Transpose(Results[i]);
                var temp = Matrix.Multiply(trans, alpha);

                if (i + 1 <= Weights.Count - 1)
                {
                    trans = Matrix.Transpose(Weights[i + 1]);
                    temp = Matrix.Multiply(trans, temp);
                }

                var res = Matrix.Sub(Weights[i], temp);

                newWeights.Add(res);
            }

            for (int i = newWeights.Count - 1; i >= 0; i--)
            {
                Matrix.Copy(Weights[newWeights.Count - 1 - i], newWeights[i]);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            // Weights matrix
            // Columns - input nodes(left)
            // Rows - output nodes(right)
            var nn = new NeuralNetwork();

            nn.SizeOfLayers = new int[] { 2, 2, 1 };

            nn.InputLayer = new double[] { 2, 3 };

            //nn.OutputLayer = new double[] { 1 };

            nn.ExpectedResult = new double[] { 1 };

            //nn.GenerateMatrix();

            nn.Weights.Add(new double[,] {
                { 0.11, 0.21 },
                { 0.12, 0.08 },
            });

            nn.Weights.Add(new double[,] {
                { 0.14, 0.15 }
            });

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"\n=== Generation - {i + 1} ===\n");

                Console.WriteLine("Weights:");
                nn.PrintWeights();

                var result = nn.GetResult();
                Console.WriteLine("Result:");
                Matrix.Print(result);

                nn.Backpropagation();
            }
        }
    }
}
