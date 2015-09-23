using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LineGraph : Graph 
{
    #region Public Vars
    public int StartOffset;
    public Text TitleText;
    public WMG_Axis_Graph AxisGraph;
    #endregion

    #region Private Vars
    //Text TitleText;

    WMG_Series _series;
    #endregion

    #region Overriden Methods
    public override void OnInitialize()
    {
        base.OnInitialize();

        //_titleText = AxisGraph.graphTitle.GetComponent<Text>();

        _series = AxisGraph.lineSeries[0].GetComponent<WMG_Series>();
        _series.SeriesDataChanged += SeriesChanged;
    }

    public override void ClearData()
    {
        base.ClearData();

        GraphData[] data = null;
        for(int index = 0; index < _seriesData.Count; index++)
        {
            data = _seriesData[index];

            for(int sub = 0; sub < data.Length; sub++)
                data[sub].Value = 0;

            SetSeriesData(index,data);
        }

        DisplaySeries(0);
    }

    protected override void OnSetData(params GraphData[] data)
    {
        base.OnSetData(data);

        //Debug.Log("setting data: " + data.Length);
        int index = 0;
        float value = 0f;
        List<Vector2> list = new List<Vector2>();
        List<Color> colors = new List<Color>();
        for(index =0; index < StartOffset; index++)
        {
            list.Add(new Vector2(index + 1,0));
            colors.Add(Color.clear);
        }
        for (index = 0; index < data.Length; index++)
        {
            value = data[index].Value;
            list.Add(new Vector2(index + StartOffset + 1,Mathf.RoundToInt(value)));
            colors.Add(GetColorValue(value));
            //AxisGraph.groups[index] = data[index].Label;
        }
        //_series.point
        _series.pointColors.SetList(colors);
        _series.pointValues.SetList(list);
        //AxisGraph.Refresh();
    }

    protected override void SetTitle(string title)
    {
        TitleText.text = title;
    }
    protected override string GetTitle()
    {
        return TitleText.text;
    }
    #endregion

    #region Virtual Methods
    protected virtual Color GetColorValue(float value) { return Color.white; }
    protected virtual void OnSeriesChanged(WMG_Series series){}
    #endregion

    #region Methods
    public void ToggleAxisGraph(bool state)
    {
        if(AxisGraph.gameObject.activeSelf != state)
            AxisGraph.gameObject.SetActive(state);
    }
    #endregion

    #region Series Event Listeners
    void SeriesChanged(WMG_Series series)
    {
        OnSeriesChanged(series);
    }
    #endregion
}
