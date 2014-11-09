/*
 * Created by SharpDevelop.
 * User: BT
 * Date: 29/10/2014
 * Time: 9:43 CH
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace FCCIAlgorithm
{
	class Program
	{
		public static void Main(string[] args)
		{
			Random random = new Random();
			Console.WriteLine("Hello World!");
			int C = 2;
			int N = 2;
			int K = 2;
			double[,] x = new double[N,K];
			for (int i = 0; i < N; i++) {
				for (int j = 0; j < K; j++) {
					x[i,j] = random.NextDouble()*255;
				}
			}
			FCCIAlgorithm.runAlgorithm(10,9*Math.Pow(10,5),Math.Pow(10,-2),100,C,K,N,x);
			
			
			Console.WriteLine("==============REAL IMAGE====================");
			string fileName = "cat.jpg";
			fileName = Path.Combine(Environment.CurrentDirectory,fileName);
			double[,] input = ColorSpaceHelper.get2dDataArrayFromImage(fileName);
			C=3;
			K=2;
			N = input.GetLength(0);
			FCCIAlgorithm.runAlgorithm(10,9*Math.Pow(10,5),Math.Pow(10,-2),100,C,K,N,input);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		
		
		
		
	}
}