using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_datefinder
{
    public class DateFinder
    {
        /// <summary>
        /// Defines the culture used to interpret dates. Default is CultureInfo.InvariantCulture.
        /// </summary>
        private CultureInfo CultureProvider { get; set; }

        /// <summary>
        /// Minimum integer considered as year. Default is 1900.
        /// </summary>
        private int MinimumValidYear { get; set; }
        /// <summary>
        /// Maximum integer considered as year. Default is 2050.
        /// </summary>
        private int MaximumValidYear { get; set; }

        public DateFinder()
        {
            CultureProvider = CultureInfo.InvariantCulture;
            MinimumValidYear = 1900;
            MaximumValidYear = 2050;

        }

        /// <summary>
        /// Initialize a new DateFinder engine.
        /// </summary>
        /// <param name="provider">Defines the culture used to interpret dates.</param>
        public DateFinder(CultureInfo provider)
        {
            CultureProvider = provider;
            MinimumValidYear = 1900;
            MaximumValidYear = 2050;
        }

        /// <summary>
        /// Initialize a new DateFinder engine.
        /// </summary>
        /// <param name="min_year">Minimum integer considered as year.</param>
        /// <param name="max_year">Maximum integer considered as year.</param>
        public DateFinder(int min_year, int max_year)
        {
            MinimumValidYear = min_year;
            MaximumValidYear = max_year;
        }

        /// <summary>
        /// Initialize a new DateFinder engine.
        /// </summary>
        /// <param name="provider">Defines the culture used to interpret dates.</param>
        /// <param name="min_year">Minimum integer considered as year.</param>
        /// <param name="max_year">Maximum integer considered as year.</param>
        public DateFinder(CultureInfo provider, int min_year, int max_year)
        {
            CultureProvider = provider;
            MinimumValidYear = min_year;
            MaximumValidYear = max_year;
        }

        /// <summary>
        /// Checks if a string may be the beginning of a date.
        /// </summary>
        /// <param name="s">String to analyse.</param>
        private bool IsKeyword(string s)
        {
            //Prepare string s
            s = s.ToLowerInvariant();
            s = s.Replace('/', ' ').Replace('-', ' ').Replace(':', ' ').Replace('.', ' ').Replace(", ", " ").Replace('\\', ' ').Replace("'", "").Replace(",", "");
            string[] split = s.Split(' ');

            string[] abb_month = CultureProvider.DateTimeFormat.AbbreviatedMonthNames.Select(a => a.ToLowerInvariant()).ToArray();
            string[] abb_day = CultureProvider.DateTimeFormat.AbbreviatedDayNames.Select(a => a.ToLowerInvariant()).ToArray();
            string[] month = CultureProvider.DateTimeFormat.DayNames.Select(a => a.ToLowerInvariant()).ToArray();
            string[] day = CultureProvider.DateTimeFormat.MonthNames.Select(a => a.ToLowerInvariant()).ToArray();

            foreach (string word in split) //If at least one word is a keyword, s is a keyword
            {
                if (abb_month.Contains(word) || abb_day.Contains(word) || month.Contains(word) || day.Contains(word) || IsNumber(word.Replace("st", "").Replace("th", "").Replace("rd", "").Replace("nd", "")))
                    return true;
            }

            return false;
        }

        private bool IsNumber(string s)
        {
            int n;
            return int.TryParse(s, out n);
        }

        /// <summary>
        /// Performs checks before parsing the string that may or may not contain a date.
        /// </summary>
        /// <param name="s">String to be converted.</param>
        /// <returns></returns>
        private bool PreProcess(string s)
        {
            s = s.Trim();

            //If there are two numbers separated by a blank space, then return false
            string[] split = s.Split(' ').Where(x => x != "").ToArray();
            if (split.Length == 2 && IsNumber(split[0]) && IsNumber(split[1])) return false;

            //If there are three numbers, but no year (between Minimum and Maximum or apostrophe)
            if (split.Length == 3 && !s.Contains("'") && IsNumber(split[0]) && IsNumber(split[1]) && IsNumber(split[2])
                && !(TestRange(int.Parse(split[0]), MinimumValidYear, MaximumValidYear) || TestRange(int.Parse(split[1]), MinimumValidYear, MaximumValidYear) || TestRange(int.Parse(split[2]), MinimumValidYear, MaximumValidYear)))
                return false;

            return true;
        }

        /// <summary>
        /// Performs sanity check after the conversion.
        /// </summary>
        /// <param name="date">The date got from DateTime.Parse()</param>
        private bool PostProcess(DateTime date)
        {
            //The year must be between MinimumValidYear and MaximumValidYear
            if (!TestRange(date.Year, MinimumValidYear, MaximumValidYear))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a number is between two numbers, inclusive.
        /// </summary>
        /// <returns></returns>
        private bool TestRange(int numberToCheck, int bottom, int top)
        {
            return (numberToCheck >= bottom && numberToCheck <= top);
        }

        /// <summary>
        /// Checks if the day was specified in the original string.
        /// </summary>
        /// <param name="original">Original string of the parsed date.</param>
        /// <param name="result">The resulting parsed date.</param>
        private bool DayExists(string original, DateTime result)
        {
            bool exists = false;

            if (result.Day != 1)
            {
                //If the resulting day is different from 1, it exists necessarily
                exists = true;
            }
            else
            {
                original = original.ToLowerInvariant();
                original = original.Replace('/', ' ').Replace('-', ' ').Replace(':', ' ').Replace('.', ' ').Replace(", ", " ").Replace('\\', ' ').Replace("'", "");
                string[] split = original.Split(' ');

                if (result.Month != 1) //If not January (month #1)
                {
                    //If at least one '1' exists, then the day exists

                    foreach (string s in split)
                    {
                        int n;
                        if (int.TryParse(s, out n) && n == 1)
                            exists = true;
                    }
                }
                else
                {
                    //If two '1' exists, then the day exists
                    //If there is one '1' and the name of the month, then the day exists

                    int ones = 0;
                    foreach (string s in split)
                    {
                        int n;
                        if (int.TryParse(s, out n) && n == 1)
                            ones++;
                    }
                    if (ones == 2)
                    {
                        exists = true;
                    }
                    else if (ones == 1 && (original.Contains(CultureProvider.DateTimeFormat.AbbreviatedMonthNames[0].ToLowerInvariant())
                        || original.Contains(CultureProvider.DateTimeFormat.MonthNames[0].ToLowerInvariant())))
                    {
                        exists = true;
                    }
                }
            }

            return exists;
        }

        /// <summary>
        /// Checks if the year was specified in the original string.
        /// </summary>
        /// <param name="original">Original string of the parsed date.</param>
        /// <param name="result">The resulting parsed date.</param>
        private bool YearExists(string original, DateTime result)
        {
            bool exists = false;

            if (result.Year != DateTime.Now.Year)
            {
                //If the resulting year is different from current year, it exists necessarily
                exists = true;
            }
            else
            {
                //If an apostrophe exists, then the year exists 
                //If at least one number between 1900 and 2050 exists, then the year exists
                //If there are exactly three numbers, then they year exists

                if (original.Contains("'"))
                {
                    exists = true;
                }
                else
                {
                    original = original.ToLowerInvariant();
                    original = original.Replace('/', ' ').Replace('-', ' ').Replace(':', ' ').Replace('.', ' ').Replace(", ", " ").Replace('\\', ' ').Replace("'", "");
                    string[] split = original.Split(' ').Where(x => x != "").ToArray();

                    foreach (string s in split)
                    {
                        int n;
                        if (int.TryParse(s, out n) && TestRange(n, 1900, 2050))
                            exists = true;
                    }

                    if (split.Length == 3 && IsNumber(split[0]) && IsNumber(split[1]) && IsNumber(split[2]))
                        exists = true;
                }
            }

            return exists;
        }

        /// <summary>
        /// Main method that finds dates in a string of natural text.
        /// </summary>
        /// <param name="text">Text to be analysed.</param>
        /// <returns>List of date results found in the text.</returns>
        public List<DateFinderResult> ExtractDates(string text)
        {
            List<DateFinderResult> dates = new List<DateFinderResult>();

            //Prepare text
            text = text.Replace('\n', ' ').Replace('\r', ' ').Replace("1st", "1").Replace("2nd", "2").Replace("3rd", "3").Replace("4th", "4").Replace("5th", "5").Replace("6th", "6").Replace("7th", "7").Replace("8th", "8").Replace("9th", "9");
            //Split by blank spaces and delete empty strings
            string[] split = text.Split(' ').Where(x => x != "").ToArray();

            for (int i = 0; i < split.Length; i++)
            {
                if (IsKeyword(split[i])) //A keyword may indicate a possible date
                {
                    //Now let's check the three words ahead to see if it is really a date
                    int j = (i + 3 < split.Length) ? 3 : split.Length - i - 1; //Don't go out of the array!
                    for (; j >= 0; j--)
                    {
                        string cmp = ""; //This is the string we'll try to parse
                        for (int k = 0; k <= j; k++) cmp += split[i + k] + " "; //Builds the string from splitted words
                        if (PreProcess(cmp))
                        {
                            DateTime date = new DateTime();
                            bool isdate = DateTime.TryParse(cmp, CultureProvider, DateTimeStyles.AssumeUniversal, out date);
                            if (isdate && PostProcess(date))
                            {
                                DateFinderResult res = new DateFinderResult(date, DayExists(cmp, date), YearExists(cmp, date));
                                dates.Add(res);
                                i = i + j; //Keep searching after the just found date
                                break;
                            }
                        }
                    }
                }
            }

            return dates;
        }


    }
}
