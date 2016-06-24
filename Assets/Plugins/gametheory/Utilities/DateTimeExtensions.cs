using System;

namespace gametheory.Utilities
{
	/// <summary>
	/// Several helpful methods for the DateTime Class.
	/// </summary>
	public static class DateTimeExtensions
	{
	    #region Methods
	    public static string MonthDayString(this DateTime date)
	    {
	        return date.Month + "/" + date.Day;
	    }
	    public static string YYYYMMDDString(this DateTime date)
	    {
	        string month = (date.Month < 10) ? "0" + date.Month.ToString() : date.Month.ToString();
	        string day = (date.Day < 10) ? "0" + date.Day.ToString() : date.Day.ToString();
	        return date.Year.ToString() + "-" + month + "-" + day;
	    }
	    public static DateTime StartOfMonth(this DateTime date)
	    {
	        return new DateTime(date.Year, date.Month, 1).Date;
	    }
	    public static DateTime EndOfMonth(this DateTime date)
	    {
	        return new DateTime(date.Year,date.Month,DateTime.DaysInMonth(date.Year,date.Month));
	    }
	    public static DateTime StartOfWeek(this DateTime date)
	    {
	        return date.AddDays(-(int)date.DayOfWeek).Date;
	    }
	    public static DateTime EndOfWeek(this DateTime date)
	    {
	        return (date.AddDays(DayOfWeek.Saturday - date.DayOfWeek)).Date;
	    }
		public static string AbbreviatedMonth(this DateTime date)
		{
			switch (date.Month)
			{
			case 1:
				return "Jan.";
			case 2:
				return "Feb.";
			case 3:
				return "Mar.";
			case 4:
				return "Apr.";
			case 5:
				return "May";
			case 6:
				return "Jun.";
			case 7:
				return "Jul.";
			case 8:
				return "Aug.";
			case 9:
				return "Sep.";
			case 10:
				return "Oct.";
			case 11:
				return "Nov.";
			case 12:
				return "Dec.";
			default:
				return "";
			}
		}
	    #endregion
	}
}
