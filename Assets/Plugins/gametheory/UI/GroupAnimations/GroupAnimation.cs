using UnityEngine;
using System.Collections;

public class GroupAnimation : MonoBehaviour 
{
    #region Public Vars
    public Animator Animator;
    #endregion

    #region Protected Vars
    protected int _index;
    #endregion

    #region Unity Methods
    void OnDidApplyAnimationProperties()
    {
        OnAppliedChanges();
    }
    #endregion

    #region Virtual Methods
    protected virtual void OnAppliedChanges(){}
    #endregion

    #region Methods
    public void SetTrigger(string trigger)
    {
        Animator.SetTrigger(trigger);
    }
    #endregion
}
