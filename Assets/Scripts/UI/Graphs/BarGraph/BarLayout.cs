using UnityEngine;
using System.Collections;

public class BarLayout : MonoBehaviour 
{
    #region Public Vars
    /// <summary>
    /// The value that "zero" displays at
    /// </summary>
    public float _minFill =0.02f;
    public BarGraphSection[] _sections;
    #endregion

    #region Methods
    public void SetData(params GraphData[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if(data[i].Value < _minFill)
                _sections[i].AnimateFill(_minFill);
            else
                _sections[i].AnimateFill(data[i].Value);

            _sections[i]._label.text = data[i].Label;
        }
    }

    public void SetValues(params float[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if(values[i] < _minFill)
                _sections[i].AnimateFill(_minFill);
            else
                _sections[i].AnimateFill(values[i]);
        }
    }
    public void SetLabels(params string[] labels)
    {
        for (int i = 0; i < labels.Length; i++)
        {
            _sections[i]._label.text = labels[i];
        }
    }
	public void Clear()
	{
		BarGraphSection section = null;
		for (int index = 0; index < _sections.Length; index++)
		{
			section = _sections[index];
			section.SetLabel("");
			section.Fill(0f);
		}
	}
    #endregion
}
