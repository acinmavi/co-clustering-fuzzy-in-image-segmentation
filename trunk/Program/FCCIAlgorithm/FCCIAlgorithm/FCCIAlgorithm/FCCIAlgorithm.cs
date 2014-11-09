/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 11/9/2014
 * Time: 5:14 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FCCIAlgorithm
{
	/// <summary>
	/// Description of FCCIAlgorithm.
	/// </summary>
	public class FCCIAlgorithm
	{
		public FCCIAlgorithm()
		{
		}
		
		public static void runAlgorithm(double Tu,double Tv,double eps,int maxIter,int C,int K,int N,double[,] x){
			Random random = new Random();
			double[,] U = new double[C,N];
			double[,] P = new double[C,K];
			double[,] V = new double[C,K];
			double[] sum = new double[N];
			double[,,] D = new double[C,N,K];
			double[,] preU = new double[C,N];
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
//			double sum2 = 0;
//			for (int i = 0; i < N; i++) {
//				for (int c = 0; c < C; c++) {
//					sum2 +=U[c,i];
//				}
//				Console.WriteLine("Total U with i="+ i +" = " +sum2);
//				sum2 = 0;
//			}
			//
//			Console.WriteLine("U(0) === ");
//			for (int i = 0; i < N; i++) {
//				for (int c = 0; c < C; c++) {
//					Console.Write(U[c,i] + " " );
//				}
//			}
//			Console.WriteLine();
			int count = 0;
			//save preU
			for (int i = 0; i < N; i++) {
				for (int c = 0; c < C; c++) {
					preU[c,i] = U[c,i];
				}
			}
			
			
			while(true){
				//save preU
				for (int i = 0; i < N; i++) {
					for (int c = 0; c < C; c++) {
						preU[c,i] = U[c,i];
					}
				}
				
				//Calculate pcj using(13)
				for (int c = 0; c < C; c++) {
					for (int j = 0; j < K; j++) {
						double up =0;
						double down =0;
						for (int i = 0; i < N; i++) {
							down+=U[c,i];
							up+=U[c,i]*x[i,j];
							if(up<0 || down < 0){
							Console.WriteLine("aaaaa");	
							}
						}
						P[c,j] = up/down;
					}
				}
				
				//Calculate Dcij using(3)
				for (int c = 0; c < C; c++) {
					for (int i = 0; i < N; i++) {
						for (int j = 0; j < K; j++) {
							D[c,i,j] = Math.Pow((x[i,j] - P[c,j]),2);
						}
					}
				}
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
				
//				double totalVcj = 0;
//				for (int c = 0; c < C; c++) {
//					for (int j = 0; j < K; j++) {
//						totalVcj+=V[c,j];
//					}
//					Console.WriteLine("Total V with c= "+c+" : " +totalVcj);
//					totalVcj = 0;
//				}
				
				//Calculate uci using (9)
				double totalUk=0;
				for (int i = 0;i < N; i++) {
					for (int c = 0; c < C; c++) {
						double pow = 0;
						for (int j = 0; j < K; j++) {
							pow-=(V[c,j]*D[c,i,j]/Tu);
						}
						U[c,i] = Math.Exp(pow);
						totalUk+=	U[c,i];
					}
					
					for (int c = 0; c < C; c++) {
						U[c,i] =U[c,i]/totalUk;
						if(Double.IsNaN(U[c,i])){
							U[c,i] = 0;
						}
					}
					totalUk=0;
				}
//				double totalUkCheck = 0;
//				
//				for (int i = 0;i < N; i++) {
//					for (int c = 0; c < C; c++) {
//						totalUkCheck +=U[c,i];
//					}
//					Console.WriteLine("Total U with i= "+i+" : " +totalUkCheck);
//					totalUkCheck = 0;
//				}
				
				
				count++;
				Console.WriteLine( "Calculate U( "+count+")");
//				for (int i = 0; i < N; i++) {
//					for (int c = 0; c < C; c++) {
//						Console.Write(U[c,i] + " " );
//					}
//				}
				Console.WriteLine();
				double maxEps = 0;
				for (int c = 0; c < C; c++) {
					for (int i = 0; i < N; i++) {
						double compare = Math.Abs(U[c,i]-preU[c,i]);
//						Console.WriteLine(c+" --" + i + "---" + U[c,i] + " -- " + preU[c,i] + " --- " +compare + " -- "+ "Max eps = " + maxEps);
						
						if(compare >= maxEps){
							maxEps = Math.Abs(U[c,i]-preU[c,i]);
						}
					}
				}
				
				if((maxEps <=eps) || count== maxIter){
					Console.WriteLine("finish count = " + count);
					break;
				}
			}
			Console.WriteLine("=====================================");
			Console.WriteLine("Final Uci(C,N) : ") ;
			printMatrix(U);
			Console.WriteLine();
			Console.WriteLine("=====================================");
			Console.WriteLine("Final Pcj (C,K) : ") ;
			printMatrix(P);
			
			
			double Snew = XieBeniClusterValidity.runValidity(U,x,P,C,K,N);
			Console.WriteLine("Snew after validity : " +  Snew);
		}
		
		public static void printMatrix(double[,] arr){
			int rowLength = arr.GetLength(0);
			int colLength = arr.GetLength(1);

			for (int i = 0; i < rowLength; i++)
			{
				for (int j = 0; j < colLength; j++)
				{
					Console.Write(string.Format("{0} ", arr[i, j]));
				}
				Console.Write(Environment.NewLine + Environment.NewLine);
			}
		}
	}
}
