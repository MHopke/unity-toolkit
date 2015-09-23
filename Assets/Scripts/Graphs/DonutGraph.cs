using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using gametheory.UI;

public class DonutGraph : VisualElement 
{
    #region Public Vars
    public bool ShowText;

    public Image Fill;
    public Image Background;
    public Text Text;
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
        Fill.fillAmount = percent;
        Text.text = Mathf.RoundToInt(percent * 100f) + "%";
    }
    #endregion
}
