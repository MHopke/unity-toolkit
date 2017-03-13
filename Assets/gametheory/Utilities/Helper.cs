using UnityEngine;
using System;

namespace gametheory.Utilities
{
	/// <summary>
	/// The Helper class contains a grouping of methods so that they can be
	/// easily re-used.
	/// </summary>
	static public class Helper 
	{
	    #region Constants
	    const int DISPLAY_PRECISION = 2;
	    const int MAX_PACE_LENGTH = 10;

	    const double DEFAULT_MALE_WEIGHT = 69600.0;
	    const double DEFAULT_FEMALE_WEIGHT = 56500.0;
	    const double MAX_PACE = 1000.0;
	    const double STEPS_PER_MILE = 2000.0;
	    public const double MILES_PER_METER = 0.000621371;
	    public const double METERS_PER_MILE = 1609.34;
	    public const double POUNDS_PER_GRAM = 0.00220462;

	    public const string VALUE_STRING = "{x}";
	    #endregion

	    #region Date Methods
		public static string GetDayOfWeek(int dayOW =-1)
		{
			string day = "";

			DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

			if(dayOW >= 0 && dayOW < 7)
				dayOfWeek = (DayOfWeek)dayOW;

			switch(dayOfWeek)
			{
				case DayOfWeek.Monday:
				day += "Mon";
				break;
				case DayOfWeek.Tuesday:
				day += "Tue";
				break;
				case DayOfWeek.Wednesday:
				day += "Wed";
				break;
				case DayOfWeek.Thursday:
				day += "Thu";
				break;
				case DayOfWeek.Friday:
				day += "Fri";
				break;
				case DayOfWeek.Saturday:
				day += "Sat";
				break;
				case DayOfWeek.Sunday:
				day += "Sun";
				break;
			}

			return day;
		}
		public static string GetCurrentDay(int day=-1)
		{
			return (day != -1) ? day.ToString() : DateTime.Today.Day.ToString();
		}
		public static int DayLookUp(string day)
		{
			return int.Parse(day);
		}

		public static string GetCurrentMonth()
		{
			string month = "";
			switch(DateTime.Today.Month)
			{
				case 1:
				month += "Jan";
				break;
				case 2:
				month += "Feb";
				break;
				case 3:
				month += "Mar";
				break;
				case 4:
				month += "Apr";
				break;
				case 5:
				month += "May";
				break;
				case 6:
				month += "Jun";
				break;
				case 7:
				month += "Jul";
				break;
				case 8:
				month += "Aug";
				break;
				case 9:
				month += "Sep";
				break;
				case 10:
				month += "Oct";
				break;
				case 11:
				month += "Nov";
				break;
				case 12:
				month += "Dec";
				break;
			}

			return month;
		}
		public static int MonthLookUp(string month)
		{
			switch(month)
			{
			case "Jan":
				return 1;
			case "Feb":
				return 2;
			case "Mar":
				return 3;
			case "Apr":
				return 4;
			case "May":
				return 5;
			case "Jun":
				return 6;
			case "Jul":
				return 7;
			case "Aug":
				return 8;
			case "Sep":
				return 9;
			case "Oct":
				return 10;
			case "Nov":
				return 11;
			case "Dec":
				return 12;
			default:
				return -1;
			}
		}

		public static string GetCurrentYear()
		{
			return DateTime.Today.Year.ToString();
		}
		public static int YearLookUp(string year)
		{
			return int.Parse(year);
		}

		public static string GetCurrentTime()
		{
			return DateTime.Now.ToLongTimeString();
		}
	    public static bool DateIsMoreRecent(string dateOne, string dateTwo)
	    {
	        string[] dateOneParts = dateOne.Split(' ');
	        string[] dateTwoParts = dateTwo.Split(' ');
	        
	        //[[1] = day [2] = month [3] = year [4] = time
	        
	        /*Debug.Log("Look ups: " + YearLookUp(dateOneParts[3]) + " " + MonthLookUp(dateOneParts[2]) + " " 
	                  + DayLookUp(dateOneParts[1]) + System.DateTime.Parse(dateOneParts[4]));*/
	        
	        if(YearLookUp(dateOneParts[3]) > YearLookUp(dateTwoParts[3]))
	            return true;
	        else if(YearLookUp(dateOneParts[3]) > YearLookUp(dateTwoParts[3]) &&
	                MonthLookUp(dateOneParts[2]) > MonthLookUp(dateTwoParts[2]))
	            return true;
	        else if(MonthLookUp(dateOneParts[2]) == MonthLookUp(dateTwoParts[2]) &&
	                DayLookUp(dateOneParts[1]) > DayLookUp(dateTwoParts[1]))
	            return true;
	        else if(DayLookUp(dateOneParts[1]) == DayLookUp(dateTwoParts[1]) &&
	                DateTime.Parse(dateOneParts[4]) > DateTime.Parse(dateTwoParts[4]))
	            return true;
	        else
	            return false;
	    }

	    public static int MonthDifference(int monthOne, int monthTwo)
	    {
	        if (monthOne < monthTwo)
	        {
	            //Debug.Log(monthOne + " " + monthTwo + " " + ((12 - monthTwo) + monthOne));
	            return (12 - monthTwo) + monthOne;
	        }
	        else
	        {
	            //Debug.Log(monthOne + " " + monthTwo + " " + (monthOne - monthTwo));
	            return monthOne - monthTwo;
	        }
	    }
	    #endregion

