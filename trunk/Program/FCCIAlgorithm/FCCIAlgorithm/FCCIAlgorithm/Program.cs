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
			Console.WriteLine("==============FCCI ALGORITHM====================");
			string folder = Path.Combine(Environment.CurrentDirectory,"Images") ;
			if(args.Length > 0)
			{
				folder = args[0];
			}
			string[] filePaths = Directory.GetFiles(folder, "*.jpg",
			                                        SearchOption.AllDirectories);
			
			//
			string processFolder = Path.Combine(Directory.GetParent(folder).FullName,"ProcessImages");
			if(!Directory.Exists(processFolder)){
				Directory.CreateDirectory(processFolder);
			}
			//delete all old file in process folder
			Array.ForEach(Directory.GetFiles(processFolder), File.Delete);
			
			
			//copy file from original folder to process folder
			foreach (var originalFile in filePaths) {
				File.Copy(originalFile,Path.Combine(processFolder,Path.GetFileName(originalFile)),true);
			}
			
			//now process algorithm on process folder
			filePaths = Directory.GetFiles(processFolder, "*.jpg",
			                               SearchOption.AllDirectories);

			foreach (var processFilePath in filePaths) {
				FCCIAlgorithm fcci = new FCCIAlgorithm(processFilePath);
				fcci.Process();
			}
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		
		
		
		
	}
}