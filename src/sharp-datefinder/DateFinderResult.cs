using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_datefinder
{
    public class DateFinderResult
    {
        /// <summary>
        /// The parsed date.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// True if the day was specified in the original string.
        /// </summary>
        public bool IsDaySet { get; }

        /// <summary>
        /// True if the year was specified in the original string.
        /// </summary>
        public bool IsYearSet { get; }

        public DateFinderResult(DateTime Date, bool IsDaySet, bool IsYearSet)
        {
            this.Date = Date;
            this.IsDaySet = IsDaySet;
            this.IsYearSet = IsYearSet;
        }
    }
}
