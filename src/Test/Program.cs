using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sharp_datefinder;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader reader = new StreamReader("test.txt");
            string text = reader.ReadToEnd();
            reader.Close();

            DateFinder engine = new DateFinder();
            List<DateFinderResult> dates = engine.ExtractDates(text);
            
            foreach(DateFinderResult res in dates)
            {
                Console.WriteLine("{0} | day was set {1} | year was set {2}", res.Date.ToShortDateString(), res.IsDaySet, res.IsYearSet);
            }

            Console.Read();
        }
    }
}
