using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.IO;
namespace Main
{
    class GNUDB
	{
	}
	class Config
	{
        Random RNG = new Random();
		public string GetRandomString(int length)
		{
			string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			string RandomString = "";
			for (int i = 0; i < length; i++)
			{
				RandomString += Characters[RNG.Next(0,Characters.Length-1)];
			}
            return (RandomString);
        }
		public string GetRandomEmailAdress()
		{
			return ($"{GetRandomString(32)}@{GetRandomString(8)}.com");
		}
		public void FileHandler()
		{
            if (!File.Exists("GNUDB.txt"))
			{
                using (FileStream fs = File.Create("GNUDB.txt")) { }
				string[] Write = {GetRandomEmailAdress()};
				File.WriteAllLines("GNUDB.txt",Write);
			}
			else
			{
				if (!File.ReadLines("GNUDB.txt").Skip(0).Take(1).First().Contains("@"))
				{
                    string[] Write = {GetRandomEmailAdress()};
                    File.WriteAllLines("GNUDB.txt", Write);
                }
			}
        }
    }
}