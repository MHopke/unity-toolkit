using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using gametheory.UI;

#region Enums
public enum BarGraphSectionType { SLIDER, FILL }
#endregion

public class BarGraphSection : VisualElement
{
    #region Constants
    const float FILL_TIME = 0.75f;
    #endregion

    #region Public Vars
    public BarGraphSectionType Type;

    public Image BarFill;
    public Slider _barSlider;
    public Text _label;
    #endregion

    #region Private Vars
    bool _increase;
    float _fillDelta;
    float _targetFill;

    Image _barFill;
    #endregion

    #region Unity Methods
    void Update()
    {
        AddFill(_fillDelta * Time.deltaTime);

        if (_increase)
        {
            if (GetCurrentFill() >= _targetFill)
                Finish();
        }
        else if (GetCurrentFill() <= _targetFill)
            Finish();
    }
    #endregion

    #region Overriden Methods
    public override void PresentVisuals(bool display)
    {
        base.PresentVisuals(display);

        if(_barSlider)
        {
            _barFill = _barSlider.fillRect.GetComponent<Image>();

            if (_barFill)
                _barFill.enabled = display;
        }

        if (BarFill)
            BarFill.enabled = display;

        if (_label)
            _label.enabled = display;
    }
    #endregion

    #region Method
    public void Fill(float percent)
    {
        SetCurrentFill(percent);
    }

    public void AnimateFill(float percent)
    {
        float value = GetCurrentFill();

        _fillDelta = (percent - value) / FILL_TIME;
        _targetFill = percent;
        
        if (percent > value)
            _increase = true;
        else
            _increase = false;

        enabled = true;
    }
    public void SetLabel(string text)
    {
        _label.text = text;
    }
    void Finish()
    {
        SetCurrentFill(_targetFill);
        enabled = false;
    }
    float GetCurrentFill()
    {
        if (Type == BarGraphSectionType.SLIDER)
            return _barSlider.value;
        else if (Type == BarGraphSectionType.FILL)
            return BarFill.fillAmount;

        return 0f;
    }
    void SetCurrentFill(float value)
    {
        if (value > 1.0f)
            value = 1.0f;

        if (Type == BarGraphSectionType.SLIDER)
            _barSlider.value = value;
        else if (Type == BarGraphSectionType.FILL)
            BarFill.fillAmount = value;
    }
    void AddFill(float value)
    {
        if (Type == BarGraphSectionType.SLIDER)
            _barSlider.value += value;
        else if (Type == BarGraphSectionType.FILL)
            BarFill.fillAmount += value;
    }
    #endregion
}
