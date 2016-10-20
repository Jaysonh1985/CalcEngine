using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Models.LogFile
{
    public class LogFile
    {
        public void LogFileWriter()
        {
            using (StreamWriter writer = new StreamWriter("C:\\programs\\file.txt"))
            {
                string animal = "cat";
                int size = 12;
                // Use string interpolation syntax to make code clearer.
                writer.WriteLine($"The {animal} is {size} pounds.");
            }
        }
    }
}