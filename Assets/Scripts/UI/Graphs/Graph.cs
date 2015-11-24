using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using gametheory.UI;

public class Graph : MonoBehaviour 
{
    #region Public Vars
    public Color On;
    public Color Off;

    public Font OnFont;
    public Font OffFont;

    public Image Underline;

    public int SeriesCount;

    public ExtendedButton[] SeriesButtons;
    #endregion

    #region Protected Vars
    protected bool _dataDirty;
    protected bool _initialized;

    protected int _seriesDisplayed;

    protected List<GraphData[]> _seriesData;
    #endregion

    #region UI Methods
    public void DisplaySeries(int seriesIndex)
    {
        bool newState = (_seriesDisplayed != seriesIndex);

        if(newState)
        {
            SeriesButtons[seriesIndex].Label.font = OnFont;

            for(int index =0; index < _seriesData.Count; index++)
            {
                if(index != seriesIndex)
                {
                    SeriesButtons[index].Label.font = OffFont;
                }
            }

            Underline.rectTransform.SetParent(SeriesButtons[seriesIndex].transform,false);
            Underline.rectTransform.anchoredPosition = new Vector2(0f,Underline.rectTransform.anchoredPosition.y);

            _seriesDisplayed = seriesIndex;
        }

        if(_dataDirty || newState)
            SetData(_seriesData[seriesIndex]);
    }
    #endregion

    #region Static Methods
    public static string DayLabel(DateTime date)
    {
        CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
        DateTimeFormatInfo dtfi = ci.DateTimeFormat;

        string label = dtfi.GetAbbreviatedDayName(date.DayOfWeek);
        return label.Substring(0,1).ToUpper() + label.Substring(1,label.Length -1) + ". " + date.Day;//Date.DayOfWeekString();
    }
    public static string WeekLabel(DateTime date)
    {
        return date.MonthDayString(); //+ " -\n" + date.AddDays(6).MonthDayString();
    }
    public static string MonthLabel(DateTime date)
    {
        //CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
        //DateTimeFormatInfo dtfi = ci.DateTimeFormat;

        //string label = //dtfi.GetAbbreviatedMonthName(date.Month);
        //return label.Substring(0,1).ToUpper() + label.Substring(1,label.Length-1);
        return date.AbbreviatedMonth();
    }
    #endregion

    #region Methods
    public void Initialize()
    {
        if (_initialized)
            return;
        
        OnInitialize();

        _initialized = true;

        _seriesData = new List<GraphData[]>();

        for(int index = 0; index < SeriesCount; index++)
            _seriesData.Add(new GraphData[]{});
    }

    public void RedrawData()
    {
        ExtendedButton button = null;
        for(int index = 0; index < SeriesButtons.Length; index++)
        {
            button = SeriesButtons[index];

            if(index == 0)
            {
                button.Label.color = On;
                button.Label.font = OnFont;
                button.Button.image.color = On;
            }
            else
            {
                button.Label.font = OffFont;
                button.Label.color = Off;
                button.Button.image.color = Off;
            }
        }

        SetData(_seriesData[0]);

        _seriesDisplayed = 0;
    }

    public void SetSeriesData(int seriesIndex, params GraphData[] data)
    {
        if(seriesIndex < _seriesData.Count)
        {
            _seriesData[seriesIndex] = data;
            
            _dataDirty = true;
        }
    }

    public void SetData(params GraphData[] data)
    {
        if (_dataDirty)
            _dataDirty = false;

        OnSetData(data);
    }
    public void SetValues(params float[] values)
    {
        OnSetValues(values);
    }
    public void SetLabels(params string[] labels)
    {
        OnSetLabels(labels);
    }
    #endregion

    #region Virtual Methods
    public virtual void OnInitialize(){}
    public virtual void CleanUp(){}
	public virtual void ClearData()
	{
		_seriesDisplayed = 0;
		
		int sub = 0;
		GraphData[] series = null;
		GraphData data = null;
		for(int index = 0; index < _seriesData.Count; index++)
		{
			series = _seriesData[index];
			
			for (sub = 0; sub < series.Length; sub++)
			{
				data = series[sub];
				data.Label = "";
				data.Value = 0f;
				data.Date = DateTime.MinValue;
			}
		}
	}

    protected virtual void OnSetData(params GraphData[] data){}
    protected virtual void OnSetValues(params float[] data){}
    protected virtual void OnSetLabels(params string[] labels){}
    protected virtual void SetTitle(string title){}
    protected virtual string GetTitle(){return "";}
    #endregion

    #region Accessors
    public string Title
    {
        get { return GetTitle(); }
        set { SetTitle(value); }
    }
    #endregion
}

public class GraphData
{
    #region Public Vars
    public float Value;

    public string Label;

    public DateTime Date;
    #endregion

    #region Constructors
    public GraphData(){}
    public GraphData(float value, DateTime date)
    {
        Value = value;
        Date = date;
    }
    #endregion
}