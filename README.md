# sharp-datefinder
Given a text, finds possible dates and converts them to DateTime object using `DateTime.Parse()`.
It also checks if the day or the year were specified in the original text date.

## Example
```c
string text = "See you on February 26, 2014. And remember to buy milk for the October 2020 deadline. Have a nice day, 21st August 2016.";

DateFinder engine = new DateFinder();
List<DateFinderResult> dates = engine.ExtractDates(text);

foreach(DateFinderResult res in dates)
{
  Console.WriteLine("{0} | day was set {1} | year was set {2}", res.Date.ToShortDateString(), res.IsDaySet, res.IsYearSet);
}
```
