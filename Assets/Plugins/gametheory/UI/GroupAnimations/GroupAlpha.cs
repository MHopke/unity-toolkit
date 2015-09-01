using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class GroupAlpha : GroupAnimation 
{
    #region Public Vars
    public float Alpha;
    public Graphic[] Group;
    #endregion

    #region Private Vars
    Color _color;
    #endregion

    #region Overridden Methods
    protected override void OnAppliedChanges()
    {
        base.OnAppliedChanges();

        for(_index = 0; _index < Group.Length; _index++)
        {
            _color = Group[_index].color;
            Group[_index].color = new Color(_color.r,_color.g,_color.b,Alpha);
        }
    }
    #endregion
}
