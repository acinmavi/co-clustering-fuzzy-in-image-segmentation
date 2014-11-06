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

namespace FCCIAlgorithm
{
	class Program
	{
		public static void Main(string[] args)
		{
			Random random = new Random();
			Console.WriteLine("Hello World!");
			int C = 3;
			int N = 5;
			int K = 2;
			double[,] x = new double[N,K];
			for (int i = 0; i < N; i++) {
				for (int j = 0; j < K; j++) {
					x[i,j] = random.NextDouble();
				}
			}
			FCCI(1,1,1,100,C,K,N,x);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		public static void FCCI(double Tu,double Tv,float eps,int maxIter,int C,int K,int N,double[,] x){
			Random random = new Random();
			double[,] U = new double[C,N];
			double[,] P = new double[C,K];
			double[,] V = new double[C,K];
			double[] sum = new double[N];
			double[,,] D = new double[C,N,K];
			List<double[,]> lsU = new List<double[,]>();
			//Initialize uci such that 0≤uci≤1
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
			
			//check again
			double sum2 = 0;
			for (int i = 0; i < N; i++) {
				for (int c = 0; c < C; c++) {
					sum2 +=U[c,i];
				}
				Console.WriteLine(sum2);
				sum2 = 0;
			}
			//
			lsU.Add(U);
			
			int count = 0;
			
			while(true){
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
				
				//Calculate Dcij using(3)
				for (int c = 0; c < C; c++) {
					for (int i = 0; i < N; i++) {
						for (int j = 0; j < K; j++) {
							D[c,i,j] = Math.Pow(x[i,j] - P[c,j],2);
						}
					}
				}
				//Calculate vcj using (11)
				double[,] upDivForV = new double[C,K];
				double[,] downDivForV = new double[C,K];
				for (int c = 0; c < C; c++) {
					for (int j = 0; j < K; j++) {
						
						double pow = 0;
						for (int i = 0; i < N; i++) {
							pow-=(U[c,i]*D[c,i,j]/Tv);
						}
						upDivForV[c,j] = Math.Exp(pow);
						downDivForV[c,j]+=upDivForV[c,j];
					}
				}
				
				for (int c = 0; c < C; c++) {
					for (int j = 0; j < K; j++) {
						V[c,j] = upDivForV[c,j]/downDivForV[c,j];
					}
				}
				
				//Calculate uci using (9)
				
				double[,] upDivForU = new double[C,N];
				double[,] downDivForU = new double[C,N];
				for (int c = 0; c < C; c++) {
					for (int i = 0; i < N; i++) {
						double pow = 0;
						for (int j = 0; j < K; j++) {
							pow-=(V[c,j]*D[c,i,j]/Tu);
						}
						upDivForU[c,i] = Math.Exp(pow);
						downDivForU[c,i]+=upDivForU[c,i];
					}
					
					for (int i = 0; i < N; i++) {
						U[c,i] = upDivForU[c,i]/downDivForU[c,i];
					}
				}
				
				//check again
				sum2 = 0;
				for (int i = 0; i < N; i++) {
					for (int c = 0; c < C; c++) {
						sum2 +=U[c,i];
					}
					Console.WriteLine(sum2);
					sum2 = 0;
				}
				//
				count++;
				lsU.Add(U);
				
				if((Math.Abs(1) <=eps) || count== maxIter){
					break;
				}
			}
		}
	}
}