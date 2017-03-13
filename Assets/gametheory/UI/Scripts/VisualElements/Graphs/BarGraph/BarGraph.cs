using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Globalization;
using gametheory.UI;

public class BarGraph : Graph 
{
    #region Public Vars
    public ExtendedText TitleText;

    public BarLayout Bars;
    #endregion

    #region Overidden Methods
    protected override void OnSetData(params GraphData[] data)
    {
        base.OnSetData(data);

        Bars.SetData(data);
    }
    protected override void OnSetValues(params float[] data)
    {
        base.OnSetValues(data);

        Bars.SetValues(data);
    }
    protected override void OnSetLabels(params string[] labels)
    {
        base.OnSetLabels(labels);

        Bars.SetLabels(labels);
    }
    protected override string GetTitle()
    {
        return TitleText.Text;
    }
    protected override void SetTitle(string title)
    {
        TitleText.Text = title;
    }
	public override void ClearData()
	{
		base.ClearData();
		
		Bars.Clear();
	}
    #endregion
}