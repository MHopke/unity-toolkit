using UnityEngine;
using System.Collections;

public class GroupScale : GroupAnimation 
{
    #region Public Vars
    public float Scale;
    public RectTransform[] Group;
    #endregion

    #region Overridden Methods
    protected override void OnAppliedChanges()
    {
        base.OnAppliedChanges();

        for(_index =0; _index < Group.Length; _index++)
        {
            Group[_index].SetXYScale(Scale,Scale);
        }
    }
    #endregion
}