	    #region Time Methods
		/// <summary>
		/// Converts the time (in seconds) to a TimeString.
		/// </summary>
		/// <returns>The TimeString representation</returns>
		/// <param name="time">Time.</param>
		public static TimeString ConvertTimeToTimeString(double time)
		{
			TimeString timestring = new TimeString();
			int seconds = (int)time;
			int minutes = seconds / 60;
			if(minutes < 10)
				timestring.minutes = "0" + minutes;
			else if(minutes >= 10 && minutes < 60)
				timestring.minutes = minutes.ToString();
			else
			{
				int hours = minutes / 60;
				if(hours < 10)
					timestring.hours = "0" + hours;
				else
					timestring.hours = hours.ToString();

				int sminutes = minutes % 60;
				if(sminutes < 10)
					timestring.minutes = "0" + sminutes;
				else
					timestring.minutes = sminutes.ToString();
			}

			seconds %= 60;
			if(seconds < 10)
				timestring.seconds = "0" + seconds;
			else
				timestring.seconds = seconds.ToString();

			return timestring;
		}
		public static string ConvertSecondsToHourString(double time)
		{
	        return ConvertSecondsToHourString((int)time);
		}
	    public static string ConvertSecondsToHourString(float time)
	    {
	        return ConvertSecondsToHourString((int)time);
	    }
	    public static string ConvertSecondsToMinuteString(double time,bool leadingZero=true)
	    {
	        string timestring = "";
	        int seconds = (int)time;
	        int minutes = seconds / 60;
	        if(leadingZero && minutes < 10)
	        {
	            timestring += "0" + minutes;
	        }
	        else
	        {
	            timestring += minutes.ToString();
	        }

	        timestring += ":";

	        seconds %= 60;
	        if(seconds < 10)
	            timestring += "0" + seconds;
	        else
	            timestring += seconds.ToString();

	        return timestring;
	    }
	    public static string ConvertSecondsToHourString(int seconds)
	    {
	        string timestring = "";

	        int minutes = seconds / 60;
	        if(minutes < 10)
	        {
	            timestring += "00:";
	            timestring += "0" + minutes;
	        }
	        else if(minutes >= 10 && minutes < 60)
	        {
	            timestring += "00:";
	            timestring += minutes.ToString();
	        }
	        else
	        {
	            int hours = minutes / 60;
	            if(hours < 10)
	                timestring += "0" + hours;
	            else
	                timestring += hours.ToString();

	            timestring += ":";

	            int sminutes = minutes % 60;
	            if(sminutes < 10)
	                timestring += "0" + sminutes;
	            else
	                timestring += sminutes.ToString();
	        }

	        timestring += ":";

	        seconds %= 60;
	        if(seconds < 10)
	            timestring += "0" + seconds;
	        else
	            timestring += seconds.ToString();

	        return timestring;
	    }
	    public static string ConvertTimeSpanToString(TimeSpan time)
	    {
	        return ConvertSecondsToHourString(time.TotalSeconds);
	    }
	    #endregion

	    #region String Methods
	    public static string ColorFormattedString(string str, char delimeter)
	    {
	        string[] pieces = str.Split(delimeter);

	        string formatted = pieces[0] + "<color=#39C4E5>" + pieces[1] + "</color>";

	        if(pieces.Length > 2)
	            formatted += pieces[2];

	        return formatted;
	    }
	    public static string ColorFormattedString(string mainString, string colorSection, bool appendColor)
	    {        
	        if(appendColor)
	            return mainString + "<color=#39C4E5>" + colorSection + "</color>";
	        else
	            return "<color=#39C4E5>" + colorSection + "</color>" + mainString;
	    }

		public static string ReplaceValue(string baseStr, object value,string delim="{x}")
		{
			return baseStr.Replace(delim,value.ToString());
		}
	    public static string InsertValuesIntoString(string baseString, params object[] values)
	    {
	        string newString = "";
	        string[] arr = baseString.Split(new string[] { VALUE_STRING },StringSplitOptions.None);
	        int length = Mathf.Min(arr.Length, values.Length);

	        int index = 0;
	        for (index = 0; index < length; index++)
	        {
	            newString += (arr[index] + values[index].ToString());
	        }

	        if (arr.Length > values.Length)
	        {
	            for (index = index; index < arr.Length; index++)
	                newString += arr[index];
	        }

	        return newString;
	    }
	    public static string TruncateString(string str, int maxChars)
	    {
	        if (str.Length > maxChars)
	        {
	            string endStr = str.Substring(str.Length - 2, 2);
	            return str.Substring(0, maxChars - 5) + "..." + endStr;
	        }
	        else
	            return str;
	    }
	    public static bool CheckIfSpaces(string str)
	    {
	        bool onlySpaces = true;
	        for (int index = 0; index < str.Length; index++)
	        {
	            if (str[index] != ' ')
	            {
	                onlySpaces = false;
	                break;
	            }
	        }

	        return onlySpaces;
	    }
	    #endregion
	}

	public class TimeString
	{
	    #region Public Vars
		public string hours;
		public string minutes;
		public string seconds;
	    #endregion

	    #region Constructors
		public TimeString()
		{
			hours = "00";
			minutes = "00";
			seconds = "00";
		}
	    #endregion
	}
}