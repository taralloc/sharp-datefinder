# sharp-datefinder
Given a text, extracts possible dates and converts them to DateTime object using `DateTime.Parse()`.
It also checks if the day or the year were specified in the original text date.

## Example
```c
string text = "See you on February 26, 2014. Remember to buy milk for the October 2020 deadline. Today, 21st August 2016.";

DateFinder engine = new DateFinder();
List<DateFinderResult> dates = engine.ExtractDates(text);

foreach(DateFinderResult res in dates)
{
  Console.WriteLine("{0} | day was set {1} | year was set {2}", res.Date.ToShortDateString(), res.IsDaySet, res.IsYearSet);
}
```

Output
```
02/26/2014 | day was set True | year was set True
10/01/2020 | day was set False | year was set True
08/21/2016 | day was set True | year was set True
```
