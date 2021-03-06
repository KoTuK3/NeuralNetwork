﻿using System;
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


        public List<Matrix> Weights { get; set; } = new List<Matrix>();

        public List<Matrix> Results { get; set; } = new List<Matrix>();

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
                Console.WriteLine(item);
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

        public void Normalize(Matrix args, Func<double, double> func)
        {
            for (int i = 0; i < args.Height; i++)
            {
                args[i, 0] = func(args[i, 0]);
            }
        }

        private Matrix _input;
        private Matrix _output;

        public Matrix GetResult()
        {
            var inputMatrix = new Matrix(new double[InputLayer.Length, 1]);

            for (int i = 0; i < InputLayer.Length; i++)
            {
                inputMatrix[i, 0] = InputLayer[i];
            }

            _input = inputMatrix;

            Console.WriteLine("Results: ");

            var temp = inputMatrix;

            //Test
            Console.WriteLine(temp);
            Console.WriteLine();

            Results.Add(inputMatrix);

            for (int i = 0; i < Weights.Count; i++)
            {
                temp = Weights[i] * temp;

                //Test
                Console.WriteLine(temp);
                Console.WriteLine();

                if (i < Weights.Count - 1)
                {
                    Results.Add(temp);
                    Normalize(temp, Sigmoid);
                }
            }

            _output = temp;

            return temp;
        }
        // Test
        public double N { get; set; } = 0.05; // Learning rate
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

        private double _delta;
        private double _error = 0.01;
        public void Training()
        {
            int i = 1;
            while (Math.Abs(ExpectedResult[0] - _output[0, 0]) > _error && i < 100)
            {
                Console.WriteLine($"\n=== Generation - {i++} ===\n");
                Backpropagation();
                Console.WriteLine($"Res: {GetResult()[0, 0]}");
            }
        }

        public void Backpropagation()
        {
            // Test
            _delta = _output[0, 0] - ExpectedResult[0];
            var alpha = N * _delta;
            var newWeights = new List<Matrix>();

            for (int i = Weights.Count - 1; i >= 0; i--)
            {
                var trans = Matrix.Transpose(Results[i]);
                var temp = trans * alpha;

                if (i + 1 <= Weights.Count - 1)
                {
                    trans = Matrix.Transpose(Weights[i + 1]);
                    // trans = Matrix.Change(trans, SigmoidDerivative);
                    temp = trans * temp;
                }

                var res = Weights[i] - temp;

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

            nn.ExpectedResult = new double[] { 5 };

            //nn.GenerateMatrix();

            //nn.Weights.Add(new double[,] {
            //    { 0.11, 0.21 },
            //    { 0.12, 0.08 },
            //});

            //nn.Weights.Add(new double[,] {
            //    { 0.14, 0.15 }
            //});

            nn.GenerateMatrix();

            Console.WriteLine("=== Data - 1 ===");

            nn.InputLayer = new double[] { 2, 3 };
            nn.ExpectedResult = new double[] { 5 };

            Console.WriteLine($"Res: {nn.GetResult()[0,0]}");
            nn.Training();

            Console.WriteLine($"Res: {nn.GetResult()[0, 0]}");




            Console.WriteLine("=== Data - 2 ===");

            nn.InputLayer = new double[] { 3, 3 };
            nn.ExpectedResult = new double[] { 6 };

            Console.WriteLine($"Res: {nn.GetResult()[0, 0]}");
            nn.Training();
            Console.WriteLine($"Res: {nn.GetResult()[0, 0]}");


            nn.InputLayer = new double[] { 2, 3 };
            Console.WriteLine($"Res Main: {nn.GetResult()[0, 0]}");

            nn.InputLayer = new double[] { 2, 3 };
            Console.WriteLine($"Res main: {nn.GetResult()[0, 0]}");
        }
    }
}
