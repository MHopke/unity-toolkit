using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using gametheory.UI;

public class DonutGraph : VisualElement 
{
    #region Constants
    const float WAIT_TIME = 0.025f;
    #endregion

    #region Public Vars
    public bool ShowText;
    public bool Animates;

    public float AnimationTime;

    public Image Fill;
    public Image Background;
    public Text Text;
    #endregion

    #region Private Vars
    float _targetValue;
    float _fillAmount;
    #endregion

    #region Overriden Methods
    public override void PresentVisuals(bool display)
    {
        base.PresentVisuals(display);

        if(Fill)
            Fill.enabled = display;

        if(Background)
            Background.enabled = display;

        if(Text && ShowText)
            Text.enabled = display;
    }
    #endregion

    #region Methods
    public void SetValue(float percent)
    {
        _targetValue = percent;
        _fillAmount = _targetValue / AnimationTime;

        if(Animates)
            StartCoroutine(Animate());
        else
        {
            Fill.fillAmount = percent;
            Text.text = Mathf.RoundToInt(percent * 100f) + "%";
        }
    }
    #endregion

    #region Coroutines
    IEnumerator Animate()
    {
        Fill.fillAmount = 0f;

        while(Fill.fillAmount < _targetValue)
        {
            Fill.fillAmount += _fillAmount * WAIT_TIME;
            Text.text = Mathf.RoundToInt(Fill.fillAmount * 100f) + "%";
            yield return new WaitForSeconds(WAIT_TIME);
        }

        Fill.fillAmount = _targetValue;
        Text.text = Mathf.RoundToInt(Fill.fillAmount * 100f) + "%"; 
    }
    #endregion
}
