/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 11/9/2014
 * Time: 5:14 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FCCIAlgorithm
{
	/// <summary>
	/// Description of FCCIAlgorithm.
	/// </summary>
	public class FCCIAlgorithm
	{
		private double Tu;
		public  double Tv ;
		public List<CIELab> lsCeiLab ;
		public double Sold;
		public double Snew;
		public int C,K,N;
		private string imageFilePath;
		private double eps;
		private int maxIter;
		private int imageWidth;
		private int imageHeight;
		private double[,] x ;
		public FCCIAlgorithm(string imageFilePath)
		{
			Tu = 10;
			Tv = 9*Math.Pow(10,7);
			lsCeiLab = new List<CIELab>();
			Sold=double.MaxValue;
			eps = Math.Pow(10,-2);
			maxIter = 100;
			this.imageFilePath = imageFilePath;
			C=2;
			K=2;
		}
		public override string ToString()
		{
			return string.Format("[All parameters : Tu={0}, Tv={1}, C={2}, K={3}, N={4}, ImageFilePath={5}, Eps={6}, MaxIter={7}, ImageWidth={8}, ImageHeight={9}]", Tu, Tv, C, K, N, imageFilePath, eps, maxIter, imageWidth,
			                     imageHeight);
		}

		public void Process(){
			Console.WriteLine("Starting get data from image : " + imageFilePath);
			x = GetDataFromImage();
			Console.WriteLine("Starting algorithm with parameter :");
			Console.WriteLine(ToString());
			Snew = RunAlgorithm();
			while(Snew<Sold){
				Console.WriteLine("with C = " + C + " Sold = " + Sold + " Snew = " + Snew);
				Console.WriteLine("So increase C = C+1 then do algorithm again");
				C=C+1;
				Sold=Snew;
				Snew = RunAlgorithm();
			}
			C =C-1;
			Console.WriteLine("So number of cluster C = " + C);
			Console.WriteLine(ToString());
		}
		
		public double RunAlgorithm(){
			
			Random random = new Random();
			double[,] U = new double[C,N];
			double[,] P = new double[C,K];
			double[,] V = new double[C,K];
			double[] sum = new double[N];
			double[,,] D = new double[C,N,K];
			double[,] preU = new double[C,N];
			//Initialize uci such that 0≤uci≤1
			Console.WriteLine("Initialize uci such that 0≤uci≤1");
			for (int c = 0; c < C; c++) {
				for (int i = 0; i < N; i++) {
					U[c,i] = random.NextDouble();
					sum[i]+=U[c,i];
				}
			}
			for (int c = 0; c < C; c++) {
				for (int i = 0; i < N; i++) {
					U[c,i] = U[c,i]/sum[i];
				}
			}
			
			int count = 0;
			while(true){
				count++;
//				Console.WriteLine( "Calculate in  "+count+"th");
				//save preU
				for (int i = 0; i < N; i++) {
					for (int c = 0; c < C; c++) {
						preU[c,i] = U[c,i];
					}
				}
				
//				Console.WriteLine("Calculate pcj");
				//Calculate pcj using(13)
				for (int c = 0; c < C; c++) {
					for (int j = 0; j < K; j++) {
						double up =0;
						double down =0;
						for (int i = 0; i < N; i++) {
							down+=U[c,i];
							up+=U[c,i]*x[i,j];
						}
						P[c,j] = up/down;
					}
				}
				
//				Console.WriteLine("Calculate Dcij");
				//Calculate Dcij using(3)
				for (int c = 0; c < C; c++) {
					for (int i = 0; i < N; i++) {
						for (int j = 0; j < K; j++) {
							D[c,i,j] = Math.Pow((x[i,j] - P[c,j]),2);
						}
					}
				}
				
//				Console.WriteLine("Calculate vcj");
				//Calculate vcj using (11)
				double totalVc = 0;
				for (int c = 0; c < C; c++) {
					for (int j = 0; j < K; j++) {
						double pow = 0;
						for (int i = 0; i < N; i++) {
							pow-=(U[c,i]*D[c,i,j]/Tv);
						}
						V[c,j] = Math.Exp(pow);
						totalVc+=V[c,j];
					}
					
					for (int j = 0; j < K; j++) {
						V[c,j] = V[c,j]/totalVc;
					}
					totalVc = 0;
				}
				
				//Calculate uci using (9)
//				Console.WriteLine("Calculate uci");
				double totalUk=0;
				for (int i = 0;i < N; i++) {
					for (int c = 0; c < C; c++) {
						double pow = 0;
						for (int j = 0; j < K; j++) {
							pow-=(V[c,j]*D[c,i,j]/Tu);
						}
						U[c,i] = Math.Exp(pow);
						totalUk+=U[c,i];
					}
					
					for (int c = 0; c < C; c++) {
						U[c,i] =U[c,i]/totalUk;
						if(Double.IsNaN(U[c,i])){
							U[c,i] = 0;
						}
					}
					totalUk=0;
				}
				double maxEps = 0;
				for (int c = 0; c < C; c++) {
					for (int i = 0; i < N; i++) {
						double compare = Math.Abs(U[c,i]-preU[c,i]);
						if(compare >= maxEps){
							maxEps = compare;
						}
					}
				}
				
				if((maxEps <=eps) || count== maxIter){
					Console.WriteLine("finish calculation algorithm after " + count + " times") ;
					break;
				}
			}

			//time to defuzzy
			//save index : this value decide point i(i=0:n-1) belong to what cluster (c=0:C-1)
			int[] index = new int[N];
			for (int i = 0; i < N; i++) {
				double maxValue = 0;
				int clusterIndex = 0;
				for (int c = 0; c < C; c++) {
					if(maxValue<U[c,i])
					{
						maxValue = U[c,i];
						clusterIndex = c;
					}
				}
				index[i] = clusterIndex;
			}
			
			//
			for (int i = 0; i < N; i++) {
				int cluster = index[i];
				CIELab ceilab = lsCeiLab[i];
				ceilab.A = P[cluster,0];
				ceilab.B = P[cluster,1];
				lsCeiLab[i]=ceilab;
			}
			//save file
			string fileOut = Path.GetFileNameWithoutExtension(imageFilePath)+"_With"+C+"clusters"+Path.GetExtension(imageFilePath) ;
			SaveCIELabsToImage(Path.Combine(Path.GetDirectoryName(imageFilePath),fileOut));
			
			double Snew = XieBeniClusterValidity.runValidity(U,x,P,C,K,N);
			Console.WriteLine("Snew after validity : " +  Snew);
			return Snew;
		}
		
		
		public double[,] GetDataFromImage(){
			Bitmap img = (Bitmap) Image.FromFile(imageFilePath);
			Color pixelColor;
			CIELab ceiLab;
			imageWidth = img.Width;
			imageHeight = img.Height;
			N = img.Width*img.Height;
			double[,] array2D = new double[N,2];
			for (int i = 0; i < imageWidth; i++) {
				for (int j = 0; j <imageHeight; j++) {
					pixelColor = img.GetPixel(i, j);
					ceiLab = ColorSpaceHelper.RGBtoLab(pixelColor);
					ceiLab.iWidth = i;
					ceiLab.iHeight = j;
					lsCeiLab.Add(ceiLab);
				}
			}
			
			for (int i = 0; i < lsCeiLab.Count; i++) {
				array2D[i,0] = lsCeiLab[i].A;
				array2D[i,1] = lsCeiLab[i].B;
			}
			return array2D;
		}
		
		public void SaveCIELabsToImage(String fileName){
			try {
				Bitmap bitmap = new Bitmap(imageWidth,imageHeight, PixelFormat.Format32bppRgb);
				foreach (var element in lsCeiLab) {
					RGB rgb = ColorSpaceHelper.LabtoRGB(element);
					bitmap.SetPixel(element.iWidth,element.iHeight,Color.FromArgb(0,rgb.Red,rgb.Green,rgb.Blue));
				}
				bitmap.Save(fileName);
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
			
		}
	}
}
