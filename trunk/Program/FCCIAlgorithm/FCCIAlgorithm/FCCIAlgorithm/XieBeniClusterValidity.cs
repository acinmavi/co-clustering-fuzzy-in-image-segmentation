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
	/// Description of XieBeniClusterValidity.
	/// </summary>
	public class XieBeniClusterValidity
	{
		public XieBeniClusterValidity()
		{
		}
		
		public static double runValidity(double[,] U,double[,] x,double[,] P,int C,int K,int N)
		{
			double S=0;
			double xigma = 0;
			double xigmaCompare = 0;
			for (int c = 0; c < C; c++) {
				for (int i = 0; i < N; i++) {
					double totalXp = 0;
					for (int j = 0; j < K; j++) {
						totalXp+=Math.Pow((x[i,j]-P[c,j]),2);
					}
					xigmaCompare = U[c,i]*U[c,i]*totalXp;
					if(xigmaCompare > xigma ) xigma = xigmaCompare;
				}
			}
			
			double dmin = 100;
			for (int c = 0; c < C; c++) {
				double dminCompare = 0;
				for (int j = 0; j < K; j++) {
					if((c+1)<C){
						dminCompare+=Math.Pow(P[c+1,j]-P[c,j],2);
					}else{
						dminCompare+=Math.Pow(P[0,j]-P[c,j],2);
					}
				}
				if(dminCompare < dmin ){
					dmin =dminCompare;
				}
			}
			
			S = xigma/N/(Math.Pow(dmin,2));
			return S;
		}
	}
}
