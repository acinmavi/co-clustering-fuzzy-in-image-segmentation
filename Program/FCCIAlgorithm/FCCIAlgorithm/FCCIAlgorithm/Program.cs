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
using System.Drawing;
using System.IO;
using System.Linq;

namespace FCCIAlgorithm
{
	class Program
	{
		public static void Main(string[] args)
		{
//			Random random = new Random();
//			Console.WriteLine("Hello World!");
//			int C = 2;
//			int N = 2;
//			int K = 2;
//			double[,] x = new double[N,K];
//			for (int i = 0; i < N; i++) {
//				for (int j = 0; j < K; j++) {
//					x[i,j] = random.NextDouble()*255;
//				}
//			}
//			FCCIAlgorithm.runAlgorithm(10,9*Math.Pow(10,5),Math.Pow(10,-2),100,C,K,N,x);
			
			Console.WriteLine("==============REAL IMAGE====================");
			string fileName = "cat.jpg";
			fileName = Path.Combine(Environment.CurrentDirectory,fileName);
			Console.WriteLine("convert image to 2d array");
			List<CIELab> lsCeiLab = new List<CIELab>();
			Bitmap img = (Bitmap) Image.FromFile(fileName);
			double[,] input = ColorSpaceHelper.get2dDataArrayFromImage(fileName,out lsCeiLab);
			Console.WriteLine("convert done : " + input.GetLength(0) + " points");
			int C=2;
			int K=2;
			int N = input.GetLength(0);
			double Tu = 10;
			double Tv =9*Math.Pow(10,7);
			double Sold =double.MaxValue;
			
			List<CIELab> lsCeiLabClone = lsCeiLab.Select(o=>o.clone()).ToList();
			
			//=============================FCCI=========================================
			double Snew = FCCIAlgorithm.runAlgorithm(Tu,Tv,Math.Pow(10,-2),100,C,K,N,input,lsCeiLab);
			Console.WriteLine("with C = " + C + " Sold = " + Sold + " Snew = " + Snew);
			string fileOut = "cluster"+C+"-cat.jpg";
			ColorSpaceHelper.saveCIELabsToImage(lsCeiLab,fileOut,img.Width,img.Height);
			while(Snew<Sold){
				C=C+1;
				Sold=Snew;
				Snew = FCCIAlgorithm.runAlgorithm(Tu,Tv,Math.Pow(10,-2),100,C,K,N,input,lsCeiLab);
				Console.WriteLine("with C = " + C + " Sold = " + Sold + " Snew = " + Snew);
				fileOut = "cluster"+C+"-cat.jpg";
			ColorSpaceHelper.saveCIELabsToImage(lsCeiLab,fileOut,img.Width,img.Height);
			}
			C =C-1;
			
			Console.WriteLine("So number of cluster C = " +C);
			FCCIAlgorithm.runAlgorithm(Tu,Tv,Math.Pow(10,-2),100,C,K,N,input,lsCeiLab,true);
			
			//compare 2 list
//			for (int i = 0; i < lsCeiLab.Count; i++) {
//				Console.WriteLine(lsCeiLab[i] +"----"+ lsCeiLabClone[i]);
//			}
			
			fileOut = "final-cluster"+C+"-cat.jpg";
			ColorSpaceHelper.saveCIELabsToImage(lsCeiLab,fileOut,img.Width,img.Height);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		
		
		
		
	}
}